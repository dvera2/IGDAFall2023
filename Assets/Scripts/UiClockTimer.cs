using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiClockTimer : MonoBehaviour
{
    public Image Clock;

    private void Start()
    {
        GameEvents.TimeUpdated += GameEvents_TimeUpdated;
    }

    private void OnDestroy()
    {
        GameEvents.TimeUpdated -= GameEvents_TimeUpdated;
    }

    private void GameEvents_TimeUpdated(TimeArgs time)
    {
        if(Clock != null)
        {
            Clock.fillAmount = time.Time / time.Duration;
        }
    }
}
