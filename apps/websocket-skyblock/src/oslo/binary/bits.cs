namespace oslo.binary;

public static class Bits
{
    public static uint Rotl32(uint x, int n)
    {
        return ((x << n) | (x >>> (32 - n))) >>> 0;
    }

    public static ulong Rotl64(UInt64 x, int n)
    {
        return ((x << n) | (x >> 64 - n)) & 0xffffffffffffffff;
    }

    public static uint Rotr32(uint x, int n)
    {
        return ((x << (32 - n)) | (x >>> n)) >>> 0;
    }

    public static ulong Rotr64(UInt64 x, int n)
    {
        return ((x << 64 - n) | (x >> n)) & 0xffffffffffffffff;
    }
}