using UnityEngine;
using Newtonsoft.Json;

public class JsonHelper
{
    private const string LocalDataPath = "Resources/LocalData";
    private const string FileType = ".json"; 

    public static T DeserializeFileSimple<T>()
    {
        return DeserializeObject<T>(LoadJsonFile(typeof(T).ToString()));
    }
    public static string SerializeFileSimple<T>(T t)
    {
        string json = SerializeObject(t);

        ExportJsonFile(typeof(T).ToString(), json);

        return json;
    }
    public static T DeserializeFile<T>(string fileName)
    {
        return DeserializeObject<T>(LoadJsonFile(fileName));
    }
    public static string SerializeFile<T>(string fileName,T t)
    {
        string json = SerializeObject(t);

        ExportJsonFile(fileName, json);

        return json;
    }
    public static T DeserializeObject<T>(string json)
    { 
        return JsonConvert.DeserializeObject<T>(json);
    }
    public static string SerializeObject<T>(T t)
    {
        return JsonConvert.SerializeObject(t);
    }
    private static string LoadJsonFile(string fileName)
    {
        return FileHelper.LoadFile(string.Format("{0}/{1}", Application.dataPath, LocalDataPath), fileName, FileType);
    }
    private static void ExportJsonFile(string fileName,string json)
    {
        FileHelper.ExportFile(json, string.Format("{0}/{1}", Application.dataPath, LocalDataPath), fileName, FileType);
    }
}
