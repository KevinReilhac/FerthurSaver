
# Ferthur Saver

Save and load things.. in files.. probably not your best choice.


## Features

- Save things
- Load things
- Encrypt/Decript your files
- Implement your own encryptors and serializers (probably better than mine)


## Authors

- [@Kévin "Kebab" Reilhac](https://www.github.com/KevinReilhac)


## Installation

Install FerthurSaver with Unity Package Manager

```bash
  https://github.com/KevinReilhac/FerthurSaver.git#upm
```
    
## Usage/Examples

### Initalization

```csharp
//Pretty Json save file, no encryption
Save.Initialize(path, new JsonUtilitySaveSerializer(prettyPrint: true), null);

//Binary save file with aes encryption
string IV 	= "Wow Very Crypt";
string Key 	= "This is so secure";

Save.Initialize(path, new BinarySaveSerializer(), new AesSaveEncryptor(IV, Key));
```

### Set data
```csharp
Save.AddFeature<int>("MyPatheticInt", 666);
Save.AddFeature<float>("MyAnnoyingFloat", 6,9);
//OR
Save.Set<string>("MyUninterestingString", "stringValue");
```

### Get data
```csharp
Feature<int> intFeature = Save.Get<int>("MyPatheticInt", defaultValue: -42);
Debug.Log(intFeature.Value); // -> "666"
```

### Write/Read Save
```csharp
    Save.WriteSave("SaveName");
    Save.ReadSave("SaveName");

    //OR
    await Save.WriteSaveAsync("SaveName");
    await Save.ReadSaveAsync("SaveName");

    //Events
    Save.onWriteAsyncStart += Stuff();
    Save.onWriteAsyncComplete += OtherStuff();
```
## Documentation

[Read Documentation](https://kevinreilhac.github.io/FerthurSaver/)
____________________


I am not responsible for any use of this module, find a better one.