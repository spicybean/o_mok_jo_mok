using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScenePanelController : MonoBehaviour
{
    [SerializeField] private GameObject giveUpPanel;

    public void OnClickGiveUpPanel()
    {
        giveUpPanel.SetActive(true);
    }
    
    public void OnClickGiveUpButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void OnClickCancelButton()
    {
        giveUpPanel.SetActive(false);
    }
    
    
    
    
}
