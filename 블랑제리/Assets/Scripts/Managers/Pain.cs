using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using DG.Tweening;

public class Pain : MonoBehaviour, IPoolObject
{
    SpawnManager sm;
    [HideInInspector] public Rigidbody2D rigid;

    public int level;
    public float defScale;
    public bool isMerge;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        sm = SpawnManager.Instance;
        rigid = GetComponent<Rigidbody2D>();
    }

    public void OnGettingFromPool()
    {
        isMerge = false;

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.OnStart(() =>
        {
            rigid.simulated = false;
            transform.localScale = new Vector2(0.01f, 0.01f);

        });
        seq.Append(transform.DOScale(new Vector3(defScale, defScale), 0.1f))
            .OnComplete(() =>
            {
                transform.localScale = new Vector3(defScale, defScale);
                rigid.simulated = true;
            });
    }

    public void SetLevel(int _level)
    {
        level = _level;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Pain"))
        {
            Pain other =  collision.gameObject.GetComponent<Pain>();

            if(other.level == level && !isMerge && !other.isMerge)
            {
                other.isMerge = true;
                isMerge = true;
                other.rigid.simulated = false;
                rigid.simulated = false;

                Vector2 thisTrans = transform.position;
                Vector2 otherTrans = other.transform.position;
                Vector2 middlePos = new Vector2((thisTrans.x + otherTrans.x) / 2f, (thisTrans.y + otherTrans.y) / 2f);

                Sequence mergeSeq = DOTween.Sequence().SetUpdate(true);
                mergeSeq.Append(transform.DOMove(middlePos, 0.1f));
                mergeSeq.Join(other.transform.DOMove(middlePos, 0.1f));
                mergeSeq.OnComplete(() =>
                {
                    // Done Merge
                    sm.Destroy_Pain(level, this);
                    sm.Destroy_Pain(level, other);
                    sm.Spawn_Effect(defScale + 0.2f, middlePos);

                    sm.Spawn_Pain(++level, middlePos);
                });
            }
        }
    }
}
