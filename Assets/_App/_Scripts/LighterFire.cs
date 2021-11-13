using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterFire : AnimationParticle
{
    [SerializeField] private LayerMask layerMask;

    private void Update()
    {
        CheckCollisions();
    }

    public override void OnActivate(Vector2 direction)
    {
        StartCoroutine(DespawnAfterTime());
    }

    private IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(1.6f);
        gameObject.SetActive(false);
    }

    private void CheckCollisions()
    {
        bool hitAlready = false;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - .3f, transform.position.y), new Vector2(0, 1), .6f, layerMask); //Distance = 2 Pixels
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - .3f), new Vector2(1, 0), .6f, layerMask); //Distance = 2 Pixels
        if (hit.collider != null)
        {
            hitAlready = true;
            GameObject target = hit.transform.gameObject;

            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Enemy>().TakeDamage(1, false);
            }

            if (target.CompareTag("Player"))
            {
                target.GetComponent<Player>().TakeDamage();
            }
        }
        if (!hitAlready && hit2.collider != null)
        {
            GameObject target = hit2.transform.gameObject;

            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<Enemy>().TakeDamage(1, false);
            }

            if (target.CompareTag("Player"))
            {
                target.GetComponent<Player>().TakeDamage();
            }
        }
    }
}
