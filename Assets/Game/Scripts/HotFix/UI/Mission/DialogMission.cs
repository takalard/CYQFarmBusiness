using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogMissionData : IUIData
{
    // TODO: Query
}

public class DialogMission : UIPanel
{
    // Start is called before the first frame update
    protected override void OnInit(IUIData uiData = null)
    {



    }
    protected override void ProcessMsg(int eventId, QMsg msg)
    {

    }

    protected override void OnClose()
    {

    }

    //OnStartGame
    public void OnBtnStartClick(GameObject sender)
    {
        //LoadingScene.ShowLoadingScene("Farm", true);
        XUIKit.OpenPanel<FarmView>((_View_) => {
            XUIKit.ClosePanel<DialogMission>();
            XUIKit.ClosePanel<MainView>();
        }, prefabName: "FarmView");

    }
}
