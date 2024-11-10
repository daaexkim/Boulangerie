using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSize : MonoBehaviour
{
    private Vector2 baseScale = new Vector2(0.79f, 0.79f);

    public void Adjusting(CameraBound camBound)
    {
        transform.localScale = new Vector3(camBound.Width / 4.620853f * baseScale.x, camBound.Height / 10f * baseScale.y, 1f);
    }
}
