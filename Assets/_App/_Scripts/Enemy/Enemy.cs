using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Stuff.")]
    [SerializeField] private int maxHealth;
    private int currentHealth;

    // Other gameobjects
    [HideInInspector] public Transform playerPosition;

    // Build-In Methods
    private void Start()
    {
        playerPosition = GameObject.Find("Player").transform;

        OnSpawn(); // Weghalen deze shit!!!
    }

    // Methods for Spawning/Despawning
    public virtual void OnSpawn()
    {
        currentHealth = maxHealth;
    }

    // Methods for Health/Damage
    public void RemoveHealth(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        RemoveHealth(damage);
    }

    public Vector2 CalculateDirectionTowardsPlayer()
    {

        Vector2 v = playerPosition.position - transform.position;
        return v;
    }

    // Methods that NEED to be overridden in enemy subclasses.
    public virtual void OnDeath()
    {
        Debug.LogError("OnDeath() should not be called here.");
    }

    public virtual void OnDespawn()
    {
        Debug.LogError("OnDespawn() should not be called here.");
    }
}