using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

// public static class AIController
// {
//     
//     public static (int,int) AIBestMove(GameController.playerType[,] board, GameController.playerType aiType, int originalIndex)
//     {
//         //RanjuRule ranjuRule = new RanjuRule(board);
//         float value = float.MinValue;
//         float beta = float.MaxValue;
//         (int,int) currentPlayerMove = (originalIndex/15, originalIndex%15);
//         //중심점으로부터 
//         int XrangeLeft = currentPlayerMove.Item2 - 2 >= 0 ? currentPlayerMove.Item2 - 2 : 0;
//         int XrangeRight = currentPlayerMove.Item2 + 2 <= 14 ? currentPlayerMove.Item2 + 2 : 14;
//         int YrangeTop = currentPlayerMove.Item1 - 2 >= 0 ? currentPlayerMove.Item1 - 2 : 0;
//         int YrangeBottom = currentPlayerMove.Item1 + 2 <= 14 ? currentPlayerMove.Item1 + 2 : 14;
//         (int,int)[] direction = new (int, int)[]{ (0, 1),(0,-1), (1, 0),(-1, 0), (1, 1), (-1, 1),(-1, -1), (1, -1)};
//         (int,int) bestMove = (0,0);
//        
//         float prevValue = value;
//         for (int i = 0; i < direction.Length; i++)
//         {
//             //현재 움직일 위치
//             (int,int) currentAiMove = (originalIndex/15 + direction[i].Item1, originalIndex%15 + direction[i].Item2);
//             //바운더리 설정
//             if (currentAiMove.Item1 >= YrangeTop && currentAiMove.Item1 <= YrangeBottom &&
//                 currentAiMove.Item2 >= XrangeLeft && currentAiMove.Item2 <= XrangeRight)
//             {
//                 //움직인 위치에 바둑돌이 존재 하는가
//                 if (board[currentAiMove.Item1, currentAiMove.Item2] ==
//                     GameController.playerType.None)
//                 {
//                         
//                     board[currentAiMove.Item1, currentAiMove.Item2] = aiType;
//                     float maxValue = AlphaBetaMove(originalIndex,currentAiMove,0, value ,beta,false,aiType,board);
//                     board[currentAiMove.Item1, currentAiMove.Item2] = GameController.playerType.None;
//                     value = Mathf.Max(value, maxValue);
//                     
//                     if (value >= beta)
//                     {
//                         bestMove = currentAiMove;
//                     }
//                 }
//             }
//         }
//         
//         
//         return bestMove;
//     }
//
//     private static float AlphaBetaMove(int originIndex,(int,int) index, int depth, float alpha, float beta, bool Maximizing, GameController.playerType aiType, GameController.playerType[,] board)
//     {
//         
//         GameController.playerType playerType = aiType != GameController.playerType.Black ? GameController.playerType.Black : GameController.playerType.White;
//
//         (int,int) currentPlayerMove = (originIndex/15, originIndex%15);
//         
//         //중심점으로부터의 바운더리
//         int XrangeLeft = currentPlayerMove.Item2 - 2 >= 0 ? currentPlayerMove.Item2 - 2 : 0;
//         int XrangeRight = currentPlayerMove.Item2 + 2 <= 14 ? currentPlayerMove.Item2 + 2 : 14;
//         int YrangeTop = currentPlayerMove.Item1 - 2 >= 0 ? currentPlayerMove.Item1 - 2 : 0;
//         int YrangeBottom = currentPlayerMove.Item1 + 2 <= 14 ? currentPlayerMove.Item1 + 2 : 14;
//         
//         //방향
//         (int,int)[] direction = new (int, int)[]{ (0, 1),(0,-1), (1, 0),(-1, 0), (1, 1), (-1, 1),(-1, -1), (1, -1)};
//         //가중치
//         float heuristic = 0;
//         
//         //ai가 둔 윛에서 삼, 사, 오가 되는지 체크
//         //된다면 그만큼 가중치를 더한다.
//         if (CheckThree(board, aiType, index.Item1 * 15 + index.Item2))
//         {
//             heuristic += 2 + depth;
//             if (CheckFour(board, aiType, index.Item1 * 15 + index.Item2))
//             {
//                 heuristic += 3 + depth;
//                 if (CheckWin(board, aiType))
//                 {
//                     heuristic += 5 + depth;
//                 }
//             }
//             return heuristic;
//         }
//         //플레이어가 둔 위치에서 삼, 사, 오 가 되는지 체크
//         // 된다면 그만큼 가중치를 뺀다.
//         if (CheckThree(board, playerType, index.Item1 * 15 + index.Item2))
//         {
//             heuristic -= 2 - depth;
//             if (CheckFour(board, playerType, index.Item1 * 15 + index.Item2))
//             {
//                 heuristic -= 3 - depth;
//                 if (CheckWin(board, playerType))
//                 {
//                     heuristic -= 5 - depth;
//                 }
//             }
//             return heuristic;
//         }
//
//         if (depth == 3)
//         {
//             if (CheckThree(board, aiType, index.Item1 * 15 + index.Item2))
//             {
//                 heuristic += 2 + depth;
//                 if (CheckFour(board, aiType, index.Item1 * 15 + index.Item2))
//                 {
//                     heuristic += 3 + depth;
//                     if (CheckWin(board, aiType))
//                     {
//                         heuristic += 5 + depth;
//                     }
//                 }
//                 return heuristic;
//             }
//             if (CheckThree(board, playerType, index.Item1 * 15 + index.Item2))
//             {
//                 heuristic -= 3 - depth;
//                 if (CheckFour(board, playerType, index.Item1 * 15 + index.Item2))
//                 {
//                     heuristic -= 4 - depth;
//                     if (CheckWin(board, playerType))
//                     {
//                         heuristic -= 6 - depth;
//                     }
//                 }
//                 return heuristic;
//             }
//             return 0;
//         }
//         if (Maximizing)
//         {
//             float value = float.MinValue;
//             for (int i = 0; i < direction.Length; i++)
//             {
//                 //현재 움직일 위치
//                 (int,int) currentAiMove = (index.Item1 + direction[i].Item1, index.Item1 + direction[i].Item2);
//                 //바운더리 설정
//                 if (currentAiMove.Item1 >= YrangeTop && currentAiMove.Item1 <= YrangeBottom &&
//                     currentAiMove.Item2 >= XrangeLeft && currentAiMove.Item2 <= XrangeRight)
//                 {
//                     //움직인 위치에 바둑돌이 존재 하는가
//                     if (board[index.Item1 + direction[i].Item1, index.Item1 + direction[i].Item2] ==
//                         GameController.playerType.None)
//                     {
//                         
//                         board[index.Item1 + direction[i].Item1, index.Item1 + direction[i].Item2] = aiType;
//                         float maxValue = AlphaBetaMove(originIndex,currentAiMove,depth+1, alpha,beta,false,aiType,board);
//                         board[index.Item1 + direction[i].Item1, index.Item1 + direction[i].Item2] = GameController.playerType.None;
//                         value = Mathf.Max(value, maxValue);
//                         alpha = Mathf.Min(alpha, value);
//                         if (alpha >= beta)
//                         {
//                             break;
//                         }
//                     }
//                 }
//             }
//             return value;
//         }
//         else
//         {
//             //검은색을 위한 가지치기
//             //여덞 방향으로 나아가고
//             //바운더리내에서
//             //빈칸이 나올때까지 전진
//             //빈칸이 나오면 거기서 알파베타 가지치기 시전
//             
//             float value = float.MaxValue;
//             for (int i = 0; i < direction.Length; i++)
//             {
//                 (int,int) currentPlayMove = (index.Item1 + direction[i].Item1, index.Item1 + direction[i].Item2);
//                 if (currentPlayMove.Item1 >= YrangeTop && currentPlayMove.Item1 <= YrangeBottom && currentPlayMove.Item2 >= XrangeLeft && currentPlayMove.Item2 <= XrangeRight)
//                 {
//                     
//                     //돌이 없을때 둘 공간 확보
//                     if (board[index.Item1 + direction[i].Item1, index.Item1 + direction[i].Item2] == GameController.playerType.None)
//                     {
//                         board[index.Item1 + direction[i].Item1, index.Item1 + direction[i].Item2] = playerType;
//                         float maxValue = AlphaBetaMove( originIndex, currentPlayMove,depth+1, alpha, beta,true,aiType,board);
//                         board[index.Item1 + direction[i].Item1, index.Item1 + direction[i].Item2] = GameController.playerType.None;
//                         value = Mathf.Max(value, maxValue);
//                         beta = Mathf.Min(beta, value);
//                         if (alpha >= beta)
//                         {
//                             break;
//                         }
//                     }
//                     //돌이 있을때 다음으로 이동
//                     else
//                     {
//                         //현재에서 빈칸이 나올때 까지 이동
//                         currentPlayMove = (currentPlayMove.Item1 + direction[i].Item1, currentPlayMove.Item1 + direction[i].Item2);
//                     }
//                 }
//             }
//             return value;
//         }
//     }
//
//     private static bool CheckTwo(GameController.playerType[,] board, GameController.playerType player, int index)
//     {
//         (int,int)[] direction = new (int, int)[]{ (0, 1),(0,-1), (1, 0),(-1, 0), (1, 1), (-1, 1),(-1, -1), (1, -1)};
//         for (int i = 0; i < direction.Length; i++)
//         {
//             if (board[index / 15 + direction[i].Item1, index % 15 + direction[i].Item2] == player)
//             {
//                 return true;
//             }
//         }
//
//         return false;
//     }
//     //막기 가중치
//     //prev 이목 삼목 사목 저장
//     //current index 확인 이목 삼목 사목 풀리면, 가중치 추가 또는 마이너스
//     //삼,사,오 개수를 구하고 이를 heuristic에 가중치로 더하자.
//     private static bool CheckFour(GameController.playerType[,] board, GameController.playerType player, int index)
//     {
//         RanjuRule ranjuRule = new RanjuRule(board);
//         if (ranjuRule.CheckFourInAllDirections(index, player))
//         {
//             return true;
//         }
//         return false;
//     }
//     private static bool CheckThree(GameController.playerType[,] board, GameController.playerType player, int index)
//     {
//         RanjuRule ranjuRule = new RanjuRule(board);
//         if (ranjuRule.CheckThreeInAllDirections(index, player))
//         {
//             return true;
//         }
//
//         return false;
//     }
//     private static bool CheckWin(GameController.playerType[,] board, GameController.playerType player)
//     {
//         RanjuRule ranjuRule = new RanjuRule(board);
//         if (ranjuRule.CheckWin(player))
//         {
//             return true;
//         }
//         return false;
//     }
//     private static bool isTied(int rowMin, int rowMax, int colMin, int colMax, GameController.playerType[,] board)
//     {
//         for (int i = rowMin; i <= rowMax; i++)
//         {
//             for (int j = colMin; j <= colMax; j++)
//             {
//                 if (board[i, j] == GameController.playerType.None) return false;
//             }
//         }
//         return true;
//     }
// }
//

