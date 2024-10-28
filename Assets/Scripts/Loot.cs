using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
//using UnityEngine.Tilemaps;

public class Loot : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private DropItem[] dropItems;

    private PlayerManager player;

    private SpriteRenderer itemSprite;
    private AudioSource collectSound;
    private string itemID;

    private float totalChance = 0f;
    private float selectedValue;


    private void Awake()
    {
        itemSprite = GetComponent<SpriteRenderer>();
        collectSound = GetComponent<AudioSource>();
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




    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = other.gameObject.GetComponent<PlayerManager>();
            collectSound.Play();
            player.LootEffect(itemID);

            itemSprite.enabled = false;
            GetComponent<Collider2D>().enabled = false;

            Invoke("DestroyObject", 3f);

        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Destroy(gameObject);
        }
        
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
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
