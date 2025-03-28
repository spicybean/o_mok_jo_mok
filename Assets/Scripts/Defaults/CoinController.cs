using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinController : Singleton<CoinController>
{
    [SerializeField] private TextMeshProUGUI mainCoinText;
    [SerializeField] private TextMeshProUGUI gameplayCoinText;

    public int currentCoin;

    private void Start()
    {
        CoinTextChanged(currentCoin);
    }

    public void CoinTextChanged(int coin)
    {
        currentCoin = DataManager.instance.currentUserAccount.coin;
        Debug.Log("beforeCoin :" + currentCoin);
        currentCoin += coin;
        Debug.Log("afterCoin :" + currentCoin);
        DataManager.instance.currentUserAccount.coin = currentCoin;
        DataManager.instance.SaveCurrentUserAccountData(DataManager.instance.currentUserAccount.userIndex);
        DataManager.instance.SaveAccountsData();
        
        
        mainCoinText.text = "코인 : " + currentCoin.ToString();
    }

    public void GamePlayCoinChanged()
    {
        gameplayCoinText.text = mainCoinText.text;
    }
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
}
