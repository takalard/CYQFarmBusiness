﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class FsmUpdateManifest : IFsmNode
{
	public string Name { private set; get; } = nameof(FsmUpdateManifest);

	void IFsmNode.OnEnter()
	{
		PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.UpdateManifest);
		Boot.Instance.StartCoroutine(UpdateManifest());
	}
	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}

	private IEnumerator UpdateManifest()
	{
		yield return new WaitForSecondsRealtime(0.5f);

		// 更新补丁清单
		var package = YooAssets.GetAssetsPackage("DefaultPackage");
		var operation = package.UpdateManifestAsync(PatchUpdater.PackageVersion, 30);
		yield return operation;

		if(operation.Status == EOperationStatus.Succeed)
		{
			FsmManager.Transition(nameof(FsmCreateDownloader));
		}
		else
		{
			Debug.LogWarning(operation.Error);
			PatchEventDispatcher.SendPatchManifestUpdateFailedMsg();
		}
	}
}