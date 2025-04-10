namespace oslo.binary;

// TODO: add tests for put methods

public class BigEndian : ByteOrder
{
    public int Uint8(byte[] data, int offset)
    {
        if (Buffer.ByteLength(data) < offset + 1)
            throw new Exception("Insufficient bytes");

        return data[offset];
    }

    public int Uint16(byte[] data, int offset)
    {
        if (Buffer.ByteLength(data) < offset + 2)
            throw new Exception("Insufficient bytes");

        return (data[offset] << 8) | data[offset + 1];
    }

    public int Uint32(byte[] data, int offset)
    {
        if (Buffer.ByteLength(data) < offset + 4)
            throw new Exception("Insufficient bytes");

        int result = 0;

        for (int i = 0; i < 4; i++)
            result |= data[offset + i] << (i * 8);

        return result;
    }

    public long Uint64(byte[] data, int offset)
    {
        if (Buffer.ByteLength(data) < offset + 8)
            throw new Exception("Insufficient bytes");

        long result = 0;

        for (int i = 0; i < 8; i++)
            result |= (long)data[offset + i] << (56 - i * 8);

        return result;
    }

    public void PutUint8(byte[] target, byte value, int offset)
    {
        if (target.Length < 1 + offset)
            throw new Exception("Insufficient space");

        target[offset] = value;
    }

    public void PutUint16(byte[] target, UInt16 value, int offset)
    {
        if (target.Length < 2 + offset)
            throw new Exception("Insufficient space");

        target[offset] = (byte)(value >> 8);
        target[offset + 1] = (byte)(value & 0xff);
    }

    public void PutUint32(byte[] target, UInt32 value, int offset)
    {
        if (target.Length < 4 + offset)
            throw new Exception("Insufficient space");

        for (int i = 0; i < 4; i++)
            target[offset + i] = (byte)((value >> ((3 - i) * 8)) & 0xff);
    }

    public void PutUint64(byte[] target, UInt64 value, int offset)
    {
        if (target.Length < 8 + offset)
            throw new Exception("Insufficient space");

        for (int i = 0; i < 8; i++)
            target[offset + i] = (byte)(value >> ((7 - i) * 8) & 0xff);
    }
}

public class LittleEndian : ByteOrder
{
    public int Uint8(byte[] data, int offset)
    {
        if (Buffer.ByteLength(data) < offset + 1)
            throw new Exception("Insufficient bytes");

        return data[offset];
    }

    public int Uint16(byte[] data, int offset)
    {
        if (Buffer.ByteLength(data) < offset + 2)
            throw new Exception("Insufficient bytes");

        return data[offset] | (data[offset + 1] << 8);
    }

    public int Uint32(byte[] data, int offset)
    {
        if (Buffer.ByteLength(data) < offset + 4)
            throw new Exception("Insufficient bytes");

        int result = 0;

        for (int i = 0; i < 4; i++)
            result |= data[offset + i] << (i * 8);

        return result;
    }

    public long Uint64(byte[] data, int offset)
    {
        if (Buffer.ByteLength(data) < offset + 8)
            throw new Exception("Insufficient bytes");

        long result = 0;

        for (int i = 0; i < 8; i++)
            result |= (long)data[offset + i] << (i * 8);

        return result;
    }

    public void PutUint8(byte[] target, byte value, int offset)
    {
        if (target.Length < 1 + offset)
            throw new Exception("Insufficient space");

        target[offset] = value;
    }

    public void PutUint16(byte[] target, UInt16 value, int offset)
    {
        if (target.Length < 2 + offset)
            throw new Exception("Insufficient space");

        target[offset + 1] = (byte)(value >> 8);
        target[offset] = (byte)(value & 0xff);
    }

    public void PutUint32(byte[] target, UInt32 value, int offset)
    {
        if (target.Length < 4 + offset)
            throw new Exception("Insufficient space");

        for (int i = 0; i < 4; i++)
            target[offset + i] = (byte)((value >> (i * 8)) & 0xff);
    }

    public void PutUint64(byte[] target, UInt64 value, int offset)
    {
        if (target.Length < 8 + offset)
            throw new Exception("Insufficient space");

        for (int i = 0; i < 8; i++)
            target[offset + i] = (byte)(value >> (i * 8) & 0xff);
    }
}



interface ByteOrder
{
    int Uint8(byte[] data, int offset);
    int Uint16(byte[] data, int offset);
    int Uint32(byte[] data, int offset);
    long Uint64(byte[] data, int offset);
    void PutUint8(byte[] target, byte value, int offset);
    void PutUint16(byte[] target, UInt16 value, int offset);
    void PutUint32(byte[] target, UInt32 value, int offset);
    void PutUint64(byte[] target, UInt64 value, int offset);
}