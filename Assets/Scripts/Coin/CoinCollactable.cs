using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollactable : MonoBehaviour
{
    public class CoinCollectable : ItemCollactableBase
    {
        public int coinValue = 1;

        protected override void OnCollect()
        {
            // Adiciona as moedas no ItemManager
            ItemManager.Instance.AddCoins(coinValue);

            // Opcional: som, efeito, animação, etc.
            Debug.Log("+ " + coinValue + " coin(s)! Total: " + ItemManager.Instance.coins);
        }
    }
}
