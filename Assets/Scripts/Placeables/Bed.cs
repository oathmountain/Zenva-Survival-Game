using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Building, IInteractible
{
    public float wakeUpTime;
    public float startCanSleepTime;
    public float endCanSleepTime;
    public float sleepToGive;

    public string GetInteractPromt()
    {
        return CanSleep() ? "Sleep" : "Can't Sleep";
    }

    public void OnInteract()
    {
        if (CanSleep())
        {
            DayNightCycle.instance.time = wakeUpTime;
            PlayerNeeds.instance.Sleep(sleepToGive);
        }
    }

    bool CanSleep()
    {
        return DayNightCycle.instance.time >= startCanSleepTime || DayNightCycle.instance.time <= endCanSleepTime;
    }
}
