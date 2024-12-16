using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    private Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private readonly int moveX = Animator.StringToHash("MoveX");
    private readonly int moveY = Animator.StringToHash("MoveY");
    private readonly int isMoving = Animator.StringToHash("isMoving");
    public void SetAnimation(Vector2 dir)
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
}
