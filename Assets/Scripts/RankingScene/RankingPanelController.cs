using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPanelController : MonoBehaviour
{
    private int rank;
    private int tier;
    private int point;
    private string userName;
    private float winRate;

    public GameObject rankBoxPrefab;
    public Transform rankBoxParent;



    private void Start()
    {
        rank = 1;
        tier = 18;
        point = 0;
        userName = "Player";
        winRate = 0.0f;

        CreateRankingTable();
    }

    private void CreateRankingTable()
    {
         

    }

    
}
