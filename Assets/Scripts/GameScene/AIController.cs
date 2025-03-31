using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Debug = System.Diagnostics.Debug;
using Random = System.Random;

public static class AIController
{
    // MCTS 탐색을 위한 노드 정의
    private class Node
    {
        public GameController.playerType[,] BoardState; // 현재 보드 상태
        public (int x, int y) Move; // 현재 노드의 움직임
        public float Wins; // 이긴 횟수
        public int Simulations; // 시뮬레이션 횟수
        public List<Node> Children; // 자식 노드
        public Node Parent; // 부모 노드

        public Node(GameController.playerType[,] boardState, (int x, int y) move, Node parent)
        {
            BoardState = CopyBoard(boardState);
            Move = move;
            Parent = parent;
            Wins = 0;
            Simulations = 0;
            Children = new List<Node>();
        }

        // 승리 확률 계산
        public double WinRate => Simulations > 0 ? (double)Wins / Simulations : 0;
    }

    public static int timeScale = 10000;
    // MCTS를 통한 최적 수 계산
    public static (int, int) AIBestMoveMCTS(GameController.playerType[,] board, GameController.playerType player,(int x,int y) currentMove, int maxIterations)
    {
        
        Node root = new Node(board, currentMove, null);
        Stopwatch sw = new Stopwatch();
        sw.Start();
        for(int i = 0; i < maxIterations; i++)
        {
            if (sw.ElapsedMilliseconds > timeScale) break;
            Node selectedNode = Select(root); // 노드 선택
            Node expandedNode = Expand(selectedNode, player); // 노드 확장
            var result = Task.Run(()=>Simulate(expandedNode, player == GameController.playerType.White ? GameController.playerType.Black : GameController.playerType.White)); // 시뮬레이션 수행
            Backpropagate(expandedNode, result.Result, player); // 결과를 역전파
            
        }

        // 가장 높은 승리 확률을 가진 자식 노드 선택
        Node bestChild = root.Children.Count > 0 ? root.Children[0] : null;
        double bestWinRate = 0;
        foreach (var child in root.Children)
        {
            if (child.WinRate > bestWinRate)
            {
                bestWinRate = child.WinRate;
                bestChild = child;
            }
        }

        return bestChild?.Move ?? (-1, -1); // 최적 수 반환
    }

    private static Node Select(Node root)
    {
        Node current = root;
        
        while (current.Children.Count > 0) // Leaf 노드에 도달할 때까지 탐색
        {
            current = UCBSelect(current.Children); // UCB 기반으로 자식 노드 선택
        }

        return current;
    }

    private static Node Expand(Node node, GameController.playerType player)
    {
        List<(int x, int y)> possibleMoves = GetPossibleMoves(node.BoardState,player ,node.Move);
        Random rnd = new Random();
        int randomMove = rnd.Next(0, possibleMoves.Count);
        
        foreach (var move in possibleMoves)
        {
            var newBoard = node.BoardState;
            newBoard[move.x, move.y] = player;
            Node childNode = new Node(newBoard, move, node);
            node.Children.Add(childNode);
            
        }

        return node.Children.Count > 0 ? node.Children[randomMove] : node;
    }
    
    private static double Simulate(Node node, GameController.playerType player)
    {
        GameController.playerType currentPlayer = player;
        var prevMove = node.Move;
        var board = node.BoardState;
        RanjuRule ranju = new RanjuRule(board);
        while (true)
        {
            List<(int x, int y)> moves = GetPossibleMoves(board,currentPlayer ,prevMove);
            if (moves.Count == 0) break; // 게임 종료 조건
            var randomMove = moves[new Random().Next(moves.Count)];
            prevMove = randomMove;
            board[randomMove.x, randomMove.y] = currentPlayer;
            ranju.UpdateBoardState(board);
            
            
            if (ranju.CheckWin(currentPlayer)) return player == currentPlayer ?  1 :  -1;
            currentPlayer = currentPlayer == GameController.playerType.Black ? GameController.playerType.White : GameController.playerType.Black;
        }
        
        if(ranju.CheckWin(currentPlayer)) return player == currentPlayer ?  1 : -1;
        // 승리 여부 반환 (예: 1: 승리, 0: 무승부, -1: 패배)
        
        
        return 0;
    }

    private static void Backpropagate(Node node, double result, GameController.playerType player)
    {
        Node current = node;
        while (current != null)
        {
            current.Simulations++;
            if ((result == 1 && current.BoardState[current.Move.x, current.Move.y] == player) ||
                (result == -1 && current.BoardState[current.Move.x, current.Move.y] != player))
            {
                current.Wins++;
            }

            current = current.Parent;
        }
    }

