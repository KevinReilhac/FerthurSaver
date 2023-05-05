using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SideRift.SaveSystem;

[System.Serializable]
public class ClassA
{
    public ClassB b = new ClassB();
    public int test = -444;
    [SerializeField] public Guid guid = Guid.NewGuid();
}

[System.Serializable]
public class ClassB
{
    [SerializeField] public float aaa = 0.1f;
    [SerializeField] public System.Numerics.Vector3 vector = new System.Numerics.Vector3(5, 7, 666);
    [SerializeField] public Quaternion quat = new Quaternion(0f, 4f, 10f, 70f);
}

public class SaveTester : MonoBehaviour
{
    [SerializeField] private string path;
    private void Awake()
    {
        Save.Initialize(new string[] {}, path);

        Save.AddFeature<int>("Integer", 666);
        Save.AddFeature<int>("Integer_2", 123);
        Save.AddFeature<int>("Integer_3", 456);
        Save.AddFeature<int>("Integer_4", 789);
        Save.AddFeature<bool>("Bool_1", false);
        Save.AddFeature<bool>("Bool_2", true);
        Save.AddFeature<float>("Float_0", 6.798f);
        Save.AddFeature<ClassA>("Complex", new ClassA());
        Save.WriteSave(0);
    }


}
