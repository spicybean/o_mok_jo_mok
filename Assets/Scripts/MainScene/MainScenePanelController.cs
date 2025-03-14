using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScenePanelController : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    
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
        CloseButton();
    }

    #endregion

    #region MainScenePanel

    // 게임 시작 버튼
    public void OnClickGamePlayButton()
    {
        PanelControl(5);
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
