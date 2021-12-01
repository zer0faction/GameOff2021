using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBoss : Enemy
{
    [Header("Jumping.")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float timeToReachJumpApex;

    [Header("Other GameObjects, etc.")]
    [SerializeField] private Controller2D controller;
    [SerializeField] private Animator frogAnimator;
    [SerializeField] private AudioSource audioSource;

    private Vector2 velocity;
    private Vector2 oldVelocity;
    private Vector2 deltaPosition;
    private float currentMoveSpeed;

    private float gravity;
    private float jumpPower;

    private bool isGrounded;

    private bool reachedApex = true;
    private float maxHeightReached = Mathf.NegativeInfinity;

    private BossController bossController;

    private float jumpTimer = 0f;

    [Header("Object Pools.")]
    [SerializeField] private ObjectPool jumpParticleObjectPool;

    enum state
    {
        Jumping,
        StandingOnFloor,
        FallingFromAbove
    }
    [SerializeField] private state currentState;

    public override void OnSpawn()
    {
        base.OnSpawn();

        isGrounded = false;
        currentState = state.FallingFromAbove;

        gravity = -2 * jumpHeight / (timeToReachJumpApex * timeToReachJumpApex);
        jumpPower = 2 * jumpHeight / timeToReachJumpApex;
    }

    public override void CheckCollisions()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 1.5f, transform.position.y - 1.5f), new Vector2(transform.position.x + 1.5f, transform.position.y + 1.5f), 3, layerMask); //Distance = 2 Pixels
        if (hit.collider != null)
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag("Player"))
            {
                target.GetComponent<Player>().TakeDamage();
            }
        }
    }

    public void FallFromAbove()
    {
        StartCoroutine(FallFromAboveAfterTime());
    }

    private IEnumerator FallFromAboveAfterTime()
    {
        yield return new WaitForSeconds(2f);
        transform.position = new Vector2(playerPosition.position.x, 18f);
        currentState = state.FallingFromAbove;
    }

    private IEnumerator WaitBeforeJump()
    {
        yield return new WaitForSeconds(2.12f);

        if(currentState != state.FallingFromAbove)
        {
            currentState = state.Jumping;

            Jump();
        }
    }

    public void SetController(BossController bossController)
    {
        this.bossController = bossController;
    }

    public override void RemoveHealth(int damage)
    {
        bossController.SetHealthbarStats(currentHealth - damage);

        base.RemoveHealth(damage);

        int rand = Random.Range(1, 100);
        if (rand < 15)
        {
            audioSource.Play();
        }
    }

    public override void OnDeath()
    {
        GameObject particle = onDeathParticlePool.GetPooledObject();
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.GetComponent<AnimationParticle>().OnActivate(new Vector2(0, 0));
        }

        bossController.NextLevel();

        gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
        switch (currentState)
        {
            case state.FallingFromAbove:

                frogAnimator.Play("FrogJump");

                jumpTimer += Time.deltaTime;

                if (isGrounded && jumpTimer > .1f)
                {
                    jumpTimer = 0f;
                    StartCoroutine(WaitBeforeJump());
                    currentState = state.StandingOnFloor;
                }

                break;

            case state.Jumping:

                frogAnimator.Play("FrogJump");

                maxHeightReached = Mathf.Max(transform.position.y, maxHeightReached);

                jumpTimer += Time.deltaTime;

                if (isGrounded && jumpTimer > .1f)
                {
                    jumpTimer = 0f;
                    StartCoroutine(WaitBeforeJump());
                    currentState = state.StandingOnFloor;
                }

                break;

            case state.StandingOnFloor:

                frogAnimator.Play("FrogIdle");

                velocity.x = 0;
                break;
        }

        oldVelocity = velocity;
        velocity.y += gravity * Time.deltaTime;
        deltaPosition = (oldVelocity + velocity) * 0.5f * Time.deltaTime;

        controller.Move(deltaPosition);

        isGrounded = controller.collisions.below;
        if (isGrounded) { velocity.y = 0; }
    }

    private void Jump()
    {
        GameObject particle = jumpParticleObjectPool.GetPooledObject();
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.GetComponent<AnimationParticle>().OnActivate(new Vector2(0, 0));
        }

        float vel = Random.Range(3.5f, 4.75f);

        Debug.Log("Jump!");
        if(playerPosition.position.x > transform.position.x)
        {
            //Jump to the left

            maxHeightReached = Mathf.NegativeInfinity;
            velocity.x = vel;
            velocity.y = jumpPower;
            reachedApex = false;

        } else
        {
            //Jump to the right

            maxHeightReached = Mathf.NegativeInfinity;
            velocity.x = -vel;
            velocity.y = jumpPower;
            reachedApex = false;
        }
    }
}
