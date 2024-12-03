using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameMode gameMode;
    public bool isGameover;
    public int curScore, tarScore;
    public int curCoin, tarCoin;
    private int lastScore = 0;
    public int[] itemPrices;

    public void ReSet()
    {
        Time.timeScale = 1f;
        isGameover = false;
        curScore = 0;
        tarScore = 0;
        lastScore = 0;
        curCoin = 0;
        tarCoin = 0;
    }

    public void GainScore(int amount)
    {
        tarScore += amount;
    }
    public void GainCoin(int amount)
    {
        int calAmount = gameMode == GameMode.Bebe ? amount / 2 : amount;

        tarCoin += calAmount;
    }

    public IEnumerator CoinRoutine()
    {
        while (true)
        {
            if (curCoin < tarCoin)
                curCoin += 5;
            else if (curCoin > tarCoin)
                curCoin -= 5;

            yield return new WaitForSeconds(0.025f);
        }
    }
    public IEnumerator ScoreRoutine()
    {
        SpawnManager sm = SpawnManager.Instance;
        UIManager um = UIManager.Instance;

        while (true)
        {
            if (curScore != tarScore)
                curScore++;

            if (curScore % 50 == 0 && curScore != 0 && curScore != lastScore)
            {
                for (int i = 0; i < 5; i++)
                    sm.Spawn_CoinEffect(um.scoreText.transform.position, um.coinTrans.position, true);
                lastScore = curScore;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void GameOver()
    {
        if (isGameover)
            return;

        isGameover = true;
        StartCoroutine(GameoverRoutine());
    }
    private IEnumerator GameoverRoutine()
    {
        SpawnManager sm = SpawnManager.Instance;
        List<Pain> liveList = new List<Pain>(sm.painList);

        foreach (Pain pain in liveList)
        {
            pain.Burned();
            yield return new WaitForSeconds(0.05f);
        }

        if(sm.newPain != null)
        {
            sm.newPain.Burned();
            yield return new WaitForSeconds(0.2f);
        }

        sm.Spawn_Effect(8f, Vector2.zero, Color.white);

        foreach (Pain pain in liveList)
        {
            sm.Destroy_Pain(pain.level, pain);
            yield return null;
        }

        if(sm.newPain != null)
            sm.Destroy_Pain(sm.newPain.level, sm.newPain);
        yield return new WaitForSeconds(0.5f);

        UIManager um = UIManager.Instance;
        um.gameoverPannel.SetUI(1000, tarScore, tarCoin);

        BtnManager bm = BtnManager.Instance;
        bm.Stop();
        bm.Tab(um.gameoverPannel.trans.gameObject);
    }
}

public enum GameMode { Bebe, Jeune, Adulte }