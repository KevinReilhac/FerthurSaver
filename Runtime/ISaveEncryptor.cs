using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveEncryptor
{
    byte[] Encrypt(byte[] bytes);
    byte[] Decript(byte[] encryptedBytes);
}
