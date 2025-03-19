using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPanelController : MonoBehaviour
{
    public RankSystem rankSystem;
    public GameObject pointPrefab;
    public Transform pointBox;
    public List<GameObject> pointObjects = new List<GameObject>();

    public TextMeshProUGUI rankText;
    public TextMeshProUGUI currentPointText;
    public TextMeshProUGUI leftToRankUPText;


    private void Start()
    {
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
    }

    
    public void GetPointsUI()
    {
        
        if (pointObjects.Count <= rankSystem.currentPoint)
        {
            GameObject newPoint = Instantiate(pointPrefab, pointBox);
            pointObjects.Add(newPoint);
        }
       
        if (pointObjects.Count >= rankSystem.GetRequiredPoints())
        {
            
            ClearPoints();
            rankSystem.RankUp();
        }
    }

    // ���� ������ ���� �Լ�
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

    }

   
    public void UpdateUI()
    {
        rankText.text = $"Rank: {rankSystem.omockTier}";
        currentPointText.text = $"Points: {rankSystem.currentPoint}";
        leftToRankUPText.text = $"Left to RankUP: {rankSystem.GetRequiredPoints() - rankSystem.currentPoint}";
    }
}
