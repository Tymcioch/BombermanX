using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class MegaBomb : MonoBehaviour
{
    [Header("Bomb Parameters")]
    [SerializeField] private float ignitTime;
    [SerializeField] private float explodeTime;
    [SerializeField] private int   range;
    [SerializeField] private GameObject tileExplosionPrefarb;
    [SerializeField] private GameObject lootPrefarb;

    private Tilemap destructible;
    private Tilemap solid;
    private CameraManager mainCamera;

    private bool isExploding = false;

    private GameObject bombTemp;
    private CircleCollider2D myCollider;





    public void Boom(GameObject bombInstance, GameObject myTilemap, GameObject myCamera)
    {
        bombTemp = bombInstance;
        destructible = myTilemap.transform.Find("Destructible").GetComponent<Tilemap>();
        solid = myTilemap.transform.Find("Solid").GetComponent<Tilemap>();
        mainCamera = myCamera.GetComponent<CameraManager>();

        myCollider = bombTemp.GetComponent<CircleCollider2D>();

        StartCoroutine(Explosion());
    }



    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(ignitTime);


        //Debug.Log("Bomb Exploding");
        bombTemp.transform.position = new Vector2((float)Math.Round(transform.position.x, 0),
                                                  (float)Math.Round(transform.position.y, 0));


        bombTemp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Flame_MegaBomb");

        isExploding = true;
        myCollider.isTrigger = true;
        myCollider.radius = range;

        SetExplosionEffect();
        mainCamera.CameraShake();
        bombTemp.transform.Find("Explosion").gameObject.SetActive(true);



        yield return new WaitForSeconds(explodeTime);

        isExploding = false;
        bombTemp.SetActive(false);

        //niech jeszcze eksplozje siê wykasuj¹
        yield return new WaitForSeconds(3);

        Destroy(bombTemp);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isExploding) return;

        if (other.gameObject.CompareTag("Bomb"))
        {
            Bomb bomb = other.gameObject.GetComponent<Bomb>();
            Debug.Log("Instant Explosion");
            bomb.InstantExplosion();
        }


        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource deathSound = GetComponentInParent<AudioSource>();

            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player.Death(deathSound);
        }

    }


    private void SetExplosionEffect()
    {
        Vector3Int bombPos = Vector3Int.FloorToInt(new Vector2(bombTemp.transform.position.x - 9, bombTemp.transform.position.y + 4));


        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j<= range; j++)
            {              
                if (Vector3.Magnitude(new Vector2(i, j)) >= (range + 0.2)) continue;

                Vector3Int tilePos = new Vector3Int(bombPos.x + i, bombPos.y + j);
                if (!destructible.HasTile(tilePos) && !solid.HasTile(tilePos)) continue;

                StartCoroutine(DestroyTile(tilePos));
            }
        }
    }


    IEnumerator DestroyTile(Vector3Int tilePos)
    {
        destructible.SetTile(tilePos, null);
        solid.SetTile(tilePos, null);
        GameObject tileExplosion = Instantiate(tileExplosionPrefarb, new Vector2(tilePos.x + 9, tilePos.y - 4), Quaternion.identity);

        yield return new WaitForSeconds(explodeTime);
        //bez lootu :P
        //GameObject tileLoot = Instantiate(lootPrefarb, new Vector2(tilePos.x + 9, tilePos.y - 4), Quaternion.identity);

        yield return new WaitForSeconds(1.5f - explodeTime);
        Destroy(tileExplosion);
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (isExploding) return;

        if (bombTemp != null)
            bombTemp.GetComponent<CircleCollider2D>().isTrigger = false;
    }

}
