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

    [SerializeField] private int currentCoin;

    private void Start()
    {
        currentCoin = 300;
    }

    public void CoinTextChanged(int coin)
    {
        string[] strCoin = mainCoinText.text.Split(':');
        int changeCoin = int.Parse(strCoin[1]);
        currentCoin = changeCoin + coin;
        
        mainCoinText.text = strCoin[0] + ": " + currentCoin.ToString();
    }

    public void GamePlayCoinChanged()
    {
        gameplayCoinText.text = mainCoinText.text;
    }
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
}
