using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ebac.Core.Singleton;

public class UIInGameManager : Singleton<UIInGameManager> 
{
    public TextMeshProUGUI uiTextCoins;
    public TextMeshProUGUI uiTextcoinsSpecial;

    public static void UpdateTexCoins(string s)
    {
        Instance.uiTextCoins.text = s;
    }
    public static void UpdateTexCoinsSpecial(string s)
    {
        Instance.uiTextcoinsSpecial.text = s;
    }
}

