using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Vector2 velocity;
    [SerializeField] private float speed = 4;
    [SerializeField] private int damage = 1;

    private ObjectPool objectPool;

    [SerializeField] private LayerMask layerMask;

    private void Start()
    {
        objectPool = GameObject.FindGameObjectWithTag("KnifeDestroyedPool").GetComponent<ObjectPool>();
    }

    public void OnActivate(Vector2 direction)
    {
        bool x = true;
        float y = 0;
        if(direction.x == 1)
        {
            x = false;
        }
        if (direction.y == -1)
        {
            y = 90;
            direction = Rotate(direction, -90);
        }
        if (direction.y == 1)
        {
            y = -90;
            direction = Rotate(direction, 90);
        }

        velocity = direction;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = x;
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            transform.eulerAngles.z + y
        );
    }

    private void Update()
    {
        //Move the object
        transform.Translate(velocity * Time.deltaTime * speed);
        CheckCollisions();
    }

    private void OnTargetHit()
    {
        GameObject particle = objectPool.GetPooledObject();
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.GetComponent<AnimationParticle>().OnActivate(new Vector2(0, 0));
        }
        gameObject.SetActive(false);
    }

    private void CheckCollisions()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0.02f, 0.02f), 1, layerMask); //Distance = 2 Pixels
        if (hit.collider != null)
        {
            GameObject target = hit.transform.gameObject;

            if (target.CompareTag("Solid"))
            {
                // On Wall hit
                OnTargetHit();
            }
            else if (target.CompareTag("Enemy"))
            {
                Debug.Log("Hit an enemy!");
                OnTargetHit();
                target.GetComponent<Enemy>().TakeDamage(damage, true);
            } else if (target.CompareTag("EnemySpawner"))
            {
                EnemySpawner e = target.GetComponent<EnemySpawner>();
                if (e.isDestructible)
                {
                    //
                }
            }
        }
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
