using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy : Enemy
{
    [SerializeField] private Animator realAnimator;
    [SerializeField] private float timeBetweenAttacks;
    private ObjectPool particlePool;

    private void Start()
    {
        particlePool = GameObject.FindGameObjectWithTag("PlantAttackObjectPool").GetComponent<ObjectPool>();
    }

    public override void Update()
    {
        if(playerPosition.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        } else
        {
            spriteRenderer.flipX = false;
        }

        base.Update();
    }

    private void ChooseAttack()
    {
        int rand = Random.Range(2, 99);
        if(rand > 18)
        {
            StartCoroutine(FireSingleProjectile(timeBetweenAttacks - 1f));
        } else
        {
            StartCoroutine(FireTwoProjectiles(timeBetweenAttacks));
        }
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        ChooseAttack();
    }

    private IEnumerator FireSingleProjectile(float randomTimeBetweenNextAttack)
    {
        yield return new WaitForSeconds(randomTimeBetweenNextAttack);

        realAnimator.Play("PlantBeforeAttack");
        yield return new WaitForSeconds(.75f);
        realAnimator.Play("PlantAttack");
        yield return new WaitForSeconds(.25f);
        SpawnParticleFireToPlayer();
        yield return new WaitForSeconds(.5f);

        realAnimator.Play("PlantIdle");

        ChooseAttack();
    }

    private IEnumerator FireTwoProjectiles(float randomTimeBetweenNextAttack)
    {
        yield return new WaitForSeconds(randomTimeBetweenNextAttack);

        realAnimator.Play("PlantBeforeAttack");
        yield return new WaitForSeconds(.75f);
        realAnimator.Play("PlantAttack");

        yield return new WaitForSeconds(.25f);
        SpawnParticleFireToPlayer();
        yield return new WaitForSeconds(.5f);

        yield return new WaitForSeconds(.25f);
        SpawnParticleFireToPlayer();
        yield return new WaitForSeconds(.5f);

        realAnimator.Play("PlantIdle");

        ChooseAttack();
    }

    private void SpawnParticleFireToPlayer()
    {
        GameObject particle = particlePool.GetPooledObject();
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.GetComponent<Projectile>().OnSpawn();
        }
    }
}
