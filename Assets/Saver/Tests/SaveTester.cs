using System.Threading.Tasks;
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
    [SerializeField] public float bbb = 0.1f;
    [SerializeField] public float ccc = 100.1f;
}

public class SaveTester : MonoBehaviour
{
    [SerializeField] private string path;
    private async void Start()
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

        await Save.WriteSave("0");
        await Save.ReadSave("0");

        Feature<int> feature = Save.Get<int>("Integer", defaultValue: -1);
        Debug.Log(Save.Get<ClassA>("Complex").Value.b.ccc);
        Debug.Log(feature);
        Debug.Log(feature.Value);
    }


}
