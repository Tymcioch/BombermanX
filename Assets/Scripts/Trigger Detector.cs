using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource deathSound = GetComponentInParent<AudioSource>();

            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player.Death(deathSound);
        }
        

        if (other.gameObject.CompareTag("Bomb"))
        {
            Bomb bomb = other.gameObject.GetComponent<Bomb>();
            Debug.Log("Instant Explosion");
            bomb.InstantExplosion();
        }

    }
}
