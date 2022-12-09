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
using Assets.Scripts.Farm;
using Assets.Scripts.Common;

public class FarmViewData : IUIData
{
	// TODO: Query
	public Breed[] listDatas = new Breed[22];
	public bool[] isOpenCells = new bool[22];
	public FieldFarm[] fieldFarms = new FieldFarm[3];
	public BreedFarm[] breedFarms = new BreedFarm[9];
	public List<int> idBreedsSorted;
	public int idNeedUpdate;
	// Use this for initialization
	public int countElementField, countElementCage1, countElementCage2;
	int tempField, tempCage1, tempCage2;
	//public CommonObjectScript common;
	public ReadXML elementPlantDataXML;

	//public WarningTextView textHarvest;

	private int randValue;
	bool isCansick;

	public static bool isNeedWarning;

    public FarmViewData()
    {
        DoInitialize();
    }

	private void DoInitialize()
    {
		for (int i = 0; i < 22; i++)
		{
			if (i < 3)
				fieldFarms[i] = new FieldFarm(i + 1);
			if (i < 9)
				breedFarms[i] = new BreedFarm(i + 1);
			listDatas[i] = new Breed();
		}


        //elementPlantDataXML = new ReadXML("Farm/XMLFile/ElementFarm");
        elementPlantDataXML = new ReadXML("ElementFarm");
        //common = GameObject.FindGameObjectWithTag("CommonObject").GetComponent<CommonObjectScript>();
        isCansick = true;
        bool isWarning = false;
        idBreedsSorted = new List<int>();
        #region read data in mission
        countElementField = 0; countElementCage1 = 0; countElementCage2 = 0;
        tempField = 0; tempCage1 = 0; tempCage2 = 0;
        isCansick = MissionData.farmDataMission.isCanSick;
        foreach (FieldFarm fieldFarm in MissionData.farmDataMission.fieldFarms)
        {
            fieldFarms[fieldFarm.idField - 1].startNumber = fieldFarm.startNumber;
            fieldFarms[fieldFarm.idField - 1].targetNumber = fieldFarm.targetNumber;
            fieldFarms[fieldFarm.idField - 1].startLevel = fieldFarm.startLevel;
            fieldFarms[fieldFarm.idField - 1].targetLevel = fieldFarm.targetLevel;
            fieldFarms[fieldFarm.idField - 1].maxLevel = fieldFarm.maxLevel;
            fieldFarms[fieldFarm.idField - 1].currentNumber = fieldFarm.currentNumber;
            fieldFarms[fieldFarm.idField - 1].currentLevel = fieldFarm.currentLevel;
        }
        #region đặt trước cây để làm guide
        //if (VariableSystem.mission == 4)
        //{
        //    listDatas[tempField] = new Breed(MissionData.farmDataMission.breedsFarm[0].idBreed, fieldFarms[0].currentLevel, elementPlantDataXML.getDataByValue("id", MissionData.farmDataMission.breedsFarm[0].idBreed.ToString()), false, false);
        //    tempField++;
        //}
        //if (VariableSystem.mission == 38)
        //{
        //    listDatas[tempField] = new Breed(MissionData.farmDataMission.breedsFarm[0].idBreed, fieldFarms[0].currentLevel, elementPlantDataXML.getDataByValue("id", MissionData.farmDataMission.breedsFarm[0].idBreed.ToString()), true, false);
        //    tempField++;
        //}
        #endregion
        foreach (BreedFarm breedFarm in MissionData.farmDataMission.breedsFarm)
        {
            idBreedsSorted.Add(breedFarm.idBreed);
            if (breedFarm.idBreed < 5)
            {
                countElementField++;
                if (breedFarm.startNumber != 0)
                {
                    for (int i = 0; i < breedFarm.startNumber; i++)
                    {
                        listDatas[tempField + i] = new Breed(breedFarm.idBreed, fieldFarms[0].currentLevel, elementPlantDataXML.getDataByValue("id", breedFarm.idBreed.ToString()));
                    }
                    tempField += breedFarm.startNumber;
                    isWarning = true;
                }
            }
            else if (breedFarm.idBreed < 8)
            {
                countElementCage1++;
                if (breedFarm.startNumber != 0)
                {
                    for (int i = 0; i < breedFarm.startNumber; i++)
                    {
                        listDatas[tempCage1 + i + 12] = new Breed(breedFarm.idBreed, fieldFarms[1].currentLevel, elementPlantDataXML.getDataByValue("id", breedFarm.idBreed.ToString()));
                    }
                    tempCage1 += breedFarm.startNumber;
                    isWarning = true;
                }
            }
            else
            {
                countElementCage2++;
                if (breedFarm.startNumber != 0)
                {
                    for (int i = 0; i < breedFarm.startNumber; i++)
                    {
                        listDatas[tempCage2 + i + 18] = new Breed(breedFarm.idBreed, fieldFarms[2].currentLevel, elementPlantDataXML.getDataByValue("id", breedFarm.idBreed.ToString()));
                    }
                    tempCage2 += breedFarm.startNumber;
                    isWarning = true;
                }
            }
            breedFarms[breedFarm.idBreed - 1] = breedFarm;
        }
        #endregion
        idBreedsSorted.Sort();
        // print(isWarning.ToString());
        if (isWarning)
        {
            //if (MissionData.targetCommon.startScene != 1)
            //    common.WarningVisible(CommonObjectScript.Button.Farm);

            //textHarvest = new WarningTextView("Harvest farm !", 8);
        }

        #region IsOpenCells
        for (int i = 0; i < 22; i++)
        {
            isOpenCells[i] = false;
            if (i < 12)
            {
                if (i < fieldFarms[0].startNumber) isOpenCells[i] = true;
            }
            else if (i < 18)
            {
                if (i - 12 < fieldFarms[1].startNumber) isOpenCells[i] = true;
            }
            else
            {
                if (i - 18 < fieldFarms[2].startNumber) isOpenCells[i] = true;
            }
        }
        #endregion


        //Code of HungBV
        //SetListMachineHaved();

    }
}

