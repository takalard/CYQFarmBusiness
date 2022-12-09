using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

//Class luu mission current
public class CurrentMission
{
    public string FB_id { get; set; }
    public string Name { get; set; }
    public string Mission { get; set; }

    public CurrentMission(string id, string name, string mission)
    {
        this.FB_id = id;
        this.Name = name;
        this.Mission = mission;
    }

    public CurrentMission()
    {
        this.FB_id = "";
        this.Name = "";
        this.Mission = "";
    }
}

public class MissionDataSave
{
    public int Mission;
    public long Score;
    public int Star;
    public int Open;//0-false. 1-true

    public MissionDataSave(int mission, long score, int star, int open)
    {
        Mission = mission;
        Score = score;
        Star = star;
        Open = open;
    }

    public MissionDataSave()
    {
        Mission = 0;
        Score = 0;
        Star = 0;
        Open = 0;
    }
}

public class AchievementCache
{
    //group nhiem vu
    public int Group;
    //Level hien tai cua group
    public int Level;
    //Gia tri hien tai
    public int Value;
    //Thong bao mission hoan thanh
    public int Notify;//0 - False, 1 - true
    //public AchievementCache()
    //{
    //    this.Group = 1;
    //    this.Level = 1;
    //    this.Value = 0;
    //}

    public AchievementCache(int group, int level, int value, int notify)
    {
        this.Group = group;
        this.Value = value;
        this.Level = level;
        this.Notify = notify;
    }
    public AchievementCache()
    {
        this.Group = 1;
        this.Value = 0;
        this.Level = 1;
        this.Notify = 0;
    }
}

public class DataCache
{
    public static string FB_ID = "FB_ID";
    public static string FB_USER = "FB_USER";
    public static string Achievement_data_key = "Achievement_data_key";
    public static string Mission_data_key = "Mission_data_key";
    public static string Current_mission_data_key = "Current_mission_data_key";

    public static AchievementCache[] dataAchievementCache;
    public static MissionDataSave[] dataMissionCache;
    public static CurrentMission[] dataCurrentMissionCache;
    //public static string XML_Current_Mission_Path = "CurrentMissionSave.xml";
    //public static string XML_Data_Mission_Path = "DataMissionSave.xml";
    //public static string XML_Data_Achievement_Path = "AchievementCache.xml";

    //serialize xml theo tung phan tu

    //public static List<MissionDataSave> DeserializeMissionDataSaveListFromXML(string filePath)
    //{
    //    if (!System.IO.File.Exists(filePath))
    //    {
    //        Debug.LogError("File " + filePath + " not exist!");
    //        return new List<MissionDataSave>();
    //    }
    //    XmlSerializer deserializer = new XmlSerializer(typeof(List<MissionDataSave>), new XmlRootAttribute("MissionDataSaveRoot"));
    //    TextReader textReader = new StreamReader(filePath);
    //    List<MissionDataSave> movies = (List<MissionDataSave>)deserializer.Deserialize(textReader);
    //    textReader.Close();
    //    return movies;
    //}

    //public static void readXMLTest()
    //{
    //    string xmlDataCache1 = Application.persistentDataPath + "/" + XML_Current_Mission_Path;
    //    TextReader textReader = new StreamReader(xmlDataCache1);
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.Load(textReader);
    //    XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
    //    Debug.Log("TRUOC");
    //    foreach (XmlNode node in xmlNodeList)
    //    {
    //        Debug.Log("aaaaaaaaaaaaaaaaaaaaa " + node.Attributes["Id"].Value);
    //    }
    //    Debug.Log("SAU");
    //    XmlNode root = xmlDoc.DocumentElement;
    //    XmlElement elem = xmlDoc.CreateElement("CurrentMissionCache");
    //    elem.SetAttribute("Id", "112312");
    //    elem.SetAttribute("Name", "NameDG");
    //    elem.SetAttribute("Mission", "MissionDG");
    //    root.AppendChild(elem);
    //    textReader.Close();
    //    xmlDoc.Save(xmlDataCache1);
    //}

    //Add mission xml node
    public static void UpdateMissionScore(long score, int star, int mission, int open)
    {
        MissionDataSave data = dataMissionCache[mission - 1];
        if (data.Star < star)
        {
            data.Star = star;
        }
        if (data.Score < score)
        {
            data.Score = score;
        }
        data.Open = open;
    }

