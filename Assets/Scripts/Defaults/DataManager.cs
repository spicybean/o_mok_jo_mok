using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

//GameData(UserAccount)
[Serializable]
public class UserAccountData
{
    public int userIndex;
    public string username;
    public string email;
    public string password;
    public int usertier;
    public int points;
    public int totalmatch;
    public int winmatch;
    public int losematch;
    public int tiematch;
    public Sprite image;
    public int coin;

    
    // ranking panel 로 보내기
    public float GetWinRate()
    {
        if (totalmatch <= 0) return 0;
        return ((float) winmatch + tiematch * 0.5f) / (float)totalmatch;
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
    public GameController.GameState gameState;
    public WinLoseType winLoseType;
    public string playerName;
    public string enemyName;
    public Sprite playerImage;
    public Sprite enemyImage;
    public string starterName;
    public int playerTier;
    public int enemyTier;
    public int[] gamePlayData;
    public string dateTime;
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
    public UserAccountData currentUserAccount = new UserAccountData();
    

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

    private void Start()
    {
        InitScene();
        DataManager.instance.userAccountList = DataManager.instance.LoadAccountsData();
    }

    public void InitScene()
    {
        SceneManager.LoadScene("MainScene");
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
    
    public UserAccountData SetCurrentUserAccountData(string email)
    {
        for (int i = 0; i < userAccountList.Count; i++)
        {
            if (userAccountList[i].email == email)
            {
                currentUserAccount = userAccountList[i];
            }
        }
        return currentUserAccount;
    }

    public UserAccountData SaveCurrentUserAccountData(int userIndex)
    {
        return userAccountList[userIndex] = currentUserAccount;
    }
    
    public bool CheckEmailAlreadyExists(string email)
    {
        foreach (var userData in DataManager.instance.userAccountList)
        {
            if (userData.email == email) return true;
            else continue;
        }

        return false;
    }

    public bool ComparePassword(string email, string password)
    {
        foreach (var userData in DataManager.instance.userAccountList)
        {
            if (userData.email == email)
            {
                if (userData.password == password)
                {
                    return true;
                }
            }
        }

        return false;
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
        //저장 부분
        ReplayData tempReplayData = currentReplay;
        for (int i = Static._maxSaveCount; i >= 1; i--)
        {
            currentReplaySlotNum = i;
            if (CheckReplayData())
            {
                //최대갯수 도달 시 가장 오래된 세이브 파일 삭제
                if (i == Static._maxSaveCount)
                {
                    DeleteReplayData();
                    continue;
                }
                //최신순 정렬
                else
                {
                    for (int j = i; j >= 1; j--)
                    {
                        //파일명 다음슬롯으로 이름 변경
                        currentReplaySlotNum = j;
                        LoadReplayData();
                        currentReplaySlotNum = j + 1;
                        string data = JsonUtility.ToJson(currentReplay);
                        RefreshReplaySavePath();
                        File.WriteAllText(replayPath, data);

                        // 1번 슬롯의 기존 세이브 지우고 새로운 세이브 추가
                        if (j == 1)
                        {
                            currentReplaySlotNum = j;
                            DeleteReplayData();
                            currentReplay = tempReplayData;
                            data = JsonUtility.ToJson(currentReplay);
                            RefreshReplaySavePath();
                            File.WriteAllText(replayPath, data);
                            break;
                        }
                    }
                }

                break;
            }
            else
            {
                if (i == 1)
                {
                    string data = JsonUtility.ToJson(currentReplay);
                    data = JsonUtility.ToJson(currentReplay);
                    RefreshReplaySavePath();
                    File.WriteAllText(replayPath, data);
                    break;
                }
            }
        }
    }

    //리플레이 데이터를 불러옴
    public ReplayData LoadReplayData()
    {
        RefreshReplaySavePath();
        string data = File.ReadAllText(replayPath);
        currentReplay = JsonUtility.FromJson<ReplayData>(data);
        return currentReplay;
    }

    //리플레이관련 저장 정보를 DataManager에서 초기화 함
    public void ClearReplayData()
    {
        currentReplaySlotNum = -1;
        currentReplay = new ReplayData(); ; 
    }

    public void DeleteReplayData()
    {
        RefreshReplaySavePath();
        File.Delete(replayPath);
    }
    
    #endregion
}