public partial class FarmView : UIPanel
{
	//[SerializeField]
	//private GameObject goGlow;

	public FarmViewData theData;

	private Button Btn_Home;
	private Button Btn_Achievement;
	private Button Btn_Notice;
	private Button Btn_FreeGem;

	[SerializeField]
	private Text Txt_ButtonClick;

	public GameObject[] listObjects;//list object in game

	string[] tempNames = new string[] { "dat", "chuong", "chuongca" };
	Transform tempObject;//temp object to get Object

	protected override void OnInit(IUIData uiData = null)
	{
		//theData = uiData as FarmViewData ?? new FarmViewData();
        theData = uiData as FarmViewData;
        Debug.Log("theData ===== "+theData);
        // List<string> btnsName = new List<string>();
        // btnsName.Add("ButtonList/Btn_Home");
        // btnsName.Add("ButtonList/Btn_Achievement");
        // btnsName.Add("ButtonList/Btn_Notice");
        // btnsName.Add("ButtonList/Btn_FreeGem");
        // foreach (string btnName in btnsName)
        //       {
        // 	GameObject btnObj = GameObject.Find(btnName);
        // 	Button btn = btnObj.GetComponent<Button>();
        // 	btn.onClick.AddListener(delegate () {
        // 		this.OnButtonClick(btnObj);
        // 	});
        // }
        // //// 注册事件
        // //RegisterEvent(UIEventID.MenuPanel.ChangeMenuColor);
        // UpdateMissionLevels();
        UpdateImgAllCell();
	}

	private void UpdateImgAllCell()// update image for all the cell wwhen player come back Farm.
	{
		for (int i = 0; i < 22; i++)
		{
            if (!theData.isOpenCells[i])
            {
				#region not open cell
				if (i < 12)
					UIUtils.SetSprite(listObjects[i].GetComponent<Image>(), tempNames[0] + "0");
				else if (i < 18)
				{
					UIUtils.SetSprite(listObjects[i].GetComponent<Image>(), tempNames[1] + "0");
					tempObject = listObjects[i].transform.Find("wattle");
					tempObject.GetComponent<Image>().enabled = false;
				}
				else UIUtils.SetSprite(listObjects[i].GetComponent<Image>(), tempNames[2] + "0");
				#endregion
			}
            else
            {
                if (i < 12)
                {
                    #region field
                    //listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[0] + farmCenter.fieldFarms[0].currentLevel;
					UIUtils.SetSprite(listObjects[i].GetComponent<Image>(), tempNames[0] + theData.fieldFarms[0].currentLevel);
					#endregion
				}
                else
                {
                    #region img cell
                    if (i < 18)
                    {
                        #region cage
						UIUtils.SetSprite(listObjects[i].GetComponent<Image>(), tempNames[1] + theData.fieldFarms[1].currentLevel + "-0");
						tempObject = listObjects[i].transform.Find("wattle");
						UIUtils.SetSprite(tempObject.GetComponent<Image>(), tempNames[1] + theData.fieldFarms[1].currentLevel + "-1");

						#endregion
					}
                    else
                    {
                        #region pond
						UIUtils.SetSprite(listObjects[i].GetComponent<Image>(), tempNames[2] + theData.fieldFarms[2].currentLevel);
						#endregion

					}
                    #endregion
                }
                UpdateImgCell(i);
            }
        }
	}

