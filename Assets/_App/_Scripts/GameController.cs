using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Stats. Etc.")]
    [SerializeField] private int startingContinues;
    public int continues;

    private void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        continues = startingContinues;
    }

    public void AddContinue()
    {
        continues++;
    }

    public void RemoveContinue()
    {
        continues--;
        if(continues > 0)
        {
            //Out of continues, have to restart the whole game.
        } else
        {
            //Restart the current Level.
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
