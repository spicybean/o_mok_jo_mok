using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainScenePanelController : MonoBehaviour
{
    // private CoinController coinController;
    [SerializeField] private GameObject[] panels;

    //InputFields
    [SerializeField] private TMP_InputField logInEmail;
    [SerializeField] private TMP_InputField logInPassword;
    [SerializeField] private Image signUpImage;
    [SerializeField] private TMP_InputField signUpUsername;
    [SerializeField] private TMP_InputField signUpPassword;
    [SerializeField] private TMP_InputField signUpConfirmPassword;
    [SerializeField] private TMP_InputField signUpEmail;
    [SerializeField] private GameObject mainScenePanel;
    [SerializeField] private GameObject gamePlayPanel;

    private void Awake()
    {
        // coinController = FindObjectOfType<CoinController>();
    }

    private void Start()
    {
        CloseButton();
        panels[0].SetActive(true);
        RefreshProfile();
    }
    
    #region PanelControl

    public void PanelControl(int panelIndex)
    {
        CloseButton();
        ShowPanel(panelIndex);
    }
    
    // 패널 닫기
    public void CloseButton()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    // 패널 찾기
    public void ShowPanel(int panelIndex)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == panelIndex)
            {
                panels[i].SetActive(true);
            }
        }
    }

    #endregion
    
    #region LoginPanel

    // Login Failed Panel
    public void OnClickLoginFailedButton()
    {
        PanelControl(0);
    }

    // Login Failed Confirm Button
    public void OnClickLoginConfirmButton()
    {
        if (DataManager.instance.CheckEmailAlreadyExists(logInEmail.text) && DataManager.instance.ComparePassword(logInEmail.text, logInPassword.text))
        {
            DataManager.instance.SetCurrentUserAccountData(logInEmail.text);
            CloseButton();
            InitMainScenePanel();
            InitGamePlayPanel();
        }
        else
        {
            PanelControl(1);
        }
    }
    
    // Signup Panel
    public void OnClickSignupButton()
    {
        
        PanelControl(3);
    }
    
    // Signup Panel - Login Button
    public void OnClickSignupLoginButton()
    {
        if (!DataManager.instance.CheckEmailAlreadyExists(signUpEmail.text) && signUpConfirmPassword.text == signUpPassword.text)
        {
            DataManager.instance.currentUserAccount = new UserAccountData();
            DataManager.instance.currentUserAccount.userIndex = DataManager.instance.userAccountList.Count;
            DataManager.instance.currentUserAccount.usertier = 18;
            DataManager.instance.currentUserAccount.coin = 500;
            DataManager.instance.currentUserAccount.username = signUpUsername.text;
            DataManager.instance.currentUserAccount.password = signUpPassword.text;
            DataManager.instance.currentUserAccount.email = signUpEmail.text;
            DataManager.instance.currentUserAccount.image = signUpImage.sprite;
            DataManager.instance.userAccountList.Insert(DataManager.instance.currentUserAccount.userIndex, DataManager.instance.currentUserAccount);
            DataManager.instance.SaveAccountsData();
            CloseButton();
            PanelControl(0);
        }
        else if (!DataManager.instance.CheckEmailAlreadyExists(signUpEmail.text) && signUpConfirmPassword.text != signUpPassword.text)
        {
            PanelControl(2);
        }
        else
        {
            PanelControl(4);
        }
    }
    
   

    #endregion

    #region MainScenePanel

    private void InitMainScenePanel()
    {
        mainScenePanel.transform.GetChild(0).GetComponent<TMP_Text>().text = 
            DataManager.instance.userAccountList[DataManager.instance.currentUserAccount.userIndex].coin.ToString();
        mainScenePanel.transform.GetChild(1).GetComponent<Image>().sprite =
            DataManager.instance.userAccountList[DataManager.instance.currentUserAccount.userIndex].image;
        mainScenePanel.transform.GetChild(2).GetComponent<TMP_Text>().text =
            DataManager.instance.userAccountList[DataManager.instance.currentUserAccount.userIndex].usertier + "급 "
            + DataManager.instance.userAccountList[DataManager.instance.currentUserAccount.userIndex].username;
    }

    // 게임 시작 버튼
    public void OnClickGamePlayButton()
    {
        PanelControl(5);
        CoinController.Instance.GamePlayCoinChanged();
    }

    public void OnClickReplayButton()
    {
        // Replay Scene 넘어가기
        SceneManager.LoadScene("ReplayScene");
    }
    
    public void OnClickGameSinglePlayButton()
    {
        Debug.Log("currentCoin :" + DataManager.instance.currentUserAccount.coin);
        Debug.Log("currentName :" + DataManager.instance.currentUserAccount.username);
        if (DataManager.instance.currentUserAccount.coin >= 100)
        {
            CoinController.Instance.CoinTextChanged(-100);
            CoinController.Instance.GamePlayCoinChanged();
            DataManager.instance.currentReplay.gameState = GameController.GameState.Single;
            // Replay Scene 넘어가기
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            PanelControl(12);
        }
    }
    
    public void OnClickGameDoublePlayButton()
    {
        Debug.Log("currentCoin :" + DataManager.instance.currentUserAccount.coin);
        Debug.Log("currentName :" + DataManager.instance.currentUserAccount.username);
        if (DataManager.instance.currentUserAccount.coin >= 100)
        {
            CoinController.Instance.CoinTextChanged(-100);
            CoinController.Instance.GamePlayCoinChanged();
            DataManager.instance.currentReplay.gameState = GameController.GameState.Double;
            // Replay Scene 넘어가기
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            PanelControl(12);
        }
    }
    
    // 랭킹 버튼
    public void OnClickRankingButton()
    {
        //PanelControl(6);
        SceneManager.LoadScene("RankingScene");
    }

    // 상점 버튼
    public void OnClickStoreButton()
    {
        PanelControl(7);
    }
    
    // Store - 300 Coin Button
    public void OnClickGetCoinButton()
    {
        PanelControl(8);
        CoinController.Instance.CoinTextChanged(300);
    }

    public void OnClickGet3000CoinButton()
    {
        PanelControl(9);
        CoinController.Instance.CoinTextChanged(3000);
    }
    
    public void OnClickGet6000CoinButton()
    {
        PanelControl(10);
        CoinController.Instance.CoinTextChanged(6000);
    }
    
    public void OnClickGet10000CoinButton()
    {
        PanelControl(11);
        CoinController.Instance.CoinTextChanged(10000);
    }

    
    
    // Main Scene Panel - Back Button
    public void OnClickMainScenePanelBackButton()
    {
        CloseButton();
    }

    public void OnClickSignUpProfileButton()
    {
        ShowPanel(14);
        ShowPanel(13);
    }

    public void OnClickSignUpProfileImageButton()
    {
        signUpImage.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
        PanelControl(3);
    }

    public void RefreshProfile()
    {
        InitMainScenePanel();
        InitGamePlayPanel();
    }
    
    #endregion
    
    #region GamePlayPanel
    private void InitGamePlayPanel()
    {
        gamePlayPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = 
            DataManager.instance.userAccountList[DataManager.instance.currentUserAccount.userIndex].coin.ToString();
        gamePlayPanel.transform.GetChild(2).GetComponent<Image>().sprite =
            DataManager.instance.userAccountList[DataManager.instance.currentUserAccount.userIndex].image;
        gamePlayPanel.transform.GetChild(3).GetComponent<TMP_Text>().text =
            DataManager.instance.userAccountList[DataManager.instance.currentUserAccount.userIndex].usertier + "급 "
            + DataManager.instance.userAccountList[DataManager.instance.currentUserAccount.userIndex].username;
    }
    
    #endregion
}
