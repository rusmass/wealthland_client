using UnityEngine;
using System.IO;

public class FileHelper
{
    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool IsFileExists(string filePath)
    {
        return File.Exists(filePath);
    }
    /// <summary>
    /// 判断文件夹是否存在
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <returns></returns>
    public static bool IsDirectoryExists(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }
    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="directoryPath">文件夹路径</param>
    /// <param name="fileName">文件名</param>
    /// <param name="fileType">文件类型</param>
    /// <returns></returns>
    public static string LoadFile(string directoryPath, string fileName,string fileType)
    {
        string filePath = string.Format("{0}/{1}{2}", directoryPath, fileName, fileType);

        if (IsDirectoryExists(directoryPath))
        {
            return LoadFile(filePath);
        }

        Debug.Log("不存在文件夹," + directoryPath);
        return "";
    }
    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    public static string LoadFile(string filePath)
    {
        if (IsFileExists(filePath))
        {
            StreamReader sr = File.OpenText(filePath);
            string json = sr.ReadToEnd();
            sr.Dispose();
            return json;
        }

        Debug.Log("不存在此文件路径," + filePath);
        return "";
    }
    /// <summary>
    /// 导出文件
    /// </summary>
    /// <param name="context"></param>
    /// <param name="directoryPath"></param>
    /// <param name="fileName"></param>
    /// <param name="fileType"></param>
    public static void ExportFile(string context, string directoryPath, string fileName, string fileType)
    {
        string filePath = string.Format("{0}/{1}{2}", directoryPath, fileName, fileType);

        if (!IsDirectoryExists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        ExportFile(context, filePath);
    }
    /// <summary>
    /// 导出文件
    /// </summary>
    /// <param name="context"></param>
    /// <param name="filePath"></param>
    public static void ExportFile(string context,string filePath)
    {
        StreamWriter sw = null;
        if (!IsFileExists(filePath))
        {
            sw = File.CreateText(filePath);
        }
        else
        {
            sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
        }
        if (sw != null)
        {
            sw.Write(context.ToCharArray());
            sw.Dispose();
        }
    }
}
