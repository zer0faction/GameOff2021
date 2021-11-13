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

    private void Start()
    {
        lives = 1; //GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().continues;

        Debug.Log("Start");

        GoText.text = "";
        stageText.text = "Stage " + level;
        continuesText.text = "X " + lives;

        StartCoroutine(StartIE());
    }

    private IEnumerator StartIE()
    {
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Wtf");
        UIToPutInactiveOne.SetActive(false);

        float time = 1.5f;

        GoText.text = "GO !!!";
        yield return new WaitForSeconds(time);

        GoText.text = "";
    }
}
