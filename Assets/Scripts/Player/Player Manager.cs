using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{
    [Header("Effects Config")]
    [SerializeField] private float speedEffect  = 1f;
    [SerializeField] private float maxSpeed     = 16f;
    [SerializeField] private int maxRange       = 15;
    [SerializeField] public int shieldDuration;
    private GameObject shield;
    private GameObject flower;

    [Header("Diseases Config")]
    [SerializeField] private float  diseaseTime;
    [SerializeField] private float  slowWalkingSpeed;
    [SerializeField] private float  tooSpeedySpeed;
    [SerializeField] private int    diseaseCount;
    [SerializeField] private int    heavyDiseaseAmount;

    private bool    isSlowWalking  = false;
    private float   initSpeed     = 0f;
    private bool    isSmallRange   = false;
    private int     initRange       = 0;


    private PlayerMovement player;



    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        shield = transform.Find("Shield").gameObject;
        flower = transform.Find("Flower").gameObject;
    }


    public void LootEffect(string itemID)
    {
        switch (itemID)
        {
            //Normal
            case ("Speed1"):
                if (player.speed >= maxSpeed || initSpeed >= maxSpeed) return;
                if (isSlowWalking) initSpeed += speedEffect;
                else player.speed += speedEffect;
                break;


            case ("Range1"):
                if (isSmallRange) initRange++;
                else player.range++;
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
                Disease(Random.Range(1, diseaseCount)); //Random bierze zakres [min, max) w przypadku int);
                break;


            //Rare
            case ("Swap"):
                List<GameObject> playersToSwap = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

                for (int i = 0; i < playersToSwap.Count; i++)
                {
                    if (playersToSwap[i] == player.gameObject) playersToSwap.RemoveAt(i);
                }

                GameObject radomPlayer = playersToSwap[Random.Range(0, playersToSwap.Count)]; //Random.Range(0, playersToSwap.Count);

                Vector2 tempPosiotion = player.transform.position;
                player.transform.position = radomPlayer.transform.position;
                radomPlayer.transform.position = tempPosiotion;

                break;


            case ("RangeMax"):
                if (isSmallRange) initRange = maxRange;
                else player.range = maxRange;
                break;


            case ("MegaBomb"):
                player.hasMegaBomb = true;
                break;


            case ("Flower"):
                flower.SetActive(true);
                player.flowerEnabled = true;
                break;


            case ("DiseaseMax"):
                int random1 = Random.Range(1, diseaseCount);
                int random2 = Random.Range(1, diseaseCount);
                int random3 = Random.Range(1, diseaseCount);
                if    (random2 == random1) random2 = (random2 % diseaseCount) + 1;
                while (random3 == random1 || random3 == random2) random3 = (random3 % diseaseCount) + 1;
                Disease(random1);
                Disease(random2);
                Disease(random3);
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
    private void Disease(int random)
    {
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


            case (6):
                StartCoroutine(TooSpeedy());
                break;


                //case (6):
                //    StartCoroutine(FastExplosion());
                //    break;

                //choroba jak bomba w revolt?
        }
    }

    private IEnumerator SlowWalking()
    {
        Debug.Log("Disease: SlowWalking");
        initSpeed = player.speed;

        isSlowWalking = true;
        player.speed = slowWalkingSpeed;

        yield return new WaitForSeconds(diseaseTime);

        isSlowWalking = false;
        player.speed = initSpeed;
    }

    private IEnumerator TooSpeedy()
    {
        Debug.Log("Disease: TooSpeedy");
        initSpeed = player.speed;

        isSlowWalking = true;
        player.speed = tooSpeedySpeed;

        yield return new WaitForSeconds(diseaseTime);

        isSlowWalking = false;
        player.speed = initSpeed;
    }

    private IEnumerator SmallRange()
    {
        Debug.Log("Disease: SmallRange");
        initRange = player.range;

        isSmallRange = true;
        player.range = 1;

        yield return new WaitForSeconds(diseaseTime);

        isSmallRange = false;
        player.range = initRange;
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

    //private IEnumerator FastExplosion()
    //{
    //    Debug.Log("Disease: Fast Explosion");

    //    initRange = player.range;

    //    isSmallRange = true;
    //    player.range = 1;

    //    yield return new WaitForSeconds(diseaseTime);

    //    isSmallRange = false;
    //    player.range = initRange;
    //}



}
