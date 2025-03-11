using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    //[Header("Event List")]
    ////public GameObject gObject;
    ////public delegate void MyDelegate();
    ////public MyDelegate variableFunction;
    ////this does that thing i want with teh unity event bubble.
    ////public UnityEvent methodToInvoke;
    ////this could work because then I can drag the different events to the different times.
    //public List<UnityEvent> events;
    // Start is called before the first frame update
    [Header("Event Sequence")]
    //this kinda has its own event sequence
    public List<TimedEvent> eventSequence;
    public float timer;
    public ScoreManager scoreManager;
    void Start()
    {
        //instantiate an event for when the game will end or what to show.
        //as well as what will activate when they do not successfully make something happen.
        scoreManager = this.GetComponent<ScoreManager>();
        // Convert time values for all events in the sequence
        foreach (TimedEvent timedEvent in eventSequence)
        {
            // Convert time based on the timeType (minutes to seconds)
            if (timedEvent.timeType == TimedEvent.TimeType.minutes)
            {
                timedEvent.timeToInvoke *= 60; // Convert to seconds
            }
        }
    }
    void Update()
    {
        //first things first I need a timer;
        timer += Time.deltaTime;
        if(eventSequence == null)
        {
            Debug.Log("Event sequence has no events");
            return;
        }

        foreach (TimedEvent timedEvent in eventSequence)
        {

            if (timer >= timedEvent.timeToInvoke && !timedEvent.hasPerformed)
            {
                timedEvent.eventToInvoke?.Invoke();
                timedEvent.hasPerformed = true;
            }
        }
    }
    public void EndGame()
    {
        //game end stuff
        print("game End");
        Application.Quit();
    }
}
[System.Serializable]
public class TimedEvent
{
    //what is I add a minute to second setting?
    //public string eventName;  // Name of the event
    [Header("In Seconds")]
    public float timeToInvoke;  // Time at which to invoke the event
    //currently it is just in seconds and the minute switch doesnt work.
    public enum TimeType { minutes,seconds}
    public TimeType timeType;
    public UnityEvent eventToInvoke;  // The UnityEvent to invoke at the specified time
    public bool hasPerformed = false;
}

