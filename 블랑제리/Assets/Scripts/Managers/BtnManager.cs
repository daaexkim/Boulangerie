using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BtnManager : Singleton<BtnManager>
{
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
}
