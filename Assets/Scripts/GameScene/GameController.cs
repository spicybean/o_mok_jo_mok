using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;



public class GameController : MonoBehaviour, IPointerClickHandler
{
    public enum playerType{None,Black, White}
    public playerType[,] omokBoard;
    public playerType turn;
    public playerType playerStone;
    public playerType opponentStone;
    public enum GameState{Single, Double, Multi}
    
    public GameState gameState;
    public RanjuRule ranjuRule;
    
    public RankPanelController rankPanelController;
    public GameObject[] omokButtons;

    public GameObject omokPrefab;
    public GameObject selectedCell;
    
    public DateTime startTime;
    public Timer timer;
    
    private int totalomokCells = 15 * 15;
    private int turncounter = 0;

    public int playerLife = 3;
    public int enemyLife = 3;
   
    private int prevIndex;
    
    //player index
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text enemyName;
    [SerializeField] private Image playerProfile;
    [SerializeField] private Image enemyProfile;
    
    private void Awake()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
       SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void LoseLife(playerType player)
    {
        if (player == playerStone)
        {
            playerLife--;
        }
        else
        {
            enemyLife--;
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        DataManager.instance.currentReplay.gamePlayData = new int[totalomokCells];
        Array.Fill(DataManager.instance.currentReplay.gamePlayData, -1);
        DataManager.instance.currentReplay.dateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        SetOmokBoard();
        ranjuRule = new RanjuRule(omokBoard);
        playerStone = playerType.Black;
        opponentStone = playerType.White;
        turn = playerStone;
        gameState = DataManager.instance.currentReplay.gameState;
        timer.timerType = Timer.TimerType.Decrease;
        timer.timeLimit = 15;
       
    }

    void FixedUpdate()
    {
        timer.OnTimerEndDelegate = () => LoseLife(turn);
        Debug.Log($"player: {playerLife}, opponent: {enemyLife}");
    } 
   
    void OnDestroy() {
        // 이벤트에서 함수를 제거해 리소스 누수 방지
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetGameState(DataManager.instance.currentReplay.gameState);
    }
    
    public void SetGameState(GameState _gameState)
    {
        if (_gameState == GameState.Single)
        {
            // ToDo AI배틀 관련 Init하기
            DataManager.instance.currentReplay.enemyName = "AI봇";
            DataManager.instance.currentReplay.enemyImage = Resources.Load<Sprite>("Images/profile icon/ai-icon");
            DataManager.instance.currentReplay.playerName = DataManager.instance.currentUserAccount.username;
            DataManager.instance.currentReplay.playerTier = DataManager.instance.currentReplay.enemyTier = DataManager.instance.currentUserAccount.usertier;
            DataManager.instance.currentReplay.playerImage = DataManager.instance.currentUserAccount.image;
            playerName.text = DataManager.instance.currentUserAccount.usertier + "급 " + DataManager.instance.currentUserAccount.username;
            enemyName.text = DataManager.instance.currentUserAccount.usertier + "급 AI봇";
            playerProfile.sprite = DataManager.instance.currentUserAccount.image;
            enemyProfile.sprite = Resources.Load<Sprite>("Images/profile icon/ai-icon");
        }
        else if (_gameState == GameState.Double)
        {
            // ToDo 더블배틀 관련 Init하기
            DataManager.instance.currentReplay.playerName = "플레이어 1";
            DataManager.instance.currentReplay.enemyName = "플레이어 2";
            DataManager.instance.currentReplay.playerTier = DataManager.instance.currentReplay.enemyTier = DataManager.instance.currentUserAccount.usertier;
            DataManager.instance.currentReplay.playerImage = DataManager.instance.currentUserAccount.image;
            DataManager.instance.currentReplay.enemyImage = Resources.Load<Sprite>("Images/profile icon/1-icon");
            playerName.text = "플레이어 1";
            enemyName.text = "플레이어 2";
            playerProfile.sprite = DataManager.instance.currentUserAccount.image;
            enemyProfile.sprite = Resources.Load<Sprite>("Images/profile icon/1-icon");
        }
    }

    
       
    
    async void AIPlayTurn((int x,int y) playerMove)
    {
        var bestMove =  await Task.Run(() =>AIController.AIBestMoveMCTS(omokBoard, GameController.playerType.White,(playerMove.x,playerMove.y),5));//AIController.AIBestMoveMCTS(omokBoard, GameController.playerType.White,playerMove ,5);
        Debug.Log(bestMove);
        if (bestMove != (-1, -1))
        {
            
            SetTurn(turn, bestMove.Item1 * 15 + bestMove.Item2);
            Debug.Log(bestMove.Item1 * 15 + bestMove.Item2);
        }
        
        
    }
    void SelectStone()
    {
        //TODO: 돌 색 고르기
    }

   
    // }
    void SetOmokBoard()
    {
        omokButtons = new GameObject[totalomokCells];
        omokBoard = new playerType[15, 15];
        
        for (int i = 0; i < totalomokCells; i++)
        {
            var OmokObejct = Instantiate(omokPrefab, transform);
            omokButtons[i] = OmokObejct;
            omokButtons[i].name = "OmokCell" + i;
            omokButtons[i].GetComponent<OmokCell>().initCell(i);
            omokBoard[i / 15, i % 15] = playerType.None;
        }
    }

    public void RemoveForbiddenCells()
    {
        ranjuRule.UpdateBoardState(omokBoard);
        for (int i = 0; i < omokButtons.Length; i++)
        {
            if (omokButtons[i].GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.Forbidden)
            {
                omokButtons[i].GetComponent<Image>().sprite = omokButtons[i].GetComponent<OmokCell>().SpriteType();
                omokButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                omokButtons[i].GetComponent<OmokCell>().My_MarkerType = OmokCell.MarkerType.None;
                
            }
        }
        ranjuRule.UpdateBoardState(omokBoard);
    }
    public void SetForbiddenCell()
    {
        ranjuRule.UpdateBoardState(omokBoard);
        for (int i = 0; i < omokButtons.Length; i++)
        {
            if (omokButtons[i].GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.None)
            {
                if (ranjuRule.RanJu(i))
                {
                    omokButtons[i].GetComponent<Image>().sprite = omokButtons[i].GetComponent<OmokCell>().SpriteType(OmokCell.MarkerType.Forbidden);
                    omokButtons[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    omokButtons[i].GetComponent<OmokCell>().My_MarkerType = OmokCell.MarkerType.Forbidden;
                    
                }
            }

            if (omokButtons[i].GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.Forbidden)
            {
                if (ranjuRule.RanJu(i))
                {
                    omokButtons[i].GetComponent<Image>().sprite = omokButtons[i].GetComponent<OmokCell>().SpriteType(OmokCell.MarkerType.Forbidden);
                    omokButtons[i].GetComponent<Image>().color = new Color(1, 1, 1,1);
                    omokButtons[i].GetComponent<OmokCell>().My_MarkerType = OmokCell.MarkerType.Forbidden;
                }
                else
                {
                    omokButtons[i].GetComponent<Image>().sprite = omokButtons[i].GetComponent<OmokCell>().SpriteType();
                    omokButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    omokButtons[i].GetComponent<OmokCell>().My_MarkerType = OmokCell.MarkerType.None;
                }
            }
        }
        ranjuRule.UpdateBoardState(omokBoard);
        
    }
    
    void SetTurn(playerType player, int index)
    {
        playerType prevPlayer = player;
        selectedCell = null;
        prevIndex = index;
        switch (player)
        {
            case playerType.Black:
                //바둑판이 빈칸이 아니면 되돌아간다
                if (omokBoard[index / 15, index % 15] != playerType.None) return;
                //현재 턴의 유저의 바둑알 위치 표시
                omokBoard[index / 15, index % 15] = player;
                //다음턴으로 넘어 간다
                turncounter++;
                //현재 셀에 현재 턴 바둑알 둔다
                omokButtons[index].GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.Black);
                
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
               
                DataManager.instance.currentReplay.gamePlayData[turncounter - 1] = index;
                RemoveForbiddenCells();
                break;
            case playerType.White:
                if (omokBoard[index / 15, index % 15] != playerType.None) return;
             
                omokBoard[index / 15, index % 15] = player;
              
                turncounter++;
                
                omokButtons[index].GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.White);
               
                turn = turn == playerType.Black ? playerType.White : playerType.Black;
                
                DataManager.instance.currentReplay.gamePlayData[turncounter - 1] = index;
               
                SetForbiddenCell();
                SetForbiddenCell();
                break;
        }
        ranjuRule.UpdateBoardState(omokBoard);
        
        timer.ResetTimer();
        timer.ResumeTimer();
        if (ranjuRule.CheckWin(prevPlayer))
        {
            DataManager.instance.currentReplay.winLoseType = WinLose(prevPlayer);
            DataManager.instance.SaveReplayData();
        }
        
        if (turncounter >= totalomokCells - 10)
        {
            int count = 0;
            for (int i = 0; i < omokButtons.Length; i++)
            {
                if (omokButtons[i].GetComponent<OmokCell>().My_MarkerType != OmokCell.MarkerType.None)
                {
                    count++;
                }
            }
            if (count == totalomokCells)
            {
                rankPanelController.ShowRankPanel();
                rankPanelController.DrawPointsUI();
            }
        }
        
        
    }

