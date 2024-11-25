using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslateManager : Singleton<TranslateManager>
{
    public TMP_FontAsset font_en, font_ko, font_fr;

    public Country curCountry;
}

public enum Country {
    ko, en 
}