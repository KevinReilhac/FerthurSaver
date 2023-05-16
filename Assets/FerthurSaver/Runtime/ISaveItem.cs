using System;

namespace FerthurSaver
{
    /// <summary>
    /// Inherit from this to override how the class is write in Save file
    /// </summary>
    public interface ISaveItem
    {
        string ToSaveText();
        static object FromSaveText(string stringData) {throw new NotImplementedException();}
    }
}