public static class AIController
{
    private const int TIME_LIMIT_MS = 10000;
    // 평가 함수: 점수를 계산하여 게임 상태를 평가
 public static int Evaluate(GameController.playerType[,] board, GameController.playerType player)
{
    int score = 0;

    RanjuRule ruleChecker = new RanjuRule(board);

    // 승리 조건에 대한 점수
    if (ruleChecker.CheckWin(GameController.playerType.Black)) 
        score += player == GameController.playerType.Black ? 1000 : -1000;

    if (ruleChecker.CheckWin(GameController.playerType.White)) 
        score += player == GameController.playerType.White ? 1000 : -1000;

    // 상대방 돌을 막는 점수 계산
    GameController.playerType opponent = player == GameController.playerType.Black ? GameController.playerType.White : GameController.playerType.Black;
    var directions = new (int, int)[]
    {
        (-1, 0), (1, 0), (0, -1), (0, 1), // 상하좌우
        (-1, -1), (-1, 1), (1, -1), (1, 1) // 대각선
    };

    for (int i = 0; i < board.GetLength(0); i++)
    {
        for (int j = 0; j < board.GetLength(1); j++)
        {
            if (board[i, j] == player || board[i, j] == opponent)
            {
                foreach (var direction in directions)
                {
                    int connectedCount = 1;
                    bool openStart = false; // 연결 시작이 열려 있는지 확인
                    bool openEnd = false;  // 연결 끝이 열려 있는지 확인

                    int ni = i + direction.Item1;
                    int nj = j + direction.Item2;

                    // 연결된 상대방 돌 개수 계산
                    while (ni >= 0 && ni < board.GetLength(0) && nj >= 0 && nj < board.GetLength(1)
                           && board[ni, nj] == opponent)
                    {
                        connectedCount++;
                        ni += direction.Item1;
                        nj += direction.Item2;
                    }

                    // 연결 시작이 비어 있는지 확인
                    int si = i - direction.Item1;
                    int sj = j - direction.Item2;
                    if (si >= 0 && si < board.GetLength(0) && sj >= 0 && sj < board.GetLength(1)
                        && board[si, sj] == GameController.playerType.None)
                    {
                        openStart = true;
                    }

                    // 연결 끝이 비어 있는지 확인
                    if (ni >= 0 && ni < board.GetLength(0) && nj >= 0 && nj < board.GetLength(1)
                        && board[ni, nj] == GameController.playerType.None)
                    {
                        openEnd = true;
                    }

                    // 상대방 돌이 4개 연결되어 있고 열려 있는 경우 (막아야 하는 경우)
                    if (connectedCount == 4 && (openStart || openEnd))
                    {
                        score += 800; // 높은 점수로 막기를 우선시
                    }

                    // 가중치 부여: 플레이어의 돌 점수
                    if (board[i, j] == player)
                    {
                        if (connectedCount == 2) score += 10;
                        else if (connectedCount == 3) score += 50;
                        else if (connectedCount == 4) score += 200;
                        else if (connectedCount >= 5) score += 1000;
                    }
                }
            }
        }
    }

    return score;
}

