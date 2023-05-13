using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SideRift.SaveSystem
{
    public class AesSaveEncryptor : ISaveEncryptor
    {
        private AesCryptoServiceProvider _aes = null;

        public AesSaveEncryptor(string IV, string Key)
        {
            _aes = new AesCryptoServiceProvider();

            _aes.BlockSize = 128;
            _aes.KeySize = 256;
            _aes.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            _aes.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            _aes.Padding = PaddingMode.PKCS7;
            _aes.Mode = CipherMode.CBC;
        }

        public AesSaveEncryptor() : this("Juzy4gcymkJP6HZE", "xjmfsEkN0WVG423i")
        {
            Debug.LogWarning("This AesSaveEncryptor constructor only exists for testing purposes.");
        }

        public AesSaveEncryptor(AesCryptoServiceProvider aes)
        {
            this._aes = aes;
        }

        public byte[] Decript(byte[] encryptedBytes)
        {
            ICryptoTransform icrypt = _aes.CreateDecryptor(_aes.Key, _aes.IV);
            byte[] dec = icrypt.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            icrypt.Dispose();

            return dec;
        }

        public byte[] Encrypt(byte[] bytes)
        {
            ICryptoTransform icrypt = _aes.CreateEncryptor(_aes.Key, _aes.IV);
            byte[] enc = icrypt.TransformFinalBlock(bytes, 0, bytes.Length);
            icrypt.Dispose();

            return (enc);
        }
    }

}