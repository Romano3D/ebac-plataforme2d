using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyHelper : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void KillPlayer()
    {
        if (player == null)
        {
            Debug.LogError("PlayerDestroyHelper: Nenhum Player encontrado no pai!");
            return;
        }

        player.DestroyMe();
    }
}
