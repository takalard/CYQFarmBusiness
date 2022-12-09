using Assets.Scripts.Common;
using Assets.Scripts.Farm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmCenterScript : MonoBehaviour
{
    //是否暂停
    public bool isPause = false;

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

    void Awake()
    {
        // see if we've got farmobject still playing
        GameObject[] farmObject = GameObject.FindGameObjectsWithTag("FarmObject");
        if (farmObject.Length > 1)
        {
            for (int i = 1; i < farmObject.Length; i++)
            {
                Destroy(farmObject[i]);
            }
        }
        else
        {
            idNeedUpdate = -1;
            for (int i = 0; i < 22; i++)
            {
                if (i < 3)
                    fieldFarms[i] = new FieldFarm(i + 1);
                if (i < 9)
                    breedFarms[i] = new BreedFarm(i + 1);
                listDatas[i] = new Breed();
            }
        }
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        elementPlantDataXML = new ReadXML("Farm/XMLFile/ElementFarm");
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
        SetListMachineHaved();

        //if (DialogShop.BoughtItem[10]) isCansick = false;
    }
    // Update is called once per frame
    void Update()
    {
        //if (!(CommonObjectScript.isEndGame || CommonObjectScript.isGuide))
        //{
        //    foreach (FieldFarm fieldFarm in MissionData.farmDataMission.fieldFarms)
        //    {
        //        fieldFarms[fieldFarm.idField - 1].currentNumber = fieldFarm.currentNumber;
        //        //print(fieldFarms[fieldFarm.idField - 1].currentLevel + " so với " + fieldFarm.currentLevel);
        //        if (fieldFarms[fieldFarm.idField - 1].currentLevel != fieldFarm.currentLevel)
        //        {
        //            #region upgrade Level Farm
        //            fieldFarms[fieldFarm.idField - 1].currentLevel = fieldFarm.currentLevel;
        //            idNeedUpdate = 1;
        //            if (fieldFarm.idField == 1)
        //            {
        //                for (int i = 0; i < 12; i++) if (isOpenCells[i] && listDatas[i].idBreed > 0) listDatas[i].Yield += 2;
        //            }
        //            else if (fieldFarm.idField == 2)
        //            {
        //                for (int i = 12; i < 18; i++) if (isOpenCells[i] && listDatas[i].idBreed > 0) listDatas[i].Yield += 2;
        //            }
        //            else
        //            {
        //                for (int i = 18; i < 22; i++) if (isOpenCells[i] && listDatas[i].idBreed > 0) listDatas[i].Yield += 2;
        //            }
        //            #endregion
        //        }
        //    }
        //    isNeedWarning = false;
        //    for (int i = 0; i < 22; i++)
        //    {
        //        if (isOpenCells[i])
        //            if (listDatas[i].status.Equals("sick_fix"))
        //            {
        //                #region healing
        //                listDatas[i].timeHealing -= Time.deltaTime;
        //                if (listDatas[i].timeHealing < 0)
        //                {
        //                    listDatas[i].timeHealing = 5f;
        //                    listDatas[i].status = "normal";
        //                    idNeedUpdate = i;
        //                }
        //                #endregion
        //            }
        //            else if (listDatas[i].idBreed != 0)
        //            {
        //                if (listDatas[i].stage == 3 || listDatas[i].status.Equals("sick"))
        //                {
        //                    isNeedWarning = true;
        //                }
        //                else
        //                {
        //                    #region grow up
        //                    listDatas[i].timeGrowUp += Time.deltaTime;
        //                    if (Mathf.Abs(listDatas[i].maxtimeGrowUp / 4 - listDatas[i].timeGrowUp) <= Time.deltaTime / 2)
        //                    {
        //                        randSick(i);
        //                    }
        //                    else if (Mathf.Abs(listDatas[i].maxtimeGrowUp * 3 / 4 - listDatas[i].timeGrowUp) <= Time.deltaTime / 2)
        //                    {
        //                        randSick(i);
        //                    }
        //                    else if (listDatas[i].timeGrowUp >= listDatas[i].maxtimeGrowUp)
        //                    {
        //                        listDatas[i].stage = 3;
        //                        if (DialogShop.BoughtItem[7])//auto harvest
        //                        {
        //                            if (Application.loadedLevelName.Equals("Farm")) idNeedUpdate = i;
        //                            else
        //                            {
        //                                //auto increase yeild
        //                                CommonObjectScript.arrayMaterials[listDatas[i].idBreed - 1] += listDatas[i].Yield;
        //                                MissionData.farmDataMission.breedsFarm[breedFarms[listDatas[i].idBreed - 1].index].currentNumber++;
        //                                if (listDatas[i].idBreed < 5) MissionData.farmDataMission.harvestField.currentNumber++;
        //                                else MissionData.farmDataMission.harvestCage.currentNumber++;
        //                                listDatas[i] = new Breed();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            idNeedUpdate = i;
        //                            if (!Application.loadedLevelName.Equals("Farm"))
        //                            {
        //                                common.WarningVisible(CommonObjectScript.Button.Farm);
        //                            }
        //                            if (VariableSystem.language == null || VariableSystem.language.Equals("Vietnamese"))
        //                                textHarvest = new WarningTextView("Thu hoạch ở nông trại !", 8);
        //                            else
        //                                textHarvest = new WarningTextView("Harvest farm !", 8);
        //                        }
        //                    }
        //                    else if (listDatas[i].timeGrowUp / listDatas[i].maxtimeGrowUp >= 0.5 && listDatas[i].stage == 1)
        //                    {
        //                        listDatas[i].stage = 2;
        //                        randSick(i);
        //                        idNeedUpdate = i;
        //                    }
        //                    #endregion
        //                }
        //            }
        //    }
        //}
        //if (Application.loadedLevelName.Equals("Mission"))//mission complete
        //{
        //    GameObject.Destroy(this.gameObject);
        //}
    }

    public void randSick(int index)
    {
        //if (isCansick)
        //{
        //    randValue = Random.Range(0, 1250) % 100;
        //    if (randValue < 7)
        //    {
        //        listDatas[index].status = "sick";
        //        if (!Application.loadedLevelName.Equals("Farm"))
        //        {
        //            common.WarningVisible(CommonObjectScript.Button.Farm);
        //        }
        //        if (VariableSystem.language == null || VariableSystem.language.Equals("Vietnamese"))
        //            textHarvest = new WarningTextView("Dịch bệnh ở nông trại !", 5);
        //        else
        //            textHarvest = new WarningTextView("Disease in farm !", 5);
        //        idNeedUpdate = index;
        //    }
        //}
    }
    public IEnumerator GrowUp(int indexPopup)
    {
        yield return new WaitForSeconds(0.5f);

        //if (listDatas[indexPopup].idBreed != 0)
        //{
        //    GameObject.FindGameObjectWithTag("CommonObject").GetComponent<CommonObjectScript>().AddDiamond(0);//update diamond
        //    listDatas[indexPopup].timeGrowUp = listDatas[indexPopup].maxtimeGrowUp;
        //    listDatas[indexPopup].stage = 3;
        //    listDatas[indexPopup].status = "normal";
        //    if (DialogShop.BoughtItem[7])//auto harvest
        //    {
        //        if (Application.loadedLevelName.Equals("Farm")) idNeedUpdate = indexPopup;
        //        else
        //        {
        //            //auto increase yeild
        //            CommonObjectScript.arrayMaterials[listDatas[indexPopup].idBreed - 1] += listDatas[indexPopup].Yield;

        //            MissionData.farmDataMission.breedsFarm[breedFarms[listDatas[indexPopup].idBreed - 1].index].currentNumber++;
        //            if (listDatas[indexPopup].idBreed < 5) MissionData.farmDataMission.harvestField.currentNumber++;
        //            else MissionData.farmDataMission.harvestCage.currentNumber++;
        //            listDatas[indexPopup] = new Breed();
        //        }
        //    }
        //    else
        //    {
        //        if (!Application.loadedLevelName.Equals("Farm"))
        //        {
        //            common.WarningVisible(CommonObjectScript.Button.Farm);
        //        }
        //        if (VariableSystem.language == null || VariableSystem.language.Equals("Vietnamese"))
        //            textHarvest = new WarningTextView("Thu hoạch ở nông trại !", 8);
        //        else
        //            textHarvest = new WarningTextView("Harvest farm !", 8);
        //        idNeedUpdate = indexPopup;
        //    }
        //}
    }

    //Code Of Hungbv
    void SetListMachineHaved()
    {
        //if (FactoryScenesController.ListMachineHaved == null)
        //{
        //    FactoryScenesController.ListMachineHaved = new List<int>();
        //    for (int countListMachinedatas = 0; countListMachinedatas < MissionData.factoryDataMission.machinedatas.Count; countListMachinedatas++)
        //    {
        //        if (MissionData.factoryDataMission.machinedatas[countListMachinedatas].startNumber != 0)
        //        {
        //            for (int countStartNumber = 0; countStartNumber < MissionData.factoryDataMission.machinedatas[countListMachinedatas].startNumber; countStartNumber++)
        //            {
        //                if (MissionData.factoryDataMission.machinedatas[countListMachinedatas].maxLevel > MissionData.factoryDataMission.machinedatas[countListMachinedatas].startLevel)
        //                    FactoryScenesController.ListMachineHaved.Add(MissionData.factoryDataMission.machinedatas[countListMachinedatas].iDMachine);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    FactoryScenesController.ListMachineHaved.Clear();
        //    for (int countListMachinedatas = 0; countListMachinedatas < MissionData.factoryDataMission.machinedatas.Count; countListMachinedatas++)
        //    {
        //        if (MissionData.factoryDataMission.machinedatas[countListMachinedatas].startNumber != 0)
        //        {
        //            for (int countStartNumber = 0; countStartNumber < MissionData.factoryDataMission.machinedatas[countListMachinedatas].startNumber; countStartNumber++)
        //            {
        //                if (MissionData.factoryDataMission.machinedatas[countListMachinedatas].maxLevel > MissionData.factoryDataMission.machinedatas[countListMachinedatas].startLevel)
        //                    FactoryScenesController.ListMachineHaved.Add(MissionData.factoryDataMission.machinedatas[countListMachinedatas].iDMachine);
        //            }
        //        }
        //    }
        //}
    }
}
