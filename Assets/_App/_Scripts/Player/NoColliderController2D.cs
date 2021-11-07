using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoColliderController2D : MonoBehaviour
{
    private float speed = 0;

    public void Move(Vector2 velocity)
    {
        transform.Translate(velocity * Time.deltaTime * speed);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
