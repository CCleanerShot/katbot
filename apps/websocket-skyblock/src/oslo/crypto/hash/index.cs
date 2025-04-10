namespace oslo.crypto.hash;

public abstract class Hash
{
    public int blockSize;
    public int size;

    public abstract void Update(byte[] data);
    public abstract byte[] Digest();
}