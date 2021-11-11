using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CustomPlayerAnimator : MonoBehaviour
{
    //. Animation states
    const string SIDEWAYS_PLAYER_IDLE = "AimingSidewaysIdle";
    const string SIDEWAYS_PLAYER_WALKING = "AimingSidewaysWalk";
    const string SIDEWAYS_PLAYER_JUMPING = "AimingSidewaysJump";

    const string DOWN_PLAYER_IDLE = "AimingDownIdle";
    const string DOWN_PLAYER_WALKING = "AimingDownWalk";
    const string DOWN_PLAYER_JUMPING = "AimingDownJump";

    const string UP_PLAYER_IDLE = "AimingUpIdle";
    const string UP_PLAYER_WALKING = "AimingUpWalk";
    const string UP_PLAYER_JUMPING = "AimingUpJump";

    //. References to other gameobjects
    Animator animator;

    string currentAnimation = "";
    string currentWeaponDirection = "";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string newAnimation, string weaponDirection)
    {
        if (currentAnimation == newAnimation && currentWeaponDirection == weaponDirection) return;
        currentAnimation = newAnimation;
        currentWeaponDirection = weaponDirection;

        switch (newAnimation)
        {
            case "Idle":
                SwitchAnimationToIdle(weaponDirection);
                break;
            case "Walk":
                SwitchAnimationToWalk(weaponDirection);
                break;
            case "Jump":
                SwitchAnimationToJump(weaponDirection);
                break;
            case "TakeDamage":
                animator.Play("TakeDamage");
                break;
        }
    }

    public void SwitchAnimationToIdle(string weaponDirection)
    {
        switch (weaponDirection)
        {
            case "Sideways":
                animator.Play(SIDEWAYS_PLAYER_IDLE);
                break;
            case "Down":
                animator.Play(DOWN_PLAYER_IDLE);
                break;
            case "Up":
                animator.Play(UP_PLAYER_IDLE);
                break;
        }
    }

    public void SwitchAnimationToWalk (string weaponDirection)
    {
        switch (weaponDirection)
        {
            case "Sideways":
                animator.Play(SIDEWAYS_PLAYER_WALKING);
                break;
            case "Down":
                animator.Play(DOWN_PLAYER_WALKING);
                break;
            case "Up":
                animator.Play(UP_PLAYER_WALKING);
                break;
        }
    }

    public void SwitchAnimationToJump(string weaponDirection)
    {
        switch (weaponDirection)
        {
            case "Sideways":
                animator.Play(SIDEWAYS_PLAYER_JUMPING);
                break;
            case "Down":
                animator.Play(DOWN_PLAYER_JUMPING);
                break;
            case "Up":
                animator.Play(UP_PLAYER_JUMPING);
                break;
        }
    }
}
