using Newtonsoft.Json;
using System;
using UnityEngine;

using System.IO;
public class ConfigManager
{
    public static readonly ConfigManager inst = new ConfigManager();
    Config _config;

    public void loadConfig(Action callback)
    {
        var txtAsset = Resources.Load<TextAsset>("config");
        // StreamReader sr = new StreamReader(Application.dataPath + "/Resources/config");
        string json = txtAsset.text;//sr.ReadToEnd();
        // this._config = JsonUtility.FromJson<Config>(json);
        this._config = JsonConvert.DeserializeObject<Config>(json);
        // var p = Resources.LoadAsync("config",Type.j);
        // var config = JsonConvert.DeserializeObject(p.asset.text);
        Debug.Log(this._config.shop);
        Debug.Log("配置加载完成======================");
        callback?.Invoke();
    }
    public CShop getShopConfig()
    {
        return this._config.shop;
    }
    public CGame getGameConfig()
    {
        return this._config.game;
    }

    public CMap getMapConfig()
    {
        return this._config.map;
    }
}