using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Stats. Etc.")]
    [SerializeField] private int startingContinues;
    [HideInInspector] public int continues;
    private int currentLevel;

    [Header("Other GameObjects.")]
    [SerializeField] private Animator makeScreenBlackAnimator;

    [Header("Checkpoints stuff")]
    [SerializeField] private Transform spawnPosFromCheckpoint;
    private bool startFromCheckpoint;
    private float xPosCheckpointSpawn;
    private float yPosCheckpointSpawn;

    public void SetCheckpointTransform(Transform t)
    {
        xPosCheckpointSpawn = t.position.x;
        yPosCheckpointSpawn = t.position.y;
    }

    public void SetStartfromCheckpointToTrue()
    {
        startFromCheckpoint = true;
    }

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
        StartCoroutine(StartGameCor());
    }

    private IEnumerator StartGameCor()
    {
        continues = startingContinues;
        currentLevel = 1;

        startFromCheckpoint = false;

        makeScreenBlackAnimator.Play("BlackScreenThingy");
        yield return new WaitForSeconds(.98f);

        StartCoroutine(LoadCurrentLevel());
    }

    public void LevelCleared()
    {
        StartCoroutine(GoToNextLevel());
    }

    private IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(1.75f);
        makeScreenBlackAnimator.Play("BlackScreenThingy");
        yield return new WaitForSeconds(.98f);

        startFromCheckpoint = false;
        currentLevel++;

        //if(level > hoeveel levels er zijn){}

        StartCoroutine(LoadCurrentLevel());
    }

    public void AddContinue()
    {
        continues++;
    }

    public void RemoveContinue()
    {
        continues--;
        if(continues < 0)
        {
            //Out of continues, have to restart the whole game.
        } else
        {
            //Restart the current Level.
            StartCoroutine(OnDeath());
        }
    }

    public bool GetStartfromCheckpoint()
    {
        return startFromCheckpoint;
    }

    private IEnumerator OnDeath()
    {
        yield return new WaitForSeconds(.98f);
        makeScreenBlackAnimator.Play("BlackScreenThingy");
        yield return new WaitForSeconds(.98f);

        StartCoroutine(LoadCurrentLevel());
    }

    private IEnumerator LoadCurrentLevel()
    {
        SceneManager.LoadSceneAsync(currentLevel);
        while (SceneManager.GetActiveScene().buildIndex != currentLevel)
        {
            yield return null;
        }

        yield return new WaitForSeconds(.1f);

        //Set the player the right position
        if (startFromCheckpoint)
        {
            Debug.Log("StartFromCheckpoint is true");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(xPosCheckpointSpawn, yPosCheckpointSpawn, 0);
        }

        makeScreenBlackAnimator.Play("BlackScreenThingyFadeIn");
        yield return new WaitForSeconds(.98f);
        makeScreenBlackAnimator.Play("NothingHappens");
    }
}
