using System;
using System.Collections.Generic;
using System.Xml;

namespace Assets.Scripts.Farm
{
    public class FarmDataMission
    {
        public bool isCanSick;
        public List<FieldFarm> fieldFarms;
        public List<BreedFarm> breedsFarm;
        public HarverstFarm harvestField, harvestCage;

        FieldFarm tempField;
        BreedFarm tempBreed;
        int countField, countBreed, temp;

        public void getDataFromXML(XmlNode node)
        {
            countBreed = 0;
            countField = 0;
            fieldFarms = new List<FieldFarm>();
            breedsFarm = new List<BreedFarm>();
            harvestField = new HarverstFarm(1);
            harvestCage = new HarverstFarm(2);
            if (node.Attributes["fail"] != null)
                isCanSick = Convert.ToBoolean(node.Attributes["fail"].Value);
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name.Equals("Harvest"))
                {
                    temp = Convert.ToInt16(node.ChildNodes[i].Attributes["id"].Value);
                    if (temp == 1) harvestField.targetNumber = Convert.ToInt16(node.ChildNodes[i].Attributes["targethaverst"].Value);
                    else harvestCage.targetNumber = Convert.ToInt16(node.ChildNodes[i].Attributes["targethaverst"].Value);
                }
                else if (node.ChildNodes[i].Name.Equals("Field"))
                {
                    tempField = new FieldFarm(Convert.ToInt16(node.ChildNodes[i].Attributes["id"].Value));
                    tempField.index = countField;
                    countField++;
                    tempField.startNumber = Convert.ToInt16(node.ChildNodes[i].Attributes["startNumber"].Value);
                    tempField.currentNumber = tempField.startNumber;
                    if (node.ChildNodes[i].Attributes["targetNumber"] != null)
                        tempField.targetNumber = Convert.ToInt16(node.ChildNodes[i].Attributes["targetNumber"].Value);
                    if (node.ChildNodes[i].Attributes["startLevel"] != null)
                    {
                        tempField.startLevel = Convert.ToInt16(node.ChildNodes[i].Attributes["startLevel"].Value);
                        tempField.currentLevel = tempField.startLevel;
                    }
                    if (node.ChildNodes[i].Attributes["targetLevel"] != null)
                        tempField.targetLevel = Convert.ToInt16(node.ChildNodes[i].Attributes["targetLevel"].Value);
                    if (node.ChildNodes[i].Attributes["maxLevel"] != null)
                        tempField.maxLevel = Convert.ToInt16(node.ChildNodes[i].Attributes["maxLevel"].Value);
                    if (tempField.maxLevel < tempField.targetLevel) tempField.maxLevel = tempField.targetLevel;

                    fieldFarms.Add(tempField);
                }
                else
                {
                    tempBreed = new BreedFarm(Convert.ToInt16(node.ChildNodes[i].Attributes["id"].Value));
                    tempBreed.index = countBreed;
                    countBreed++;
                    if (node.ChildNodes[i].Attributes["targetPlant"] != null)
                        tempBreed.targetNumber = Convert.ToInt16(node.ChildNodes[i].Attributes["targetPlant"].Value);
                    if (node.ChildNodes[i].Attributes["startNumber"] != null)
                    {
                        tempBreed.startNumber = Convert.ToInt16(node.ChildNodes[i].Attributes["startNumber"].Value);
                        tempBreed.currentNumber = 0;
                    }
                    breedsFarm.Add(tempBreed);
                }
            }
        }
    }

    public class FieldFarm : ItemAbstract
    {
        public int idField;//1 ruong, 2 chuong, 3 ao
        public int startNumber;
        public int targetNumber;
        public int startLevel;
        public int targetLevel;
        public int maxLevel;
        public FieldFarm(int id, int startN, int targetN, int startL, int targetL, int maxLV)
        {
            idField = id;
            startNumber = startN;
            targetNumber = targetN;
            startLevel = startL;
            targetLevel = targetL;
            maxLevel = maxLV;
            currentNumber = startNumber;
            currentLevel = startLevel;
        }
        public FieldFarm(int id)
        {
            idField = id;
            startNumber = 0;
            targetNumber = 0;
            startLevel = 1;
            targetLevel = 1;
            maxLevel = 1;
            currentNumber = startNumber;
            currentLevel = startLevel;
        }

        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return targetLevel;
            }
            else
            {
                return targetNumber;
            }
        }
        public override int getType()
        {
            return idField;
        }

    }

    public class BreedFarm : ItemAbstract
    {
        public int idBreed;
        public int startNumber;
        public int targetNumber;
        public BreedFarm(int id)
        {
            idBreed = id;
            startNumber = 0;
            targetNumber = 0;
        }
        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return 0;
            }
            else
            {
                return targetNumber;
            }
        }
        public override int getType()
        {
            return idBreed;
        }
    }
    public class HarverstFarm : ItemAbstract
    {
        public int idField;
        public int targetNumber;
        public HarverstFarm(int id)
        {
            idField = id;
            targetNumber = 0;
            currentNumber = 0;
        }
        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return 0;
            }
            else
            {
                return targetNumber;
            }
        }
        public override int getType()
        {
            return idField;
        }
    }
}
