using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
    {
        public TextMeshProUGUI coinText; // arraste o TMP aqui no Inspector
        private int lastCoinCount;

        private void Start()
        {
            UpdateText();
        }
    private void Update()
    {
        if (ItemManager.Instance.coins != lastCoinCount)
        {
            UpdateText();
        }
    }
         private void UpdateText()
        {
            lastCoinCount = ItemManager.Instance.coins;
            coinText.text = "X:  " + lastCoinCount;
        }
    }