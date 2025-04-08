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
    //why was this static??
    public float score;
    public float quota;
    [System.Serializable]
    public struct pointStatus
    {
        //we want to submit the data of the orders change in points and the overall score
        public string pointValue;
        public string currentScore;
    }
    [ContextMenu("Add points")]
    public void ScoreHistCheck()
    {
        AddScore(Random.Range(-100,100));
    }
    public void AddScore(float points)
    {
        var data = new pointStatus()
        {
            pointValue = points.ToString(),
            currentScore = score.ToString()
        };
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
        if (score > quota)
        {
            eventManager.EndGame();
        }
    }
    private IEnumerator ShowFloatingScore(float points)
    {
        floatingScoreText.text = $"+{points}";
        floatingScoreText.gameObject.SetActive(true);

       
        yield return new WaitForSeconds(4f);

       
        floatingScoreText.gameObject.SetActive(false);
        score += points;
        scoreText.text = $"{score}";
    }
}