    public static void SaveMissionDataCache(bool submitToServer = false)
    {
        string dataSave = "";
        string dataSendServer = "";
        for (int i = 0; i < dataMissionCache.Length; i++)
        {
            dataSave += dataMissionCache[i].Mission + "-" + dataMissionCache[i].Score + "-" + dataMissionCache[i].Star + "-" + dataMissionCache[i].Open + ",";
            //Chi gui nhung mission da open len server
            if (dataMissionCache[i].Open == 1)
            {
                if (dataSendServer.Length > 0)
                    dataSendServer += ",";
                dataSendServer += dataMissionCache[i].Mission + "-" + dataMissionCache[i].Score + "-" + dataMissionCache[i].Star + "-" + dataMissionCache[i].Open;
            }
        }
        Debug.Log("Data save " + dataSave);
        PlayerPrefs.SetString(Mission_data_key, dataSave);
        if (submitToServer)
        {
            Debug.Log("Data send server  " + dataSendServer);
            //AudioControl.getMonoBehaviour().StartCoroutine(DHS.PostMeInfoMissionUpdate(FB.UserId, dataSendServer));
        }
    }

    public static void GetMissionDataCache()
    {
        int max_mission = 100;
        if (dataMissionCache != null)
        {
            dataMissionCache = null;
        }
        dataMissionCache = new MissionDataSave[max_mission];
        //Tao moi neu chua co
        if (!PlayerPrefs.HasKey(Mission_data_key))
        {
            string datas = "1-0-0-1,";
            for (int i = 2; i <= max_mission; i++)
            {
                //Mission - Score - Star - Open
                //if (DataMissionControlNew.test)
                //{
                //    datas += i + "-0-0-1,";

                //    //if (i < 16)
                //    //    datas += i + "-0-0-1,";
                //    //else datas += i + "-0-0-0,";
                //}
                //else
                //{
                //    datas += i + "-0-0-0,";
                //}
            }

            PlayerPrefs.SetString(Mission_data_key, datas);
        }
        string missionData = PlayerPrefs.GetString(Mission_data_key);
        string[] data = missionData.Split(',');
        for (int i = 0; i < max_mission; i++)
        {
            string[] infoData = data[i].Split('-');
            //Debug.Log("Info " + data[i]);
            string mission = infoData[0];
            string score = infoData[1];
            string star = infoData[2];
            string open = infoData[3];
            dataMissionCache[i] = new MissionDataSave(Convert.ToUInt16(mission), Convert.ToUInt32(score), Convert.ToUInt16(star), Convert.ToUInt16(open));
        }

    }

