using UnityEngine;
using Client;

public class Test : MonoBehaviour
{
	void Start ()
    {
        Read();
        //ReadAll();
        Save();
        //SaveAll();
    }
    public void Save()
    {
        PlayerInfo info = new PlayerInfo();
        info.RoundNumber = 5;
        info.playerName = "lisi";
        LocalDataManager.Instance.WriteFile(info, FileNames.Test);

    }
    public void SaveAll()
    {
        LocalDataManager.Instance.SaveData();
    }
    public void Read()
    {
        PlayerInfo info = LocalDataManager.Instance.ReadFile<PlayerInfo>(FileNames.Test);

        if (info != null)
        {
            Debug.Log(info.RoundNumber+"    "+info.playerName);
        }

    }
    public void ReadAll()
    {
        LocalDataManager.Instance.ReadData();
    }
}
