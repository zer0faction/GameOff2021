using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Enemy
{
    [Header("Other GameObjects, etc.")]
    [SerializeField] private NoColliderController2D controller;
    private Vector2 currentVector2;


    public override void OnSpawn()
    {
        base.OnSpawn();

        controller.SetSpeed(2f);

        // Set target to fly toward
        Vector2 v2 = CalculateDirectionTowardsPlayer();
        currentVector2 = v2.normalized * 2.75f;
    }

    public override void Update()
    {
        CheckCollisions();
        controller.Move(currentVector2);
    }

    private void CheckCollisions()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(.002f, .002f), .4f, layerMask); //Distance = 2 Pixels
        if (hit.collider != null)
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag("Player"))
            {
                target.GetComponent<Player>().TakeDamage();
            }
        }
    }

    public override void TakeDamage(int damage, bool knife)
    {
        //Do nothing, cant be hit.
    }
}
