using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProtoTools
{

    public const string double_ = "double";
    public const string float_ = "float";
    public const string int32_ = "int32";
    public const string int64_ = "int64";
    public const string uint32_ = "uint32";
    public const string uint64_ = "uint64";
    public const string bool_ = "bool";
    public const string string_ = "string";


    public const string double_s = "double[]";
    public const string float_s = "float[]";
    public const string int32_s = "int32[]";
    public const string int64_s = "int64[]";
    public const string uint32_s = "uint32[]";
    public const string uint64_s = "uint64[]";
    public const string bool_s = "bool[]";
    public const string string_s = "string[]";
    public const string map_int_int = "map(int,int)";
    public const string map_int_long = "map(int,long)";
    public const string map_int_float = "map(int,float)";
    public const string map_int_bool = "map(int,bool)";
    public const string map_int_string = "map(int,string)";
    public const string Vector2 = "Vector2";
    public const string Vector3 = "Vector3";
    public static readonly string[] VariableType = new[] {
            "double", "float", "int32", "int64", "uint32", "uint64", "sint32", "sint64", "fixed32", "fixed64","sfixed32", "sfixed64", "bool", "string", "bytes",
            "double[]", "float[]", "int32[]", "int64[]", "uint32[]", "uint64[]", "sint32[]", "sint64[]", "fixed32[]", "fixed64[]","sfixed32[]", "sfixed64[]", "bool[]", "string[]", "bytes[]",
         "map(int,int)","map(int,long)","map(int,float)","map(int,bool)","map(int,string)","Vector2","Vector3"
};

    public static readonly string[] CostomType = new[] { "ItemCount[]", "ItemCount", "VectorUint[]", "VectorInt[]"};
    public const string items = "ItemCount[]";
    public const string item = "ItemCount";
    public const string VectorUints = "VectorUint[]";//多维数组
    public const string VectorInts = "VectorInt[]";//多维数组

    private static string[] AllType;
    public static string[] GetFieldType()
    {
        var AllType = new string[ProtoTools.VariableType.Length + ProtoTools.CostomType.Length];
        ProtoTools.VariableType.CopyTo(AllType, 0);
        ProtoTools.CostomType.CopyTo(AllType, ProtoTools.VariableType.Length);

        return AllType;
    }


    public static string conversionProtoType(string type)
    {
        if (type == "int") return int32_;
        if (type == "uint") return uint32_;
        if (type == "long") return int64_;
        if (type == "ulong") return uint64_;
        if (type == "float") return float_;
        if (type == "bool") return bool_;
        if (type == "string") return string_;

        if (type == "int[]") return int32_s;
        if (type == "uint[]") return uint32_s;
        if (type == "long[]") return int64_s;
        if (type == "ulong[]") return uint64_s;
        if (type == "float[]") return float_s;
        if (type == "bool[]") return bool_s;
        if (type == "string[]") return string_s;
        if (type == map_int_int) return map_int_int;
        if (type == map_int_bool) return map_int_bool;
        if (type ==map_int_float) return map_int_float;
        if (type == map_int_long) return map_int_long;
        if (type == map_int_string) return map_int_string;
        if (type == Vector2) 
            return Vector2;
        if (type == Vector3)
            return Vector3;
        return string.Empty;
    }

    public static bool GetVariableString(string type)
    {
        if (!VariableType.Contains(type))
        {
            return false;
        }
        return true;
    }

    public static bool GetComstomString(string type)
    {
        if (!CostomType.Contains(type))
        {
            return false;
        }
        return true;
    }
}