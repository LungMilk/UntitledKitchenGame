using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerUI : MonoBehaviour
{
    public Image fillUI;

    public float Duration;
    public float remainingDuration;

    // Start is called before the first frame update
    public void ChangeVisiblity()
    {
        fillUI.enabled = !fillUI.enabled;
    } 
    [ContextMenu("Begin")]
    public void Begin()
    {
        remainingDuration = Duration;
        StartCoroutine(UpdateTimer());
    }

    [ContextMenu("REesetUI")]
    public void ResetUI()
    {
        remainingDuration = Duration;
    }
    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            fillUI.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
    }
}

