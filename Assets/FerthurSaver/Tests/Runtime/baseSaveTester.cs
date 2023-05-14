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
    [System.Serializable]
    internal class Complex
    {
        public Complex(int intValue, float floatValue, Vector3 vectorValue)
        {
            this.intValue = intValue;
            this.floatValue = floatValue;
            this.complexValue = new Complex2(vectorValue);
        }
        public int intValue;
        public float floatValue;
        public Complex2 complexValue;
    }

    [System.Serializable]
    internal class Complex2
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;

        public Complex2(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 vector => new Vector3(x, y, z);
    }

    internal abstract class baseSaveTester
    {
        protected const string FILENAME = "TEMP";
        private bool isInitialized = false;

        [SetUp]
        public abstract void Setup();

        [Test]
        public virtual void SimpleValue()
        {
            Save.AddFeature<int>("TestInteger1", 666);

            Save.WriteSave(FILENAME);
            Save.ReadSave(FILENAME);
            Assert.AreEqual(666, Save.Get<int>("TestInteger1").Value);
        }

        [Test]
        public void ComplexValue()
        {
            Complex complex = new Complex(66, 6.8f, new Vector3(1f, 2f, 3f));

            Save.AddFeature<Complex>("complex", complex);
            Save.WriteSave(FILENAME);
            Save.ReadSave(FILENAME);

            Feature<Complex> complexFeature = Save.Get<Complex>("complex");

            Assert.NotNull(complexFeature);
            Assert.AreEqual(complexFeature.Value.intValue, complex.intValue);
            Assert.AreEqual(complexFeature.Value.floatValue, complex.floatValue);
            Assert.NotNull(complexFeature.Value.complexValue);
            Assert.AreEqual(complexFeature.Value.complexValue.vector, complex.complexValue.vector);
        }

        private const int DATA_SET_LENGHT = 10000;
        [Test]
        public void BigDataSet()
        {
            int[] integers = new int[DATA_SET_LENGHT];

            for (int i = 0; i < DATA_SET_LENGHT; i++)
                integers[i] = Random.Range(int.MinValue, int.MaxValue);
            Save.AddFeature("BigDataSet", integers);
            Save.WriteSave(FILENAME);
            Save.ReadSave(FILENAME);
            Feature<int[]> readedIntegers = Save.Get<int[]>("BigDataSet");
            Assert.AreEqual(integers, readedIntegers.Value);
        }
    }
}