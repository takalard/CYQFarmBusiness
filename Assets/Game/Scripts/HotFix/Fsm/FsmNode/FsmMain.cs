using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

internal class FsmMain : IFsmNode
{
	public string Name { private set; get; } = nameof(FsmMain);

	void IFsmNode.OnEnter()
	{
		Debug.Log("进入游戏主任务流程！");
		UIKit.OpenPanel<MainView>((_MainView_) => {
			UIKit.ClosePanel<LoginView>();
		}, prefabName: "MainView");

		//FsmManager.Transition(nameof(FsmMission));
	}
	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}
}