    WinLoseType WinLose(playerType player)
    {
        if (player == playerType.Black)
        {
            rankPanelController.ShowRankPanel();
            rankPanelController.GetPointsUI();
            rankPanelController.rankSystem.AddPoints();
            return WinLoseType.Win;
        }
        else if(player == playerType.White)
        {
            rankPanelController.ShowRankPanel();
            rankPanelController.LosePointsUI();
            rankPanelController.rankSystem.LosePoints();
            return WinLoseType.Lose;
        }
        else
        {
            return WinLoseType.Draw;
        }
    }
    
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerEventData.InputButton.Left == eventData.button)
        {
            var cell = eventData.pointerCurrentRaycast.gameObject;
            var previousCellSelected = selectedCell != null ? selectedCell : cell;
            //눌렀던 셀을 한번더 누르면 바둑알을 놓는다
            if (gameState == GameState.Double)
            {
                if (cell.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.PlaceMark)
                {
                    playerType tmp = turn;
                    prevIndex = cell.GetComponent<OmokCell>().index;
                    SetTurn(turn,cell.GetComponent<OmokCell>().index);
                }
            }

            if (gameState == GameState.Single)
            {
                if (cell.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.PlaceMark && turn == playerStone)
                {
                    playerType tmp = turn;
                    prevIndex = cell.GetComponent<OmokCell>().index;
                    SetTurn(turn,cell.GetComponent<OmokCell>().index);
                    if (ranjuRule.CheckWin(tmp))
                    {
                        DataManager.instance.currentReplay.winLoseType = WinLose(tmp);
                        DataManager.instance.SaveReplayData();
                    }
                    
                   
                    AIPlayTurn((prevIndex / 15, prevIndex % 15));
                    
                
             
                
                }
            }
            
            //전에 선택되었던 셀의 선택을 취소하고 새롭게 선택된 셀에 이미지를 변경한다.
            if(cell.GetComponent<OmokCell>().My_MarkerType != OmokCell.MarkerType.PlaceMark)
            {
                if (cell.GetComponent<OmokCell>().My_MarkerType == OmokCell.MarkerType.None)
                {
                    previousCellSelected.GetComponent<OmokCell>().PlaceMark(turncounter);
                                   
                    cell.GetComponent<OmokCell>().PlaceMark(turncounter, OmokCell.MarkerType.PlaceMark);
                    selectedCell = cell;
                }
                
            }
        }
    }
}


