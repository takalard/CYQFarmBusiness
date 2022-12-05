using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;

public class EditorHelper
{
    /// <summary> 运行cmd命令，同步等待运行完成。命令失败时抛出异常。（不完全支持特殊字符） </summary>
    /// <param name="command">要运行的命令（包含参数）</param>
    /// <param name="timeout"></param>
    /// <param name="actionName">命令描述（用于日志）</param>
    /// <param name="errorPause">失败时暂停cmd窗口</param>
    /// <param name="errorAlert">失败时modal messagebox方式弹出错误警告</param>
    /// <param name="workingDir">工作目录</param>
    /// <exception cref="Exception"></exception>
    public static void RunCommand(string command, int timeout = -1, string actionName = null,
        bool errorPause = false, bool errorAlert = true, string workingDir = null, bool throwError = true)
    {
        if (actionName == null)
        {
            actionName = command;
            UnityEngine.Debug.LogFormat("RunCommand: {0}", command);
        }
        else
        {
            UnityEngine.Debug.LogFormat("RunCommand {0}: {1}", actionName, command);
        }

        string responseFile = Path.GetTempFileName();

        // 清理状态
        Directory.CreateDirectory(Path.GetDirectoryName(responseFile));
        if (File.Exists(responseFile))
            File.Delete(responseFile);

        // 启动进程运行命令
        string arguments;
        if (errorPause)
            arguments = string.Format("/v:on /c \"({0}) && ((echo !errorlevel!)>{1}) || (((echo !errorlevel!)>{1}) && pause)\"", command, responseFile);
        else
            arguments = string.Format("/v:on /c \"({0}) & ((echo !errorlevel!)>{1})\"", command, responseFile);
        var processStart = new ProcessStartInfo(@"cmd.exe", arguments)
        {
            CreateNoWindow = true,
            UseShellExecute = true,
            RedirectStandardError = false,
            RedirectStandardOutput = false,
            WorkingDirectory = workingDir != null ? Path.GetFullPath(workingDir) : "",
        };

        // 通过监控responseFile捕获命令输出
        int responseCode = -1;
        bool captureNothing = false;
        bool captureTimeout = false;
        DateTime startTime = DateTime.Now;
        using (Process process = Process.Start(processStart))
        {
            // 循环检测方式检查命令退出
            while (true)
            {
                if (File.Exists(responseFile))
                {
                    string response = File.ReadAllText(responseFile);
                    responseCode = int.Parse(response.Trim());
                    break;
                }

                if (timeout != -1 && DateTime.Now > startTime + TimeSpan.FromMilliseconds(timeout))
                {
                    UnityEngine.Debug.LogErrorFormat("RunCommand {0} error: timeout", actionName);
                    captureTimeout = true;
                    break;
                }

                if (process.HasExited)
                {
                    UnityEngine.Debug.LogErrorFormat("RunCommand {0} error: no exitCode captured. Process exitCode: {1}", actionName, process.ExitCode);
                    captureNothing = true;
                    break;
                }

                process.WaitForExit(100);
            }

            // 命令失败时，等待进程完成（等待任意键继续）
            if (!captureTimeout)
                process.WaitForExit();

            // 处理结果
            if (responseCode == 0)
            {
                UnityEngine.Debug.LogFormat("RunCommand {0} done.", actionName);
                return;
            }
            else
            {
                string errorInfo;
                if (captureNothing)
                    errorInfo = "no exitCode captured. Process exitCode: " + process.ExitCode;
                else if (captureTimeout)
                    errorInfo = "timeout";
                else
                    errorInfo = "exitCode: " + responseCode;

                if (errorAlert)
                {
                    EditorUtility.DisplayDialog("错误！", string.Format("{0} 失败：\n{1}", actionName, errorInfo), "确定");
                }

                if (throwError)
                    throw new Exception(errorInfo);
            }
        }


    }

    public static void SetScriptingDefine(string define, bool enabled)
    {
#if UNITY_EDITOR && UNITY_STANDALONE
        SetScriptingDefine(BuildTargetGroup.Standalone, define, enabled);
#elif UNITY_EDITOR && UNITY_ANDROID
        SetScriptingDefine(BuildTargetGroup.Android, define, enabled);
#elif UNITY_EDITOR && UNITY_IOS
        SetScriptingDefine(BuildTargetGroup.iOS, define, enabled);
#endif

    }

    public static bool GetScriptingDefine(string define)
    {
#if UNITY_EDITOR && UNITY_STANDALONE
        if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';').Contains(define))
        {
            return true;
        }
#elif UNITY_EDITOR && UNITY_ANDROID
        if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Split(';').Contains(define))
        {
            return true;
        }
#elif UNITY_EDITOR && UNITY_IOS
        if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS).Split(';').Contains(define))
        {
            return true;
        }
#endif
        return false;
    }

    public static string[] GetScriptingDefine()
    {
#if UNITY_STANDALONE
        return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';');
#elif UNITY_ANDROID
        return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Split(';');
#elif  UNITY_IOS
        return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS).Split(';');
#endif
        return new string[0];
    }
    private static void SetScriptingDefine(BuildTargetGroup buildTargetGroup, string define, bool enabled)
    {

        var str = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';');
        var defineList = new System.Collections.Generic.List<string>();
        defineList.AddRange(str);


        bool changed = false;
        if (enabled)
        {
            if (!defineList.Contains(define))
            {
                defineList.Add(define);
                changed = true;
            }

        }
        else if (!enabled)
        {
            if (defineList.Contains(define))
            {
                defineList.RemoveAll(s => s == define);
                changed = true;
            }

        }

        if (changed)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", defineList.ToArray()));
        }

    }
    internal static string Cmd(string str, string workdir = "")
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.WorkingDirectory = workdir == string.Empty ? "" : workdir;
        process.Start();

        process.StandardInput.WriteLine(str);
        process.StandardInput.AutoFlush = true;
        process.StandardInput.WriteLine("exit");

        StreamReader reader = process.StandardOutput;//截取输出流

        string output = reader.ReadLine();//每次读取一行

        while (!reader.EndOfStream)
        {
            output += reader.ReadLine();
        }

        process.WaitForExit();
        return output;
    }

    [UnityEditor.MenuItem("Tools/清除异常进度条")]
    public static void ClearEditorProgressBar()
    {
        EditorUtility.ClearProgressBar();
    }
}