    //-------------------------CURRENT MISSION---------------------------
    //Add current mission xml node
    public static void SaveCurrentMission(string data = "")
    {
        if (String.IsNullOrEmpty(data))
        {
            string dataSave = "";
            for (int i = 0; i < dataCurrentMissionCache.Length; i++)
            {
                if (dataSave.Length > 0)
                    dataSave += ",";
                dataSave += dataCurrentMissionCache[i].FB_id + "-" + dataCurrentMissionCache[i].Name + "-" + dataCurrentMissionCache[i].Mission;
            }
            PlayerPrefs.SetString(Current_mission_data_key, dataSave);
        }
        else
        {
            PlayerPrefs.SetString(Current_mission_data_key, data);
            GetCurrentMission();
        }
    }
    public static void GetCurrentMission()
    {
        if (!PlayerPrefs.HasKey(Current_mission_data_key))
        {
            PlayerPrefs.SetString(Current_mission_data_key, "Me-User-1");
        }
        if (dataCurrentMissionCache != null)
        {
            dataCurrentMissionCache = null;
        }
        string current_data = PlayerPrefs.GetString(Current_mission_data_key);
        string[] data = current_data.Split(',');
        dataCurrentMissionCache = new CurrentMission[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            string[] info = data[i].Split('-');
            //fb - User name -  missison
            dataCurrentMissionCache[i] = new CurrentMission(info[0], info[1], info[2]);
        }
    }
    public static void SetMeCurrentMission(int mission)
    {
        for (int i = 0; i < DataCache.dataCurrentMissionCache.Length; i++)
        {
            if ("Me".Equals(DataCache.dataCurrentMissionCache[i].FB_id))
            {
                int old = Convert.ToInt16(DataCache.dataCurrentMissionCache[i].Mission);
                if (old < mission)
                {
                    DataCache.dataCurrentMissionCache[i].Mission = "" + mission;
                    DataCache.UpdateMissionScore(0, 0, mission, 1);//Them mission moi vao xml
                }
            }
        }
        DataCache.SaveCurrentMission();
    }
    //-------------------------ACHIEVEMENT---------------------------
    //Ghi de len du lieu cu
    public static void ReplaceAchievementCache(int groupLevel, int value, int level = -1)
    {
        dataAchievementCache[groupLevel - 1].Value = value;
        if (level != -1)
        {
            dataAchievementCache[groupLevel - 1].Level = level;
        }
    }
    //Cap nhat them du lieu
    public static void AddAchievementCache(int groupLevel, int addValue, int addLevel = 0)
    {
        dataAchievementCache[groupLevel - 1].Level += addLevel;
        dataAchievementCache[groupLevel - 1].Value += addValue;

    }
    public static void GetAchievementCacheData()
    {
        Debug.Log("-------------GetAchievementCacheData--------------------");
        if (dataAchievementCache != null)
        {
            dataAchievementCache = null;
        }
        dataAchievementCache = new AchievementCache[22];
        //Tao achievement
        if (!PlayerPrefs.HasKey(Achievement_data_key))
        {
            string achi = "";
            for (int i = 1; i <= 22; i++)
            {
                achi += i + "-1-0-0,";
            }
            //Debug.Log("Create new achievement " + achi);
            PlayerPrefs.SetString(Achievement_data_key, achi);
        }
        string achievement = PlayerPrefs.GetString(Achievement_data_key);
        //Debug.Log(achievement);
        string[] achie = achievement.Split(',');
        for (int i = 0; i < dataAchievementCache.Length; i++)
        {
            //Debug.Log(achie[i]);
            string[] infoAchie = achie[i].Split('-');
            string group = infoAchie[0];
            string level = infoAchie[1];
            string value = infoAchie[2];
            string notify = infoAchie[3];
            //Debug.Log(group +" " +  dataAchievementCache[i].Group);
            dataAchievementCache[i] = new AchievementCache();
            dataAchievementCache[i].Group = Convert.ToInt16(group);
            dataAchievementCache[i].Level = Convert.ToInt16(level);
            dataAchievementCache[i].Value = Convert.ToInt32(value);
            dataAchievementCache[i].Notify = Convert.ToInt16(notify);
        }
    }
    public static void SaveAchievementCache(bool sendServer = false)
    {
//        try
//        {
//            Debug.Log("-------------------SaveAchievementCache-----------------");
//            if (dataAchievementCache != null)
//            {
//                string achievement = "";
//                for (int i = 0; i < dataAchievementCache.Length; i++)
//                {
//                    string s = "" + dataAchievementCache[i].Group + "-" + dataAchievementCache[i].Level + "-" + dataAchievementCache[i].Value + "-" + dataAchievementCache[i].Notify + ",";
//                    achievement += s;
//                }
//                //Debug.Log("----------LUU ACHIEVEMENT------------ " + achievement);
//                PlayerPrefs.SetString(Achievement_data_key, achievement);
//                if (FB.IsLoggedIn && sendServer)
//                {
//                    //Nếu chưa có playerprefs thì sẽ submit lên luôn
//                    //Nếu có rồi thì phải check nó cập nhật hoàn thành từ server về thì mới cho up lên
//                    bool check = !PlayerPrefs.HasKey(DataMissionControlNew.key_update_achievement_data_from_server) ||
//                        (PlayerPrefs.HasKey(DataMissionControlNew.key_update_achievement_data_from_server) && PlayerPrefs.GetInt(DataMissionControlNew.key_update_achievement_data_from_server) == 1);
//                    if (check)
//                    {
//                        AudioControl.getMonoBehaviour().StartCoroutine(DHS.PostMeInfoUpdate(DFB.UserId, "" + VariableSystem.diamond, "" + achievement, "", (www) =>
//                        {
//                            Debug.Log("----------Update achievement to server success!------------- " + achievement);
//                        }));
//                    }
//                    else
//                    {
//                        Debug.Log("----------KHONG CHO UP ACHIEVEMENT VA DIAMOND LEN SERVER------------- " + PlayerPrefs.GetInt(DataMissionControlNew.key_update_mission_data_from_server, 0));
//                    }
//                }
//            }
//        }
//        catch (Exception e)
//        {
//            Debug.Log("------------ERROR ---------------" + e.Message);
//            if (DataMissionControlNew.test)
//            {
//#if UNITY_ANDROID
//                MobilePlugin.getInstance().ShowToast("ERROR " + e.Message);
//#endif
//            }
//        }
    }
    public static void DeleteUserData()
    {
        //PlayerPrefs.DeleteKey(FB_ID);
        //PlayerPrefs.DeleteKey(Mission_data_key);
        //PlayerPrefs.DeleteKey(Current_mission_data_key);
        //PlayerPrefs.DeleteKey(Achievement_data_key);
        //PlayerPrefs.DeleteKey("diamond");
        //PlayerPrefs.DeleteKey(DataMissionControlNew.key_update_mission_data_from_server);
        //VariableSystem.diamond = 8;
        //VariableSystem.heart = 5;
    }
    public static void RestoreUserData(int diamond, string achievement)
    {
        //Debug.Log("Restore user data");
        //VariableSystem.diamond = diamond;
        //VariableSystem.heart = PlayerPrefs.GetInt("heart", 5);
        //string[] achie = achievement.Split(',');
        //if (achie.Length > 5)
        //{
        //    for (int i = 0; i < dataAchievementCache.Length; i++)
        //    {
        //        string[] infoAchie = achie[i].Split('-');
        //        string group = infoAchie[0];
        //        string level = infoAchie[1];
        //        string value = infoAchie[2];
        //        string notify = infoAchie[3];
        //        dataAchievementCache[i].Group = Convert.ToInt16(group);
        //        dataAchievementCache[i].Level = Convert.ToInt16(level);
        //        dataAchievementCache[i].Value = Convert.ToInt32(value);
        //        dataAchievementCache[i].Notify = Convert.ToInt16(notify);
        //    }
        //    //Debug.Log("---Luu achievement----");
        //    SaveAchievementCache();
        //    GetAchievementCacheData();
        //}
        //Debug.Log("----------------ACHIEVEMENT da dc cap nhat tu -----------------");
        //PlayerPrefs.SetInt(DataMissionControlNew.key_update_achievement_data_from_server, 1);

    }

