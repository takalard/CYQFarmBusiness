using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

internal class FsmLogin : IFsmNode
{
	public string Name { private set; get; } = nameof(FsmLogin);

	void IFsmNode.OnEnter()
	{
		Debug.Log("进入游戏登录流程！");
		//var operation = YooAsset.YooAssets.LoadSceneAsync("MainScene");
		//operation.Completed += OnCompleted;

		XUIKit.OpenPanel<LoginView>((_LoginView_) => {
			Debug.Log("FsmLogin ## 33333333333333333333 ## OnCompleted #");
			XUIKit.ClosePanel<PatchView>();
		}, prefabName: "LoginView");
	}

	private void OnCompleted(YooAsset.SceneOperationHandle obj)
	{
		//GameFsmManager.Transition(nameof(FsmLogin));

	}

	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}
}