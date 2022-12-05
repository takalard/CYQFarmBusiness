/**
 * 字符串处理工具类
 */

using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
public static class StringUtil
{
    public static ObjectPool<StringBuilder> ObjectPool = new ObjectPool<StringBuilder>(delegate (StringBuilder text) {
        text.Length = 0;
    }, 1);

    public static Regex rx = new Regex("^[\u4e00-\u9fa5]");

    public static bool ISChinese(string strWord)
    {
        string strRex = @"[\u4e00-\u9fa5]";
        return System.Text.RegularExpressions.Regex.IsMatch(strWord, strRex);
    }

    public static string FromTimeMessage(string message)
    {
        return $"{(DateTime.Now.ToString("HH:mm:ss"))}:{DateTime.Now.Millisecond}:{message}";
    }

    public static void Concat(string param1, int param2, out string value)
    {
        var mCache = ObjectPool.Get();
        mCache.Append(param1);
        mCache.Append(param2);
        value = mCache.ToString();
        ObjectPool.Release(mCache);
    }

    public static void Concat(string param1, float param2, out string value)
    {
        var mCache = ObjectPool.Get();
        mCache.Append(param1);
        mCache.Append(param2);
        value = mCache.ToString();
        ObjectPool.Release(mCache);
    }

    public static void Concat(string param1, string param2, out string value)
    {
        var mCache = ObjectPool.Get();
        mCache.Append(param1);
        mCache.Append(param2);
        value = mCache.ToString();
        ObjectPool.Release(mCache);
    }

    public static void Concat(string param1, string param2, string param3, out string value)
    {
        var mCache = ObjectPool.Get();
        mCache.Append(param1);
        mCache.Append(param2);
        mCache.Append(param3);
        value = mCache.ToString();
        ObjectPool.Release(mCache);
    }

    public static void PathConcat(string path1, string path2, out string value)
    {
        var mCache = ObjectPool.Get();
        mCache.Append(path1);
        mCache.Append("/");
        mCache.Append(path2);
        value = mCache.ToString();
        ObjectPool.Release(mCache);
    }

    public static int[] GetStrArray(string str, char symbol)
    {
        string[] temp = str.Split(symbol);
        int[] res = new int[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            int value = 0;
            if (int.TryParse(temp[i], out value))
                res[i] = value;
        }
        return res;
    }

    public static string[] ExcelSingleSplit(string str)
    {
        if (string.IsNullOrEmpty(str))
            return new string[0];
        str = str.Trim();
        var list = new System.Collections.Generic.List<string>();

        if (str.Contains(";"))
            list.AddRange(str.Split(';'));
        else if (str.Contains("|"))
            list.AddRange(str.Split('|'));
        else if (str.Contains(","))
            list.AddRange(str.Split(','));
        else if (str.Contains("-"))
            list.AddRange(str.Split('-'));

        if (list.Count > 1)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i]))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
            return list.ToArray();
        }

        return new string[1] { str };


    }

    public static string[] ExcelSingleSplit(string str, char[] flag)
    {
        str = str.Trim();
        var list = new System.Collections.Generic.List<string>();

        list.AddRange(str.Split(flag));

        if (list.Count > 1)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i]))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
            return list.ToArray();
        }
        return new string[1] { str };

    }

    public static string ToUpperFirst(string str)
    {
        return Regex.Replace(str, @"^\w", t => t.Value.ToUpper());
    }

    //1. 首字母大写 2. 每个下划线后的第一个字符大写 3. 删除所有破折号
    public static string ConvertDashToCamelCase(string input)
    {
        Debug.LogFormat("123456input ： {0}", input);
        StringBuilder sb = new StringBuilder();
        bool caseFlag = false;
        for (int i = 0; i < input.Length; ++i)
        {
            char c = input[i];
            if (c == '_')
            {
                caseFlag = true;
            }
            else if (caseFlag)
            {
                sb.Append(char.ToUpper(c));
                caseFlag = false;
            }
            else
            {
                sb.Append(char.ToLower(c));
            }
        }
        string outStr = ToUpperFirst(sb.ToString());
        Debug.LogFormat("123456outStr ： {0}", outStr);
        return outStr;
    }

    public static string FirstCharToLower(string input)
    {
        return Regex.Replace(input, @"^\w", t => t.Value.ToLower());
    }
    public static string LineText(string text, int tabCount = 0)
    {
        string ret = "";
        for (int i = 1; i <= tabCount; i++)
        {
            Concat("\t", ret, out ret);
        }
        Concat(ret, text, out ret);
        Concat(ret, "\n", out ret);
        return ret;
    }
    public static string TableText(string text, int tabCount = 0)
    {
        string ret = "";
        for (int i = 1; i <= tabCount; i++)
        {
            Concat("\t", ret, out ret);
        }
        Concat(ret, text, out ret);
        return ret;
    }

    public static string LineTextFormat(string text, int tabCount, params string[] vs)
    {
        string ret = "";
        for (int i = 1; i <= tabCount; i++)
        {
            Concat("\t", ret, out ret);
        }
        Concat(ret, string.Format(text, vs), out ret);
        Concat(ret, "\n", out ret);
        return ret;
    }
}