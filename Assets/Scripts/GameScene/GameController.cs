using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



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
        startTime = DateTime.Now;
        SetOmokBoard();
        ranjuRule = new RanjuRule(omokBoard);
        
        turn = playerType.Black;
        gameState = GameState.Single;
    }

    void Update()
    {
        // Single 플레이 모드에서 AI의 턴 처리
        
    }

    void AIPlayTurn((int x,int y) playerMove)
    {
        // AI 최적 수 계산
        var bestMove = AIController.AIBestMoveMCTS(omokBoard, GameController.playerType.White,playerMove ,100);
        if (bestMove != (-1, -1)) // 유효한 수가 있으면
        {
            SetTurn(playerType.White, bestMove.Item1 * 15 + bestMove.Item2);
        }
    }
    void SelectStone()
    {
        //TODO: 돌 색 고르기
    }

   
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
                DataManager.instance.currentReplay.gamePlayData[turncounter] = index;
                
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                
                RemoveForbiddenCells();
                
                break;
            case playerType.White:
                if (omokBoard[index / 15, index % 15] != playerType.None) return;
               
                omokBoard[index / 15, index % 15] = player;
                turncounter++;
                omokButtons[index].GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.White);
                DataManager.instance.currentReplay.gamePlayData[turncounter] = index;
                
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                SetForbiddenCell();
                SetForbiddenCell();
                break;
        }
        ranjuRule.UpdateBoardState(omokBoard);
        
        
        
        if (turncounter >= totalomokCells - 10)
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
        }
        
    }


    void WinLose(GameObject player)
    {
        if (player.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.Black)
        {
            rankPanelController.ShowRankPanel();
            rankPanelController.GetPointsUI();
            rankPanelController.rankSystem.AddPoints();
        }
        else if(player.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.White)
        {
            rankPanelController.ShowRankPanel();
            rankPanelController.LosePointsUI();
            rankPanelController.rankSystem.LosePoints();
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
                AIPlayTurn((cell.GetComponent<OmokCell>().index/15,cell.GetComponent<OmokCell>().index%15));
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


