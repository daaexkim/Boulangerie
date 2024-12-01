using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BtnManager : Singleton<BtnManager>
{
    public void Tab(GameObject obj)
    {
        if (TouchManager.Instance.isTouching)
            return;

        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            UIManager.Instance.raycastPannel.SetActive(true);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            UIManager.Instance.raycastPannel.SetActive(false);
            obj.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => obj.SetActive(false));
        }
    }

    public void Tab_NoRayCast(GameObject obj)
    {
        if (TouchManager.Instance.isTouching)
            return;

        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            obj.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => obj.SetActive(false));
        }
    }

    public void TranslateDown() {
        SpawnManager sm = SpawnManager.Instance;
        foreach(Pain pain in sm.painList)
        {
            pain.Translation();
        }
        if(sm.newPain)
            sm.newPain.Translation();
    }

    public void TranslateUp()
    {
        SpawnManager sm = SpawnManager.Instance;
        foreach (Pain pain in sm.painList)
        {
            pain.ResetTranslation();
        }
        if(sm.newPain)
            sm.newPain.ResetTranslation();
    }

    public void MixBtn()
    {
        if (!ItemCoin(0))
            return;
        StartCoroutine(MixRoutine());
    }
    private IEnumerator MixRoutine()
    {
        SpawnManager sm = SpawnManager.Instance;
        List<Pain> curList = new List<Pain>(sm.painList);

        Pain aPain = null;
        Pain bPain = null;
        
        while(curList.Count >= 2)
        {
            aPain = curList[Random.Range(0, curList.Count)];
            aPain.SetFace(PainState.Merge);
            curList.Remove(aPain);
            bPain = curList[Random.Range(0, curList.Count)];
            bPain.SetFace(PainState.Merge);
            curList.Remove(bPain);

            Vector2 aPos = aPain.transform.position;
            Vector2 bPos = bPain.transform.position;
            aPain.transform.position = bPos;
            bPain.transform.position = aPos;

            sm.Spawn_Effect(aPain.defScale + 1f, bPos, Color.white);
            sm.Spawn_Effect(bPain.defScale + 1f, aPos, Color.white);

            yield return new WaitForSeconds(0.2f);
        }   
    }

    public void FireBtn()
    {
        if (!ItemCoin(1))
            return;
        SpawnManager sm = SpawnManager.Instance;
        List<Pain> painList = sm.painList;

        if (painList.Count <= 0)
            return;

        int maxLv = 0;
        foreach(Pain pain in painList)
        {
            if (pain.level > maxLv)
                maxLv = pain.level;
        }

        List<Pain> tarPains = painList.FindAll(data => data.level == maxLv);
        Pain tarPain = tarPains[Random.Range(0, tarPains.Count)];
        tarPain.SetFace(PainState.Fall);

        StartCoroutine(FireRoutine(tarPain));
    }
    private IEnumerator FireRoutine(Pain tarPain)
    {
        SpawnManager sm = SpawnManager.Instance;

        List<Vector2> poses = new List<Vector2>();
        int count = tarPain.level + 2;
        for (int i = 0; i < count; i++)
            poses.Add(tarPain.transform.position + new Vector3(-tarPain.defScale / 2f + tarPain.defScale / 2f / count + (i * tarPain.defScale / count),
                Random.Range(-tarPain.defScale / count, tarPain.defScale / count)));

        poses = poses.OrderBy(x => Random.value).ToList();

        foreach(Vector2 pos in poses)
        {
            sm.Spawn_FireEffect(tarPain.defScale, pos, tarPain);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void TaupeBtn()
    {
        if (!ItemCoin(2))
            return;
        SpawnManager sm = SpawnManager.Instance;
        Pain oldPain = null;
        List<Pain> availPains = new List<Pain>();

        foreach(Pain pain in sm.painList)
        {
            if (pain.isBited)
                continue;
            availPains.Add(pain);
        }

        if(availPains.Count <= 0)
        {
            return;
        }

        oldPain = availPains[Random.Range(0, availPains.Count)];
        oldPain.SetFace(PainState.Fall);

        oldPain.Bited(true);
    }

    private bool ItemCoin(int id)
    {
        GameManager gm = GameManager.Instance;
        int price = gm.itemPrices[id];

        if(gm.curCoin >= price)
        {
            SpawnManager sm = SpawnManager.Instance;
            UIManager um = UIManager.Instance;
            for (int i = 0; i < price / 10; i++)
                sm.Spawn_CoinEffect(um.coinTrans.position, um.itemTrans[id].position, false);
            return true;
        }

        return false;
    }
}
