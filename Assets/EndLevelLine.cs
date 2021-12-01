using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelLine : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    private bool playedAnimation = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!playedAnimation)
            {
                Instantiate(particle,transform.position, Quaternion.identity);
                playedAnimation = true;
            }
            Player player = collision.gameObject.GetComponent<Player>();
            Debug.Log("Player hit!");
            player.OnLevelCleared();
        }
    }
}
