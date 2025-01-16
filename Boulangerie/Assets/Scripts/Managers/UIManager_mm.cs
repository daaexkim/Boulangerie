using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UIManager_mm : Singleton<UIManager_mm>
{
    public TextMeshProUGUI coinTxt;
    public TextMeshProUGUI[] topScoreTxts;

    private void Start()
    {
        GameManager gm = GameManager.Instance;

        coinTxt.text = $"<sprite=0>{gm.curCoin}";
        for (int i = 0; i < 3; i++)
            topScoreTxts[i].text = $"TOP {gm.topScores[i]}";
    }
}
