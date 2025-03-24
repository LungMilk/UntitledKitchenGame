using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //score manager just has to store and be referenced by anything that wants to increase the score
    //delegate function??? but lets keep it strict object reference.
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI floatingScoreText;
    public EventManager eventManager;
    public float score;
    public float quota;
    public void AddScore(float points)
    {
        StartCoroutine(ShowFloatingScore(points));
    }

    private void Start()
    {
        score = 0;
        eventManager = this.GetComponent<EventManager>();
        scoreText.text = $"Score: {score}/{quota}";
        floatingScoreText.gameObject.SetActive(false);
    }
    private void Update()
    {
        //scoreText.text = $"Score: {score}/{quota}";
        //if (score >quota ) { eventManager.EndGame(); }
    }
    private IEnumerator ShowFloatingScore(float points)
    {
        floatingScoreText.text = $"+{points}";
        floatingScoreText.gameObject.SetActive(true);

       
        yield return new WaitForSeconds(4f);

       
        floatingScoreText.gameObject.SetActive(false);
        score += points;
        scoreText.text = $"{score}";

       
        if (score > quota)
        {
            eventManager.EndGame();
        }
    }
}
