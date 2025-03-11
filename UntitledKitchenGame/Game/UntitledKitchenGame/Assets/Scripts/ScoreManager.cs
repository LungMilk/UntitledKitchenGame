using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //score manager just has to store and be referenced by anything that wants to increase the score
    //delegate function??? but lets keep it strict object reference.
    public TextMeshProUGUI scoreText;
    public float score;
    public float quota;

    private void Start()
    {
        score = 0;
        scoreText.text = "Score: 0";
    }
    private void Update()
    {
        scoreText.text = "Score: " + score;
    }
}
