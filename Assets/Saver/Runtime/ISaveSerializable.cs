using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SideRift.SaveSystem
{
    public interface ISaveItem
    {
        string ToSaveText();
        static object FromSaveText(string stringData) {throw new NotImplementedException();}
    }
}
