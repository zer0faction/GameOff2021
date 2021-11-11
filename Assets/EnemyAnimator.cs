using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string currentAnimation = "";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimation(string animation)
    {
        if(animation == currentAnimation)
        {
            return;
        }

        currentAnimation = animation;
        animator.Play(animation);
    }
}
