using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIController
{
    
    public static (int,int) AIBestMove(GameController.playerType[,] board, GameController.playerType aiType, int playerIndex)
    {
        RanjuRule ranjuRule = new RanjuRule(board);
        float value = float.MinValue;
        float beta = float.MaxValue;
        (int,int) currentPlayerMove = (playerIndex/15, playerIndex%15);
        int XrangeLeft = currentPlayerMove.Item2 - 2 >= 0 ? currentPlayerMove.Item2 - 2 : 0;
        int XrangeRight = currentPlayerMove.Item2 + 2 <= 14 ? currentPlayerMove.Item2 + 2 : 14;
        int YrangeTop = currentPlayerMove.Item1 - 2 >= 0 ? currentPlayerMove.Item1 - 2 : 0;
        int YrangeBottom = currentPlayerMove.Item1 + 2 <= 14 ? currentPlayerMove.Item1 + 2 : 14;
        (int,int) bestMove = (0,0);
        //minMax alphabetaStart
        float prevValue = value;
        for (int i = YrangeTop; i < YrangeBottom; i++)
        {
            for (int j = XrangeLeft; j < XrangeRight; j++)
            {
                if (board[i, j] == GameController.playerType.None)
                {
                    board[i, j] = aiType;
                    value = Math.Max(value, AlphaBetaMove(value, beta,0,(i,j),ranjuRule,board,aiType,playerIndex,false));
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

    private static float AlphaBetaMove(float alpha, float beta,int depth,(int,int) index,RanjuRule ranju, GameController.playerType[,] board, GameController.playerType aiType,int playerIndex , bool Maximizing)
    {
        ranju.UpdateBoardState(board);
        GameController.playerType playerType = aiType == GameController.playerType.Black ? GameController.playerType.Black : GameController.playerType.White;
        (int,int) currentPlayerMove = (playerIndex/15, playerIndex%15);
        int XrangeLeft = currentPlayerMove.Item2 - 2 >= 0 ? currentPlayerMove.Item2 - 2 : 0;
        int XrangeRight = currentPlayerMove.Item2 + 2 <= 14 ? currentPlayerMove.Item2 + 2 : 14;
        int YrangeTop = currentPlayerMove.Item1 - 2 >= 0 ? currentPlayerMove.Item1 - 2 : 0;
        int YrangeBottom = currentPlayerMove.Item1 + 2 <= 14 ? currentPlayerMove.Item1 + 2 : 14;
        if (ranju.CheckFiveInAllDirections(index.Item1 * board.GetLength(0)+index.Item2, aiType))
        {
            return -10 + depth;
        }

        if (ranju.CheckFiveInAllDirections(index.Item1 * board.GetLength(0)+index.Item2, playerType))
        {
            return  10 - depth;
        }
        if (ranju.IsDraw())
        {
            return 0;
        }

        
        if (Maximizing)
        {
            float value = float.MinValue;
            for (int i = YrangeTop; i < YrangeBottom; i++)
            {
                for (int j = XrangeLeft; j < XrangeRight; j++)
                {
                    if (board[i, j] != GameController.playerType.None) continue;
                    
                    board[i, j] = aiType;
                    value =  Mathf.Max(value,AlphaBetaMove(alpha, beta, depth+1
                        ,(i,j),ranju ,board,aiType,playerIndex ,false));
                    board[i, j] = GameController.playerType.None;
                    alpha = Mathf.Max(alpha,value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                if (alpha >= beta)
                {
                    break;
                }
            }
            return value;
        }
        else
        {
            float value = float.MaxValue;
            for (int i = YrangeTop; i < YrangeBottom; i++)
            {
                for (int j = XrangeLeft; j < XrangeRight; j++)
                {
                    if (board[i, j] != GameController.playerType.None) continue;
                    
                    board[i, j] = playerType;
                    value = Mathf.Min(value,AlphaBetaMove( alpha, beta,depth +1 
                        ,(i,j),ranju,board, aiType,playerIndex ,true)) ;
                    board[i, j] = GameController.playerType.None;
                    beta = Mathf.Min(beta,value);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                if (beta <= alpha)
                {
                    break;
                }
            }
            return value;
        }
        
        
        
    }
    
}
