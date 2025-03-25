using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

//GameData(UserAccount)
public class UserAccountData
{
    public string username;
    public string nickname;
    public string password;
    public int usertier;
    public int points;
    public int totalmatch;
    public int winmatch;
    public int losematch;
    public int tiematch;
    public Image image;

    public static List<UserAccountData> GetUserAccountData()
    {
        return new List<UserAccountData>()
        {
            new UserAccountData("user1", 18, 2, 10, 5, 3, 2),
            new UserAccountData("user2", 17, 0, 20, 10, 5, 5),
            new UserAccountData("user3", 5, 4, 30, 15, 10, 5),
            new UserAccountData("user4", 3, 5, 40, 20, 15, 5),
        };
        
    }
    public UserAccountData() { }

    public UserAccountData(string _username, int _usertier, int _points, int _totalmatch, int _winmatch, int _losematch, int _tiematch)
    {
        username = _username;
        usertier = _usertier;
        points = _points;
        totalmatch = _totalmatch;
        winmatch = _winmatch;
        losematch = _losematch;
        tiematch = _tiematch;
        
    }

    public float GetWinRate()
    {
        if (totalmatch <= 0) return 0;
        return (float) winmatch + tiematch * 0.5f / (float)totalmatch;
    }   
    //etc...
}

public enum WinLoseType
{
    Win,
    Lose,
    Draw
}

//GameData(ReplayMode)
public class ReplayData
{
    public WinLoseType winLoseType;
    public string playerName;
    public string enemyName;
    public Sprite playerImage;
    public Sprite enemyImage;
    public string starterName;
    public int playerTier;
    public int enemyTier;
    public int[] gamePlayData;
    public DateTime datetime;
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
        string data = JsonUtility.ToJson(userAccountList);
        RefreshReplaySavePath();
        File.WriteAllText(accountPath, data);
    }

    public UserAccountData LoadAccountsData()
    {
        
        string data = File.ReadAllText(accountPath);
        return JsonUtility.FromJson<UserAccountData>(data);
    }

    public void AccountsSavePath()
    {
        accountPath = dataPath + "/userAccount"+userAccountList+".json";
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
        //Debug.Log(replayPath);
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

    public void DeleteReplayData()
    {
        File.Delete(replayPath);
    }
    #endregion
}
