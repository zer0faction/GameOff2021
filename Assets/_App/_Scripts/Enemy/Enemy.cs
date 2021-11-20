using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Stuff.")]
    [SerializeField] private int maxHealth;
    public int currentHealth;

    [Header("Enemy Stats")]
    public bool isBoss;
    [HideInInspector] private float timeBetweenTakingFirethrowerDamage = .2f;
    private bool canTakeDamageFromFireThrower = true;

    [Header("Other gameobjects, components, etc.")]
    [SerializeField] public LayerMask layerMask;
    [SerializeField] public SpriteRenderer spriteRenderer;
    public EnemyAnimator animator;
    private ObjectPool onDeathParticlePool;

    // Other gameobjects
    [HideInInspector] public Transform playerPosition;

    public bool despawnOnLineHit = true;

    // Build-In Methods
    private void Start()
    {

    }

    public virtual void Update()
    {
        CheckCollisions();
    }

    // Other Methods

    private void CheckCollisions()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(.002f, .002f), 1, layerMask); //Distance = 2 Pixels
        if (hit.collider != null)
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag("Player"))
            {
                target.GetComponent<Player>().TakeDamage();
            }
        }
    }

    // Methods for Spawning/Despawning
    public virtual void OnSpawn()
    {
        onDeathParticlePool = GameObject.FindGameObjectWithTag("KilledEnemyParticle").GetComponent<ObjectPool>();
        canTakeDamageFromFireThrower = true;
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    // Methods for Health/Damage
    public virtual void RemoveHealth(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public IEnumerator SetCanTakeDamageFromFireThrowerTrue()
    {
        yield return new WaitForSeconds(timeBetweenTakingFirethrowerDamage);
        canTakeDamageFromFireThrower = true;
    }

    public virtual void TakeDamage(int damage, bool knife)
    {
        if (knife)
        {
            StartCoroutine(BlinkWhite());
            RemoveHealth(damage);
        } else
        {
            if (canTakeDamageFromFireThrower)
            {
                StartCoroutine(BlinkWhite());
                canTakeDamageFromFireThrower = false;
                StartCoroutine(SetCanTakeDamageFromFireThrowerTrue());
                RemoveHealth(damage);
            }
        }
    }

    private IEnumerator BlinkWhite()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds(.08f);
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }

    public Vector2 CalculateDirectionTowardsPlayer()
    {
        Vector2 v = playerPosition.position - transform.position;
        return v;
    }

    public virtual void OnDeath()
    {
        GameObject particle = onDeathParticlePool.GetPooledObject();
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.GetComponent<AnimationParticle>().OnActivate(new Vector2(0, 0));
        }
        gameObject.SetActive(false);
    }

    public void SetSpriteRightDirection(Vector2 vector)
    {
        if(vector.x > 0)
        {
            spriteRenderer.flipX = false;
        } else
        {
            spriteRenderer.flipX = true;
        }
    }

    // Methods that NEED to be overridden in enemy subclasses.
    public virtual void OnDespawn()
    {
        Debug.LogError("OnDespawn() should not be called here.");
    }
}