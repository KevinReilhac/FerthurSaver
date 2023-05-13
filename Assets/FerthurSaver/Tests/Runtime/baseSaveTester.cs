using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

namespace FerthurSaver.Tests
{
    internal static class Helper
    {
        public static IEnumerator AsIEnumeratorReturnNull<T>(this Task<T> task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                ExceptionDispatchInfo.Capture(task.Exception).Throw();
            }

            yield return null;
        }
    }

    internal abstract class baseSaveTester
    {
        protected const string FILENAME = "TEMP";

        public abstract void Setup();

        [Test]
        public void SimpleValueTest()
        {
            Save.AddFeature<int>("TestInteger1", 666);
            Save.Set<string>("MyUninterestingString", "stringValue", createIfNotExist: true);

            Save.Get<int>("MyPatheticInt", defaultValue: -42);
            Save.WriteSave(FILENAME);
            Save.ReadSave(FILENAME);
            Assert.AreEqual(666, Save.Get<int>("TestInteger1").Value);
        }

    }
}