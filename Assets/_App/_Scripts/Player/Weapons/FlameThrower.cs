using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawningParticle;
    private float timerBetweenSpawningParticle = 0;

    [SerializeField] private float timeBetweenDealingDamage;
    private float timerBetweenDealingDamage = 0;

    [SerializeField] private ObjectPool flameParticlePool;

    public void FireFlameThrower(Vector2 direction)
    {
        if(timerBetweenDealingDamage > timeBetweenDealingDamage)
        {

            

        } else
        {
            timerBetweenDealingDamage += Time.deltaTime;
        }

        if(timerBetweenSpawningParticle > timeBetweenSpawningParticle)
        {
            GameObject particle = flameParticlePool.GetPooledObject();
            if (particle != null)
            {
                particle.SetActive(true);
                particle.transform.position = transform.position;
                particle.transform.rotation = transform.rotation;
                particle.GetComponent<AnimationParticle>().OnActivate(direction);
            }
            timerBetweenSpawningParticle = 0;

        } else
        {
            timerBetweenSpawningParticle += Time.deltaTime;
        }
    }
}
