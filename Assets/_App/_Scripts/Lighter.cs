using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] private float timeBeforeStart;
    [SerializeField] private float timeBetweenFireSpawns;
    [SerializeField] private float timeFireIsActive;
    [SerializeField] private ObjectPool spawningParticleObjectPool;
    [SerializeField] private GameObject fireParticle;

    private void Start()
    {
        StartCoroutine(SpawnFireParticle(true));
    }

    private IEnumerator SpawnFireParticle(bool firstTime)
    {
        if (firstTime)
        {
            yield return new WaitForSeconds(timeBeforeStart);
        }

        fireParticle.SetActive(true);
        yield return new WaitForSeconds(timeFireIsActive);
        fireParticle.SetActive(false);
        yield return new WaitForSeconds(timeBetweenFireSpawns - .8f);

        GameObject particle = spawningParticleObjectPool.GetPooledObject();
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = new Vector2(transform.position.x - .2f, transform.position.y + 1);
            particle.transform.rotation = transform.rotation;
            particle.GetComponent<AnimationParticle>().OnActivate(new Vector2(0, 0));
        }

        yield return new WaitForSeconds(.8f);
        StartCoroutine(SpawnFireParticle(false));
    }
}
