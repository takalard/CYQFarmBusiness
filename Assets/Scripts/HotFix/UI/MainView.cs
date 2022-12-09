/****************************************************************************
 * Copyright (c) 2018.3 vin129
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System;
using System.Collections;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class MainViewData : IUIData
{
	// TODO: Query
}

public partial class MainView : UIPanel
{
	//[SerializeField]
	//private GameObject goGlow;

	private Button Btn_Home;
	private Button Btn_Achievement;
	private Button Btn_Notice;
	private Button Btn_FreeGem;

	[SerializeField]
	private Text Txt_ButtonClick;

	protected override void OnInit(IUIData uiData = null)
	{
		List<string> btnsName = new List<string>();
		btnsName.Add("ButtonList/Btn_Home");
		btnsName.Add("ButtonList/Btn_Achievement");
		btnsName.Add("ButtonList/Btn_Notice");
		btnsName.Add("ButtonList/Btn_FreeGem");
		foreach (string btnName in btnsName)
        {
			GameObject btnObj = GameObject.Find(btnName);
			Button btn = btnObj.GetComponent<Button>();
			btn.onClick.AddListener(delegate () {
				this.OnButtonClick(btnObj);
			});
		}
		//// 注册事件
		//RegisterEvent(UIEventID.MenuPanel.ChangeMenuColor);
		UpdateMissionLevels();
	}

	private void UpdateMissionLevels()
    {
		//打印所有坐标
		var tbMissions = JsonConfigManager.Config["Missions"];
		//var rowMissions = tbMissions[1];
		//Debug.Log("rowMissions ==== MapId = " + rowMissions["MapId"] + " Levels = " + rowMissions["Levels"]);
		foreach(var rowMission in tbMissions)
		{
			Debug.Log("rowMissions ==== MapId = " + rowMission["MapId"] + " Levels = " + rowMission["Levels"]);
			//var 
		}

	}

	protected override void ProcessMsg(int eventId, QMsg msg)
	{
		//switch (eventId)
		//{
		//	// 处理事件
		//	case (int) UIEventID.MenuPanel.ChangeMenuColor:
		//		Debug.LogFormat("{0}:Process EventId {1}", Transform.name, eventId);
		//		var color = default(Color);
		//		ColorUtility.TryParseHtmlString("#00FFFFFF", out color);
		//		ImageBg.color = color;
		//		break;
		//}
	}

	protected override void OnClose()
	{

	}

	private void OnButtonClick(GameObject sender)
    {
		Debug.Log("MainView ## OnButtonClick # sender.name = "+sender.name);
		this.Txt_ButtonClick.text = sender.name+" Button Clicked.";
    }
}