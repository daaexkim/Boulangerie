using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class SpawnManager : Singleton<SpawnManager>
{
    public Pain newPain;
    public List<Pain> painList;

    #region Pain
    public Pain Spawn_Pain(int level)
    {
        Pain pain = PoolManager.Instance.GetFromPool<Pain>($"Pain_{level}");

        painList.Add(pain);
        pain.SetLevel(level);

        pain.transform.localPosition = Vector2.zero;
        return pain;
    }
    public void Spawn_Pain(int level, Vector2 pos)
    {
        Pain pain = Spawn_Pain(level);
        pain.transform.position = pos;
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
