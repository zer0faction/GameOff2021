using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("References to other GameObjects, etc.")]
    [SerializeField] private Slider bossHealthBar;
    [SerializeField] private GameObject bossEnemy;
    [SerializeField] private Transform enemySpawnPosition;

    public void SetHealthbarStats(int health)
    {
        int i = health;
        if(health < 0)
        {
            i = 0;

            Debug.Log("Go To Next Level!!!!!!!!!!!");
            Debug.Log("Go To Next Level!!!!!!!!!!!");
            
        }
        bossHealthBar.value = i;
    }

    private void Start()
    {
        StartCoroutine(SpawnBossFight());
    }

    private IEnumerator SpawnBossFight()
    {
        yield return new WaitForSeconds(5f);
        GameObject g = Instantiate(bossEnemy, enemySpawnPosition.position, Quaternion.identity);
        Enemy e = g.GetComponent<Enemy>();
        g.GetComponent<FrogBoss>().SetController(this);
        bossHealthBar.maxValue = e.maxHealth;
        SetHealthbarStats(e.maxHealth);
        e.OnSpawn();
    }

    public void NextLevel()
    {
        StartCoroutine(GoToNextLevel());
    }

    private IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LevelCleared();
    }
}
