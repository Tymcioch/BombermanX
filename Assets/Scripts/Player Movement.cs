using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] public float       speed;
    [SerializeField] public int         range;
    [SerializeField] public GameObject  player;
    [SerializeField] public GameObject  tilemap;

    [Header("Bomb Settings")]
    [SerializeField] public GameObject bombPrefarb;
    [SerializeField] public GameObject detonatorPrefarb;
    [SerializeField] private float ignitTime;
    [SerializeField] public int bombsCapacity;
    [SerializeField] public int bombsQuantity;
    [SerializeField] public int detonatorsQuantity = 3;
    private Queue<GameObject> bombQueue = new Queue<GameObject>();


    public enum PlayerType { Red, Blue }
    [SerializeField] public PlayerType playerType;
    private PlayerInput_Red inputRed;
    private PlayerInput_Blue inputBlue;

    [Header("Buffs")]
    [SerializeField] public bool    canKick;
    [SerializeField] private float  kickSpeed;
    [SerializeField] public bool    shieldEnabled;
    [SerializeField] public float   shieldDuration;
    private GameObject shield;



    private Rigidbody2D         rb2D;
    private Vector2             moveDirection;
    private Animator            animator;
    private AudioSource         bombPlaceSound;

    private Bomb    bomb;





    private void Awake()
    {
        rb2D            = GetComponent<Rigidbody2D>();
        animator        = GetComponent<Animator>();
        bombPlaceSound  = GetComponent<AudioSource>();
        shield          = transform.Find("Shield").gameObject;

        bombsQuantity = bombsCapacity;

        // Inicjalizacja wej�cia w zale�no�ci od gracza
        if      (playerType == PlayerType.Red)  inputRed  = new PlayerInput_Red();
        else if (playerType == PlayerType.Blue) inputBlue = new PlayerInput_Blue();
    }




    private void Update()
    {
        Move();

        // Obs�uga stawiania bomb na podstawie typu gracza
        if (playerType == PlayerType.Red && inputRed.PlaceBomb.PlaceBomb.WasPerformedThisFrame())
        {
            PlaceBomb();
        }
        else if (playerType == PlayerType.Blue && inputBlue.PlaceBomb.PlaceBomb.WasPerformedThisFrame())
        {
            PlaceBomb();
        }


        //detonacja
        if (playerType == PlayerType.Red && inputRed.DetonateBomb.DetonateBomb.WasPerformedThisFrame())
        {
            Debug.Log("Detonating");
            ExplodeDetonator();
        }
        else if (playerType == PlayerType.Blue && inputBlue.DetonateBomb.DetonateBomb.WasPerformedThisFrame())
        {
            Debug.Log("Detonating");
            ExplodeDetonator();
        }
    }




    private void Move()
    {
        if (playerType == PlayerType.Red)
        {
            moveDirection = inputRed.Movement.Move.ReadValue<Vector2>().normalized;
        }
        else if (playerType == PlayerType.Blue)
        {
            moveDirection = inputBlue.Movement.Move.ReadValue<Vector2>().normalized;
        }

        rb2D.MovePosition(rb2D.position + (moveDirection * (speed / 100)));

        SetAnimation(moveDirection);
    }




    private readonly int moveX = Animator.StringToHash("MoveX");
    private readonly int moveY = Animator.StringToHash("MoveY");
    private readonly int isMoving = Animator.StringToHash("isMoving");
    private void SetAnimation(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            animator.SetBool(isMoving, false);
            return;
        }
        else animator.SetBool(isMoving, true);

        animator.SetFloat(moveX, dir.x);
        animator.SetFloat(moveY, dir.y);
    }



    public void Death(AudioSource deathSound)
    {
        if (shieldEnabled)
        {
            shieldEnabled = false;
            shield.SetActive(false);
            return;
        }

        deathSound.Play();
        Destroy(gameObject);
    }



    public void EnableShield()
    {
        StartCoroutine(ShieldCoroutine());
    }
    public IEnumerator ShieldCoroutine()
    {
        shieldEnabled = true;
        shield.SetActive(true);

        yield return new WaitForSeconds(shieldDuration);

        shieldEnabled = false;
        shield.SetActive(false);
    }


    //obs�uga bomb
    private void PlaceBomb()
    {
        if (bombsQuantity <= 0) return;

        bombsQuantity--;
        bombPlaceSound.Play();
        //sprawdzi�, czy nie ma tu ju� bomby!


        if (detonatorsQuantity > 0) PlaceDetonator();

        else
        {
            GameObject bombTemp = Instantiate(bombPrefarb, new Vector2((float)Math.Round(player.transform.position.x, 0),
                                                                       (float)Math.Round(player.transform.position.y, 0)),
                                                                        Quaternion.identity);
            bomb = bombTemp.GetComponent<Bomb>();
            bomb.Boom(range, ignitTime, this, bombTemp, tilemap);
        }
    }


    private void PlaceDetonator()
    {
        detonatorsQuantity--;

        GameObject bombTemp = Instantiate(detonatorPrefarb, new Vector2((float)Math.Round(player.transform.position.x, 0),
                                                                        (float)Math.Round(player.transform.position.y, 0)),
                                                                        Quaternion.identity);
        bomb = bombTemp.GetComponent<Bomb>();
        bombQueue.Enqueue(bombTemp);
    }


    private void ExplodeDetonator()
    {
        GameObject bombTemp = bombQueue.Dequeue();
        bomb = bombTemp.GetComponent<Bomb>();
        bomb.Boom(range, ignitTime, this, bombTemp, tilemap);
        bomb.InstantExplosion();
    }


    // kopanie bomb
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!canKick) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            Bomb bombToKick = other.gameObject.GetComponent<Bomb>();
            bombToKick.SetKick(moveDirection, kickSpeed);
        }
    }




    private void OnEnable()
    {


        if (playerType == PlayerType.Red)
        {
            inputRed.Enable();

        }
        else if (playerType == PlayerType.Blue)
        {
            inputBlue.Enable();
        }

        //input.PlaceBomb.PlaceBomb.performed += ctx => PlaceBomb();
        //input.ExplodeBomb.ExplodeBomb.performed += ctx => ExplodeNextBomb();
    }


    private void OnDisable()
    {
        if (playerType == PlayerType.Red)
        {
            inputRed.Disable();
        }
        else if (playerType == PlayerType.Blue)
        {
            inputBlue.Disable();
        }
    }
}
