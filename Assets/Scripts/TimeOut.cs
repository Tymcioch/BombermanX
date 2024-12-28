using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TimeOut : MonoBehaviour
{
    [SerializeField] private float gameLength;

    [Header("Block Ending")]
    [SerializeField] private Tilemap frame;
    [SerializeField] private float timeBetween;


    void Awake()
    {
        Debug.Log(frame.cellBounds.size.x);
        StartCoroutine(Disaster());
    }


    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Pause");
            SceneManager.LoadScene("Pause");
        }


        if (players.Length <= 1)
        {
            StartCoroutine(Pause());
        }


    }


    IEnumerator Pause()
    {
        yield return new WaitForSeconds(1);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 1)
        {
            Debug.Log("Win");

        }

        if (players.Length <= 0)
        {
            Debug.Log("Tie");
        }

        SceneManager.LoadScene("Pause");
    }


    IEnumerator Disaster()
    {
        yield return new WaitForSeconds(gameLength);

        int random = Random.Range(1, 2);

        switch (random)
        {
            case 1:
                StartCoroutine(Blocks());
                break;


        }
    }


    IEnumerator Blocks()
    {
        int sizeX = frame.cellBounds.size.x - 2;
        int sizeY = frame.cellBounds.size.y - 2;

        int x = -10;
        int y = 5;

        Tile myTile = (Tile)frame.GetTile(new Vector3Int(x, y));



        while (sizeX > 0 || sizeY > 0)
        {
            x++;
            y--;

            //right
            for (int i = 0; i < sizeX - 1; i++)
            {
                frame.SetTile(new Vector3Int(x, y), myTile);
                yield return new WaitForSeconds(timeBetween);
                x += 1;
            }


            //down
            for (int i = 0; i < sizeY - 1; i++)
            {
                frame.SetTile(new Vector3Int(x, y), myTile);
                yield return new WaitForSeconds(timeBetween);
                y -= 1;
            }


            Debug.Log(sizeX);
            //left
            for (int i = 0; i < sizeX - 1; i++)
            {
                frame.SetTile(new Vector3Int(x, y), myTile);
                yield return new WaitForSeconds(timeBetween);
                x -= 1;
            }



            //up
            for (int i = 0; i < sizeY - 1; i++)
            {
                frame.SetTile(new Vector3Int(x, y), myTile);
                yield return new WaitForSeconds(timeBetween);
                y += 1;
            }

            sizeX -= 2;
            sizeY -= 2;

        }


        //while (!(frame.HasTile(new Vector3Int(x, y)) &&
        //        frame.HasTile(new Vector3Int(x + 1, y)) &&
        //        frame.HasTile(new Vector3Int(x - 1, y)) &&
        //        frame.HasTile(new Vector3Int(x, y + 1)) &&
        //        frame.HasTile(new Vector3Int(x, y - 1))))
        //{

        //    frame.SetTile(new Vector3Int(x, y), myTile);
        //    yield return new WaitForSeconds(timeBetween);

        //    if(frame.HasTile(new Vector3Int(x - 1, y) && frame.HasTile(new Vector3Int(x, y + 1)))


        //}




    }

}
