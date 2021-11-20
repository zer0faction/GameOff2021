using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite healthOne;
    [SerializeField] private Sprite healthTwo;
    [SerializeField] private Sprite healthThree;
    [SerializeField] private Sprite healthZero;

    [Header("Other GameObjects, Components, etc.")]
    [SerializeField] private Image healthImage;
    [SerializeField] private Animator animator;

    private void Start()
    {
        SetHealth(3,false);
    }

    public void SetHealth(int health, bool loseHealth)
    {
        switch (health)
        {
            case 1:
                healthImage.sprite = healthOne;
                break;
            case 2:
                healthImage.sprite = healthTwo;
                break;
            case 3:
                healthImage.sprite = healthThree;
                break;
            case 0:

                healthImage.sprite = healthZero;
                break;
            default:
                return;
        }

        if (loseHealth)
        {
            //EarthQuake effect
            animator.Play("ShakeHealthBar",-1,0f);
        }

    }
}
