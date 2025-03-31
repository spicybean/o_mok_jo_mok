using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingPanelController : MonoBehaviour
{
    [SerializeField] private GameObject rankingPanel;
    public GameObject rankBoxPrefab;
    public Transform rankBoxParent;
    private List<UserAccountData> userList = new List<UserAccountData>();
   

    private void Start()
    {
        //userList = UserAccountData.GetUserAccountData();
        DataManager.instance.userAccountList = DataManager.instance.LoadAccountsData();
        userList = DataManager.instance.userAccountList;
        //Debug.Log("유저리스트 : " + userList.Count);
        CreateRankingTable();
    }

    // lsit sort
    private void CreateRankingTable()
    {

        userList.Sort((a, b) =>
        {
            if (b.usertier != a.usertier)
            {
                return a.usertier.CompareTo(b.usertier);
               
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

            int rank = i + 1;

            rankBox.transform.GetChild(0).GetComponent<TMP_Text>().text = userList[i].usertier.ToString();
            rankBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = userList[i].points.ToString();
            rankBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = userList[i].GetWinRate().ToString();
            rankBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = userList[i].username;
            rankBox.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = rank.ToString();
            //Debug.Log("유저정보 : " + userList[i].usertier + " " + userList[i].points + " " + userList[i].GetWinRate() + " " + userList[i].username + " " );
        }

    }

    public void OnClickBackToMainSceneButton()
    {
        SceneManager.LoadScene("MainScene");
    }


}
