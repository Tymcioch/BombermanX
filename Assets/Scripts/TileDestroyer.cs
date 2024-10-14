using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    GameObject player;
    Vector3Int playerPos;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    //-9 4

    void Update()
    {
        playerPos = Vector3Int.FloorToInt(new Vector2(player.transform.position.x - 9, player.transform.position.y + 4));
        tilemap.SetTile(playerPos, null);
    }
}
