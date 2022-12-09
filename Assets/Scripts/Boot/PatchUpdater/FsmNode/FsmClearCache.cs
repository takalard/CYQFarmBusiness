using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HybridCLR;
using UnityEngine;
using YooAsset;

internal class FsmClearCache : IFsmNode
{
	public string Name { private set; get; } = nameof(FsmClearCache);

	public static List<string> AOTMetaAssemblyNames { get; } = new List<string>()
	{
		"mscorlib.dll",
		"System.dll",
		"System.Core.dll",
	};

	private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

	public static byte[] GetAssetData(string dllName)
	{
		return s_assetDatas[dllName];
	}

	void IFsmNode.OnEnter()
	{
		Debug.Log("清理未使用的缓存文件！");
		var operation = YooAsset.YooAssets.ClearUnusedCacheFiles();
		operation.Completed += Operation_Completed;
	}

	private void Operation_Completed(YooAsset.AsyncOperationBase obj)
	{
		//加载dll
		//YooAsset.YooAssets.LoadSceneAsync("MainScene");
		var assets = new List<string>
		{
			"HotFix.dll",
		}.Concat(AOTMetaAssemblyNames);


		foreach (var asset in assets)
        {
			//AssetObject
			var operation = YooAssets.LoadAssetSync<TextAsset>(asset);
			var assetBytes = (operation.AssetObject as TextAsset).bytes;
			s_assetDatas[asset] = assetBytes;
			Debug.Log($"dll:{asset}  size:{assetBytes.Length}");
		}

		LoadMetadataForAOTAssemblies();

#if !UNITY_EDITOR
        System.Reflection.Assembly.Load(GetAssetData("HotFix.dll"));
#endif

		YooAsset.YooAssets.LoadSceneAsync("MainScene");

	}

	/// <summary>
	/// Îªaot assembly¼ÓÔØÔ­Ê¼metadata£¬ Õâ¸ö´úÂë·Åaot»òÕßÈÈ¸üÐÂ¶¼ÐÐ¡£
	/// Ò»µ©¼ÓÔØºó£¬Èç¹ûAOT·ºÐÍº¯Êý¶ÔÓ¦nativeÊµÏÖ²»´æÔÚ£¬Ôò×Ô¶¯Ìæ»»Îª½âÊÍÄ£Ê½Ö´ÐÐ
	/// </summary>
	private static void LoadMetadataForAOTAssemblies()
	{
		/// ×¢Òâ£¬²¹³äÔªÊý¾ÝÊÇ¸øAOT dll²¹³äÔªÊý¾Ý£¬¶ø²»ÊÇ¸øÈÈ¸üÐÂdll²¹³äÔªÊý¾Ý¡£
		/// ÈÈ¸üÐÂdll²»È±ÔªÊý¾Ý£¬²»ÐèÒª²¹³ä£¬Èç¹ûµ÷ÓÃLoadMetadataForAOTAssembly»á·µ»Ø´íÎó
		/// 
		HomologousImageMode mode = HomologousImageMode.SuperSet;
		foreach (var aotDllName in AOTMetaAssemblyNames)
		{
			byte[] dllBytes = GetAssetData(aotDllName);
			// ¼ÓÔØassembly¶ÔÓ¦µÄdll£¬»á×Ô¶¯ÎªËühook¡£Ò»µ©aot·ºÐÍº¯ÊýµÄnativeº¯Êý²»´æÔÚ£¬ÓÃ½âÊÍÆ÷°æ±¾´úÂë
			LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
			Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
		}
	}

	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}
}