    private void UpdateImgCell(int i)// update image for one cell.
    {
        /* Update one field
             * if exist breed is planted: If harvested => destroy object. If explaned => change Img cell
             * If not exist breed is planted: if change state => change animation. If new plant => create object
             */
        if (i < 0 || !theData.isOpenCells[i]) { print("Error parameter : " + i); return; }
        if (i < 12)
        {
            #region field
            tempObject = listObjects[i].transform.Find("plant");
            if (theData.listDatas[i].idBreed == 0)
            {
                if (tempObject != null) GameObject.Destroy(tempObject.gameObject);
                else if (theData.isOpenCells[i])
                {
                    //listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[0] + farmCenter.fieldFarms[0].currentLevel;
                    UIUtils.SetSprite(listObjects[i].GetComponent<Image>(), tempNames[0] + theData.fieldFarms[0].currentLevel);
                }
            }
            else
            {
                if (tempObject != null)
                {
                    #region change animation
                    //UISprite spriteOfPlant = tempObject.GetComponentInChildren<UISprite>();
                    //spriteOfPlant.spriteName = farmCenter.listDatas[i].nameBreed + farmCenter.listDatas[i].stage.ToString();
                    UIUtils.SetSprite(tempObject.GetComponent<Image>(), theData.listDatas[i].nameBreed + theData.listDatas[i].stage.ToString());
                    //if (theData.listDatas[i].stage == 1)
                    //{
                    //    spriteOfPlant.width = 175;
                    //    spriteOfPlant.height = 95;
                    //}
                    //else if (theData.listDatas[i].stage == 2)
                    //{
                    //    spriteOfPlant.width = 190;
                    //    spriteOfPlant.height = 120;
                    //}
                    //else
                    //{
                    //    spriteOfPlant.width = 200;
                    //    spriteOfPlant.height = 130;
                    //}
                    tempObject.GetComponent<Animator>().Play("stage" + theData.listDatas[i].stage);
                    //setStatusSick(i, farmCenter.listDatas[i].status, spriteOfPlant);
                    #endregion
                }
                else
                {
                    create_Plant(i, theData.listDatas[i].nameBreed, theData.listDatas[i].stage);
                }
            }
            #endregion
        }
        else if (i < 18)
        {
            //#region cage
            //tempObject = listObjects[i].transform.Find("animal0");
            //if (farmCenter.listDatas[i].idBreed == 0)
            //{
            //    if (tempObject != null)
            //    {
            //        Animator[] temps = listObjects[i].transform.GetComponentsInChildren<Animator>();
            //        foreach (Animator temp in temps)
            //        {
            //            GameObject.Destroy(temp.gameObject);
            //        }
            //    }
            //    else
            //    {
            //        tempObject = listObjects[i].transform.Find("wattle");
            //        listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[1] + farmCenter.fieldFarms[1].currentLevel + "-0";
            //        tempObject.GetComponent<UISprite>().enabled = true;
            //        tempObject.GetComponent<UISprite>().spriteName = tempNames[1] + farmCenter.fieldFarms[1].currentLevel + "-1";
            //    }
            //}
            //else
            //{
            //    if (tempObject != null)
            //    {
            //        for (int j = 0; j < farmCenter.listDatas[i].Yield / 2; j++)
            //        {
            //            tempObject = listObjects[i].transform.Find("animal" + j);
            //            if (tempObject == null)
            //            {
            //                create_Animal(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage, farmCenter.listDatas[i].Yield / 2, true);
            //            }
            //            else
            //            {
            //                tempObject.GetComponent<Animator>().Play("stage" + (farmCenter.listDatas[i].stage == 3 ? "2" : "1"));
            //                setStatusSick(i, farmCenter.listDatas[i].status, tempObject.GetComponentsInChildren<UISprite>());
            //            }
            //        }
            //    }
            //    else
            //    {
            //        create_Animal(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage, farmCenter.listDatas[i].Yield / 2);
            //    }
            //}
            //#endregion
        }
        else
        {
            //#region pond
            //tempObject = listObjects[i].transform.Find("animal0");
            //if (farmCenter.listDatas[i].idBreed == 0)//after harvest
            //{
            //    if (tempObject != null)
            //    {
            //        Animator[] temps = listObjects[i].transform.GetComponentsInChildren<Animator>();
            //        foreach (Animator temp in temps)
            //        {
            //            GameObject.Destroy(temp.gameObject);
            //        }
            //    }
            //    else listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[2] + farmCenter.fieldFarms[2].currentLevel;
            //}
            //else
            //{
            //    if (tempObject != null)
            //    {
            //        for (int j = 0; j < farmCenter.listDatas[i].Yield / 2; j++)
            //        {
            //            tempObject = listObjects[i].transform.Find("animal" + j);
            //            if (tempObject == null)
            //            {
            //                create_Animal(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage, farmCenter.listDatas[i].Yield / 2, true);
            //            }
            //            else
            //            {
            //                tempObject.GetComponent<Animator>().Play("stage" + (farmCenter.listDatas[i].stage == 3 ? "2" : "1"));
            //                setStatusSick(i, farmCenter.listDatas[i].status, tempObject.GetComponentsInChildren<UISprite>());
            //            }
            //        }
            //    }
            //    else
            //    {
            //        create_Animal(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage, farmCenter.listDatas[i].Yield / 2);
            //    }
            //}
            //#endregion
        }
        //if (farmCenter.listDatas[i].stage == 3 && DialogShop.BoughtItem[7])
        //{
        //    #region auto harvest
        //    CreateAnimationHarvest(farmCenter.listDatas[i].idBreed + "." + farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].Yield, i);
        //    CreateAnimationAddValue(farmCenter.listDatas[i].idBreed + "." + farmCenter.listDatas[i].nameBreed, "+" + farmCenter.listDatas[i].Yield, true, i);
        //    CommonObjectScript.arrayMaterials[farmCenter.listDatas[i].idBreed - 1] += farmCenter.listDatas[i].Yield;
        //    MissionData.farmDataMission.breedsFarm[farmCenter.breedFarms[farmCenter.listDatas[i].idBreed - 1].index].currentNumber++;
        //    if (farmCenter.listDatas[i].idBreed < 5) MissionData.farmDataMission.harvestField.currentNumber++;
        //    else MissionData.farmDataMission.harvestCage.currentNumber++;
        //    farmCenter.listDatas[i] = new Breed();
        //    CommonObjectScript.audioControl.PlaySound("Chat hang");
        //    if (indexPopup == i)
        //    {
        //        animatorKhungitem.Play("CloseKhungItem");
        //        CollapseFrame_Timer();
        //        indexPopup = -1;
        //    }
        //    UpdateImgCell(i);
        //    #endregion
        //}
    }

