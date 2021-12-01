using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private Transform position;
    [SerializeField] private GameObject particle;

    private bool playedAnimation = false;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (gameController.GetStartfromCheckpoint())
        {
            //Set stuff like dont make animation go off, etc.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!playedAnimation)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
                playedAnimation = true;
            }
            gameController.SetStartfromCheckpointToTrue();
            gameController.SetCheckpointTransform(position);
        }
    }
}
