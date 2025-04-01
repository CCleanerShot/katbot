using oslo.binary;
using oslo.crypto.hash;

namespace oslo.crypto.sha2;

public class SHA256 : Hash
{
    public static uint[] K = new uint[] {
    0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
    0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
    0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
    0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
    0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
    0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
    0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
    0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
    };

    public SHA256()
    {
        blockSize = 64;
        size = 32;
    }

    private byte[] blocks = new byte[64];
    private int currentBlockSize = 0;
    private UInt32[] H = new UInt32[] { 0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19 };
    private ulong l = 0;
    private UInt32[] w = new UInt32[64];

    public override void Update(byte[] data)
    {
        l = (ulong)Buffer.ByteLength(data) * 8;
        if (currentBlockSize + Buffer.ByteLength(data) < 64)
        {
            data.CopyTo(blocks, currentBlockSize);
            currentBlockSize += Buffer.ByteLength(data);
            return;
        }

        int processed = 0;

        if (currentBlockSize > 0)
        {
            byte[] next = data.Take(64 - currentBlockSize).ToArray();
            next.CopyTo(blocks, currentBlockSize);
            process();
            processed += Buffer.ByteLength(next);
            currentBlockSize = 0;
        }

        while (processed + 64 <= Buffer.ByteLength(data))
        {
            byte[] next = data.Skip(processed).Take(processed + 64).ToArray();
            next.CopyTo(blocks, 0);
            process();
            processed += 64;
        }

        if (Buffer.ByteLength(data) - processed > 0)
        {
            byte[] remaining = data.Skip(processed).ToArray();
            remaining.CopyTo(blocks, 0);
            currentBlockSize = Buffer.ByteLength(remaining);
        }
    }

    public override byte[] Digest()
    {
        blocks[currentBlockSize] = 0x80;
        currentBlockSize += 1;

        if (64 - currentBlockSize < 8)
        {
            Utility.Fill<byte>(blocks, 0, currentBlockSize, Buffer.ByteLength(blocks)); // TODO: check
            process();
            currentBlockSize = 0;
        }

        Utility.Fill<byte>(blocks, 0, currentBlockSize, Buffer.ByteLength(blocks));
        Program.BigEndian.PutUint64(blocks, l, blockSize - 8);
        process();
        byte[] result = new byte[32];

        for (int i = 0; i < 8; i++)
            Program.BigEndian.PutUint32(result, H[i], i * 4);

        return result;
    }

    private void process()
    {
        for (int t = 0; t < 16; t++)
            w[t] = (uint)
             (blocks[t * 4 + 0] << 24
            | blocks[t * 4 + 1] << 16
            | blocks[t * 4 + 2] << 8
            | blocks[t * 4 + 3] >>> 0);

        for (int t = 16; t < 64; t++)
        {

            uint sigma1 = (Bits.Rotr32(w[t - 2], 17) ^ Bits.Rotr32(w[t - 2], 19) ^ (w[t - 2] >>> 10)) >>> 0;
            uint sigma0 = (Bits.Rotr32(w[t - 15], 7) ^ Bits.Rotr32(w[t - 15], 18) ^ (w[t - 15] >>> 3)) >>> 0;
            w[t] = (sigma1 + w[t - 7] + sigma0 + w[t - 16]) | 0;
        }

        uint a = H[0];
        uint b = H[1];
        uint c = H[2];
        uint d = H[3];
        uint e = H[4];
        uint f = H[5];
        uint g = H[6];
        uint h = H[7];

        for (int t = 0; t < 64; t++)
        {
            uint sigma1 = Bits.Rotr32(e, 6) ^ Bits.Rotr32(e, 11) ^ Bits.Rotr32(e, 25) >>> 0;
            uint ch = (e & f) ^ (~e & g) >>> 0;
            uint t1 = (h + sigma1 + ch + K[t] + w[t]) | 0;
            uint sigma0 = (Bits.Rotr32(a, 2) ^ Bits.Rotr32(a, 13) ^ Bits.Rotr32(a, 22)) >>> 0;
            uint maj = ((a & b) ^ (a & c) ^ (b & c)) >>> 0;
            uint t2 = (sigma0 + maj) | 0;
            h = g;
            g = f;
            f = e;
            e = (d + t1) | 0;
            d = c;
            c = b;
            b = a;
            a = (t1 + t2) | 0;
        }

        H[0] = (a + H[0]) | 0;
        H[1] = (b + H[1]) | 0;
        H[2] = (c + H[2]) | 0;
        H[3] = (d + H[3]) | 0;
        H[4] = (e + H[4]) | 0;
        H[5] = (f + H[5]) | 0;
        H[6] = (g + H[6]) | 0;
        H[7] = (h + H[7]) | 0;
    }
}

