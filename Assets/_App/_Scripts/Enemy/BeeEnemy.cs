using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : Enemy
{
    private Vector2 currentVector2;
    private float currentX;
    private float currentY;

    private state currentState;

    private bool leftToRight;
    private bool runTimer = false;

    private int timesSwitched;
    private int timeSwitchedBeforeAttack;

    private Vector2 targetToFlyTo;

    [Header("Player Stats")]
    [SerializeField] private float floatAbovePlayerXSpeed;
    [SerializeField] private float originalStartingAttackSpeed;
    private float startingAttackSpeed;
    [SerializeField] float floatAbovePlayerTime = 1f; // Float Above player timers
    private float floatAbovePlayerTimer = 0f;

    [Header("Other GameObjects, etc.")]
    [SerializeField] private NoColliderController2D controller;

    // State System
    enum state
    {
        FloatAbovePlayer,
        MoveTowardsPoint,
        BeforeAttack,
        FloatAfterAttack,
        FlyAway,
    }

    public override void OnSpawn()
    {
        Debug.Log("On Spawn called from Bee!");

        base.OnSpawn();

        floatAbovePlayerTimer = 0;

        timeSwitchedBeforeAttack = Random.Range(0,2);
        timesSwitched = 0;

        startingAttackSpeed = originalStartingAttackSpeed;

        animator.SetAnimation("BeeIdle");
        controller.SetSpeed(floatAbovePlayerXSpeed);

        float x = 0;
        float y = Random.Range(-0.05f, 0.05f);

        //Check if player is left or right.
        if(playerPosition.position.x > transform.position.x)
        {
            //Move to the right first, then to the left.
            leftToRight = true;
            x = 1;
        } else
        {
            x = -1;
            leftToRight = false;
        }

        currentVector2 = new Vector2(x,y);
    }

    public override void Update()
    {
        base.Update();

        SetSpriteRightDirection(currentVector2);

        switch (currentState)
        {
            case state.FloatAbovePlayer:

                if (CheckIfBeeWentOverPlayer())
                {
                    //Start timer, if timer is over switch the direction of the bee
                    runTimer = true;
                }

                if (runTimer)
                {
                    floatAbovePlayerTimer += Time.deltaTime;
                }

                if(floatAbovePlayerTimer > floatAbovePlayerTime)
                {
                    if(timesSwitched > timeSwitchedBeforeAttack)
                    {
                        //Attack!
                        StartCoroutine(WaitBeforeAttack());
                        currentState = state.BeforeAttack;
                        animator.SetAnimation("BeeAngry");
                        currentVector2 = new Vector2(0, 0);
                    } else
                    {
                        timesSwitched++;
                        floatAbovePlayerTimer = 0;
                        SwitchDirection();
                        runTimer = false;
                    }
                }

                break;
            case state.BeforeAttack:

                //Set animation, etc. Wait till

                break;
            case state.MoveTowardsPoint:

                controller.SetSpeed(startingAttackSpeed);
                startingAttackSpeed += (1 * Time.deltaTime);
                
                //Check if target is hit. If yes

                break;
            case state.FlyAway:
                break;
        }
        controller.Move(currentVector2);
    }

    private IEnumerator WaitBeforeAttack()
    {
        yield return new WaitForSeconds(1);

        // Set target to fly toward
        currentVector2 = CalculateDirectionTowardsPlayer();
        currentState = state.MoveTowardsPoint;
    }

    private void SwitchDirection()
    {
        currentVector2.x = -currentVector2.x;
    }

    private bool CheckIfBeeWentOverPlayer()
    {
        bool b;
        if(playerPosition.position.x > transform.position.x)
        {
            if (leftToRight)
            {
                b = false;
            } else
            {
                leftToRight = !leftToRight;
                b = true;
            }
        } else
        {
            if (leftToRight)
            {
                leftToRight = !leftToRight;
                b = true;
            }
            else
            {
                b = false;
            }
        }
        return b;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        currentState = state.FloatAbovePlayer;
    }
}
