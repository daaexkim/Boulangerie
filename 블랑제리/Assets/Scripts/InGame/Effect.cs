using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Effect : MonoBehaviour, IPoolObject
{
    SpriteRenderer sr;
    SpawnManager sm;
    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        sr = GetComponent<SpriteRenderer>();
        sm = SpawnManager.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetEffect(float size, Vector2 pos, Color color)
    {
        transform.localScale = new Vector2(size, size);
        transform.position = pos;
        sr.color = color;
    }

    public void Destroy()
    {
        sm.Destroy_Effect(this);
    }
}
