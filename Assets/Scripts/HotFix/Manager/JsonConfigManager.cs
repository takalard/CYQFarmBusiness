using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CatLib.Json;
using UnityEngine;

public class JsonConfigManager: Dictionary<string,JsonConfig>
{
    private static readonly Lazy<JsonConfigManager> lazy =
 new Lazy<JsonConfigManager>(() => new JsonConfigManager());
    public static JsonConfigManager Config { get { return lazy.Value; } }

    public bool isLoadDone { get; private set; }

    public void InitConfig()
    {
        isLoadDone = false;
        this.Clear();

        var operationHandler = YooAsset.YooAssets.LoadAssetSync<TextAsset>("ConfigFileList");
        var assetData = (operationHandler.AssetObject as TextAsset);
        var jsonConfigs = LitJson.JsonMapper.ToObject<List<string>>(assetData.text);
        LoadConfig(jsonConfigs);
    }

    

    private void LoadConfig(List<string> JsonConfigs)
    {
        for (int i = 0; i < JsonConfigs.Count; i++)
        {
            var CfgName = JsonConfigs[i];
            //var assetLoader = m_AssetRequestAgent.LoadAsset(libx.GameResources.GetConfigPath($"JsonConfig/{CfgName}.json.txt"));
            var operationHandler = YooAsset.YooAssets.LoadAssetSync<TextAsset>(CfgName);
            var assetData = (operationHandler.AssetObject as TextAsset);
            this[CfgName] = LitJson.JsonMapper.ToObject<JsonConfig>(assetData.text);
        }
        isLoadDone = true;
    }

    public JsonConfig GetConfig(string ConfigName)
    {
        return this[ConfigName];
    }
}