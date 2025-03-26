using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerUI : MonoBehaviour
{
    public Image fillUI;

    public int Duration;
    public int remainingDuration;

    // Start is called before the first frame update
    void Start()
    {
        Begin(Duration);
    }

    private void Begin(int second)
    {
        remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingDuration >= 0)
        {
            fillUI.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        onEnd();
    }

    private void onEnd()
    {
        fillUI.color = Color.red;
    }
}
