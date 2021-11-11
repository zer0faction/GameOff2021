using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerParticle : AnimationParticle
{
    private Vector2 velocity;
    [SerializeField] private float speed = 2;
    [SerializeField] private LayerMask layerMask;

    public override void OnActivate(Vector2 direction)
    {
        base.OnActivate(direction);

        //Move towards a random direction
        velocity = direction;
    }

    private void Update()
    {
        //Move the object
        transform.Translate(velocity * Time.deltaTime * speed);
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        bool hitAlready = false;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - .5f, transform.position.y), new Vector2(0, 1), 1, layerMask); //Distance = 2 Pixels
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y -.5f), new Vector2(1, 0), 1, layerMask); //Distance = 2 Pixels
        if (hit.collider != null)
        {
            hitAlready = true;
            GameObject target = hit.transform.gameObject;

            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Enemy>().TakeDamage(1, false);
            }
        }
        if(!hitAlready && hit2.collider != null)
        {
            GameObject target = hit2.transform.gameObject;

            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Enemy>().TakeDamage(1, false);
            }
        }
    }
}
