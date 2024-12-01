using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isGameover;
    public int curScore, tarScore, scorePerCoin;
    public int curCoin, tarCoin;
    public int[] itemPrices;

    public void GainScore(int amount)
    {
        tarScore += amount;
    }
    public void GainCoin(int amount)
    {
        tarCoin += amount;
    }

    public IEnumerator CoinRoutine()
    {
        while (true)
        {
            if (curCoin < tarCoin)
                curCoin += 10;
            else if (curCoin > tarCoin)
                curCoin -= 10;

            yield return new WaitForSeconds(0.05f);
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

            if (curScore % 50 == 0 && curScore != 0)
                for(int i = 0; i < 5; i++)
                    sm.Spawn_CoinEffect(um.scoreText.transform.position, um.coinTrans.position, true);
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
        foreach(Pain pain in sm.painList)
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

        foreach (Pain pain in sm.painList.ToList())
        {
            sm.Destroy_Pain(pain.level, pain);
            yield return null;
        }

        if(sm.newPain != null)
            sm.Destroy_Pain(sm.newPain.level, sm.newPain);
        yield return new WaitForSeconds(0.5f);

        UIManager um = UIManager.Instance;
        um.gameoverPannel.SetUI(1000, tarScore, tarCoin);
        BtnManager.Instance.Tab(um.gameoverPannel.trans.gameObject);
    }
}
