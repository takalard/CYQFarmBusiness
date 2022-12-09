using System;
using System.Xml;

namespace Assets.Scripts.Farm
{
    public class Breed
    {
        //this class cover all information of one plant/animal.
        public int idBreed;
        public string nameBreed;
        public float timeGrowUp;
        public float maxtimeGrowUp;
        public int Yield;
        public string status;
        public int stage;
        public int price;
        public float timeHealing;

        public Breed()
        {
            idBreed = 0;
            this.nameBreed = "";
            timeGrowUp = 0;
            maxtimeGrowUp = 10;
            status = "normal";
            stage = 1;
            price = 0;
            timeHealing = 5;
        }
        public Breed(int index)
        {
            idBreed = index;
            this.nameBreed = getName(index);
            timeGrowUp = 0;
            maxtimeGrowUp = 10;
            status = "normal";
            stage = 1;
            price = 0;
            timeHealing = 5;
        }
        public Breed(int index, int level, XmlNode data, bool isSick = false, bool isHarvest = true)
        {
            idBreed = index;
            this.nameBreed = getName(index);
            setData(level, data);
            timeGrowUp = maxtimeGrowUp;
            status = "normal";
            this.stage = 3;
            timeHealing = 5;
            if (isSick) status = "sick";
            if (!isHarvest)
            {
                stage = 2;
                timeGrowUp = maxtimeGrowUp / 2;
            }
        }

        public static string getName(int id)
        {
            switch (id)
            {
                case 1: return "wheat";
                case 2: return "tomato";
                case 3: return "grapes";
                case 4: return "strawberry";
                case 5: return "chicken";
                case 6: return "pig";
                case 7: return "cow";
                case 8: return "fish";
                default: return "shrimp";
            }
        }
        public void setData(int level, XmlNode node)
        {
            //string[] temp = node.Attributes["yield"].Value.Split('-');
            //Yield = Convert.ToInt16(temp[level - 1]);
            //maxtimeGrowUp = Convert.ToInt16(node.Attributes["timeGrowup"].Value);
            //price = Convert.ToInt16(node.Attributes["price"].Value);
            //int discount = 0;
            //if (idBreed < 5 && DialogShop.BoughtItem[0]) maxtimeGrowUp *= 0.8f;//Decrease 20% growing time of seeds
            //if (idBreed >= 5 && DialogShop.BoughtItem[1]) maxtimeGrowUp *= 0.8f;//Decrease 20% growing time of cattle
            //if (MissionPowerUp.PriceDrop) discount += 25;
            //if (DialogShop.BoughtItem[5]) discount += 10;
            //price = (int)(price * (1 - discount / 100f));
        }
    }
}
