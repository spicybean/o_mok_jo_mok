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
        // �������� ����
    }

    public void LosePointsUI()
    {
        //������ ����
    }
    
    public void RankUpUI()
    {

    }

    public void RankDownUI()
    {

    }

}
