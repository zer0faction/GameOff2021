using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerParticle : AnimationParticle
{
    private Vector2 velocity;
    [SerializeField] private float speed = 2;

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
    }
}
