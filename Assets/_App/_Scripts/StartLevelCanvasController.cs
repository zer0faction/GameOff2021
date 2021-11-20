using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartLevelCanvasController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int level;
    private int lives;

    [Header("UI Stuff")]
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI continuesText;
    [SerializeField] private GameObject UIToPutInactiveOne;
    [SerializeField] private TextMeshProUGUI GoText;

    private Player player;

    private void Start()
    {
        lives = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().continues;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GoText.text = "";
        stageText.text = "Stage " + level;
        continuesText.text = "X " + lives;

        StartCoroutine(StartIE());
    }

    private IEnumerator StartIE()
    {
        yield return new WaitForSeconds(2.5f);

        UIToPutInactiveOne.SetActive(false);

        float time = 1.5f;

        player.StartGame();

        GoText.text = "GO !!!";
        yield return new WaitForSeconds(time);

        GoText.text = "";
    }
}