    // Alpha-Beta Pruning 알고리즘
    public static (int, int) AIBestMove(GameController.playerType[,] board, GameController.playerType player, int maxDepth)
    {
        Stopwatch stopwatch = new Stopwatch(); // 타이머 초기화
        stopwatch.Start();
        
        int bestValue = int.MinValue;
        (int, int) bestMove = (-1, -1);
        RanjuRule ruleChecker = new RanjuRule(board);

        foreach (var move in GetPossibleMoves(ruleChecker.Board))
        {
            if (stopwatch.ElapsedMilliseconds > TIME_LIMIT_MS)
            {
                Console.WriteLine("시간 초과! 현재까지 최적의 수를 반환합니다.");
                break; // 시간이 초과되었으면 루프 중단
            }
            
            // 움직임 적용
            ruleChecker.Board[move.Item1, move.Item2] = player;
            
            // Alpha-Beta Pruning 호출
            int moveValue = AlphaBeta(ruleChecker.Board, maxDepth - 1, int.MinValue, int.MaxValue, false, player, ruleChecker);

            // 움직임 취소
            ruleChecker.Board[move.Item1, move.Item2] = GameController.playerType.None;

            // 최적의 움직임 갱신
            if (moveValue > bestValue)
            {
                bestValue = moveValue;
                bestMove = move;
            }
        }
        stopwatch.Stop();
        return bestMove;
    }

