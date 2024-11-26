using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int curScore, tarScore, scorePerCoin;
    public int curCoin, tarCoin;

    private void Start()
    {
        StartCoroutine(CoinRoutine());
        StartCoroutine(ScoreRoutine());
    }
    public void GainScore(int amount)
    {
        tarScore += amount;
    }
    public void GainCoin(int amount)
    {
        tarCoin += amount;
    }

    private IEnumerator CoinRoutine()
    {
        while (true)
        {
            if (curCoin != tarCoin)
                curCoin++;

            yield return new WaitForSeconds(0.05f);
        }
    }
    private IEnumerator ScoreRoutine()
    {
        SpawnManager sm = SpawnManager.Instance;
        UIManager um = UIManager.Instance;

        while (true)
        {
            if (curScore != tarScore)
                curScore++;

            if (curScore % 50 == 0 && curScore != 0)
                for(int i = 0; i < 5; i++)
                    sm.Spawn_CoinEffect(um.scoreText.transform.position, um.coinTrans.position);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