    private void create_Plant(int index, string name, int stage)//create one plant
    {
        Debug.Log("create_Plant index = " + index+" name = "+name);
        //plant = (GameObject)Instantiate(plantPrefabs);
        //plant.name = "plant";
        //plant.transform.parent = listObjects[index].transform;
        //plant.GetComponent<Transform>().localPosition = Vector3.zero;
        //plant.GetComponent<Transform>().localScale = Vector3.one;
        //UISprite spriteOfPlant = plant.GetComponentInChildren<UISprite>();
        //spriteOfPlant.spriteName = name + stage.ToString();
        //if (stage == 1)
        //{
        //    spriteOfPlant.width = 175;
        //    spriteOfPlant.height = 95;
        //}
        //else if (stage == 2)
        //{
        //    spriteOfPlant.width = 190;
        //    spriteOfPlant.height = 120;
        //}
        //else
        //{
        //    spriteOfPlant.width = 200;
        //    spriteOfPlant.height = 130;
        //}
        //setRenderer(spriteOfPlant, index);
        //setStatusSick(index, farmCenter.listDatas[index].status, spriteOfPlant);
        //plant.GetComponent<Animator>().Play("stage" + stage);
    }


    private void UpdateMissionLevels()
    {
		// //打印所有坐标
		// var tbMissions = JsonConfigManager.Config["Missions"];
		// //var rowMissions = tbMissions[1];
		// //Debug.Log("rowMissions ==== MapId = " + rowMissions["MapId"] + " Levels = " + rowMissions["Levels"]);
		// foreach(var rowMission in tbMissions)
		// {
		// 	Debug.Log("rowMissions ==== MapId = " + rowMission["MapId"] + " Levels = " + rowMission["Levels"]);
		// 	//var 
		// }

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
		// this.Txt_ButtonClick.text = sender.name+" Button Clicked.";
    }
}