using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//GameData(UserAccount)
public class UserAccountData
{
    public string username;
    public string nickname;
    public string password;
    //etc...
}

//GameData(ReplayMode)
public class ReplayData
{
    public string playerName;
    public string enemyName;
    public int playerTier;
    public int enemyTier;
    public int[,,] gamePlayData;
    public DateTime datetime;
    public bool bIsPlayerWin;
}
public class DataManager : MonoBehaviour
{
    //Singleton(모든 씬에서 접근 가능)
    public static DataManager instance;
    
    //SavePath(저장경로)
    public string dataPath;
    public string replayPath;
    
    //Current GameData(ReplayMode)
    public int currentReplaySlotNum;
    public ReplayData currentReplay = new ReplayData();

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion
        
        dataPath = Application.persistentDataPath;
    }
    
    #region Account Save Load Functions
    /// <summary>
    /// 세이브 파일 저장 경로 : %appdata%/localLow/DefaultCompany/O_mok_jo_mok/userAccount.json
    /// </summary>
    public void SaveAccountData()
    {
        
    }

    public void LoadAccountData()
    {
        
    }
    #endregion
    
    #region Replay Save Load Functions

    /// <summary>
    /// 세이브 파일 저장 경로 : %appdata%/localLow/DefaultCompany/O_mok_jo_mok/saveData{슬롯번호}.json
    /// </summary>
    private void RefreshReplaySavePath()
    {
        replayPath = dataPath + "/replayData"+ currentReplaySlotNum + ".json";
    }
    
    public bool CheckReplaySave()
    {
        RefreshReplaySavePath();
        if (File.Exists(replayPath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SaveReplayData()
    {
        string data = JsonUtility.ToJson(currentReplay);
        RefreshReplaySavePath();
        File.WriteAllText(replayPath, data);
    }

    public ReplayData LoadReplayData()
    {
        RefreshReplaySavePath();
        string data = File.ReadAllText(replayPath);
        return JsonUtility.FromJson<ReplayData>(data);
    }

    public void ClearReplayData()
    {
        currentReplaySlotNum = -1;
        currentReplay = null ; 
    }
    #endregion
}