    //public static void SerializeCurrentMissionToXML(CurrentMission movie, string filePath)
    //{
    //    XmlSerializer serializer = new XmlSerializer(typeof(CurrentMission));
    //    TextWriter textwriter = new StreamWriter(filePath);
    //    serializer.Serialize(textwriter, movie);
    //    textwriter.Close();
    //}
    ////Deserialize xml theo tung phan tu
    //public static CurrentMission DeserializeCurrentMissionFromXML(string filePath)
    //{
    //    if (!System.IO.File.Exists(filePath))
    //    {
    //        Debug.LogError("File " + filePath + " not exist!");
    //        return new CurrentMission();
    //    }
    //    XmlSerializer deserializer = new XmlSerializer(typeof(CurrentMission), new XmlRootAttribute("CurrentMissionData"));
    //    TextReader textReader = new StreamReader(filePath);
    //    CurrentMission movies = (CurrentMission)deserializer.Deserialize(textReader);
    //    textReader.Close();
    //    return movies;
    //}

    //public static void SerializeCurrentMissionToXML(List<CurrentMission> movies, string filePath)
    //{
    //    XmlSerializer serializer = new XmlSerializer(typeof(List<CurrentMission>), new XmlRootAttribute("CurrentMissionData"));
    //    TextWriter text = new StreamWriter(filePath);
    //    serializer.Serialize(text, movies);
    //    text.Close();
    //}

    //public static List<CurrentMission> DeserializeCurrentMissionListFromXML(string filePath)
    //{
    //    if (!System.IO.File.Exists(filePath))
    //    {
    //        Debug.LogError("File " + filePath + " not exist!");
    //        return new List<CurrentMission>();
    //    }
    //    XmlSerializer deserializer = new XmlSerializer(typeof(List<CurrentMission>), new XmlRootAttribute("CurrentMissionData"));
    //    TextReader textReader = new StreamReader(filePath);
    //    List<CurrentMission> movies = (List<CurrentMission>)deserializer.Deserialize(textReader);
    //    textReader.Close();
    //    return movies;
    //}

    //public static string Encrypt(string toEncrypt)
    //{
    //    byte[] keyArray = UTF8Encoding.UTF8.GetBytes(DString.encrypt_key);
    //    // 256-AES key
    //    byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
    //    RijndaelManaged rDel = new RijndaelManaged();
    //    rDel.Key = keyArray;
    //    rDel.Mode = CipherMode.ECB;
    //    rDel.Padding = PaddingMode.PKCS7;
    //    // better lang support
    //    ICryptoTransform cTransform = rDel.CreateEncryptor();
    //    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
    //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    //}

    //public static string Decrypt(string toDecrypt)
    //{
    //    byte[] keyArray = UTF8Encoding.UTF8.GetBytes(DString.encrypt_key);
    //    // AES-256 key
    //    byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
    //    RijndaelManaged rDel = new RijndaelManaged();
    //    rDel.Key = keyArray;
    //    rDel.Mode = CipherMode.ECB;
    //    rDel.Padding = PaddingMode.PKCS7;
    //    // better lang support
    //    ICryptoTransform cTransform = rDel.CreateDecryptor();
    //    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
    //    return UTF8Encoding.UTF8.GetString(resultArray);
    //}
}

