using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParticle : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public virtual void OnActivate(Vector2 direction)
    {
        animator.Play("Particle");
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
