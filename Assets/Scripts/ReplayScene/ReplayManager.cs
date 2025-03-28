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
    
    public enum playerType{None,Black, White}
    public playerType[,] omokBoard;
    public playerType turn;
    public GameObject[] omokButtons;
    [SerializeField] private GameObject omokBoardPath;
    public GameObject omokPrefab;
    private int totalomokCells = 15 * 15;

    private int turnCounter;
    private int lastTurnCounter;
    
    void Start()
    {
        InitReplayList();
        SetOmokBoard();
        turnCounter = -1;
        lastTurnCounter = -1;
        turn = playerType.Black;
    }

    private void InitReplayList()
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
                    replayDataList[i - 1].dateTime;
                slot.transform.GetChild(1).GetComponent<TMP_Text>().text =
                    replayDataList[i - 1].playerTier + "급 " + replayDataList[i - 1].playerName;
                slot.transform.GetChild(2).GetComponent<TMP_Text>().text =
                    replayDataList[i - 1].enemyTier + "급 " +replayDataList[i - 1].enemyName;
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

    void SetOmokBoard()
    {
        for (int i = 0; i < omokBoardPath.transform.childCount; i++)
        {
            Destroy(omokBoardPath.transform.GetChild(i).gameObject);
        }
        
        omokButtons = new GameObject[totalomokCells];
        omokBoard = new playerType[15, 15];
        
        for (int i = 0; i < totalomokCells; i++)
        {
            var OmokObejct = Instantiate(omokPrefab, omokBoardPath.transform);
            omokButtons[i] = OmokObejct;
            omokButtons[i].name = "OmokCell" + i;
            omokButtons[i].GetComponent<OmokCell>().initCell(i);
            omokBoard[i / 15, i % 15] = playerType.None;
        }
    }

    void PlaceOmok(int index)
    {
        if (turnCounter % 2 == 0)
        {
            turn = playerType.Black;
            omokButtons[index].GetComponent<OmokCell>().PlaceMark(turnCounter, OmokCell.MarkerType.Black);
        }
        else
        {
            turn = playerType.White;
            omokButtons[index].GetComponent<OmokCell>().PlaceMark(turnCounter, OmokCell.MarkerType.White);
        }
    }

    void DisplaceOmok(int index)
    {
        if (turnCounter % 2 == 0)
        {
            turn = playerType.Black;
            omokButtons[index].GetComponent<OmokCell>().PlaceMark(turnCounter, OmokCell.MarkerType.None);
        }
        else
        {
            turn = playerType.White;
            omokButtons[index].GetComponent<OmokCell>().PlaceMark(turnCounter, OmokCell.MarkerType.None);
        }
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
                currentReplayData.playerTier + "급 " + currentReplayData.playerName;
            enemyInform.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = 
                currentReplayData.enemyTier + "급 " + currentReplayData.enemyName;
        }
        
        for (int i = 0; i < currentReplayData.gamePlayData.Length; i++)
        {
            if (currentReplayData.gamePlayData[i] == -1)
            {
                lastTurnCounter = i - 1;
                break;
            }
        }
    }

    public void OnClickedFirstButton()
    {
        SetOmokBoard();
        turnCounter = -1;
    }
    public void OnClickedEndButton()
    {
        SetOmokBoard();
        for (turnCounter = 0; turnCounter <= lastTurnCounter; turnCounter++)
        {
            PlaceOmok(currentReplayData.gamePlayData[turnCounter]);
            if (turnCounter == lastTurnCounter) break;
        }
    }
    public void OnClickedBeforeButton()
    {
        if (turnCounter == -1) return;
        DisplaceOmok(currentReplayData.gamePlayData[turnCounter]);
        turnCounter--;
    }
    public void OnClickedNextButton()
    {
        if (lastTurnCounter == turnCounter) return;
        turnCounter++;
        PlaceOmok(currentReplayData.gamePlayData[turnCounter]);
    }
}
