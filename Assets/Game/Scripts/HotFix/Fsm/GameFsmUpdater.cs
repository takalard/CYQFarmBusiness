using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameFsmUpdater
{
	private static bool _isRun = false;

	/// <summary>
	/// 开启初始化流程
	/// </summary>
	public static void Run()
	{
		if (_isRun == false)
		{
			_isRun = true;

			Debug.Log("开始游戏...");

			// 注意：按照先后顺序添加流程节点
			GameFsmManager.AddNode(new FsmLogin());
			GameFsmManager.AddNode(new FsmMain());

			//开始运行
			GameFsmManager.Run(nameof(FsmLogin));
		}
		else
		{
			Debug.LogWarning("游戏逻辑已经正在进行中!");
		}
	}

	/// <summary>
	/// 处理请求操作
	/// </summary>
	public static void HandleOperation(EPatchOperation operation)
	{
		//if (operation == EPatchOperation.BeginDownloadWebFiles)
		//{
		//	FsmManager.Transition(nameof(FsmDownloadWebFiles));
		//}
		//else if(operation == EPatchOperation.TryUpdateStaticVersion)
		//{
		//	FsmManager.Transition(nameof(FsmUpdateStaticVersion));
		//}
		//else if (operation == EPatchOperation.TryUpdatePatchManifest)
		//{
		//	FsmManager.Transition(nameof(FsmUpdateManifest));
		//}
		//else if (operation == EPatchOperation.TryDownloadWebFiles)
		//{
		//	FsmManager.Transition(nameof(FsmCreateDownloader));
		//}
		//else
		//{
		//	throw new NotImplementedException($"{operation}");
		//}
	}
}