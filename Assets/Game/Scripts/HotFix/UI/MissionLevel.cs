using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using QFramework;

public class MissionLevel : MonoBehaviour 
{
    public int LevelId;
    public int x;
    public int y;
    public Text txtLevel;
    public Image imgSelected;

    
    private void Start()
    {
        
    }

    public void ItemMissionButton()
    {
        //DialogTask.complete = false;
        //AudioControl.DPlaySound("Click 1");
        ////Remove all item on Task Dialog
        //Transform dialogTask = GameObject.Find("DialogTask").transform;
        //dialogTask.GetComponent<DialogTask>().RemoveAllItem();

        //Debug.Log("READ LEVEL " + Level);
        //MissionData.READ_XML(Level);
        //GameObject.Find("UI Root").transform.Find("Mission").Find("Dialog").Find("DialogMission").gameObject.GetComponent<DialogMission>().ShowDialogMision(Level);
        //XUIKit.OpenPanel<>
        XUIKit.OpenPanel<DialogMission>((_View_) => {
            
        }, UILevel.PopUI, prefabName: "DialogMission");
    }
}
