using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Debug.Log("Player hit!");
            player.OnLevelCleared();
        }
    }
}
