using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private Type currentType;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private bool ignoreMaxEnemyCount;
    public bool isDestructible;

    [Header("Stats in case of being destructible")]
    [SerializeField] private int health;

    [Header("Other GameObjects, Scripts, Components, Etc.")]
    [SerializeField] private ObjectPool enemyObjectPool;
    [SerializeField] private ObjectPool spawnParticlesPool;

    [Header("List of transforms if type is multiple.")]
    [SerializeField] private Transform[] transforms;

    private float currentTime;
    private bool isInPlayerRange = false;
    private bool alreadySpawnedEnemies;

    enum Type
    {
        OneAndDone,
        Cooldown,
        Multiple
    }

    // Build in Methods.
    private void Update()
    {
        if (isInPlayerRange)
        {
            currentTime += Time.deltaTime;
            if (currentTime > timeBetweenSpawns)
            {
                currentTime = 0;
                if (ignoreMaxEnemyCount)
                {
                    StartCoroutine(SpawnEnemy(transform));
                    return;
                }
            }
        }
    }

    public void SetActive()
    {
        currentTime = timeBetweenSpawns;
        
        if(currentType == Type.Cooldown)
        {
            isInPlayerRange = true;
        }

        if(currentType == Type.Multiple)
        {
            SpawnMultipleEnemiesOnce();
        }

        if (currentType == Type.OneAndDone)
        {
            SpawnSingleEnemyOnce();
        }
    }

    public void SetInactive()
    {
        isInPlayerRange = false;
    }

    private void SpawnMultipleEnemiesOnce()
    {
        Debug.Log("Spawn multiple enemies once!");
        if (!alreadySpawnedEnemies)
        {
            alreadySpawnedEnemies = true;
            foreach(Transform t in transforms)
            {
                StartCoroutine(SpawnEnemy(t));
            }
        }
    }

    private void SpawnSingleEnemyOnce()
    {
        if (!alreadySpawnedEnemies)
        {
            alreadySpawnedEnemies = true;
            StartCoroutine(SpawnEnemy(transform));
        }
    }

    private IEnumerator SpawnEnemy(Transform t)
    {
        GameObject particle = spawnParticlesPool.GetPooledObject();
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = t.position;
            particle.transform.rotation = t.rotation;
            particle.GetComponent<AnimationParticle>().OnActivate(new Vector2(0, 0));
        }

        yield return new WaitForSeconds(1f);

        GameObject enemy = enemyObjectPool.GetPooledObject();
        if (enemy != null)
        {
            enemy.SetActive(true);
            enemy.transform.position = t.position;
            enemy.transform.rotation = t.rotation;
        }
        enemy.GetComponent<Enemy>().OnSpawn();
    }
}
