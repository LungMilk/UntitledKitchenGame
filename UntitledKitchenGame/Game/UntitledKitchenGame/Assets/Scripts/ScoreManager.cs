using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //score manager just has to store and be referenced by anything that wants to increase the score
    //delegate function??? but lets keep it strict object reference.
    public TextMeshProUGUI scoreText;
    public EventManager eventManager;
    public static float score;
    public float quota;

    private void Start()
    {
        score = 0;
        eventManager = this.GetComponent<EventManager>();
        scoreText.text = $"Score: {score}/{quota}";
    }
    private void Update()
    {
        scoreText.text = $"Score: {score}/{quota}";
        if (score >quota ) { eventManager.EndGame(); }
    }
}
