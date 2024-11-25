using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI scoreText;
    private void Update()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }
}

