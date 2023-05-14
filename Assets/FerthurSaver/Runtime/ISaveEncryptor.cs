
/// <summary>
/// Enryptor base class
/// </summary>
public interface ISaveEncryptor
{
    byte[] Encrypt(byte[] bytes);
    byte[] Decript(byte[] encryptedBytes);
}
