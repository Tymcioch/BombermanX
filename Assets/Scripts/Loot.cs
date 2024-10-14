using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
//using UnityEngine.Tilemaps;

public class Loot : MonoBehaviour
{
    [Header("Effects Config")]
    [SerializeField] private float speedEffect;
    [SerializeField] private float maxSpeed;

    [Header("Items")]
    [SerializeField] private DropItem[] dropItems;

    private PlayerMovement player;

    private SpriteRenderer itemSprite;
    private string itemID;

    private float totalChance = 0f;
    private float selectedValue;


    private void Awake()
    {
        itemSprite = GetComponent<SpriteRenderer>();
        SetLoot();
    }


    private void SetLoot()
    {
        foreach (DropItem item in dropItems)
        {
            item.minItemChance = totalChance;
            totalChance += item.dropChance;
            item.maxItemChance = totalChance;
            //Debug.Log("Item name: " + item.name + "\nMin: " + item.minItemChance + " Max: " + item.minItemChance);
        }

        selectedValue = Random.Range(0f, totalChance);

        foreach (DropItem item in dropItems)
        {
            if (item.minItemChance <= selectedValue && item.maxItemChance > selectedValue)
            {
                Debug.Log(item.name);
                itemSprite.sprite = item.icon;
                itemID = item.ID;
                if (itemSprite.sprite == null) Destroy(gameObject);
            }
        }
    }


    private void LootEffect()
    {
        switch (itemID)
        {
            case ("Speed1"):
                if (player.speed >= maxSpeed) return;
                player.speed += speedEffect;
                break;


            case ("Range1"):
                player.range += 1;
                break;


            case ("Bomb1"):
                player.bombsQuantity += 1;
                break;

        }

    }

        

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = other.gameObject.GetComponent<PlayerMovement>();
            LootEffect();
            Destroy(gameObject);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Destroy(gameObject);
        }
        
    }



    [Serializable]
    public class DropItem
    {
        [Header("Config")]
        public string   name;
        public Sprite   icon;
        public float    dropChance;
        public string   ID;


        [HideInInspector] public float minItemChance;
        [HideInInspector] public float maxItemChance;

        [Header("Description")]
        [TextArea] public string description;
    }
}
