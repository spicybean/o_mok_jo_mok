using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIController
{
    
    public static (int,int) AIBestMove(GameController.playerType[,] board, GameController.playerType aiType)
    {
        RanjuRule ranjuRule = new RanjuRule(board);
        
        float maxValue = float.MinValue;
        (int,int) bestMove =(0,0);
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == GameController.playerType.None)
                {
                    board[i, j] = aiType;
                    float value = MinMaxMove(board, 0, false, aiType);
                    board[i,j] = GameController.playerType.None;
                    if (value > maxValue)
                    {
                        maxValue = value;
                        bestMove = (i, j);
                    }
                }
            }
        }
        
        
        return bestMove;
    }

    private static float MinMaxMove(GameController.playerType[,] board, int depth, bool Maximizing, GameController.playerType aiType)
    {
        GameController.playerType[,] boardClone = board;
        
        GameController.playerType playerType = aiType != GameController.playerType.Black ? GameController.playerType.Black : GameController.playerType.White;

        //ai가 이겼을때
        if (CheckWin(board,aiType))
        {
            return 10 - depth;
        }
        
        //player가 이겼을때
        if (CheckWin(board,playerType))
        {
            return -10 + depth;
        }
        if (depth == 6)
        {
            return 0;
        }

        if (Maximizing)
        {
            float maxValue = float.MinValue;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == GameController.playerType.None)
                    {
                        board[i, j] = aiType;
                        float value = MinMaxMove(board, depth + 1, false, aiType);
                        board[i,j] = GameController.playerType.None;
                        maxValue = Mathf.Max(maxValue, value);
                    }
                }
            }
            return maxValue;
        }
        else
        {
            float minValue = float.MaxValue;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == GameController.playerType.None)
                    {
                        board[i, j] = playerType;
                        float value = MinMaxMove(board, depth + 1, true, aiType);
                        board[i,j] = GameController.playerType.None;
                        minValue = Mathf.Min(minValue, value);
                    }
                }
            }
            return minValue;
        }
        
    }

    private static bool CheckWin(GameController.playerType[,] board, GameController.playerType player)
    {
        RanjuRule ranjuRule = new RanjuRule(board);
        if (ranjuRule.CheckWin(player))
        {
            return true;
        }
        return false;
    }
    private static bool isTied(int rowMin, int rowMax, int colMin, int colMax, GameController.playerType[,] board)
    {
        for (int i = rowMin; i <= rowMax; i++)
        {
            for (int j = colMin; j <= colMax; j++)
            {
                if (board[i, j] == GameController.playerType.None) return false;
            }
        }
        return true;
    }
}
