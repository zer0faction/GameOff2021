using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float gravity;
    
    private Vector2 currentVelocity;
    private Controller2D controller;

    // Physics
    private Vector3 velocity;
    private Vector3 oldVelocity;
    private Vector3 deltaPosition;

    private void Start()
    {
        SetInitialVelocity(new Vector2(0, 0));
        controller = GetComponent<Controller2D>();
    }

    private void Update()
    {
        oldVelocity = velocity;
        velocity.y += gravity * Time.fixedDeltaTime;

        deltaPosition = (oldVelocity + velocity) * 0.5f * Time.fixedDeltaTime;

        controller.Move(deltaPosition);
    }

    public void SetInitialVelocity(Vector2 velocity)
    {
        currentVelocity = velocity;
    }
}
