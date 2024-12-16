using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] GameObject[] teleports;

    private GameObject myObject;
    private int nextTeleport;
    //private GameObject

    public void tp(Collider2D collision, Teleport activeTeleport)
    {
        myObject = collision.gameObject;
        //Debug.Log("teleport");

        for (int i = 0; i < teleports.Length; i++)
        {
            if (activeTeleport.gameObject == teleports[i])
            {
                nextTeleport = (i + 1) % teleports.Length;
                i = teleports.Length;
            }
        }



        if (myObject.CompareTag("Player"))
        {
            myObject.transform.position = teleports[nextTeleport].transform.position;
            myObject.GetComponent<PlayerMovement>().canTeleport = false;
        }

    }

}
