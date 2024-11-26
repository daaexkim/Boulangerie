using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public RectTransform coinTrans, mixTrans, fireTrans, taupeTrans;

    private void Update()
    {
        GameManager gm = GameManager.Instance;

        scoreText.text = gm.curScore.ToString();
        coinText.text = gm.curCoin.ToString();
    }
}

