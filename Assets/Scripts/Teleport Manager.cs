using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private GameObject[] teleports;
    [SerializeField] private float teleportDuration;
    [SerializeField] private float teleportSpins;

    private GameObject myObject;
    private int thisTeleport;
    private int nextTeleport;
    //private GameObject

    public void tp(Collider2D collision, Teleport activeTeleport)
    {
        myObject = collision.gameObject;

        for (int i = 0; i < teleports.Length; i++)
        {
            if (activeTeleport.gameObject == teleports[i])
            {
                thisTeleport = i;
                nextTeleport = (i + 1) % teleports.Length;
                i = teleports.Length;
            }
        }



        if (myObject.CompareTag("Player"))
        {
            StartCoroutine(TeleportEffect(myObject));
        }

    }


    IEnumerator TeleportEffect(GameObject myObject)
    {

        float timePassed = 0f;
        while (timePassed < teleportDuration)
        {
            myObject.transform.position = teleports[thisTeleport].transform.position; //zablokowanie ruchu

            myObject.transform.localScale = new Vector3(1 - (timePassed / teleportDuration),
                                                        1 - (timePassed / teleportDuration), 1);

            myObject.transform.rotation = Quaternion.Euler(360 * teleportSpins * (timePassed / teleportDuration),
                                                           360 * teleportSpins * (timePassed / teleportDuration),
                                                           360 * teleportSpins * (timePassed / teleportDuration));


            timePassed += Time.deltaTime;
            yield return null; // Poczekaj do nastêpnej klatki
        }


        myObject.GetComponent<PlayerMovement>().canTeleport = false;


        timePassed = 0f;
        while (timePassed < teleportDuration)
        {
            myObject.transform.position = teleports[nextTeleport].transform.position; //zablokowanie ruchu

            myObject.transform.localScale = new Vector3((timePassed / teleportDuration),
                                                        (timePassed / teleportDuration), 1);

            myObject.transform.rotation = Quaternion.Euler(1 - (360 * teleportSpins * (timePassed / teleportDuration)),
                                                           1 - (360 * teleportSpins * (timePassed / teleportDuration)),
                                                           1 - (360 * teleportSpins * (timePassed / teleportDuration)));

            timePassed += Time.deltaTime;
            yield return null; // Poczekaj do nastêpnej klatki
        }

        myObject.transform.localScale = new Vector3(1, 1);
        myObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        myObject.GetComponent<PlayerMovement>().canTeleport = true;
    }

}
