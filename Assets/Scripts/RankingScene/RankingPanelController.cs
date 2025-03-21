using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingPanelController : MonoBehaviour
{
    /*
    private int rank;
    private int tier;
    private int point;
    private string userName;
    private float winRate;
    */
    public GameObject rankBoxPrefab;
    public Transform rankBoxParent;

    UserAccountData userAccountData;

    private List<UserAccountData> userList = new List<UserAccountData>();


    private void Start()
    {
       
        CreateRankingTable();
    }


    private void CreateRankingTable()
    {

        userList.Sort((a, b) =>
        {
            if (b.usertier != a.usertier)
            {
                return b.usertier.CompareTo(a.usertier);
               
            }
            else if (b.points != a.points)
            {
                return b.points.CompareTo(a.points);
            }
            else
            {
                return b.GetWinRate().CompareTo(a.GetWinRate());
            }
            
        });
        

        //유저수 만큼 랭킹박스 생성
        for (int i = 0; i < userList.Count; i++)
        {
            GameObject rankBox = Instantiate(rankBoxPrefab, rankBoxParent);
            rankBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = userList[i].usertier.ToString();
            rankBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = userList[i].points.ToString();
            rankBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = userList[i].GetWinRate().ToString();
            rankBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = userList[i].username;
            Debug.Log("유저정보 : " + userList[i].usertier + " " + userList[i].points + " " + userList[i].GetWinRate() + " " + userList[i].username + " " );
        }

       
        //티어 먼저 앞으로 보내고 
        //포인트로 정렬
        //승률 보고
        //랭킹박스에 정보 넣기
        //랭킹박스에 순위 넣기


    }


}
