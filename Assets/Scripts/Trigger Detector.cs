using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AudioSource deathSound = GetComponentInParent<AudioSource>();
            deathSound.Play();
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            Bomb bomb = other.gameObject.GetComponent<Bomb>();
            Debug.Log("Instant Explosion");
            bomb.InstantExplosion();
        }
    }
}
