using System.IO;
using UnityEngine;

public class RemoteConfig
{
    string configPath = "JsonConfigs";
    string defaultConfigPath = "defaultConfig";
    Config loadedDataDefault;

    public RemoteConfig()
    {
        LoadGameData();

        //Debug.Log(GetDefault<float>("testfloat"));
        //Debug.Log(GetDefault<string>("test"));
        //Debug.Log(GetDefault<float>("test"));
    }

    private void LoadGameData() //using jsonutility
    {
        TextAsset defaultConfig = Resources.Load<TextAsset>(Path.Combine(configPath, defaultConfigPath));
        Debug.Log(defaultConfig.text);

        if (defaultConfig)
            loadedDataDefault = JsonUtility.FromJson<Config>(defaultConfig.text);
        else
            Debug.LogError("Cannot load game data!");
    }

    public T GetDefault<T>(string key)
    {
        try
        {
            return loadedDataDefault.GetValue<T>(key);
        }
        catch
        {
            return default(T);
        }
    }

    class Config
    {
        public T GetValue<T>(string key)
        {
            return default(T);
        }
    }
}