using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankTestCode : MonoBehaviour
{
    RankSystem rankSystem;

    public void Start()
    {
       rankSystem = GetComponent<RankSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rankSystem.AddPoints(1);
            Debug.Log($"CurrentPoints: {rankSystem.currentPoint}");
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            rankSystem.LosePoints(1);
            Debug.Log($"CurrentPoints: {rankSystem.currentPoint}");
        }
    }
}
