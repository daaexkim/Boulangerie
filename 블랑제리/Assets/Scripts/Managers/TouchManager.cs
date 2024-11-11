using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : Singleton<TouchManager>
{
    [HideInInspector] public bool isTouching; // ��Ŭ�� üũ

    public void TouchDown()
    {
        if (isTouching)
            return;

        isTouching = true;

        //SpawnManager sm = SpawnManager.Instance;

        //if (sm.lastSlime == null)
        //    return;

        //sm.lastSlime.Drag();
    }

    public void TouchUp()
    {
        if (!isTouching)
            return;

        isTouching = false;

        SpawnManager sm = SpawnManager.Instance;

        if (sm.newPain == null)
            return;

        sm.newPain.rigid.simulated = true;
        sm.newPain = null;
        sm.curSpawnCool = 0f;

        //SpawnManager sm = SpawnManager.Instance;

        //if (sm.lastSlime == null || !sm.lastSlime.gameObject.activeSelf)
        //    return;

        //StartCoroutine(sm.lastSlime.Drop(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !isTouching && !EventSystem.current.IsPointerOverGameObject())
        {
            TouchDown();
        }
        else if (Input.GetMouseButtonUp(0) && isTouching)
        {
            TouchUp();
        }

#elif UNITY_WEBGL
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (!IsTouchValid(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) && !isTouching)
            {
                TouchDown();
            }
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isTouching)
            {
                TouchUp();
            }
        }
#endif
    }

    bool IsTouchValid(int fingerId)
    {
        // ���� ��ġ �̿��� ��ġ�� �߻� ������ Ȯ��
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId != fingerId)
            {
                // �ٸ� ��ġ�� �߻� ���̸� ���� ��ġ�� ����
                return false;
            }
        }
        // ���� ��ġ�� �߻� ���̸� ��ȿ
        return true;
    }
}