using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using DG.Tweening;

public class CoinEffect : MonoBehaviour, IPoolObject
{
    SpawnManager sm;
    GameManager gm;

    public void OnCreatedInPool()
    {
        sm = SpawnManager.Instance;
        gm = GameManager.Instance;

        name = name.Replace("(Clone)", "");
    }

    public void OnGettingFromPool()
    {
    }

    public void MoveTween(Vector2 startPos, Vector2 targetPos)
    {
        transform.position = startPos;

        Vector2 spawnPos = startPos + Random.insideUnitCircle * 100f;

        Sequence moveSeq = DOTween.Sequence().SetUpdate(true);
        moveSeq.Append(transform.DOMove(spawnPos, 1f).SetEase(Ease.OutQuart))
            .Join(transform.DOShakeScale(1f, 1, 10, 1))
            .Append(transform.DOMove(targetPos, 1f))
            .OnComplete(() => {
                sm.Destroy_CoinEffect(this);
                gm.GainCoin(10);
            });
    }
}
