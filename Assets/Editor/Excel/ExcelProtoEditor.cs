using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;
using System;
using CatLib.API.Json;
using CatLib;
using LitJson;
using CatLib.Json;
using System.Text;

public class ExcelProtoEditor
{
    public static string ExcelPath
    {
        get
        {
            var _Path = Environment.CurrentDirectory+"/Excels";
            if (!Directory.Exists(_Path))
            {
                Directory.CreateDirectory(_Path);
            }
            return _Path;
        }
    }
    public static string JsonToExcelOutputPath
    {
        get
        {
            var _Path = Environment.CurrentDirectory + "/JsonToExcelOutput";
            if (!Directory.Exists(_Path))
            {
                Directory.CreateDirectory(_Path);
            }
            return _Path;
        }
    }

    //相对Assets的目录
    public static string RelativeConfigsPath = "Game/Configs";

    public const string FilePatterns = "*.xlsx";
    [MenuItem("Tools/Excel/ExcelExport")]
    public static void ExcelToJson()
    {
        //UnityToSVN.SVNUpdate(System.IO.Path.GetFullPath("./../Excels"));
        UnityEditor.EditorUtility.ClearProgressBar();
        if (!Directory.Exists(ExcelPath))
        {
            UnityEngine.Debug.LogError($"{Path.GetFullPath(ExcelPath)} Exists");
            return;
        }

       // OutProto(null, "common");//导出公共文件

        var files = Directory.GetFiles(ExcelPath, FilePatterns);
        Dictionary<string, ExcelFile> FileDict = new Dictionary<string, ExcelFile>();
        for(int i = 0;i< files.Length;i++) 
        {
            var file = files[i];
            var name = Path.GetFileNameWithoutExtension(file);

            bool result = UnityEditor.EditorUtility.DisplayCancelableProgressBar($"读取表资源", name, (i + 1) / (float)files.Length);
            if(result)
            {
                UnityEditor.EditorUtility.ClearProgressBar();
                return;
            }
            if (name.Contains("~$"))
                continue;
           // var changFileName = StringUtil.ConvertDashToCamelCase(name);

            try
            {
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                {
                    try
                    {
                        IWorkbook workbook = GetWorkBook(file, stream);
                        stream.Close();
                        if (workbook == null)
                        {
                            continue;
                        }
                        ISheet sheet = workbook.GetSheetAt(0);
                        ExcelFile File = ReadExcelRender(name, sheet);
                        if (File != null && File.fieldInfos.Count != 0)
                        {
                            FileDict[name] = File;
                        }

                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                        Debug.LogError($"载入配表{Path.GetFileName(file)}异常");
                        stream.Close();
                 
                        continue;
                    }
                }
            }catch(Exception e)
            {
                Debug.LogException(e);
                Debug.LogError($"请先关闭表{name}!");
                continue;
            }
           
        }
        UnityEditor.EditorUtility.ClearProgressBar();
        //CreateProto(FileDict);
        ExportToJson(FileDict);
        UnityEditor.EditorUtility.DisplayDialog("提示", "导出完成", "确认");
        //if (!GeneratedAllTableData(FileDict))
        //{
        //    UnityEditor.EditorUtility.DisplayDialog("提示", "导出数据异常,查看控制台", "确认");
        //}
        //else
        //{
        //    UnityEditor.EditorUtility.DisplayDialog("提示", "导出完成", "确认");
        //}
    }
    private static void ExportToJson(Dictionary<string, ExcelFile> FileDict)
    {
        var ConfigsPath = Environment.CurrentDirectory + "/Assets/" + RelativeConfigsPath;

        if (Directory.Exists(ConfigsPath))
        {
            Directory.Delete(ConfigsPath, true);
        }

        Directory.CreateDirectory(ConfigsPath);


        List<string> ConfigFileList = new List<string>();
       AssetDatabase.Refresh();

        Debug.Log("ExportToJson Count = " + FileDict.Count);

        foreach (var excel in FileDict)
       {
            var json = new JsonConfig();
            //var strJson = JsonMapper.ToJson(excel);
            //Debug.Log("strJson = "+strJson);

            json.ConfigName = excel.Key;
            json.Size = excel.Value.fieldInfos[0].Values.Count;
            for (int i = 0; i < excel.Value.fieldInfos.Count; i++)
            {
                json.FieldNames.Add(new ExcelField() { Doc = excel.Value.fieldInfos[i].Doc, Field = excel.Value.fieldInfos[i].FieldName, FieldType = excel.Value.fieldInfos[i].FieldType });
            }

            for (int i = 0; i < excel.Value.fieldInfos[0].Values.Count; i++)
            {
                json.Keys.Add(excel.Value.fieldInfos[0].Values[i]);
                JsonConfigKeyItem item = new JsonConfigKeyItem();
                item.Key = excel.Value.fieldInfos[0].Values[i];
                foreach (var field in excel.Value.fieldInfos)
                {
                    try
                    {
                        item.ValueDict.Add(field.FieldName.Name, field.Values[i]);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        throw new Exception($"表：{excel.Key} 出现重复字段：{field.FieldName.Name}");
                    }

                }
                try
                {
                    json.ValueDict.Add(item.Key, item);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    throw new Exception($"表：{excel.Key} 出现重复键值：{item.Key}");
                }

            }
            var jsonContent = System.Text.RegularExpressions.Regex.Unescape(LitJson.JsonMapper.ToJson(json));
            WriteUTF8Text($"{ConfigsPath}/{json.ConfigName}.json", jsonContent);
            ConfigFileList.Add(json.ConfigName);
        }

        WriteUTF8Text($"{ConfigsPath}/ConfigFileList.txt", LitJson.JsonMapper.ToJson(ConfigFileList));

        AssetDatabase.Refresh();

    }

