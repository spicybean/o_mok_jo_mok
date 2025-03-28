using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainScenePanelController : MonoBehaviour
{
    // private CoinController coinController;
    [SerializeField] private GameObject[] panels;

    //InputFields
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField emailInputField;

    private void Awake()
    {
        // coinController = FindObjectOfType<CoinController>();
    }

    private void Start()
    {
        CloseButton();
        panels[0].SetActive(true);
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
        PanelControl(1);
    }

    // Login Failed Confirm Button
    public void OnClickLoginConfirmButton()
    {
        PanelControl(0);
    }
    
    // Signup Panel
    public void OnClickSignupButton()
    {
        
        PanelControl(3);
    }
    
    // Signup Panel - Login Button
    public void OnClickSignupLoginButton()
    {
        DataManager.instance.userAccountList = DataManager.instance.LoadAccountsData();
        UserAccountData userAccountData = new UserAccountData();
        userAccountData.usertier = 18;
        userAccountData.username = usernameInputField.text;
        userAccountData.password = passwordInputField.text;
        userAccountData.email = emailInputField.text;
        DataManager.instance.userAccountList.Add(userAccountData);
        DataManager.instance.SaveAccountsData();
        Debug.Log(DataManager.instance.userAccountList.Count);
        CloseButton();
    }

    #endregion

    #region MainScenePanel

    // 게임 시작 버튼
    public void OnClickGamePlayButton()
    {
        PanelControl(5);
        CoinController.Instance.GamePlayCoinChanged();
    }

    public void OnClickReplayButton()
    {
        CoinController.Instance.CoinTextChanged(-100);
        if (CoinController.Instance.currentCoin < 0)
        {
            CoinController.Instance.CoinTextChanged(100);
            PanelControl(12);
        }
        else
        {
            // Replay Scene 넘어가기
            SceneManager.LoadScene("ReplayScene");
        }
    }

    public void OnClickGamePlaySingleAndMultySceneButton()
    {
        CoinController.Instance.CoinTextChanged(-100);
        
        if (CoinController.Instance.currentCoin < 0)
        {
            CoinController.Instance.CoinTextChanged(100);
            CoinController.Instance.GamePlayCoinChanged();
            PanelControl(12);
        }
        else
        {
            CoinController.Instance.GamePlayCoinChanged();
            // Game Scene 넘어가기
            SceneManager.LoadScene("GameScene");
        }
    }
    
    // 랭킹 버튼
    public void OnClickRankingButton()
    {
        PanelControl(6);
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

    // 셋팅 버튼
    public void OnClickSettingButton()
    {
    }
    
    // Main Scene Panel - Back Button
    public void OnClickMainScenePanelBackButton()
    {
        CloseButton();
    }
    
    #endregion
    
}
