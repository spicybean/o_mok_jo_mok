using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class OmokBoard : MonoBehaviour
{
    /*
     private const int BOARD_SIZE = 15;

     public int[,] board = new int[BOARD_SIZE, BOARD_SIZE];

     public void InitBoard()
     {
         for (int i = 0; i < BOARD_SIZE; i++)
         {
             for (int j = 0; j < BOARD_SIZE; j++)
             {
                 board[i, j] = 0;
             }
         }
     }

     // Start is called before the first frame update
     void Start()
     {
         InitBoard();
     }
    */

    [SerializeField] private OmokCell[] omokCells;

    public delegate void OnCellClicked(int row, int col);
    public OnCellClicked OnCellClickedDelegate;

    public void InitCells()
    {
        
    }
    
}
