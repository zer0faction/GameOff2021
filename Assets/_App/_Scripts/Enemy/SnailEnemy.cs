using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailEnemy : Enemy
{
    private Vector2 velocity;

    [Header("Stats")]
    [SerializeField] private float moveSpeedBase;
    [SerializeField] private float gravity;
    private float currentMoveSpeed;

    [Header("Other GameObjects, etc.")]
    [SerializeField] private Controller2D controller;

    private bool isGrounded = false;
    private bool hitTheGroundAfterSpawning;

    // Build in Methods
    public override void Update()
    {
        base.Update();

        SetSpriteRightDirection(new Vector2(-velocity.x, velocity.y));

        velocity.y += gravity * Time.deltaTime;

        if (hitTheGroundAfterSpawning)
        {
            if (controller.collisions.left || controller.collisions.right)
            {
                //Bumped against something on the left. Start moving to the right;
                currentMoveSpeed = -currentMoveSpeed;
            }
        }

        velocity.x = currentMoveSpeed * Time.deltaTime;

        if (isGrounded)
        {
            hitTheGroundAfterSpawning = true;
        }

        controller.Move(velocity);

        isGrounded = controller.collisions.below;
        if (isGrounded) { velocity.y = 0; }
    }

    // Overwritten Methods
    public override void OnSpawn()
    {
        base.OnSpawn();

        isGrounded = false;
        hitTheGroundAfterSpawning = false;

        if (playerPosition.position.x > transform.position.x)
        {
            currentMoveSpeed = moveSpeedBase + Random.Range(-.025f, .025f);
        } else
        {
            currentMoveSpeed = (moveSpeedBase + Random.Range(-.025f, .025f)) * -1;
        }
        velocity = new Vector2(currentMoveSpeed, 0);
    }
}
