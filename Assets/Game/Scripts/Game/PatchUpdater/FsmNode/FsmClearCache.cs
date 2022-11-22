using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class FsmClearCache : IFsmNode
{
	public string Name { private set; get; } = nameof(FsmClearCache);

	void IFsmNode.OnEnter()
	{
		Debug.Log("清理未使用的缓存文件！");
		var operation = YooAsset.YooAssets.ClearUnusedCacheFiles();
		operation.Completed += Operation_Completed;
	}

	private void Operation_Completed(YooAsset.AsyncOperationBase obj)
	{
		FsmManager.Transition(nameof(FsmLogin));
	}

	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}
}