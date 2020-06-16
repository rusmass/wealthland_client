using UnityEditor;
using System;
using System.IO;
using Core;

public class CheckWindow
{
    [MenuItem("*Resource/CheckRes")]
    public static void CheckArpgRes()
    {
        var paths = Directory.GetFiles(PathTools.ExportResourceRoot, "*.*", SearchOption.AllDirectories);
        ScanTools.ScanAll("CheckRes", paths, path => {
            for (int i = 0; i < path.Length; ++i)
            {
                if ((int)path[i] > 127)
                {
                    Console.WriteLine(path);
                    File.Delete(path);
                    break;
                }
            }
        });
    }
}