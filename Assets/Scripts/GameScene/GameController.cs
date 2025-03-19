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

    void RanjuRule()
    {
        
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
