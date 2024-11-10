using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    public AdjustSize bgTrans;
    public AdjustSize ovenTrans;

    public CameraBound camBound;
    protected override void Awake()
    {
        base.Awake();

        camBound.SetCameraBound();

        bgTrans.Adjusting(camBound);
        ovenTrans.Adjusting(camBound);

        bgTrans.transform.position += new Vector3(0, 0.8f);
        ovenTrans.transform.position = new Vector3(0, camBound.Top - 0.69f);
    }
}

[System.Serializable]
public class CameraBound
{
    public Camera camera;

    [HideInInspector] public float size_x, size_y;

    public void SetCameraBound()
    {
        camera = Camera.main;

        size_y = camera.orthographicSize;
        size_x = camera.orthographicSize * Screen.width / Screen.height;
    }

    public float Bottom
    {
        get
        {
            return size_y * -1 + camera.gameObject.transform.position.y;
        }
    }

    public float Top
    {
        get
        {
            return size_y + camera.gameObject.transform.position.y;
        }
    }

    public float Left
    {
        get
        {
            return size_x * -1 + camera.gameObject.transform.position.x;
        }
    }

    public float Right
    {
        get
        {
            return size_x + camera.gameObject.transform.position.x;
        }
    }

    public float Height
    {
        get
        {
            return size_y * 2;
        }
    }

    public float Width
    {
        get
        {
            return size_x * 2;
        }
    }
}