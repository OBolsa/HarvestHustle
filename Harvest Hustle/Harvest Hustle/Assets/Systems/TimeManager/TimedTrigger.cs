using System;
using UnityEngine;

public class TimedTrigger : IDisposable
{
    private Vector2Int waitTime;
    private readonly Action executeWhenTimeEnds;
    private int startTime;
    private int timePassed;

    public TimedTrigger(int waitTime, Action action)
    {
        startTime = TimeManagerStrike.Instance.CurrentStrikeCountTotal;
        timePassed = startTime + waitTime;
        executeWhenTimeEnds = action;
        TimeManagerStrike.Instance.StrikePassed += CheckTime;
    }
    
    //public TimedTrigger(Vector2Int waitTime, Action action)
    //{
    //    this.waitTime = TimeManager.Instance.GetTotalTime() + waitTime;
    //    executeWhenTimeEnds = action;
    //    //TimeManager.Instance.OnTimePass += CheckTime;
    //}

    public void CheckTime()
    {
        if (TimeManagerStrike.Instance.TimePassed(startTime) < timePassed)
        {
            return;
        }

        executeWhenTimeEnds.Invoke();
        Dispose();
    }
    public void CheckTime(int hours, int minutes)
    {
        float totalTimeInMinutes = TimeManager.Instance.GetTotalTimeInMinutes();
        float thisTotalTimeInMinutes = waitTime.x * 60 + waitTime.y;

        if(totalTimeInMinutes < thisTotalTimeInMinutes)
        {
            return;
        }

        executeWhenTimeEnds();
        Dispose();
    }

    public void Dispose()
    {
        TimeManagerStrike.Instance.StrikePassed += CheckTime;
        //TimeManager.Instance.OnTimePass -= CheckTime;
    }
}