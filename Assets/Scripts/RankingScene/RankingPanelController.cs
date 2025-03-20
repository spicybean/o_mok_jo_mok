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

    UserAccountData userAccountData;

    private void Start()
    {
        tier = userAccountData.usertier;
        point = userAccountData.points;
        userName = userAccountData.username;
        winRate = (float)userAccountData.winmatch  / (float)userAccountData.totalmatch;
        CreateRankingTable();
    }

    private void CreateRankingTable()
    {
        //유저수 만큼 랭킹박스 생성


    }


}
