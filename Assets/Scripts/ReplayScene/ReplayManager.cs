using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReplayManager : MonoBehaviour
{
    [SerializeField] private static int _maxSaveCount = 10;
    ReplayData[] gameDataList = new ReplayData[_maxSaveCount];
    void Start()
    {
        for (int i = 1; i <= _maxSaveCount; i++)
        {
            DataManager.instance.currentReplaySlotNum = i;
            if (DataManager.instance.CheckReplayData())
            {
                gameDataList[i] = DataManager.instance.LoadReplayData();
            }
            else
            {
                Debug.Log("비어있음");
            }
        }

        DataManager.instance.ClearReplayData();
    }

    public void OnSlotClicked()
    {
        //슬롯 버튼의 이름에서 Slot번호를 추출하여 DataManager에 전달.
        DataManager.instance.currentReplaySlotNum =
            int.Parse(EventSystem.current.currentSelectedGameObject.name.Split(' ')[2]);
        if (DataManager.instance.CheckReplayData())
        {
            DataManager.instance.LoadReplayData();
        }
        else
        {
            //디버깅용 코드(원래는 세이브 데이터가 없으면 클릭되지 않음)
            DataManager.instance.currentReplay = new ReplayData();
            DataManager.instance.SaveReplayData();
        }
    }
}
