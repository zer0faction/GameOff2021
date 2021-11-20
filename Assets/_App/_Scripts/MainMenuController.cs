using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Return))
        {
            gameController.StartGame();
        }
    }
}
