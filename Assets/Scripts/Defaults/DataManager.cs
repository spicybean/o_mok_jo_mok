using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

//GameData(UserAccount)
[Serializable]
public class UserAccountData
{
    public string username;
    public string email;
    public string password;
    public int usertier;
    public int points;
    public int totalmatch;
    public int winmatch;
    public int losematch;
    public int tiematch;
    public Image image;


    
    // ranking panel 로 보내기
    public float GetWinRate()
    {
        if (totalmatch <= 0) return 0;
        return (float) winmatch + tiematch * 0.5f / (float)totalmatch;
    }   
    //etc...
}



//GameData(ReplayMode)
public class ReplayData
{
    public string playerName;
    public string enemyName;
    public string starterName;
    public int playerTier;
    public int enemyTier;
    public int[] gamePlayData;
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
    public string accountPath;

    //Current GameData(ReplayMode)
    public int currentReplaySlotNum;
    public ReplayData currentReplay = new ReplayData();
    public List<UserAccountData> userAccountList = new List<UserAccountData>();

    // List 만들고

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
   
    public void SaveAccountsData()
    {
        try
        {
            AccountsSavePath();
            string data = JsonUtility.ToJson(new UserAccountListWrapper { accounts = userAccountList }, true);
           Debug.Log(data);
            File.WriteAllText(accountPath, data);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save account data: " + e.Message);
        }
    }

    [Serializable]
    public class UserAccountListWrapper
    {
        public List<UserAccountData> accounts;
    }

    public List<UserAccountData> LoadAccountsData()
    {
        AccountsSavePath();
        if (!File.Exists(accountPath))
        {
            Debug.LogWarning("Account data file not found. Creating a new one.");
            return new List<UserAccountData>();
        }
        try
        {
            string data = File.ReadAllText(accountPath);
            UserAccountListWrapper loadedData = JsonUtility.FromJson<UserAccountListWrapper>(data);
            return loadedData?.accounts ?? new List<UserAccountData>();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load account data: " + e.Message);
            return new List<UserAccountData>();
        }
    }

    public void AccountsSavePath()
    {
        accountPath = Path.Combine(dataPath, "userAccount.json");
    }
    #endregion

    #region Replay Save Load Functions

    /// <summary>
    /// 세이브 파일 저장 경로 : %appdata%/localLow/DefaultCompany/O_mok_jo_mok/saveData{슬롯번호}.json
    /// </summary>

    //리플레이 데이터의 경로를 파일명에 따라 동기화 함
    private void RefreshReplaySavePath()
    {
        replayPath = dataPath + "/replayData"+ currentReplaySlotNum + ".json";
    }
    
    //리플레이 데이터가 ReplayPath에 존재하는지 확인
    public bool CheckReplayData()
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
    
    //리플레이 데이터를 저장함
    public void SaveReplayData()
    {
        string data = JsonUtility.ToJson(currentReplay);
        RefreshReplaySavePath();
        File.WriteAllText(replayPath, data);
    }

    //리플레이 데이터를 불러옴
    public ReplayData LoadReplayData()
    {
        RefreshReplaySavePath();
        string data = File.ReadAllText(replayPath);
        return JsonUtility.FromJson<ReplayData>(data);
    }

    //리플레이관련 저장 정보를 DataManager에서 초기화 함
    public void ClearReplayData()
    {
        currentReplaySlotNum = -1;
        currentReplay = null ; 
    }
    #endregion
}
