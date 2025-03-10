using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //first things first I need a timer;
        timer += Time.deltaTime;
        
            // Example: Loop through events and invoke them based on their scheduled time
            foreach (var timedEvent in eventSequence)
            {
               // In this case, you'll need to implement a way to invoke based on time
               if (timer >= timedEvent.timeToInvoke) // Example of triggering an event based on time
               {
                   timedEvent.eventToInvoke.Invoke();
               }
            }
        
    }
}
[System.Serializable]
public class TimedEvent
{
    //what is I add a minute to second setting?
    //public string eventName;  // Name of the event
    [Header("In Seconds")]
    public float timeToInvoke;  // Time at which to invoke the event
    public enum TimeType { minutes,seconds}
    public TimeType timeType;
    public UnityEvent eventToInvoke;  // The UnityEvent to invoke at the specified time
    public void Start()
    {
        if (timeType == TimeType.minutes)
        {
            timeToInvoke = timeToInvoke* 60;
        }else if (timeType == TimeType.seconds)
        {
            timeToInvoke = timeToInvoke;
        }
    }
}

