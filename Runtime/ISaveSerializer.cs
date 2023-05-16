namespace FerthurSaver
{
    /// <summary>
    /// Serializer base class
    /// </summary>
    public interface ISaveSerializer
    {
        byte[] Serialize(SaveData saveData);
        SaveData Deserialize(byte[] bytes);
    }
}