    [MenuItem("Tools/Excel/JosnToExcel")]
    public static void JosnToExcel()
    {
        var ConfigFileListPath = $"Assets/{RelativeConfigsPath}/ConfigFileList.txt";
        var jsonRoot = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(ConfigFileListPath);
        if (!jsonRoot)
            throw new Exception("初始化配置失败,加载ConfigFileList.txt异常");

        List<string> JsonConfigs = LitJson.JsonMapper.ToObject<List<string>>(jsonRoot.text);
        Debug.Log("JsonConfigs = "+JsonConfigs.ToString());

        for (int i = 0; i < JsonConfigs.Count; i++)
        {
            var cfgName = JsonConfigs[i];
            var ConfigFilePath = $"Assets/{RelativeConfigsPath}/{cfgName}.json";
            Debug.Log("ConfigFilePath = " + ConfigFilePath);
            var data = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(ConfigFilePath).text;
            JsonConfig jsonConfig = LitJson.JsonMapper.ToObject<JsonConfig>(data);

            var excelPath = $"{JsonToExcelOutputPath}/{jsonConfig.ConfigName}.xlsx";
            if (File.Exists(excelPath))
            {
                File.Delete(excelPath);
            }
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(jsonConfig.ConfigName);
            IRow fieldNameRow = sheet.CreateRow(0);
            IRow fieldTypeRow = sheet.CreateRow(1);
            IRow fieldDoc = sheet.CreateRow(2);

            for (int m = 0; m < jsonConfig.FieldNames.Count; m++)
            {
                ExcelField excelField = jsonConfig.FieldNames[m];
                var field = fieldNameRow.CreateCell(m);
                var fieldType = fieldTypeRow.CreateCell(m);
                var Doc = fieldDoc.CreateCell(m);

                field.SetCellValue(excelField.Field.Name);
                if (!string.IsNullOrEmpty(excelField.Field.Annotation))
                {
                    GetComment(sheet, field.CellComment, m, 0, excelField.Field.Annotation);
                }
                fieldType.SetCellValue(excelField.FieldType.Name);
                if (!string.IsNullOrEmpty(excelField.FieldType.Annotation))
                {
                    GetComment(sheet, field.CellComment, m, 1, excelField.FieldType.Annotation);
                }
                Doc.SetCellValue(excelField.Doc.Name);
                if (!string.IsNullOrEmpty(excelField.Doc.Annotation))
                {
                    GetComment(sheet, field.CellComment, m, 2, excelField.Doc.Annotation);
                }
            }

            for (int m = 0; m < jsonConfig.Keys.Count; m++)
            {
                var valueRow = sheet.CreateRow(3 + m);
                for (int j = 0; j < jsonConfig.FieldNames.Count; j++)
                {
                    var value = valueRow.CreateCell(j);
                    value.SetCellValue(jsonConfig[jsonConfig.Keys[m]][jsonConfig.FieldNames[j].Field.Name]);
                }
            }
            Debug.Log("excelPath = "+Path.GetFullPath(excelPath));
            using (FileStream fs = new FileStream(Path.GetFullPath(excelPath), FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);//写入文件
                workbook.Close();//关闭流
            }
        }

    }

