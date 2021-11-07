﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Vertical movement.")]
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float timeTillFullSpeed;
    [SerializeField] private float timeTillFullStop;
    private float acceleration;
    private float deceleration;
    private float currentMoveSpeed = 0;

    [Header("Jumping.")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float timeToReachJumpApex;
    private float jumpPower;
    private float sidewaysJumpVelocity;
    private bool canDoubleJump;
    [SerializeField] private float timeTillDoubleJumpIsAllowed = .2f;
    private float doubleJumpTimer = 0;

    [Header("Input.")]
    [SerializeField] private float controllerAxisMin = .35f;
    bool jumpButtonPressed = false;

    [Header("Object Pools.")]
    [SerializeField] private ObjectPool jumpParticleObjectPool;

    [Header("Weapons.")]
    [SerializeField] private float timeForSingleButtonPress = .1f;
    private float timerForSingleButtonPress = 0;
    [SerializeField] private FlameThrower flameThrower;
    bool fireButtonPressed;
    bool fireButtonReleased;
    bool knifeButtonPressed;
    [SerializeField] private float timeBetweenKnifeShots;
    private float timerBetweenKnifeShots = 0;
    private string currentPosH;
    private string currentPosV;
    [SerializeField] private Transform flameThrowerPosMiddleLeft;
    [SerializeField] private Transform flameThrowerPosMiddleRight;
    [SerializeField] private Transform flameThrowerPosTopLeft;
    [SerializeField] private Transform flameThrowerPosTopRight;
    [SerializeField] private Transform flameThrowerPosDownLeft;
    [SerializeField] private Transform flameThrowerPosDownRight;
    [SerializeField] private ObjectPool knifeFireParticlePool;
    [SerializeField] private ObjectPool knifePool;

    // Physics
    private Vector3 velocity;
    private Vector3 oldVelocity;
    private bool reachedApex = true;
    private bool isGrounded = false;
    private float maxHeightReached = Mathf.NegativeInfinity;
    private float gravity;

    // State System
    enum State
    {
        Moving,
        Jumping,
        TakingDamageKnockback,
        Death
    }
    private State currentState;

    // Components
    private Controller2D characterController;

    // Varibles that need to be accessed everywhere in this class
    float inputX;
    float inputY;
    int right;
    int left;
    int up;
    int down;
    Vector3 deltaPosition;

    // Animation system
    CustomPlayerAnimator animator;
    SpriteRenderer spriteRenderer;

    // Buildin methods
    private void Awake()
    {
        // Calculating some stats
        acceleration = maxMoveSpeed / timeTillFullSpeed;
        deceleration = maxMoveSpeed / timeTillFullStop;

        gravity = -2 * jumpHeight / (timeToReachJumpApex * timeToReachJumpApex);
        jumpPower = 2 * jumpHeight / timeToReachJumpApex;
    }

    private void Start()
    {
        currentState = State.Moving;
        characterController = GetComponent<Controller2D>();
        animator = GetComponent<CustomPlayerAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Input checking
        jumpButtonPressed = Input.GetButtonDown("Jump");

        fireButtonPressed = Input.GetButton("Fire2");
        fireButtonReleased = Input.GetButtonUp("Fire2");

        knifeButtonPressed = Input.GetButtonDown("Fire1");

        inputX = Input.GetAxisRaw("Horizontal"); //.Horizontal movement
        inputY = Input.GetAxisRaw("Vertical"); //.Horizontal movement

        right = 0;
        left = 0;
        up = 0;
        down = 0;

        if (inputX > controllerAxisMin) { right = 1; }
        if (inputX < -controllerAxisMin) { left = 1; } //.Input checking
        if (inputY > controllerAxisMin) { up = 1; }
        if (inputY < -controllerAxisMin) { down = 1; } //.Input checking

        deltaPosition = new Vector3(0, 0, 0);

        // Double jump
        if (isGrounded) { canDoubleJump = true; doubleJumpTimer = 0; }
        CheckIfAnimationShouldBeJumping();
        CheckIfMovingOnGround();
        FlipAnimationRightWay();
        timerBetweenKnifeShots += Time.deltaTime;

        switch (currentState)
        {
            case State.Moving:

                HorizontalMovement(1, true, true);

                if (isGrounded)
                {
                    CheckJumpInput(false);
                }

                CheckIfBumpingHead();

                velocity.x = currentMoveSpeed;

                if (!reachedApex && maxHeightReached > transform.position.y) //. Vertical movement. 
                {
                    reachedApex = true;
                }

                maxHeightReached = Mathf.Max(transform.position.y, maxHeightReached);

                CheckIfFiring();

                oldVelocity = velocity;
                velocity.y += gravity * Time.fixedDeltaTime;

                deltaPosition = (oldVelocity + velocity) * 0.5f * Time.fixedDeltaTime;

                break;
            case State.Jumping:

                if (characterController.collisions.above)
                {
                    velocity.y = 0;
                    canDoubleJump = false;
                }

                HorizontalMovement(0.018f, false, false);

                CheckIfBumpingHead();
                IncreaseDoubleJumpTimer();

                if (isGrounded)
                {
                    // Landed again
                    currentState = State.Moving;
                    return; // ???
                }

                if(canDoubleJump && doubleJumpTimer >= timeTillDoubleJumpIsAllowed)
                {
                    CheckJumpInput(true);
                }

                CheckIfFiring();

                velocity.x = currentMoveSpeed;

                oldVelocity = velocity;
                velocity.y += gravity * Time.fixedDeltaTime;

                deltaPosition = (oldVelocity + velocity) * 0.5f * Time.fixedDeltaTime;

                break;
            case State.TakingDamageKnockback:
                break;
        }

        characterController.Move(deltaPosition); //.Move the character using the Controller2D

        isGrounded = characterController.collisions.below;
        if (isGrounded) { velocity.y = 0; }
    }

    private void FireKnife()
    {
        //Particle First.
        GameObject particle = knifeFireParticlePool.GetPooledObject();
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = new Vector2(flameThrower.transform.position.x + Random.Range(.12f,.12f), flameThrower.transform.position.y + Random.Range(.12f, .12f));
            particle.transform.rotation = flameThrower.transform.rotation;
            particle.GetComponent<AnimationParticle>().OnActivate(new Vector2(0, 0));
        }

        // Knife
        GameObject knife = knifePool.GetPooledObject();
        if (knife != null)
        {
            knife.SetActive(true);
            knife.transform.position = new Vector2(flameThrower.transform.position.x + Random.Range(.12f, .12f), flameThrower.transform.position.y + Random.Range(.12f, .12f));
            knife.transform.rotation = flameThrower.transform.rotation;

            Vector2 dir = GetFireDirection();

            knife.GetComponent<Knife>().OnActivate(dir);

        }
    }

    private void SpawnJumpParticle()
    {
        GameObject particle = jumpParticleObjectPool.GetPooledObject();
        if(particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.GetComponent<AnimationParticle>().OnActivate(new Vector2(0,0));
        }
    }

    // Other methods
    private void JumpSide(bool direction) //True = right, False = left
    {
        SpawnJumpParticle();
        currentState = State.Jumping;
        if (direction)
        {
            currentMoveSpeed = 0 + maxMoveSpeed * 1.2f;

        } else
        {
            currentMoveSpeed = 0 + maxMoveSpeed * -1.2f;
        }

        maxHeightReached = Mathf.NegativeInfinity;
        velocity.y = jumpPower;
        reachedApex = false;
    }

    private void CheckIfBumpingHead()
    {
        if (characterController.collisions.above) //. On bumping your head against the ceiling
        {
            velocity.y = 0;
            canDoubleJump = false;
        }
    }

    private void JumpUp()
    {
        SpawnJumpParticle();
        currentState = State.Jumping;
        currentMoveSpeed = 0f;
        maxHeightReached = Mathf.NegativeInfinity;
        velocity.y = jumpPower;
        reachedApex = false;
    }

    private void CheckJumpInput(bool doubleJump)
    {
        if (jumpButtonPressed)
        {
            canDoubleJump = !doubleJump;

            if (right - left == 0)
            {
                JumpUp();
            }
            else if (right - left == 1) // Right
            {
                JumpSide(true);
            }
            else if (right - left == -1) // Left
            {
                JumpSide(false);
            }
        }
    }
   
    private void HorizontalMovement(float multiplication, bool clamp, bool friction)
    {
        if (right != 0 || left != 0) //. Vertical movement speed
        {
            currentMoveSpeed += (right - left) * acceleration * Time.deltaTime * multiplication;
            if (clamp)
            {
                currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, -maxMoveSpeed, maxMoveSpeed);
            }
        }
        else if(friction)
        {
            ApplyFriction(); 
        }
    }

    private void IncreaseDoubleJumpTimer()
    {
        doubleJumpTimer += Time.deltaTime;
    }

    private void IncreaseTimerForSingleButtonPress()
    {
        timerForSingleButtonPress += Time.deltaTime;
    }

    private void ApplyFriction()
    {
        if (currentMoveSpeed != 0)
        {
            if ((Mathf.Abs(currentMoveSpeed) - deceleration * Time.deltaTime) >= 0)
            {
                int currentDir = GetCurrentDir();
                currentMoveSpeed -= deceleration * Time.deltaTime * currentDir;
            }
            else
            {
                currentMoveSpeed = 0;
            }
        }
    }

    private int GetCurrentDir()
    {
        int currentDir = 0;
        if (currentMoveSpeed > 0)
        {
            currentDir = 1;
        }
        else if (currentMoveSpeed < 0)
        {
            currentDir = -1;
        }
        return currentDir;
    }

    private void CheckIfAnimationShouldBeJumping()
    {
        if (!isGrounded)
        {
            int upDown = GetUpDown();
            if (upDown == 0)
            {
                animator.ChangeAnimationState("Jump", "Sideways");
            }
            else if(upDown == -1)
            {
                animator.ChangeAnimationState("Jump", "Down");
            }
            else if(upDown == 1)
            {
                animator.ChangeAnimationState("Jump", "Up");
            }
        }
    }

    private void CheckIfMovingOnGround()
    {
        if (isGrounded)
        {
            string anim = "Idle";
            if(currentMoveSpeed != 0)
            {
                anim = "Walk";
            }

            int upDown = GetUpDown();
            if (upDown == 0)
            {
                animator.ChangeAnimationState(anim, "Sideways");
            }
            else if (upDown == -1)
            {
                animator.ChangeAnimationState(anim, "Down");
            }
            else if(upDown == 1)
            {
                animator.ChangeAnimationState(anim, "Up");
            }
        }
    }

    private int GetUpDown()
    {
        int upDown = up - down;
        return upDown;
    }

    private int GetRightLeft()
    {
        int rightLeft = right - left;
        return rightLeft;
    }

    private void FlipAnimationRightWay()
    {
        int rightLeft = GetRightLeft();
        if (rightLeft == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (rightLeft == -1)
        {
            spriteRenderer.flipX = true;
        }
    }

    private Vector2 GetFireDirection()
    {
        int x = 1;
        if (spriteRenderer.flipX)
        {
            x = -1;
        }
        if (GetUpDown() != 0)
        {
            x = 0;
        }
        return new Vector2(x,GetUpDown());
    }

    private void CheckIfFiring()
    {
        if(knifeButtonPressed && timerBetweenKnifeShots > timeBetweenKnifeShots)
        {
            SetFirePosition();
            if (isGrounded)
            {
                currentMoveSpeed = 0;
            }
            timerBetweenKnifeShots = 0;
            FireKnife();
        }

        if (fireButtonPressed)
        {
            SetFirePosition();
            if (timerForSingleButtonPress > timeForSingleButtonPress)
            {
                flameThrower.FireFlameThrower(GetFireDirection());
                if (isGrounded)
                {
                    currentMoveSpeed = 0;
                }
            }
            else
            {
                IncreaseTimerForSingleButtonPress();
            }
        }
        else if (fireButtonReleased)
        {
            timerForSingleButtonPress = 0;
        }
    }

    private void SetFirePosition()
    {
        string positionH;
        string positionV = "Middle";

        int ud = GetUpDown();
        if (ud == 1)
        {
            //Up
            positionV = "Up";
        }
        else if (ud == -1)
        {
            //Down
            positionV = "Down";
        }

        if (!spriteRenderer.flipX)
        {
            positionH = "Right";
        }
        else
        {
            positionH = "Left";
        }

        string pos = positionV + positionH;

        switch (pos)
        {
            case "UpLeft":
                flameThrower.transform.position = flameThrowerPosTopLeft.position;
                break;
            case "DownLeft":
                flameThrower.transform.position = flameThrowerPosDownLeft.position;
                break;
            case "MiddleLeft":
                flameThrower.transform.position = flameThrowerPosMiddleLeft.position;
                break;
            case "UpRight":
                flameThrower.transform.position = flameThrowerPosTopRight.position;
                break;
            case "DownRight":
                flameThrower.transform.position = flameThrowerPosDownRight.position;
                break;
            case "MiddleRight":
                flameThrower.transform.position = flameThrowerPosMiddleRight.position;
                break;
        }
    }
}