    private static int AlphaBeta(GameController.playerType[,] board, int depth, int alpha, int beta, bool maximizingPlayer, GameController.playerType player, RanjuRule ruleChecker)
    {
        // 종료 조건: 깊이가 0이거나 승리 조건에 도달했을 경우
        if (depth == 0 || ruleChecker.CheckWin(GameController.playerType.Black) || ruleChecker.CheckWin(GameController.playerType.White))
        {
            return Evaluate(board, player);
        }

        // 가능한 움직임 가져오기
        List<(int, int)> possibleMoves = GetPossibleMoves(board);

        // 가능한 움직임이 없는 경우 종료
        if (possibleMoves.Count == 0)
        {
            return Evaluate(board, player);
        }

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            foreach (var move in possibleMoves)
            {
                board[move.Item1, move.Item2] = player; // 움직임 적용
                int eval = AlphaBeta(board, depth - 1, alpha, beta, false, player, ruleChecker);
                board[move.Item1, move.Item2] = GameController.playerType.None; // 움직임 취소
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha) break; // 가지치기
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            GameController.playerType opponent = player == GameController.playerType.Black ? GameController.playerType.White : GameController.playerType.Black;
            foreach (var move in possibleMoves)
            {
                board[move.Item1, move.Item2] = opponent; // 움직임 적용
                int eval = AlphaBeta(board, depth - 1, alpha, beta, true, player, ruleChecker);
                board[move.Item1, move.Item2] = GameController.playerType.None; // 움직임 취소
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha) break; // 가지치기
            }
            return minEval;
        }
    }

    // 가능한 모든 움직임을 생성
    private static List<(int, int)> GetPossibleMoves(GameController.playerType[,] board)
    {
        var moves = new List<(int, int)>();
        var directions = new (int, int)[]
        {
            (-1, 0), (1, 0), (0, -1), (0, 1), // 상하좌우
            (-1, -1), (-1, 1), (1, -1), (1, 1) // 대각선
        };

        // 이미 돌이 놓인 칸들 중심으로 탐색
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != GameController.playerType.None) // 돌이 이미 놓여진 위치
                {
                    foreach (var direction in directions)
                    {
                        int ni = i + direction.Item1;
                        int nj = j + direction.Item2;

                        // 경계 값 검사
                        if (ni >= 0 && ni < board.GetLength(0) && nj >= 0 && nj < board.GetLength(1))
                        {
                            // 인접한 칸이 빈 칸이라면 후보에 추가
                            if (board[ni, nj] == GameController.playerType.None && !moves.Contains((ni, nj)))
                            {
                                moves.Add((ni, nj));
                            }
                        }
                    }
                }
            }
        }

        return moves;
    }
}