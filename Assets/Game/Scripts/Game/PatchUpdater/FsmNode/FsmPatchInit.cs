using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

internal class FsmPatchInit : IFsmNode
{
	public string Name { private set; get; } = nameof(FsmPatchInit);

	void IFsmNode.OnEnter()
	{
		// 加载更新面板
		//var go = Resources.Load<GameObject>("UI_Patch");
		//var newGo = GameObject.Instantiate(go);
		//newGo.transform.parent = Boot.Instance.UIRoot.transform;
		//newGo.transform.localPosition = new Vector3(0,0,0);

		UIKit.OpenPanel<PatchView>(prefabName: "PatchView");

		Boot.Instance.StartCoroutine(Begin());
		
	}
	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}

	private IEnumerator Begin()
	{
		yield return new WaitForSecondsRealtime(0.5f);

		FsmManager.Transition(nameof(FsmUpdateStaticVersion));
	}
}