using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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