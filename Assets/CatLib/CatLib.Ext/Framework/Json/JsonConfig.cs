using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CatLib.Json
{
    [System.Serializable]
    public class JsonConfig:IEnumerable<JsonConfigKeyItem>
    {
        public List<string> Keys = new List<string>();
        public List<ExcelField> FieldNames = new List<ExcelField>();
        public int Size;
        public string ConfigName;
        public SerializableDictionary<string, JsonConfigKeyItem> ValueDict;
        public JsonConfig()
        {
            ValueDict = new SerializableDictionary<string, JsonConfigKeyItem>();
        }
        //public void Write()
        //{
        //    var jsonStr = System.Text.RegularExpressions.Regex.Unescape(LitJson.JsonMapper.ToJson(this));
        //    FileUtil.WriteAllUTF8Text($"Assets/GameMain/JsonConfig/{ConfigName}.json", jsonStr);
        //}

        public List<JsonConfigKeyItem> ToList()
        {
            return ValueDict.Values.ToList<JsonConfigKeyItem>();
        }

        public  JsonConfigKeyItem this[string key]
        {
            get
            {
                return GetItem(key);
            }
        }
        public JsonConfigKeyItem this[int key]
        {
            get
            {
                return GetItem(key);
            }
        }
        public JsonConfigKeyItem this[long key]
        {
            get
            {
                return GetItem(key);
            }
        }

        public override string ToString()
        {
            return LitJson.JsonMapper.ToJson(this);
        }

        public JsonConfigKeyItem GetItem(int Key)
        {
            return GetItem(Key.ToString());
        }
        public JsonConfigKeyItem GetItem(long Key)
        {
            return GetItem(Key.ToString());
        }

        public JsonConfigKeyItem GetItem(string Key)
        {
            if (!Keys.Contains(Key))
            {
                Debug.LogError($"查找 表：{ConfigName}  键值{Key} 不存在 ");
                return null;
            }
                
            return ValueDict[Key];
        }

        public IEnumerator<JsonConfigKeyItem> GetEnumerator()
        {
            return ValueDict.Values.GetEnumerator() ;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(string key)
        {
            return Keys.Contains(key);
        }
        public bool Contains(int key)
        {
            return Contains(key.ToString());
        }
    }
    [System.Serializable]
    public class JsonConfigKeyItem
    {
        public string Key;
        [SerializeField]
        public SerializableDictionary<string, string> ValueDict;
        public JsonConfigKeyItem()
        {
            ValueDict = new SerializableDictionary<string, string>();
            Key = string.Empty;
        }
     
        public string this[string key]
        {
            get
            {
                if (ValueDict.ContainsKey(key))
                {
                    return ValueDict[key];
                }
                return string.Empty;
            }
            set
            {
                ValueDict[key] = value;
            }
        }

        public string GetValue(string key)
        {
            return this[key];
        }

        public  int TryInt( string key)
        {
            if (int.TryParse(this[key], out int value))
            {
                return value;
            }

            return 0;
        }
        public long TyyLong(string key)
        {
            if (long.TryParse(this[key], out long value))
            {
                return value;
            }

            return 0;
        }
        public bool TryBool(string key)
        {
            return this[key].ToLower() == "true";
        }

        public  float TryFloat(string key)
        {
            if (float.TryParse(this[key], out float value))
            {
                return value;
            }

            return 0;
        }

        public Vector3 TryVector3(string key)
        {
            if (!string.IsNullOrEmpty(this[key]))
            {
                var values = this[key].Split('*');
                if (values.Length != 3)
                    return Vector3.zero;
                return new Vector3(Convert.ToSingle(values[0]), Convert.ToSingle(values[1]), Convert.ToSingle(values[2]));
            }

            return Vector3.zero;
        }

        public Vector2 TryVector2(string key)
        {
            if (!string.IsNullOrEmpty(this[key]))
            {
                var values = this[key].Split('*');
                if (values.Length != 2)
                    return Vector2.zero;
                return new Vector2(Convert.ToSingle(values[0]), Convert.ToSingle(values[1]));
            }

            return Vector2.zero;
        }
        public List<int> TryIntAry(string key)
        {
            try {

                if (string.IsNullOrEmpty(this[key]))
                {
                    return new List<int>();
                }

                var values = this[key].Split('|');
                var result = new List<int>();
                foreach (var str in values)
                    result.Add(int.Parse(str));
                return result;
            }catch(System.Exception e)
            {
                Debug.LogException(e);
                Debug.LogError($"字段：{key},类型int[],解析异常 键值：{Key}");
            }
            return new List<int>();
        }
    
        public List<long> TryLongAry(string key)
        {
            try
            {

                if (string.IsNullOrEmpty(this[key]))
                {
                    return new List<long>();
                }
                var values = this[key].Split('|');
                var result = new List<long>();
                foreach (var str in values)
                    result.Add(long.Parse(str));
                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                Debug.LogError($"字段：{key},类型long[],解析异常 键值：{Key}");

            }
            return new List<long>();
        }

        public List<string> TryStrAry(string key)
        {
            if (string.IsNullOrEmpty(this[key]))
            {
                return new List<string>();
            }
            var result = this[key].Split('|').ToList();
            return result;
        }

        public List<float> TryFloatAry(string key)
        {

            try
            {

                if (string.IsNullOrEmpty(this[key]))
                {
                    return new List<float>();
                }
                var values = this[key].Split('|');
                var result = new List<float>();
                foreach (var str in values)
                    result.Add(float.Parse(str));
                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                Debug.LogError($"字段：{key},类型float[],解析异常 键值：{Key}");

            }
            return new List<float>();
        }
        public List<bool> TryBoolAry(string key)
        {
            try
            {

                if (string.IsNullOrEmpty(this[key]))
                {
                    return new List<bool>();
                }
                var values = this[key].Split('|');
                var result = new List<bool>();
                foreach (var str in values)
                    result.Add(str.ToLower() == "true");
                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                Debug.LogError($"字段：{key},类型bool[],解析异常 键值：{Key}");

            }
            return new List<bool>();
        }

        public List<Vector3> TryVector3Ary(string key)
        {
           
            if (string.IsNullOrEmpty(this[key]))
            {
                return new List<Vector3>();
            }

            var values = this[key].Split('|');
            var result = new List<Vector3>();
            foreach (var str in values)
            {
                var item = str.Split('*');
                if (item.Length != 3)
                {
                    Debug.LogError($"字段：{key},类型Vector3[],解析异常 键值：{Key}");
                    throw new System.Exception($"字段：{key},类型Vector3[],解析异常 键值：{Key}");
                }
                result.Add(new Vector3(Convert.ToSingle(item[0]), Convert.ToSingle(item[1]), Convert.ToSingle(item[2])));
            }

            return result;
        }

        public List<Vector2> TryVector2ry(string key)
        {
            if (string.IsNullOrEmpty(this[key]))
            {
                return new List<Vector2>();
            }

            var values = this[key].Split('|');
            var result = new List<Vector2>();
            foreach (var str in values)
            {
                var item = str.Split('*');
                if (item.Length != 2)
                {
                    Debug.LogError($"字段：{key},类型Vector3[],解析异常 键值：{Key}");
                    throw new System.Exception($"字段：{key},类型Vector3[],解析异常 键值：{Key}");
                }
                result.Add(new Vector2(Convert.ToSingle(item[0]), Convert.ToSingle(item[1])));
            }

            return result;
        }

       
        public Dictionary<int,int> TryInt_IntDictionary(string key)
        {
            if (string.IsNullOrEmpty(this[key]))
            {
                return new Dictionary<int, int>();
            }

            var values = this[key].Split('|');
            var result = new Dictionary<int, int>();
            foreach (var str in values)
            {
                var item = str.Split('*');
                try
                {
                    var key1 = int.Parse(item[0]);
                    var value1 = int.Parse(item[1]);
                    result.Add(key1, value1);
                }
                catch (System.Exception)
                {

                    Debug.LogError($"字段：{key},类型map(int,int),解析异常 键值：{Key},value:{ this[key]}");
                    throw new System.Exception($"字段：{key},类型map(int,int),解析异常 键值：{Key}");
                }
               
            }
               
            return result;
        }

        public Dictionary<int, string> TryInt_stringDictionary(string key)
        {
            if (string.IsNullOrEmpty(this[key]))
            {
                return new Dictionary<int, string>();
            }

            var values = this[key].Split('|');
            var result = new Dictionary<int, string>();
            foreach (var str in values)
            {
                var item = str.Split('*');
                try
                {
                    var key1 = int.Parse(item[0]);
                    var value1 = item[1];
                    result.Add(key1, value1);
                }
                catch (System.Exception)
                {

                    Debug.LogError($"字段：{key},类型map(int,string),解析异常 键值：{Key}");
                    throw new System.Exception($"字段：{key},类型map(int,string),解析异常 键值：{Key}");
                }
              
            }

            return result;
        }

        public Dictionary<int, float> TryInt_FloatDictionary(string key)
        {
            if (string.IsNullOrEmpty(this[key]))
            {
                return new Dictionary<int, float>();
            }

            var values = this[key].Split('|');
            var result = new Dictionary<int, float>();
            foreach (var str in values)
            {
                var item = str.Split('*');
                try
                {
                    var key1 = int.Parse(item[0]);
                    var value1 = float.Parse(item[1]);
                    result.Add(key1, value1);
                }
                catch (System.Exception)
                {

                    Debug.LogError($"字段：{key},类型map(int,float),解析异常 键值：{Key}");
                    throw new System.Exception($"字段：{key},类型map(int,float),解析异常 键值：{Key}");
                }
             
            }

            return result;
        }

        public Dictionary<int, long> TryInt_LongDictionary(string key)
        {
            if (string.IsNullOrEmpty(this[key]))
            {
                return new Dictionary<int, long>();
            }

            var values = this[key].Split('|');
            var result = new Dictionary<int, long>();
            foreach (var str in values)
            {
                var item = str.Split('*');
                try
                {
                    var key1 = int.Parse(item[0]);
                    var value1 = long.Parse(item[1]);
                    result.Add(key1, value1);
                }
                catch (System.Exception)
                {

                    Debug.LogError($"字段：{key},类型map(int,long),解析异常 键值：{Key}");
                    throw new System.Exception($"字段：{key},类型map(int,long),解析异常 键值：{Key}");
                }

            }

            return result;
        }
        public Dictionary<int, bool> TryInt_BoolDictionary(string key)
        {
            if (string.IsNullOrEmpty(this[key]))
            {
                return new Dictionary<int, bool>();
            }

            var values = this[key].Split('|');
            var result = new Dictionary<int, bool>();
            foreach (var str in values)
            {
                var item = str.Split('*');
                try
                {
                    var key1 = int.Parse(item[0]);
                    var value1 = item[1].ToLower() =="true";
                    result.Add(key1, value1);
                }
                catch (System.Exception)
                {

                    Debug.LogError($"字段：{key},类型map(int,bool),解析异常 键值：{Key}");
                    throw new System.Exception($"字段：{key},类型map(int,bool),解析异常 键值：{Key}");
                }

            }

            return result;
        }
        public string ToJson()
        {
            return LitJson.JsonMapper.ToJson(this);
        }
    }

    [System.Serializable]
    public class ExcelField
    {
        public ExcelFieldItem Field;//字段
        public ExcelFieldItem Doc;//注释
        public ExcelFieldItem FieldType;//类型
    }


    [System.Serializable]
    public struct ExcelFieldItem
    {
        public string Name ; //字段
        public string Annotation ;//批注
    }
}
