using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class GameController : MonoBehaviour, IPointerClickHandler
{
    public enum playerType{None,Black, White}
    public GameObject[] omokButtons;
    public playerType[,] omokBoard;
    
    public GameObject omokPrefab;
    private int totalomokCells;
    private int turncounter;
    public GameObject selectedCell;
    public playerType turn;
   
    // Start is called before the first frame update
    void Start()
    {
        selectedCell = null;
        turncounter = 0;
        totalomokCells = 15*15;
        omokButtons = new GameObject[totalomokCells];
        omokBoard = new playerType[15, 15];
        Debug.Log($"{totalomokCells}, {7}");
        //foreach (Transform tr in transform)Debug.Log(tr.name);
        for (int i = 0; i < totalomokCells; i++)
        {
            var OmokObejct = Instantiate(omokPrefab, transform);
            omokButtons[i] = OmokObejct;
            omokButtons[i].name = "OmokCell" + i;
            omokButtons[i].GetComponent<OmokCell>().initCell(i);
            omokBoard[i / 15, i % 15] = playerType.None;

        }
        
        
        turn = playerType.Black;
        
    }
    void StartGame()
    {
        
    }
    void CheckOmok(int row, int col)
    {
        (int,int) center = (row,col);
        //directions[0]: 가로  directions[1]: 세로 directions[2]: 대각선 directions[3]: 반대 대각선
        (int,int)[] directions = new (int,int)[]{(0,1),(1,0),(1,1),(-1,1)};
        for (int i = 0; i < 5; i++)
        {
            
        }
    }
    
    void CheckDoubleThreeFour(int row, int col)
    {
        (int,int)[] directions = new (int,int)[]{(0,1),(1,0),(1,1),(-1,1)};
        
        //row >= 0 && row <= Fullindexsize/ 7
        //row count 4 from center to every direction
        //col >= 0 && col <= Fullindexsize % 7
        //col count 4 from center to every direction
        //만약 가로,세로,대각선중 중심에 돌을 두었을시 오목이 되면 적용 안됨
        //중심을 기준으로 3칸씩 총 6칸 사이에 흰돌이 없으면 4.4 3.3 적용 아니면 괜찮
        //단, 그 중심점에 두었을때 오목이 된다면 무시 가능
        //단, 각 
        //3.3일때
        
        //3.4일때
        //4.4일때
    }
    void SetTurn(playerType player, int index)
    {
        selectedCell = null;
        
        switch (player)
        {
            case playerType.Black:
                if (omokBoard[index / 15, index % 15] != playerType.None) return;
                //Debug.Log($"{player}, {index}, {omokBoard[index / 15, index % 15]}");
                omokBoard[index / 15, index % 15] = player;
                turncounter++;
                omokButtons[index].GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.Black);
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                break;
            case playerType.White:
                if (omokBoard[index / 15, index % 15] != playerType.None) return;
               // Debug.Log($"{player}, {index}, {omokBoard[index / 15, index % 15]}");
                omokBoard[index / 15, index % 15] = player;
                turncounter++;
                omokButtons[index].GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.White);
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                break;
        }
    }

   
    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (PointerEventData.InputButton.Left == eventData.button)
        {
            var cell = eventData.pointerCurrentRaycast.gameObject;
            var previousCellSelected = selectedCell != null ? selectedCell : cell;
            
            
            //눌렀던 셀을 한번더 누르면 바둑알을 놓는다
            if (cell.GetComponent<OmokCell>().GetMarkerType == OmokCell.MarkerType.PlaceMark)
            {
                SetTurn(turn,cell.GetComponent<OmokCell>().index);
            }
            //전에 선택되었던 셀의 선택을 취소하고 새롭게 선택된 셀에 이미지를 변경한다.
            if(cell.GetComponent<OmokCell>().GetMarkerType != OmokCell.MarkerType.PlaceMark)
            {
                
                if (cell.GetComponent<OmokCell>().GetMarkerType == OmokCell.MarkerType.None)
                {
                    previousCellSelected.GetComponent<OmokCell>().PlaceMark(turncounter);
                                   
                    cell.GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.PlaceMark);
                    selectedCell = cell;
                }
                
               
            }
            
            
        }
    }
}
