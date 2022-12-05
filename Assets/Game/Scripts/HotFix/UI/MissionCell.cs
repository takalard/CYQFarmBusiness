using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MissionCell : MonoBehaviour 
{
    public Image ImgBackground;
    public Text text;
    // public LayoutElement element;
    // public static float[] randomWidths = new float[3] { 100, 150, 50 };
    public GameObject levelsRoot;
    void ScrollCellIndex(int idx)
    {
        string name = "Cell " + idx.ToString();
        string imgName = "bgmission" + (idx+1).ToString();
        if (text != null)
        {
            text.text = name;
        }
        // element.preferredWidth = randomWidths[Mathf.Abs(idx) % 3];
        gameObject.name = name;

        //ImgBackground
        UIUtils.SetSprite(ImgBackground, imgName);

        var tbMissions = JsonConfigManager.Config["Missions"];
        //var rowMissions = tbMissions[1];
        //Debug.Log("rowMissions ==== MapId = " + rowMissions["MapId"] + " Levels = " + rowMissions["Levels"]);
        //foreach (var rowMission in tbMissions)
        //{
        //    Debug.Log("rowMissions ==== MapId = " + rowMission["MapId"] + " Levels = " + rowMission["Levels"]);
        //    //var 
        //}
        var MapId = idx + 1;
        if(tbMissions.Contains(MapId))
        {
            var rowMission = tbMissions[MapId];
            Debug.Log("rowMissions ==== MapId = " + rowMission["MapId"] + " Levels = " + rowMission["Levels"]);
            //List<Object> lstLevels = LitJson.JsonMapper.ToObject<List<Object>>(rowMission["Levels"]);
            var LevelData = rowMission["Levels"].ToString();
            ////var jsonData = LitJson.JsonMapper.ToJson(LevelData);
            //var jsonData = new LitJson.JsonData(LevelData);
            //foreach(var data in jsonData)
            //{
            //    Debug.Log("rowMissions ==== data = " + data.ToString());
            //}
            ////var lstLevel = LitJson.JsonMapper.ToObject<List<MissionLevel>>(LevelData);
            ////for (var i = 0; i < lstLevels.Count; i++)
            ////{
            ////    var strJson = lstLevels[i].ToString();
            ////    Debug.Log("MissionCell lstLevels[" + i + "] = " + lstLevels[i]);
            ////}

            //string pattern = @"(\[\{)(-)0-9(\}\])";
            //var matched = Regex.Match(LevelData.ToString(),pattern,RegexOptions.IgnoreCase);
            //string pattern2 = @"(\{).*(\})";
            //MatchCollection matches = Regex.Matches(matched.Value.Replace("[","").Replace("]",""),pattern2, RegexOptions.IgnoreCase);

            List<MissionLevel> lstLevels = new List<MissionLevel>();

            string pattern = @"(\{\d+,-?\d+,-?\d+\})+";
            MatchCollection matches = Regex.Matches(LevelData, pattern, RegexOptions.IgnoreCase);
            foreach (var m in matches)
            {
                var strContent = m.ToString();
                Debug.Log("m === "+ strContent);
                //string subPattern = @"(-?\d+)+";
                //MatchCollection subMatches = Regex.Matches(m.ToString(), subPattern, RegexOptions.IgnoreCase);
                //foreach (var subM in subMatches)
                //{
                //    Debug.Log("subM === " + subM.ToString());
                //}

                var strSub = strContent.Substring(1, strContent.Length - 2);
                Debug.Log("strSub === " + strSub);
                var strArray = strSub.Split(',');
                if (strArray.Length < 3) continue;
                var missionLevel = new MissionLevel();
                missionLevel.LevelId = int.Parse(strArray[0]);
                missionLevel.x = int.Parse(strArray[1]);
                missionLevel.y = int.Parse(strArray[2]);
                lstLevels.Add(missionLevel);
            }
            var levelChildren = this.levelsRoot.GetComponentsInChildren<MissionLevel>(true);
            foreach(var msLevel in levelChildren)
            {
                msLevel.LevelId = 0;
                msLevel.gameObject.SetActive(false);
            }
            Debug.Log("levelChildren.Count === " + levelChildren.Length+ " lstLevels.Count = "+ lstLevels.Count);
            for (var i=0; i<lstLevels.Count; i++)
            {
                if(i < levelChildren.Length && levelChildren[i].LevelId == 0)
                {
                    levelChildren[i].LevelId = lstLevels[i].LevelId;
                    levelChildren[i].x = lstLevels[i].x;
                    levelChildren[i].y = lstLevels[i].y;
                    levelChildren[i].transform.localPosition = new Vector3(levelChildren[i].x, levelChildren[i].y,0);
                    levelChildren[i].txtLevel.text = levelChildren[i].LevelId.ToString();
                    levelChildren[i].imgSelected.gameObject.SetActive(false);
                    levelChildren[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