    public static void WriteUTF8Text(string outFile, string outText)
    {
        try
        {
            if (string.IsNullOrEmpty(outFile))
                throw new ArgumentNullException("outFile");
            //if (!CheckFileAndCreateDirWhenNeeded(outFile))
            //    throw new Exception("!CheckFileAndCreateDirWhenNeeded(outFile)");
            if (File.Exists(outFile)) File.SetAttributes(outFile, FileAttributes.Normal);

            StringBuilder sb = new StringBuilder();
            sb.Append(outText);

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            Byte[] BytesMessage = UTF8.GetBytes(outText);
            File.WriteAllText(outFile, GetUTF8String(BytesMessage));
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("WriteAllText failed! path = {0} with err = {1}", outFile, e.Message));
            throw;
        }
    }

    public static string GetUTF8String(byte[] buffer)
    {
        if (buffer == null)
            return null;

        if (buffer.Length <= 3)
        {
            return Encoding.UTF8.GetString(buffer);
        }

        byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };

        if (buffer[0] == bomBuffer[0]
            && buffer[1] == bomBuffer[1]
            && buffer[2] == bomBuffer[2])
        {
            return new UTF8Encoding(false).GetString(buffer, 3, buffer.Length - 3);
        }

        return Encoding.UTF8.GetString(buffer);
    }

    private static IComment GetComment(ISheet sheet, IComment comment, int cellnum, int row ,object text)
    {
        IWorkbook workbook = sheet.Workbook;
        bool exportXlsx = workbook is XSSFWorkbook;
        if (comment == null)
        {
            IDrawing draw = sheet.CreateDrawingPatriarch();
            IClientAnchor clientAnchor = null;
            clientAnchor = new XSSFClientAnchor(0, 0, 0, 0, cellnum, row, cellnum + 4, 10);
            comment = draw.CreateCellComment(clientAnchor);
        }

        IRichTextString richText = new XSSFRichTextString(string.Format("{0}", text));
        comment.String = richText;
        comment.Visible = false;
        comment.Author = "takalard";
        return comment;
    }

    //private static void GeneratedTableData(string ProtoFileName)
    //{
    //    var parameters = new CompilerParameters();
    //    var provider = new CSharpCodeProvider();
    //    parameters.ReferencedAssemblies.Add(Path.GetFullPath("Assets/Plugins/Google.Protobuf.dll"));
    //    var files = new string[2];
    //    files[0] = Path.GetFullPath($"{Path.GetFullPath(ProtoCompile)}/Common.cs");
    //    files[1] = Path.GetFullPath($"{Path.GetFullPath(ProtoCompile)}/{ProtoFileName}.cs");
    //    var rusult = provider.CompileAssemblyFromFile(parameters, files);
    //    if (rusult.Errors.Count > 0)
    //    {
    //        var sb = StringUtil.ObjectPool.Get();
    //        foreach (var error in rusult.Errors)
    //        {
    //            sb.Append(StringUtil.LineText("1 CompileAssemblyFromSource Error:" + error));
    //        }

    //        Debug.LogError(sb.ToString());
    //        StringUtil.ObjectPool.Release(sb);
    //    }
    //}

    //private static bool GeneratedAllTableData(Dictionary<string, ExcelFile> FileDict)
    //{
    //    var parameters = new CompilerParameters();
    //    var provider = new CSharpCodeProvider();
    //    parameters.ReferencedAssemblies.Add(Path.GetFullPath("Assets/Plugins/Google.Protobuf.dll"));
    //    var files = Directory.GetFiles(Path.GetFullPath(ProtoCompile),"*.cs");
    //    var rusult = provider.CompileAssemblyFromFile(parameters, files);
    //    if (rusult.Errors.Count > 0)
    //    {
    //        var sb = StringUtil.ObjectPool.Get();
    //        foreach (var error in rusult.Errors)
    //        {
    //            sb.Append(StringUtil.LineText("1 CompileAssemblyFromSource Error:" + error));
    //        }

    //        Debug.LogError(sb.ToString());
    //        StringUtil.ObjectPool.Release(sb);
    //        return false;
    //    }

    //    int progress = 1;
    //    if (Directory.Exists("Assets/GameMain/ProtoData"))
    //    {
    //        Directory.Delete("Assets/GameMain/ProtoData",true);
    //    }
        
    //    Directory.CreateDirectory("Assets/GameMain/ProtoData");

    //    foreach (var file in FileDict)
    //    {
           
    //        var fileName = file.Key;
    //        var excelFile = file.Value;

    //        bool result = UnityEditor.EditorUtility.DisplayCancelableProgressBar($"导出Proto 数据", fileName, (float)progress / FileDict.Count);
    //        if (result)
    //        {
    //            UnityEditor.EditorUtility.ClearProgressBar();
    //            return false;
    //        }
    //        progress++;

    //        var message = Generator(fileName,excelFile, rusult.CompiledAssembly);
    //        if(message == null)
    //        {
    //            continue;
    //        }
    //        var type = message.GetType();
    //        var path = $"{Path.GetFullPath("Assets/GameMain/ProtoData")}/{type.Name}.bytes";

    //        using (var output = File.Create(path))
    //        {
    //            MessageExtensions.WriteTo((IMessage)message, output);
    //        }

    //    }
    //    UnityEditor.EditorUtility.ClearProgressBar();
    //    AssetDatabase.Refresh();      
    //    return true;
    //}

    private static ExcelFile ReadExcelRender(string fileName, ISheet Isheet)
    {
        if (Isheet.PhysicalNumberOfRows == 0)
        {
            return null;
        }

        ExcelFile excelfile = System.Activator.CreateInstance<ExcelFile>();

        IRow fieldNameRow = Isheet.GetRow(0);
        IRow fieldTypeRow = Isheet.GetRow(1);
        IRow fieldDoc = Isheet.GetRow(2);

        if (fieldNameRow == null || fieldTypeRow == null)
        {
            Debug.LogError($"表结构设计异常:{fileName}");
            return null;
        }

        for (int i = 0; i < fieldNameRow.LastCellNum; i++)
        {
            var filed = new ExicelFieldInfo();

            filed.FieldIdx = i;
            ICell cell = fieldNameRow.GetCell(i);
            ICell cell1 = fieldTypeRow.GetCell(i);
            ICell cell2 = fieldDoc.GetCell(i);

            Debug.Log("");

            if (cell == null || cell1 == null)
            {
                continue;
            }
            cell.SetCellType(CellType.String);
            cell1.SetCellType(CellType.String);

            filed.FieldName = new ExcelFieldItem() { Name = cell.StringCellValue };
            filed.FieldType = new ExcelFieldItem() { Name = cell1.StringCellValue };

            if (cell.CellComment != null && cell.CellComment.String != null)
            {
                var annotation = ((cell.CellComment.String.ToString().Replace("\n", "")).Replace("\r", "")).Trim();//获取当前单元格的批注
                if (!annotation.Equals("open", StringComparison.OrdinalIgnoreCase))
                {
                    filed.FieldName.Annotation = annotation;
                }
                else
                {
                    filed.FieldName.Annotation = "";
                }
            }

            if (cell1.CellComment != null && cell1.CellComment.String != null)
            {
                var annotation = ((cell1.CellComment.String.ToString().Replace("\n", "")).Replace("\r", "")).Trim();//获取当前单元格的批注
                if (!annotation.Equals("open", StringComparison.OrdinalIgnoreCase))
                {
                    filed.FieldType.Annotation = annotation;
                }
                else
                {
                    filed.FieldName.Annotation = "";
                }
            }

            if (cell2 != null )
            {
                cell2.SetCellType(CellType.String);
                filed.Doc = new ExcelFieldItem() { Name = cell2.StringCellValue };
                if(cell2.CellComment != null && cell2.CellComment.String != null)
                {
                    var annotation = ((cell2.CellComment.String.ToString().Replace("\n", "")).Replace("\r", "")).Trim();//获取当前单元格的批注
                    if (!annotation.Equals("open", StringComparison.OrdinalIgnoreCase))
                    {
                        filed.Doc.Annotation = annotation;
                    }
                    else
                    {
                        filed.FieldName.Annotation = "";
                    }
                }
              
            }

            var fieldType = ProtoTools.conversionProtoType(filed.FieldType.Name);

            if (string.IsNullOrEmpty(filed.FieldName.Name) || string.IsNullOrEmpty(filed.FieldType.Name) || (!ProtoTools.GetVariableString(fieldType) && !ProtoTools.GetComstomString(fieldType)))
                continue;
            filed.Values = new List<string>();
            excelfile.fieldInfos.Add(filed);
        }

        for (int j = 3; j < Isheet.PhysicalNumberOfRows; j++)
        {
            IRow data = Isheet.GetRow(j);
            if (data == null)
                continue;
            for (int i = 0; i < excelfile.fieldInfos.Count; i++)
            {
                var filed = excelfile.fieldInfos[i];
                string value = string.Empty;
                try
                {
                    if (data.GetCell(filed.FieldIdx) != null)
                    {
                        if (data.GetCell(filed.FieldIdx).CellType == CellType.Formula)
                        {
                            data.GetCell(filed.FieldIdx).SetCellType(CellType.String);
                            value = data.GetCell(filed.FieldIdx).StringCellValue;
                        }
                        else
                        {
                            value = data.GetCell(filed.FieldIdx).ToString();
                        }
                    }
                    if (i == 0 && string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                }catch(Exception e)
                {
                    Debug.LogException(e);
                    Debug.LogError($"解析表 {fileName}  行：{j} 列：{filed.FieldIdx} 解析异常");
                    continue;
                }
               

                filed.Values.Add(value);

            }
        }
        return excelfile;
    }
    public static IWorkbook GetWorkBook(string path, FileStream stream)
    {
        return new XSSFWorkbook(stream);//从流内容创建Workbook对象
    }

    [Serializable]
    public class ExicelFieldInfo
    {
        public int FieldIdx;
        public ExcelFieldItem FieldName;
        public ExcelFieldItem FieldType;
        public ExcelFieldItem Doc;
        public List<string> Values;
    }

    [Serializable]
    public class ExcelFile
    {
        public List<ExicelFieldInfo> fieldInfos = new List<ExicelFieldInfo>();
    }
    public enum ExcelSearchPatterns
    {
        XLSX,
        XLS
    }
}
