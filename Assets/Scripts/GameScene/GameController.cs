using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct Cell
{
    public int index;
    public int turn;
    public string name;
    public GameObject cellObject;
}

public class GameController : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] omokButtons;
    public Cell[,] omokBoard;
    private Color myTurn = Color.black;
    private Color enemyTurn = Color.blue;
    private string myturnName = "Black";
    private string enemyturnName = "White";
    public GameObject omokPrefab;
    private int totalomokCells;
    private int col;
    private int turncounter;
    public enum playerType{Black, White}
    public playerType turn;
    // Start is called before the first frame update
    void Start()
    {
        turncounter = 0;
        totalomokCells = gameObject.transform.childCount;
        omokButtons = new GameObject[totalomokCells];
        omokBoard = new Cell[totalomokCells/7, 7];
        Debug.Log($"{totalomokCells}, {7}");
        //foreach (Transform tr in transform)Debug.Log(tr.name);
        for (int i = 0; i < totalomokCells; i++)
        {
            omokButtons[i] = gameObject.transform.GetChild(i).GetComponent<Image>().gameObject;
            omokButtons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            omokBoard[i/7, i%7].index = i;
            omokBoard[i / 7, i % 7].name = "";
            omokBoard[i/7, i%7].cellObject = transform.GetChild(i).gameObject;
            Debug.Log($"{i/7}, {i%7},{transform.GetChild(i).gameObject.name}");
            
        }
        
        
        turn = playerType.Black;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMarker(int index, string name)
    {
        
    }
    void SetTurn(playerType player, int index)
    {
        switch (player)
        {
            case playerType.Black:
                if (omokBoard[index / 7, index % 7].name != "") return;
                omokBoard[index/7, index%7].cellObject.GetComponentInChildren<TMP_Text>().text = myturnName;
                omokBoard[index / 7, index % 7].name = myturnName;
                Debug.Log(omokButtons[index].gameObject.GetComponentInChildren<TMP_Text>().text);
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                turncounter++;
                Debug.Log($"{turncounter}");
                break;
            case playerType.White:
                if (omokBoard[index / 7, index % 7].name != "") return;
                omokBoard[index/7, index%7].cellObject.GetComponentInChildren<TMP_Text>().text = enemyturnName;
                omokBoard[index / 7, index % 7].name = enemyturnName;
                Debug.Log(omokButtons[index].gameObject.GetComponentInChildren<TMP_Text>().text);
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                turncounter++;
                break;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick" + name);
        if (PointerEventData.InputButton.Left == eventData.button)
        {
            var name = eventData.pointerCurrentRaycast.gameObject.transform.parent.name;
            for (int i = 0; i < omokButtons.Length; i++)
            {
                if (name == omokButtons[i].name)
                {
                    
                    SetTurn(turn, i);
                    
                }
            }
        }
    }
}
