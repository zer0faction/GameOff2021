using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("EnemySpawner"))
        {
            collision.transform.GetComponent<EnemySpawner>().SetActive();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("EnemySpawner"))
        {
            collision.transform.GetComponent<EnemySpawner>().SetInactive();
        }
    }
}
