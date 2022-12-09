//using Assets.Scripts.Factory.ReadDataMission;
using Assets.Scripts.Farm;
//using Assets.Scripts.Store;
//using Assets.Scripts.Town;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Assets.Scripts.Common
{
    public class MissionData
    {
        public static TargetCommon targetCommon;
        public static StarMission starMission;//Du lieu dieu kien tang sao (Dialog result)
        public static FarmDataMission farmDataMission;
        //public static FactoryDataMission factoryDataMission;
        //public static ShopDataMission shopDataMission;
        //public static TownDataMission townDataMission;
        public static List<NewItem> itemsInNew;
        public static string tip_en;//Text tip on xml
        public static string tip_vi;//Text tip on xml

        static ReadXML readXML;

        public static void READ_XML(int level)
        {
            if (level > 0)
            {
                readXML = new ReadXML("DataMission" + level, -1);
                targetCommon = new TargetCommon();
                targetCommon.Readxml(readXML.getDataByName("Target"));

                farmDataMission = new FarmDataMission();
                farmDataMission.getDataFromXML(readXML.getDataByName("Farm"));

                //factoryDataMission = new FactoryDataMission();
                //factoryDataMission.getDataFromXML(readXML.getDataByName("Factory"));

                //shopDataMission = new ShopDataMission();
                //shopDataMission.getDataFromXML(readXML.getDataByName("Shop"));

                //townDataMission = new TownDataMission();
                //townDataMission.GetDataFromXML(readXML.getDataByName("City"));

                //text tip
                tip_en = "";
                tip_vi = "";
                if (readXML.getDataByName("Tip") != null && readXML.getDataByName("Tip").Attributes["text"] != null)
                {
                    tip_vi = readXML.getDataByName("Tip").Attributes["text"].Value;
                }
                if (readXML.getDataByName("Tip") != null && readXML.getDataByName("Tip").Attributes["text1"] != null)
                {
                    tip_en = readXML.getDataByName("Tip").Attributes["text1"].Value;
                }
                //star mission
                starMission = new StarMission();
                if (readXML.getDataByName("Star") != null)
                {
                    starMission.Readxml(readXML.getDataByName("Star"));
                }
                itemsInNew = new List<NewItem>();
                if (readXML.getDataByName("NewItem") != null)
                {
                    foreach (XmlNode node in readXML.getDataByName("NewItem"))
                    {
                        NewItem tempNewItem = new NewItem();
                        tempNewItem.Readxml(node);
                        itemsInNew.Add(tempNewItem);
                    }
                }

            }
        }
    }

    public class TargetCommon : ItemAbstract
    {
        public int startMoney;
        public int targetMoney;
        public int maxTime;
        public int maxCustomer;
        public int targetCustomerRate;
        public List<int> itemsInShop;
        public int startScene;
        public TargetCommon()
        {
            startMoney = 0;
            targetMoney = 0;
            maxTime = 0;
            maxCustomer = 100;
            targetCustomerRate = 0;
            itemsInShop = new List<int>();
            startScene = 1;
        }
        public void Readxml(XmlNode node)
        {
            startMoney = Convert.ToInt32(node.Attributes["startMoney"].Value);
            currentNumber = startMoney;
            if (node.Attributes["targetMoney"] == null) targetMoney = 0;
            else targetMoney = Convert.ToInt32(node.Attributes["targetMoney"].Value);
            if (node.Attributes["timeMission"] != null)
                maxTime = Convert.ToInt32(node.Attributes["timeMission"].Value);
            if (node.Attributes["targetCustomerRate"] == null) targetCustomerRate = 0;
            else targetCustomerRate = Convert.ToInt32(node.Attributes["targetCustomerRate"].Value);

            if (node.Attributes["maxCustomer"] != null)
                maxCustomer = Convert.ToInt32(node.Attributes["maxCustomer"].Value);
            if (node.Attributes["item"] != null)
                getListItem(node.Attributes["item"].Value);
            if (node.Attributes["startScreen"] == null) startScene = 1;
            else startScene = Convert.ToInt32(node.Attributes["startScreen"].Value);
        }
        void getListItem(string val)
        {
            string[] tempItem = val.Split('-');
            for (int i = 0; i < tempItem.Length; i++) itemsInShop.Add(Convert.ToInt16(tempItem[i]));
        }

        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return targetCustomerRate;
            }
            else
            {
                return targetMoney;
            }
        }

        public override int getType()
        {
            return 0;
        }
    }

    //Class lay du lieu dieu kien tang sao cua moi mission
    public class StarMission
    {
        public long twoStar;
        public long threeStar;
        public int[] reward;

        public StarMission()
        {
            reward = new int[3];
            twoStar = 0;
            threeStar = 0;
        }

        public void Readxml(XmlNode node)
        {
            twoStar = Convert.ToInt32(node.Attributes["twoStar"].Value);
            threeStar = Convert.ToInt32(node.Attributes["threeStar"].Value);
            string[] str = node.Attributes["reward"].Value.Split('-');
            //Debug.Log("STR -- " + str.GetValue(0));
            for (int i = 0; i < 3; i++)
            {
                reward[i] = Convert.ToInt32(str[i]);
            }
            //Debug.Log("twostar  " + twoStar + " threestar " + threeStar + " reward " + reward[0] + " " + reward[1] + " " + reward[2]);
        }
    }

    //class get data of popup New Item .
    public class NewItem
    {
        public string type;
        public int id;
        public int level;
        public NewItem()
        {
            type = "";
            id = 0;
            level = 1;
        }

        public void Readxml(XmlNode node)
        {
            type = node.Attributes["type"].Value;
            id = Convert.ToInt16(node.Attributes["id"].Value);
            if (node.Attributes["level"] != null)
                level = Convert.ToInt16(node.Attributes["level"].Value);
        }
    }
}
