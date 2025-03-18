using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankTestCode : MonoBehaviour
{
    RankSystem rankSystem;
     public RankPanelController rankPanelController;

    public void Start()
    {
       rankSystem = GetComponent<RankSystem>();
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rankSystem.AddPoints(1);
            rankPanelController.GetPointsUI();
           // Debug.Log($"CurrentPoints: {rankSystem.currentPoint}");

        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            rankSystem.LosePoints(1);
            rankPanelController.LosePointsUI();
           // Debug.Log($"CurrentPoints: {rankSystem.currentPoint}");
        }
    }
}
