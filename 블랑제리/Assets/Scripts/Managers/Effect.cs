using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Effect : MonoBehaviour, IPoolObject
{
    SpawnManager sm;
    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        sm = SpawnManager.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetEffect(float size, Vector2 pos)
    {
        transform.localScale = new Vector2(size, size);
        transform.position = pos;
    }

    public void Destroy()
    {
        sm.Destroy_Effect(this);
    }
}
