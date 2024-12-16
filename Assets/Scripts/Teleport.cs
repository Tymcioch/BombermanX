using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    TeleportManager tManager;
    PlayerMovement player;

    private int exitIndex = 0;

    private void Awake()
    {
        tManager = FindFirstObjectByType<TeleportManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerMovement>();

            if (!player.canTeleport) return;

            tManager.tp(collision, this);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player.tpInex == 0)
            {
                player.tpInex++;
                return;
            }

            player.canTeleport = true;
            player.tpInex = 0;
        }
    }


}
