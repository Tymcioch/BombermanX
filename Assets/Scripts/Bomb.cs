using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Parameters")]
    [SerializeField] private float ignitTime;
    [SerializeField] private float explodeTime;
    [SerializeField] private GameObject tileExplosionPrefarb;
    [SerializeField] private GameObject lootPrefarb;

    private int range;

    private Tilemap destructible;
    private Tilemap solid;
    private GameObject bombTemp;

    private Coroutine myCoroutine;
    private PlayerMovement player;

    private bool isExploding    = false;
    private bool isKicked       = false;
    private Vector2 kickDirection;
    private float kickSpeed;
    private Rigidbody2D rb2D;




    private void Update()
    {
        //Debug.Log(isExploding + " " + isKicked);
        if (!isExploding && isKicked) KickBomb(kickDirection, kickSpeed);

    }



    public void Boom(int myRange, PlayerMovement playerMovement, GameObject bombInstance, GameObject myTilemap)
    {
        range = myRange;
        bombTemp = bombInstance;
        destructible = myTilemap.transform.Find("Destructible").GetComponent<Tilemap>();
        solid = myTilemap.transform.Find("Solid").GetComponent<Tilemap>();
        player = playerMovement;

        myCoroutine = StartCoroutine(Explosion());
    }



    //wywo³ywane kiedy wybuch innej bomby siêgnie tej
    public void InstantExplosion()
    {
        StopCoroutine(myCoroutine);
        ignitTime = 0f;
        myCoroutine = StartCoroutine(Explosion());
    }



    //pozosta³e funkcje
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(ignitTime);


        //Debug.Log("Bomb Exploding");
        bombTemp.transform.position = new Vector2((float)Math.Round(transform.position.x, 0),
                                                  (float)Math.Round(transform.position.y, 0));
        bombTemp.GetComponent<CircleCollider2D>().enabled = false;
        bombTemp.GetComponent<SpriteRenderer>().enabled = false;
        bombTemp.transform.Find("Explosion").gameObject.SetActive(true);
        isExploding = true;
        SetExplosionEffect();

        yield return new WaitForSeconds(explodeTime);

        //Debug.Log("Bomb Exploded");
        bombTemp.transform.Find("Explosion").gameObject.SetActive(false);
        player.bombsQuantity++;

        //niech jeszcze eksplozje siê wykasuj¹
        yield return new WaitForSeconds(3);

        Destroy(bombTemp);

    }




    private void SetExplosionEffect()
    {
        Vector3Int bombPos;
        int? rangeUp = null;
        int? rangeRight = null;
        int? rangeDown = null;
        int? rangeLeft = null;

        bombPos = Vector3Int.FloorToInt(new Vector2(bombTemp.transform.position.x - 9, bombTemp.transform.position.y + 4));

        for (int i = 1; i <= range; i++)
        {
            //range up
            if (rangeUp == null)
            {
                if (solid.HasTile(new Vector3Int(bombPos.x, bombPos.y + i))) rangeUp = i - 1;

                else if (destructible.HasTile(new Vector3Int(bombPos.x, bombPos.y + i)))
                {
                    rangeUp = i - 1;
                    StartCoroutine(DestroyTile(new Vector3Int(bombPos.x, bombPos.y + i)));
                }
                else if (i == range) rangeUp = i;
            }



            //range right
            if (rangeRight == null)
            {
                if (solid.HasTile(new Vector3Int(bombPos.x + i, bombPos.y))) rangeRight = i - 1;

                else if (destructible.HasTile(new Vector3Int(bombPos.x + i, bombPos.y)))
                {
                    rangeRight = i - 1;
                    StartCoroutine(DestroyTile(new Vector3Int(bombPos.x + i, bombPos.y)));
                }
                else if (i == range) rangeRight = i;
            }



            //range down
            if (rangeDown == null)
            {
                if (solid.HasTile(new Vector3Int(bombPos.x, bombPos.y - i))) rangeDown = i - 1;

                else if (destructible.HasTile(new Vector3Int(bombPos.x, bombPos.y - i)))
                {
                    rangeDown = i - 1;
                    StartCoroutine(DestroyTile(new Vector3Int(bombPos.x, bombPos.y - i)));
                }
                else if (i == range) rangeDown = i;
            }



            //range left
            if (rangeLeft == null)
            {
                if (solid.HasTile(new Vector3Int(bombPos.x - i, bombPos.y))) rangeLeft = i - 1;

                else if (destructible.HasTile(new Vector3Int(bombPos.x - i, bombPos.y)))
                {
                    rangeLeft = i - 1;
                    StartCoroutine(DestroyTile(new Vector3Int(bombPos.x - i, bombPos.y)));
                }
                else if (i == range) rangeLeft = i;
            }
        }

        //Set Sprite Up
        bombTemp.transform.Find("Explosion/ExplosionUp").GetComponent<SpriteRenderer>().size = new Vector2(1f, 1 + (float)rangeUp);
        bombTemp.transform.Find("Explosion/ExplosionUp").position += new Vector3(0f, (float)rangeUp / 2, 0f);

        //Set Sprite Right
        bombTemp.transform.Find("Explosion/ExplosionRight").GetComponent<SpriteRenderer>().size = new Vector2(1 + (float)rangeRight, 1f);
        bombTemp.transform.Find("Explosion/ExplosionRight").position += new Vector3((float)rangeRight / 2, 0f, 0f);

        //Set Sprite Down
        bombTemp.transform.Find("Explosion/ExplosionDown").GetComponent<SpriteRenderer>().size = new Vector2(1f, 1 + (float)rangeDown);
        bombTemp.transform.Find("Explosion/ExplosionDown").position += new Vector3(0f, -(float)rangeDown / 2, 0f);

        //Set Sprite Left
        bombTemp.transform.Find("Explosion/ExplosionLeft").GetComponent<SpriteRenderer>().size = new Vector2(1 + (float)rangeLeft, 1f);
        bombTemp.transform.Find("Explosion/ExplosionLeft").position += new Vector3(-(float)rangeLeft / 2, 0f, 0f);
    }



    public void SetKick(Vector2 moveDirection, float moveSpeed)
    {
        rb2D = bombTemp.GetComponent<Rigidbody2D>();
        isKicked = true;
        kickDirection = moveDirection;
        kickSpeed = moveSpeed;
    }


    public void KickBomb(Vector2 moveDirection, float kickSpeed)
    {
        rb2D.MovePosition(rb2D.position + (moveDirection * kickSpeed));
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            isKicked = false;
    }



    IEnumerator DestroyTile(Vector3Int tilePos)
    {
        destructible.SetTile(tilePos, null);
        GameObject tileExplosion = Instantiate(tileExplosionPrefarb, new Vector2(tilePos.x + 9, tilePos.y - 4), Quaternion.identity);

        yield return new WaitForSeconds(explodeTime);
        GameObject tileLoot      = Instantiate(lootPrefarb,          new Vector2(tilePos.x + 9, tilePos.y - 4), Quaternion.identity);

        yield return new WaitForSeconds(1.5f - explodeTime);
        Destroy(tileExplosion);
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (bombTemp != null)
            bombTemp.GetComponent<CircleCollider2D>().isTrigger = false;
    }


}