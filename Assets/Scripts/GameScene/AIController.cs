using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIController
{
    
    public static (int,int) AIBestMove(GameController.playerType[,] board, GameController.playerType aiType)
    {
        RanjuRule ranjuRule = new RanjuRule(board);
        float value = float.MinValue;
        float beta = float.MaxValue;
        (int,int) bestMove = (0,0);
        //minMax alphabetaStart
        float prevValue = value;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == GameController.playerType.None)
                {
                    board[i, j] = aiType;
                    value = Math.Max(value,
                        AlphaBetaMove(value,
                            beta,0,(i,j),board,aiType,false));
                    board[i, j] = GameController.playerType.None;
                    if (value >= prevValue)
                    {
                        bestMove = (i, j);
                        prevValue = value;
                    }
                }
              
            } 
        }
        return bestMove;
    }

    private static float AlphaBetaMove(float alpha, float beta,int depth,(int,int) index, GameController.playerType[,] board, GameController.playerType aiType, bool Maximizing)
    {
        RanjuRule ranjuRule = new RanjuRule(board);
        GameController.playerType playerType = aiType == GameController.playerType.Black ? GameController.playerType.Black : GameController.playerType.White;
        if (ranjuRule.CheckFiveInAllDirections(index.Item1 * board.GetLength(0)+index.Item2, aiType))
        {
            return -10 + depth;
        }

        if (ranjuRule.CheckFiveInAllDirections(index.Item1 * board.GetLength(0)+index.Item2, playerType))
        {
            return  10 - depth;
        }
        if (ranjuRule.IsDraw())
        {
            return 0;
        }
        
        if (Maximizing)
        {
            float value = float.MinValue;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != GameController.playerType.None) continue;
                    
                    board[i, j] = aiType;
                    value =  Mathf.Max(value,AlphaBetaMove(alpha, beta, depth+1
                        ,(i,j) ,board,aiType, false));
                    board[i, j] = GameController.playerType.None;
                    alpha = Mathf.Max(alpha,value);
                    if (value >= beta)
                    {
                        break;
                    }
                }
                if (value >= beta)
                {
                    break;
                }
            }
            return value;
        }
        else
        {
            float value = float.MaxValue;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != GameController.playerType.None) continue;
                    
                    board[i, j] = playerType;
                    value = Mathf.Min(value,AlphaBetaMove( alpha, beta,depth +1 
                        ,(i,j),board, aiType, true)) ;
                    board[i, j] = GameController.playerType.None;
                    beta = Mathf.Min(beta,value);
                    if (value <= alpha)
                    {
                        break;
                    }
                }
                if (value <= alpha)
                {
                    break;
                }
            }
            return value;
        }
        
        
        
    }
    
    public static void CheckWin(){}
}
