using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class SpawnManager : Singleton<SpawnManager>
{
    public int MAX_LEVEL;
    public int cur_maxLevel;
    public Pain newPain;
    public List<Pain> painList;
    public float maxSpawnCool;
    public float curSpawnCool;

    public void Update()
    {
        if (curSpawnCool < maxSpawnCool)
            curSpawnCool += Time.deltaTime;

        if (!newPain && curSpawnCool >= maxSpawnCool)
            newPain = SpawnManager.Instance.Spawn_Pain_Ran();
        else if(newPain && TouchManager.Instance.isTouching)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePos.x = Mathf.Clamp(mousePos.x, newPain.LBorder, newPain.RBorder);
            mousePos.y = ScreenManager.Instance.camBound.Top - 0.5f;
            mousePos.z = 0;
            newPain.transform.position = Vector3.Lerp(newPain.transform.position, mousePos, 0.5f);
        }
        
    }

    #region Pain
    public Pain Spawn_Pain(int level)
    {
        Pain pain = PoolManager.Instance.GetFromPool<Pain>($"Pain_{level}");

        painList.Add(pain);
        pain.SetLevel(level);

        pain.rigid.simulated = false;
        pain.transform.localPosition = Vector2.zero;

        return pain;
    }
    public void Merge_Pain(int level, Vector2 pos)
    {
        Pain pain = Spawn_Pain(level);
        pain.transform.position = pos;
        pain.rigid.simulated = true;

        if (level <= 5 && level > cur_maxLevel)
            cur_maxLevel = level;
    }
    public Pain Spawn_Pain_Ran()
    {
        Pain ranPain = Spawn_Pain(Random.Range(0, Mathf.Min(cur_maxLevel, 5) + 1));

        return ranPain;
    }

    public void Destroy_Pain(int level, Pain pain)
    {
        painList.Remove(pain);
        PoolManager.Instance.TakeToPool<Pain>($"Pain_{level}", pain);
    }
    #endregion
    #region Effect
    public Effect Spawn_Effect(float size, Vector2 pos)
    {
        Effect eft = PoolManager.Instance.GetFromPool<Effect>("Effect_Pang");

        eft.SetEffect(size, pos);

        return eft;
    }
    public void Destroy_Effect(Effect effect)
    {
        PoolManager.Instance.TakeToPool<Effect>("Effect_Pang", effect);
    }
    #endregion
}
