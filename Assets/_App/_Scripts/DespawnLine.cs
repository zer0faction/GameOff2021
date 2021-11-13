using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.transform.gameObject.GetComponent<Enemy>();
            if (enemy.isBoss)
            {
                //Do nothing
            }
            else
            {
                enemy.OnDeath();
            }
        }
        if (collision.CompareTag("Knife"))
        {
            collision.transform.gameObject.SetActive(true);
        }
    }
}
