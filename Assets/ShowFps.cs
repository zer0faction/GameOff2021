using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowFps : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private void Update()
    {
        fpsText.text = 1.0f / Time.deltaTime + "";
    }
}
