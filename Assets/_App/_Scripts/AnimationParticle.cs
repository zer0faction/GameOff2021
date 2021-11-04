using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParticle : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void OnActivate()
    {
        animator.Play("Particle");
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