    private static Node UCBSelect(List<Node> children)
    {
        Node bestNode = null;
        double bestValue = double.MinValue;

        Parallel.ForEach(children, child =>
        {
            double ucbValue = child.WinRate + Math.Sqrt(2 * Math.Log(child.Parent.Simulations + 1) / (child.Simulations + 1));
            if (ucbValue > bestValue)
            {
                bestValue = ucbValue;
                bestNode = child;
            }
        });
        // foreach (var child in children)
        // {
        //     double ucbValue = child.WinRate + Math.Sqrt(2 * Math.Log(child.Parent.Simulations + 1) / (child.Simulations + 1));
        //     if (ucbValue > bestValue)
        //     {
        //         bestValue = ucbValue;
        //         bestNode = child;
        //     }
        // }

        return bestNode;
    }

    private static List<(int, int)> GetPossibleMoves(GameController.playerType[,] board,GameController.playerType player ,(int x,int y) currentMove)
    {
        var moves = new List<(int, int)>();
        var defensiveMoves =new List<(int, int)>();
        var offensiveMoves = new List<(int, int)>();
        var balancedMoves = new List<(int, int)>();
        RanjuRule ranju = new RanjuRule(board);
        (int x, int y)[] direction = new (int x,int y)[] {(0,1),(-1,1),(-1,0),(-1,-1),(0,-1),(1,-1),(1,0),(1,1) };
        
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != GameController.playerType.None)
                {
                    for (int k = 0; k < direction.Length; k++)
                    {
                        (int x,int y) tmpMove = (i+direction[k].x,j+direction[k].y);
                        bool row = tmpMove.x >= 0 && tmpMove.x < board.GetLength(0);
                        bool col = tmpMove.y >= 0 && tmpMove.y < board.GetLength(1);
                        if (!row || !col) break;
                        if (row && col)
                        {
                            if (board[tmpMove.x, tmpMove.y] == GameController.playerType.None)
                            {
                                if (!moves.Contains(tmpMove))
                                {
                                    moves.Add((tmpMove.x, tmpMove.y));
                                    board[tmpMove.x, tmpMove.y] = player == GameController.playerType.Black ? GameController.playerType.White : GameController.playerType.Black;
                                    ranju.UpdateBoardState(board);
                                    if (ranju.CheckFourInAllDirections(tmpMove.x * board.GetLength(0) + tmpMove.y, player == GameController.playerType.Black ? GameController.playerType.White : GameController.playerType.Black)||
                                        ranju.CheckFiveInAllDirections(tmpMove.x * board.GetLength(0) + tmpMove.y, player == GameController.playerType.Black ? GameController.playerType.White : GameController.playerType.Black)||
                                        ranju.RanJu(tmpMove.x * board.GetLength(0) + tmpMove.y, player == GameController.playerType.Black ? GameController.playerType.White : GameController.playerType.Black))
                                    {
                                        defensiveMoves.Add((tmpMove.x, tmpMove.y));
                                    }
                                    board[tmpMove.x, tmpMove.y] = GameController.playerType.None;
                                    board[tmpMove.x, tmpMove.y] = player;
                                    ranju.UpdateBoardState(board);
                                    if(ranju.CheckFourInAllDirections(tmpMove.x * board.GetLength(0) + tmpMove.y, player)||
                                       ranju.CheckFiveInAllDirections(tmpMove.x * board.GetLength(0) + tmpMove.y, player)||
                                       ranju.RanJu(tmpMove.x * board.GetLength(0) + tmpMove.y, player))
                                    {
                                        offensiveMoves.Add((tmpMove.x, tmpMove.y));
                                    }
                                    board[tmpMove.x, tmpMove.y] = GameController.playerType.None;
                                    if (defensiveMoves.Contains(tmpMove) && offensiveMoves.Contains(tmpMove))
                                    {
                                        balancedMoves.Add((tmpMove.x, tmpMove.y));
                                    }
                                }
                                
                            }
                        }
                        
                    }
                }
            }
        }
        //defensiveMoves.Count > 0 ? defensiveMoves
        return  balancedMoves.Count >0 ? balancedMoves : defensiveMoves.Count > 0 ? defensiveMoves : offensiveMoves.Count > 0? offensiveMoves : moves; //
    }
    private static GameController.playerType[,] CopyBoard(GameController.playerType[,] board)
    {
        var newBoard = new GameController.playerType[board.GetLength(0), board.GetLength(1)];
        Array.Copy(board, newBoard, board.Length);
        return newBoard;
    }
}