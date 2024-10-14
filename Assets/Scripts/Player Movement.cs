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
    [SerializeField] public int         bombsQuantity = 1;
    [SerializeField] public GameObject  player;
    [SerializeField] public GameObject  tilemap;

    [SerializeField] public GameObject bombPrefarb;

    public enum PlayerType { Red, Blue }
    [SerializeField] public PlayerType playerType;

    [Header("Kicking")]
    [SerializeField] public bool    canKick;
    [SerializeField] private float  kickSpeed;

    private PlayerInput_Red     inputRed;
    private PlayerInput_Blue    inputBlue;

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

        // Inicjalizacja wejœcia w zale¿noœci od gracza
        if      (playerType == PlayerType.Red)  inputRed  = new PlayerInput_Red();
        else if (playerType == PlayerType.Blue) inputBlue = new PlayerInput_Blue();
    }




    private void Update()
    {
        Move();

        // Obs³uga stawiania bomb na podstawie typu gracza
        if (playerType == PlayerType.Red && inputRed.PlaceBomb.PlaceBomb.WasPerformedThisFrame())
        {
            PlaceBomb();
        }
        else if (playerType == PlayerType.Blue && inputBlue.PlaceBomb.PlaceBomb.WasPerformedThisFrame())
        {
            PlaceBomb();
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




    private void PlaceBomb()
    {
        if (bombsQuantity <= 0) return;

        bombsQuantity--;
        bombPlaceSound.Play();
        //sprawdziæ, czy nie ma tu ju¿ bomby!
        GameObject bombTemp = Instantiate(bombPrefarb, new Vector2((float)Math.Round(player.transform.position.x, 0),
                                                                   (float)Math.Round(player.transform.position.y, 0)),
                                                                   Quaternion.identity);


        bomb = bombTemp.GetComponent<Bomb>();
        bomb.Boom(range, this, bombTemp, tilemap);
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
