using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReplayManager : MonoBehaviour
{
    [SerializeField] private ReplayScenePanelController replayScenePanelController;
    [SerializeField] private GameObject playerInform;
    [SerializeField] private GameObject enemyInform;
    [SerializeField] private GameObject slotArray;
    [SerializeField] private GameObject slotPrefab;
    
    ReplayData[] replayDataList = new ReplayData[Static._maxSaveCount];//리플레이가 모두 저장된 배열
    private ReplayData currentReplayData;                       //리플레이 할 데이터 저장 변수
    
    void Start()
    {
        for (int i = 1; i <= Static._maxSaveCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotArray.transform);
            slot.name = "[Button] Replay " + i;
            DataManager.instance.currentReplaySlotNum = i;
            if (DataManager.instance.CheckReplayData())
            {
                slot.GetComponent<Button>().onClick.AddListener(() => OnSlotClicked());
                replayDataList[i-1] = DataManager.instance.LoadReplayData();
                slot.transform.GetChild(0).GetComponent<TMP_Text>().text =
                    replayDataList[i - 1].datetime.ToString("yyyy/MM/dd HH:mm:ss");
                slot.transform.GetChild(1).GetComponent<TMP_Text>().text =
                    replayDataList[i - 1].playerTier + " " + replayDataList[i - 1].playerName;
                slot.transform.GetChild(2).GetComponent<TMP_Text>().text =
                    replayDataList[i - 1].enemyTier + " " +replayDataList[i - 1].enemyName;
                if (replayDataList[i - 1].winLoseType == WinLoseType.Win)
                {
                    slot.transform.GetChild(3).GetComponent<Image>().sprite = 
                        Resources.Load<Sprite>("Images/Win_Icon");
                    slot.transform.GetChild(4).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Images/Lose_Icon");
                }
                else if (replayDataList[i - 1].winLoseType == WinLoseType.Lose)
                {
                    slot.transform.GetChild(3).GetComponent<Image>().sprite = 
                        Resources.Load<Sprite>("Images/Lose_Icon");
                    slot.transform.GetChild(4).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Images/Win_Icon");
                }
                else
                {
                    slot.transform.GetChild(3).GetComponent<Image>().sprite = 
                        Resources.Load<Sprite>("Images/Draw_Icon");
                    slot.transform.GetChild(4).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Images/Draw_Icon");
                }
            }
            else
            {
                slot.transform.GetChild(5).gameObject.SetActive(true);
            }
        }

        DataManager.instance.ClearReplayData();
    }

    public void OnSlotClicked()
    {
        //슬롯 버튼의 이름에서 Slot번호를 추출하여 DataManager에 전달.
        DataManager.instance.currentReplaySlotNum =
            int.Parse(EventSystem.current.currentSelectedGameObject.name.Split(' ')[2]);
        if (DataManager.instance.CheckReplayData())
        {
            currentReplayData = replayDataList[DataManager.instance.currentReplaySlotNum-1];
            replayScenePanelController.OnClickReplaySceneButton();
            playerInform.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = 
                currentReplayData.playerImage;
            enemyInform.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = 
                currentReplayData.enemyImage;
            playerInform.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = 
                currentReplayData.playerTier + " " + currentReplayData.playerName;
            enemyInform.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = 
                currentReplayData.enemyTier + " " + currentReplayData.enemyName;
        }
    }

    public void OnClickedFirstButton()
    {
        
    }
    public void OnClickedEndButton()
    {
        
    }
    public void OnClickedBeforeButton()
    {
        
    }
    public void OnClickedNextButton()
    {
        
    }
}
