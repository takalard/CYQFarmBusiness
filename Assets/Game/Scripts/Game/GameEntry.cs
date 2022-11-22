using System;
using System.Collections;
using UnityEngine;
using YooAsset;
using System.IO;
using UnityEngine.Rendering.Universal;
using QFramework;

public class GameEntry : MonoBehaviour
{
	public static GameEntry Instance { private set; get; }

	//[SerializeField]
	//public GameObject UIRoot = null;

	void Awake()
	{
		// Instance = this;
		// DontDestroyOnLoad(this);

		// Application.targetFrameRate = 60;
		// Application.runInBackground = true;
	}
	void OnGUI()
	{
		//GUIConsole.OnGUI();
	}
	void OnDestroy()
	{
		Instance = null;
	}
	void Update()
	{
		//EventManager.Update();
		//FsmManager.Update();
	}

	void Start()
	{
		//GamePlayMode = PlayMode;
		//Debug.Log($"资源系统运行模式：{PlayMode}");
		//var uiCamera = //Boot.Instance.
		//Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(Boot.Instance.UICamera);
		//加载UI
		//异步方法
		//UIKit.OpenPanel<LoginView>((_UIMenuPanel)=> {
		//	Debug.Log("GameEntry ## 33333333333333333333 ## OnCompleted #");
		//	UIKit.ClosePanel<PatchView>();
		//},prefabName:"LoginView");
	}

	private void OnHandleGame()
	{
	}

	// private string GetHostServerURL()
	// {
	// 	//string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
	// 	string hostServerIP = "http://127.0.0.1";
	// 	string gameVersion = "v1.0";

// #if UNITY_EDITOR
// 		if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
// 			return $"{hostServerIP}/CDN/Android/{gameVersion}";
// 		else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
// 			return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
// 		else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
// 			return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
// 		else
// 			return $"{hostServerIP}/CDN/PC/{gameVersion}";
// #else
// 		if (Application.platform == RuntimePlatform.Android)
// 			return $"{hostServerIP}/CDN/Android/{gameVersion}";
// 		else if (Application.platform == RuntimePlatform.IPhonePlayer)
// 			return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
// 		else if (Application.platform == RuntimePlatform.WebGLPlayer)
// 			return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
// 		else
// 			return $"{hostServerIP}/CDN/PC/{gameVersion}";
// #endif
	// }
}