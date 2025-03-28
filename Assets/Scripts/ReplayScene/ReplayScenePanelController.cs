using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayScenePanelController : MonoBehaviour
{
    [SerializeField] private GameObject replayScenePanel;

    public void OnClickBackToMainSceneButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void OnClickReplaySceneButton()
    {
        replayScenePanel.SetActive(true);
    }

    public void OnClickBackButton()
    {
        replayScenePanel.SetActive(false);
    }
}
