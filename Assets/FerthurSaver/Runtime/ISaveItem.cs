using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FerthurSaver
{
    public interface ISaveItem
    {
        string ToSaveText();
        static object FromSaveText(string stringData) {throw new NotImplementedException();}
    }
}
