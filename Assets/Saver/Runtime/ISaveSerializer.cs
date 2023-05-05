using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SideRift.SaveSystem
{
    public interface ISaveSerializer
    {
        Task Write(SaveData saveData, string fullFilePath);
        Task<SaveData> Read(string fullFilePath);
    }
}