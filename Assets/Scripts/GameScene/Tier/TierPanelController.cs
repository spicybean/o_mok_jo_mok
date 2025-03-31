using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TierPanelController : MonoBehaviour
{
    public TierSystem rankSystem = new TierSystem();
    public GameObject pointPrefab;
    public Transform pointBox;
    public List<GameObject> pointObjects = new List<GameObject>();

    public TextMeshProUGUI rankText;
    public TextMeshProUGUI currentPointText;
    public TextMeshProUGUI leftToRankUPText;
    public TextMeshProUGUI VictoryText;

    private string winMessage = $"게임에서 승리했습니다\n {1}포인트를 받았습니다";
    private string loseMessage = $"게임에서 패배했습니다\n {1}포인트를 잃었습니다";
    private string DrawMessage = "비겼습니다.";
    private void Start()
    {
        rankSystem = new TierSystem();
        UpdateUI();
        
    }

    private void Update()
    {
        UpdateUI();
    }

    public void ShowRankPanel()
    {
        gameObject.SetActive(true);
    }

    public void HideRankPanel()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }

    
    public void GetPointsUI()
    {
        
        if (pointObjects.Count <= rankSystem.currentPoint)
        {
            GameObject newPoint = Instantiate(pointPrefab, pointBox);
            VictoryText.text = winMessage;
            pointObjects.Add(newPoint);
        }
       
        if (pointObjects.Count >= rankSystem.GetRequiredPoints())
        {
            
            ClearPoints();
            rankSystem.RankUp();
        }
    }

    
    public void ClearPoints()
    {
        foreach (GameObject point in pointObjects)
        {
            Destroy(point);
        }
        pointObjects.Clear();
    }

    public void LosePointsUI()
    {
        if (pointObjects.Count > 0)
        {
            GameObject lastPoint = pointObjects[pointObjects.Count - 1];
            pointObjects.RemoveAt(pointObjects.Count - 1);
            Destroy(lastPoint);   
        }

        if (rankSystem.currentPoint < 0)
        {
            rankSystem.RankDown();

            int pointsToCreate = rankSystem.GetRequiredPoints() - 1;

            for (int i = 0; i < pointsToCreate; i++)
            {
                GameObject newPoint = Instantiate(pointPrefab, pointBox);
                pointObjects.Add(newPoint);
            }
        }
        VictoryText.text = loseMessage;

    }

    public void DrawPointsUI()
    {
        VictoryText.text = DrawMessage;
    }
   
    public void UpdateUI()
    {
        rankText.text = $"Rank: {rankSystem.omockTier}";
        currentPointText.text = $"Points: {rankSystem.currentPoint}";
        leftToRankUPText.text = $"Left to RankUP: {rankSystem.GetRequiredPoints() - rankSystem.currentPoint}";
    }
}
