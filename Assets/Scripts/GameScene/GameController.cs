using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


public class GameController : MonoBehaviour, IPointerClickHandler
{
    public enum playerType{None,Black, White}
    public playerType[,] omokBoard;
    public playerType turn;
    
    public enum GameState{Single, Double, Multi}
    
    public GameState gameState;
    public RanjuRule ranjuRule;
    

    
    public RankPanelController rankPanelController;
    public GameObject[] omokButtons;

    public GameObject omokPrefab;
    public GameObject selectedCell;
    
    public DateTime startTime;
    private int totalomokCells = 15 * 15;
    private int turncounter = 0;

    public int playerLife = 3;
    public int enemyLife = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        DataManager.instance.currentReplay.gamePlayData = new int[totalomokCells];
        Array.Fill(DataManager.instance.currentReplay.gamePlayData, -1);
        DataManager.instance.currentReplay.dateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        SetOmokBoard();
        ranjuRule = new RanjuRule(omokBoard);
        
        turn = playerType.Black;
        gameState = GameState.Single;
        if (gameState == GameState.Single)
        {
            // ToDo
            //playerName, playerTier, enemyTier, playerprofile, enemyprofile 연동 
            DataManager.instance.currentReplay.enemyName = "AI봇";
        }
        else if (gameState == GameState.Double)
        {
            // ToDo
            // playerTier, enemyTier, playerprofile, enemyprofile 연동 
            DataManager.instance.currentReplay.playerName = "플레이어 1";
            DataManager.instance.currentReplay.enemyName = "플레이어 2";
        }
    }

    void Update()
    {
        /*if (gameState == GameState.Single)
        {
            if (turn == playerType.White)
            {
                (int, int) aiBestMove = AIController.AIBestMove(omokBoard, turn);
                SetTurn(turn, aiBestMove.Item1 * 15 + aiBestMove.Item2);
            }
            
        }*/
    }
    void SelectStone()
    {
        //TODO: 돌 색 고르기
    }

    // public void Update()
    // {
    //     for (int i = 0; i < omokButtons.Length; i++)
    //     {
    //         if (ranjuRule.Board[i/15,i%15] == playerType.Black)
    //         {
    //             Debug.Log($"{i/15},{i%15} black");
    //         }
    //     }
    // }
    void SetOmokBoard()
    {
        omokButtons = new GameObject[totalomokCells];
        omokBoard = new playerType[15, 15];
        
        for (int i = 0; i < totalomokCells; i++)
        {
            var OmokObejct = Instantiate(omokPrefab, transform);
            omokButtons[i] = OmokObejct;
            omokButtons[i].name = "OmokCell" + i;
            omokButtons[i].GetComponent<OmokCell>().initCell(i);
            omokBoard[i / 15, i % 15] = playerType.None;
        }
    }

    public void RemoveForbiddenCells()
    {
        ranjuRule.UpdateBoardState(omokBoard);
        for (int i = 0; i < omokButtons.Length; i++)
        {
            if (omokButtons[i].GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.Forbidden)
            {
                omokButtons[i].GetComponent<Image>().sprite = omokButtons[i].GetComponent<OmokCell>().SpriteType();
                omokButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                omokButtons[i].GetComponent<OmokCell>().My_MarkerType = OmokCell.MarkerType.None;
                
            }
        }
        ranjuRule.UpdateBoardState(omokBoard);
    }
    public void SetForbiddenCell()
    {
        ranjuRule.UpdateBoardState(omokBoard);
        for (int i = 0; i < omokButtons.Length; i++)
        {
            if (omokButtons[i].GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.None)
            {
                if (ranjuRule.RanJu(i))
                {
                    omokButtons[i].GetComponent<Image>().sprite = omokButtons[i].GetComponent<OmokCell>().SpriteType(OmokCell.MarkerType.Forbidden);
                    omokButtons[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    omokButtons[i].GetComponent<OmokCell>().My_MarkerType = OmokCell.MarkerType.Forbidden;
                    
                }
            }

            if (omokButtons[i].GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.Forbidden)
            {
                if (ranjuRule.RanJu(i))
                {
                    omokButtons[i].GetComponent<Image>().sprite = omokButtons[i].GetComponent<OmokCell>().SpriteType(OmokCell.MarkerType.Forbidden);
                    omokButtons[i].GetComponent<Image>().color = new Color(1, 1, 1,1);
                    omokButtons[i].GetComponent<OmokCell>().My_MarkerType = OmokCell.MarkerType.Forbidden;
                }
                else
                {
                    omokButtons[i].GetComponent<Image>().sprite = omokButtons[i].GetComponent<OmokCell>().SpriteType();
                    omokButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    omokButtons[i].GetComponent<OmokCell>().My_MarkerType = OmokCell.MarkerType.None;
                }
            }
        }
        ranjuRule.UpdateBoardState(omokBoard);
        
    }
    
    void SetTurn(playerType player, int index)
    {
        selectedCell = null;
        
        switch (player)
        {
            case playerType.Black:
                //바둑판이 빈칸이 아니면 되돌아간다
                if (omokBoard[index / 15, index % 15] != playerType.None) return;
                //현재 턴의 유저의 바둑알 위치 표시
                omokBoard[index / 15, index % 15] = player;
                //다음턴으로 넘어 간다
                turncounter++;
                //현재 셀에 현재 턴 바둑알 둔다
                omokButtons[index].GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.Black);
                DataManager.instance.currentReplay.gamePlayData[turncounter - 1] = index;
                //턴 변경
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                RemoveForbiddenCells();
                
                break;
            case playerType.White:
                if (omokBoard[index / 15, index % 15] != playerType.None) return;
               
                omokBoard[index / 15, index % 15] = player;
                turncounter++;
                omokButtons[index].GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.White);
                DataManager.instance.currentReplay.gamePlayData[turncounter - 1] = index;
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                SetForbiddenCell();
                SetForbiddenCell();
                break;
        }

        
        
        if (ranjuRule.CheckFiveInAllDirections(index,omokBoard[index / 15, index % 15]))
        {
            //승패 알려주는 코드
            //대국이 끝남
            DataManager.instance.currentReplay.winLoseType = WinLose(omokButtons[index]);
            SaveReplay();
        }
        else if (turncounter >= totalomokCells - 10)
        {
            int count = 0;
            for (int i = 0; i < omokButtons.Length; i++)
            {
                if (omokButtons[i].GetComponent<OmokCell>().My_MarkerType != OmokCell.MarkerType.None)
                {
                    count++;
                }
            }
        
            if (count == totalomokCells)
            {
                rankPanelController.ShowRankPanel();
                rankPanelController.DrawPointsUI();
            }

            DataManager.instance.currentReplay.winLoseType =  WinLoseType.Draw;
            SaveReplay();
        }
    }

    private void SaveReplay()
    {
        //저장 부분
        ReplayData tempReplayData = DataManager.instance.currentReplay;
        for (int i = Static._maxSaveCount; i >= 1; i--)
        {
            DataManager.instance.currentReplaySlotNum = i;
            if (DataManager.instance.CheckReplayData())
            {
                //최대갯수 도달 시 가장 오래된 세이브 파일 삭제
                if (i == Static._maxSaveCount)
                {
                    DataManager.instance.DeleteReplayData();
                    continue;
                }
                //최신순 정렬
                else
                {
                    for (int j = i; j >= 1; j--)
                    {
                        //파일명 다음슬롯으로 이름 변경
                        DataManager.instance.currentReplaySlotNum = j;
                        DataManager.instance.LoadReplayData();
                        DataManager.instance.currentReplaySlotNum = j + 1;
                        DataManager.instance.SaveReplayData();
                        
                        // 1번 슬롯의 기존 세이브 지우고 새로운 세이브 추가
                        if (j == 1)
                        {
                            DataManager.instance.currentReplaySlotNum = j;
                            DataManager.instance.DeleteReplayData();
                            DataManager.instance.currentReplay = tempReplayData;
                            DataManager.instance.SaveReplayData();
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
                    DataManager.instance.SaveReplayData();
                    break;
                }
            }
        }
    }


    WinLoseType WinLose(GameObject player)
    {
        if (player.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.Black)
        {
            rankPanelController.ShowRankPanel();
            rankPanelController.GetPointsUI();
            rankPanelController.rankSystem.AddPoints();
            return WinLoseType.Win;
        }
        else if(player.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.White)
        {
            rankPanelController.ShowRankPanel();
            rankPanelController.LosePointsUI();
            rankPanelController.rankSystem.LosePoints();
            return WinLoseType.Lose;
        }
        else
        {
            return WinLoseType.Draw;
        }
    }
    void WinLose(playerType player)
    {
        if (player == playerType.Black)
        {
            rankPanelController.ShowRankPanel();
            rankPanelController.GetPointsUI();
            rankPanelController.rankSystem.AddPoints();
        }
        else if(player == playerType.White)
        {
            rankPanelController.ShowRankPanel();
            rankPanelController.LosePointsUI();
            rankPanelController.rankSystem.LosePoints();
        }
        
    }
    
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerEventData.InputButton.Left == eventData.button)
        {
            var cell = eventData.pointerCurrentRaycast.gameObject;
            var previousCellSelected = selectedCell != null ? selectedCell : cell;
            //눌렀던 셀을 한번더 누르면 바둑알을 놓는다
            if (cell.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.PlaceMark)
            {
                playerType tmp = turn;
                SetTurn(turn,cell.GetComponent<OmokCell>().index);
                if (ranjuRule.CheckWin(tmp))
                {
                    WinLose(tmp);
                }
            }
            //전에 선택되었던 셀의 선택을 취소하고 새롭게 선택된 셀에 이미지를 변경한다.
            if(cell.GetComponent<OmokCell>().My_MarkerType != OmokCell.MarkerType.PlaceMark)
            {
                if (cell.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.None)
                {
                    previousCellSelected.GetComponent<OmokCell>().PlaceMark(turncounter);
                                   
                    cell.GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.PlaceMark);
                    selectedCell = cell;
                }
                
            }
        }
    }
}


