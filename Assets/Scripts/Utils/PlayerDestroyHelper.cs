using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyHelper : MonoBehaviour
{
    public PlayerController player;

    public void killPlayer()
    {
        player.DestroyMe();
    }
}
