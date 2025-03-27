using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RanjuRule
{
   
    public GameController.playerType[,] Board;
    private int Xrange;
    private int Yrange;
    public RanjuRule( GameController.playerType[,] board)
    {
        
        this.Board = board;
        Xrange = Board.GetLength(0);
        Yrange = Board.GetLength(1) ;
    }
    
    public void UpdateBoardState(GameController.playerType[,] board)
    {
        Board = board;
        
    }

    public bool CheckWin(GameController.playerType player)
    {
        for (var i = 0; i < Board.GetLength(0); i++)
        {
            for (var j = 0; j < Board.GetLength(1); j++)
            {
                if (CheckFiveInAllDirections(i * 15 + j, player))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool RanJu(int index, GameController.playerType player = GameController.playerType.Black)
    {
        
        bool isDoubleThree = CheckDoubleThree(index,player);
        bool isDoubleFour = CheckDoubleFour(index,player);
        bool isOverFive = CheckJangMok(index,player);

        if (isDoubleThree || isDoubleFour || isOverFive)
        {
            Debug.Log(isDoubleThree ? "RanjuThree" : isDoubleFour ? "RanjuFour" : "RanjuJang");
            return true;
        }
        return false;
    }
    
    public bool CheckOutOfIndex(int row, int col)
    {
        if (row < 0 || row >= Yrange || col < 0 || col >= Xrange)
        {
            return true;
        }

        return false;
    }
    
    bool CheckDoubleThree(int index, GameController.playerType player)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (-1, 1)};
        int CountOpenThree = 0;
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckThree(index, directions[i], player))
            {
                CountOpenThree++;
            }
            
        }

        if (CountOpenThree >= 2)
        {
            return true;
        }
        return false;
    }
    bool CheckThree(int index, (int, int) direction, GameController.playerType marker = GameController.playerType.Black)
    {
        string[] patternedThree = new string[] { "01110","011010","010110" };
        string pattern = "";
        
        for (int i = -4; i < 1; i++)
        {
            for (int j = i; j < i+6; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        Board[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == GameController.playerType.None ?
                        '0' : 
                        Board[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == marker? "1" : "2";
                }
                
            }
            for (int k = 0; k < patternedThree.Length; k++)
            {
                if (pattern.Contains(patternedThree[k]))
                {
                    return true;
                }
            }

            pattern = "";
        }
        return false;
    }
    bool CheckDoubleFour(int index, GameController.playerType marker = GameController.playerType.Black)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (1, -1)};
        int CountFour = 0;
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckFour(index, directions[i],marker) == 1)
            {
                CountFour++;
            }
            else if (CheckFour(index, directions[i],marker) > 1)
            {
                return true;
            }
        }
        if (CountFour >= 2)
        {
            return true;
        }
        return false;
    }
    int CheckFour(int index, (int,int) direction,GameController.playerType marker)
    {
        string[] patternedFour = new string[] { "01111","11110","11011","10111","11101" };
        string pattern = "";
        int checkFour = 0;
        int checkOpenFour = 0;
        for (int i = -4; i < 1; i++)
        {
            for (int j = i; j < i+5; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        Board[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == GameController.playerType.None ?
                        '0'  :
                        Board[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == marker? "1" : "2";
                }
            }
            for (int k = 0; k < patternedFour.Length; k++)
            {
                if (pattern.Equals("01111") || pattern.Equals("11110"))
                {
                    checkOpenFour++;
                }
                if (pattern.Contains(patternedFour[k]))
                {
                    checkFour++;
                }
                if (pattern.Equals("11111"))
                {
                    return 0;
                }
            }

            
            pattern = "";
        }

        if (checkOpenFour >= 2)
        {
            return 1;
        }
        return checkFour;
    }

    public bool CheckFourInAllDirections(int index, GameController.playerType marker)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (1, -1)};
        
        for (int i = 0; i < directions.Length; i++)
        {
            
            if (CheckFour(index, directions[i], marker) != 0)  
            {
                return true;
            }
        }
        return false;
    }
    
    public bool CheckThreeInAllDirections(int index, GameController.playerType marker)
    { 
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (1, -1)};
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckThree(index, directions[i], marker))
            {
                return true;
            }
        }
        return false;
        
    }
    
    public bool CheckFiveInAllDirections(int index,GameController.playerType marker)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (1, -1)};
        for (int i = 0; i < directions.Length; i++)
        { if (CheckFive(index, directions[i], marker))
            {
                return true;
            }
        }
        return false;
    }

    private static readonly HashSet<string> FivePatterns = new HashSet<string>
    {
        "0111110", "2111110", "2111112", "1111120",
        "0211111", "0011111", "1111100", "2211111",
        "1111122", "1111102", "2011111"
    };
    bool CheckFive(int index, (int, int) direction, GameController.playerType marker )
    {
       

        int Y = index / 15;
        int X = index % 15;
        
        string pattern = "";
        for (int i = -5; i < 1; i++)
        {
            for (int j = i; j < i+7; j++)
            {
                if (!CheckOutOfIndex(Y + j * direction.Item1, X + j * direction.Item2))
                {
                    pattern +=  
                        Board[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == GameController.playerType.None ?
                            '0'  :
                            Board[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == marker? "1" : "2";
                }
                
            }
            //Debug.Log($"{direction}, {index}: {pattern}");
           
            if (FivePatterns.Contains(pattern))
            {
                return true;
            }
            
            
            pattern = "";
        }

        return false;
    }

    bool CheckJangMok(int index, GameController.playerType marker)
    {
        (int,int)[] directions = new (int, int)[]{ (0, 1), (1, 0), (1, 1), (-1, 1)};
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckMoreThanFive(index, directions[i], marker))
            {
                return true;
            }
        }
        return false;
    }
    bool CheckMoreThanFive(int index, (int, int) direction, GameController.playerType marker)
    {
        string patternedSix = "111111";
        string pattern = "";
        for (int i = -4; i < 1; i++)
        {
            for (int j = i; j < i+6; j++)
            {
                if (!CheckOutOfIndex(index / 15 + j * direction.Item1, index % 15 + j * direction.Item2))
                {
                    pattern += j==0 ? '1' : 
                        Board[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == GameController.playerType.None ?
                            '0' :
                            Board[index/15 + j * direction.Item1,index%15 + j * direction.Item2] == marker? "1" : "2";
                }
            }
            if (pattern.Equals(patternedSix))
            {
                return true;
            }
            
            pattern = "";
        }
        return false;
    }

    public bool IsDraw()
    {
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[i, j] == GameController.playerType.None)
                {
                    return false;  // 빈칸이 있으면 게임 계속 진행
                }
            }
        }
        return true;  // 빈칸이 없으면 무승부
    }
}
