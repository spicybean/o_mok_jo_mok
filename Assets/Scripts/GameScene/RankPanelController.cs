using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPanelController : MonoBehaviour
{
    Scrollbar scrollbar;

    private void Start()
    {
        scrollbar = GetComponentInChildren<Scrollbar>();
    }


    public void ShowRankPanel()
    {
        //dotoween?
        gameObject.SetActive(true);
    }

    public void HideRankPanel()
    {
        gameObject.SetActive(false);
    }

    public void GetPointsUI()
    {
        // 차오르는 연출
    }

    public void LosePointsUI()
    {
        //빠지는 연출
    }
    
    public void RankUpUI()
    {

    }

    public void RankDownUI()
    {

    }

}
