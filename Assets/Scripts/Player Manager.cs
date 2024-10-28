using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
    [Header("Effects Config")]
    [SerializeField] private float speedEffect = 1f;
    [SerializeField] private float maxSpeed = 16f;
    [SerializeField] public float shieldDuration;
    private GameObject shield;

    [Header("Diseases Config")]
    [SerializeField] private float diseaseTime;
    [SerializeField] private float slowWalkingSpeed;

    private bool    isSlowWalking   = false;
    private float   initSpeed       = 0f;
    private bool    isSmallRange    = false;
    private int     initRange       = 0;


    private PlayerMovement player;



    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        shield = transform.Find("Shield").gameObject;
    }


    public void LootEffect(string itemID)
    {
        switch (itemID)
        {
            case ("Speed1"):
                if (player.speed >= maxSpeed || initSpeed >= maxSpeed) return;
                if (isSlowWalking) initSpeed += speedEffect;
                else player.speed += speedEffect;
                break;


            case ("Range1"):
                if (isSmallRange) initRange++;
                player.range++;
                break;


            case ("Bomb1"):
                player.bombsCapacity++;
                player.bombsQuantity++;
                break;


            case ("KickBomb"):
                player.canKick = true;
                break;


            case ("Detonator"):
                player.detonatorsQuantity = player.bombsCapacity;
                break;


            case ("Shield"):
                StartCoroutine(ShieldCoroutine());
                break;


            case ("Disease"):
                Disease();
                break;
        }

    }


    //Obsługa Tarczy
    private IEnumerator ShieldCoroutine()
    {
        player.shieldEnabled = true;
        shield.SetActive(true);

        yield return new WaitForSeconds(shieldDuration);

        player.shieldEnabled = false;
        shield.SetActive(false);
    }

    public void DisableShield()
    {
        shield.SetActive(false);
        StopCoroutine(ShieldCoroutine());
    }



    //Diseases
    private void Disease()
    {
        int random = Random.Range(1, 5); //Random bierze zakres [min, max) w przypadku int
        Debug.Log(random);
        switch (random)
        {
            case (1):
                StartCoroutine(SlowWalking());
                break;


            case (2):
                StartCoroutine(SmallRange());
                break;


            case (3):
                StartCoroutine(Diarrhea());
                break;


            case (4):
                StartCoroutine(Constipation());
                break;


            case (5):
                StartCoroutine(SwitchControls());
                break;

                //choroba jak bomba w revolt?
        }
    }

    private IEnumerator SlowWalking()
    {
        Debug.Log("Disease: SlowWalking");
        initSpeed       = player.speed;

        isSlowWalking   = true;
        player.speed    = slowWalkingSpeed;

        yield return new WaitForSeconds(diseaseTime);

        isSlowWalking   = false;
        player.speed    = initSpeed;
    }

    private IEnumerator SmallRange()
    {
        Debug.Log("Disease: SmallRange");
        initRange       = player.range;

        isSmallRange    = true;
        player.range    = 1;

        yield return new WaitForSeconds(diseaseTime);

        isSmallRange    = false;
        player.range    = initRange;
    }

    private IEnumerator Diarrhea()
    {
        Debug.Log("Disease: Diarrhea");

        player.diarrhea = true;
        yield return new WaitForSeconds(diseaseTime);
        player.diarrhea = false;
    }

    private IEnumerator Constipation()
    {
        Debug.Log("Disease: Constipation");

        player.constipation = true;
        yield return new WaitForSeconds(diseaseTime);
        player.constipation = false;
    }

    private IEnumerator SwitchControls()
    {
        Debug.Log("Disease: Switch Controls");

        player.switchControls = true;
        yield return new WaitForSeconds(diseaseTime);
        player.switchControls = false;
    }


}
