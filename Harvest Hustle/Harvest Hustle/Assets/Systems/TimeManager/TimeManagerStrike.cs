using UnityEngine;
using System;

public class TimeManagerStrike : MonoBehaviour
{
    // Day Manager
    public DayPeriod DayPeriod
    {
        get
        {
            if (CurrentStrikeCount == 8)
            {
                return DayPeriod.Evening;
            }
            else if (CurrentStrikeCount >= 4)
            {
                return DayPeriod.Afternoon;
            }
            else
            {
                return DayPeriod.Morning;
            }
        }
    }
    private int _currentStrikeCount;
    /// <summary>
    /// Apenas a contagem atual de strikes
    /// </summary>
    public int CurrentStrikeCount
    {
        get => Mathf.Clamp(_currentStrikeCount, 0, 8);
        private set => _currentStrikeCount = value;
    }
    public DayPeriodInfo CurrentDayPeriodInfo
    {
        get => GameplayManager.instance.globalConfigs.GetDayPeriodInfo(DayPeriod);
    }

    public void DoStrike(StrikeType strike)
    {
        if(DayPeriod == DayPeriod.Evening)
        {
            return;
        }

        switch (strike)
        {
            case StrikeType.Single:
                IncreaseStrike();
                break;
            case StrikeType.Double:
                IncreaseStrike();
                IncreaseStrike();
                break;
            // Add more cases as needed.
            default:
                break;
        }
    }
    private void IncreaseStrike()
    {
        CurrentStrikeCount++;
        StrikePassed?.Invoke();
    }

    // Day Tracker
    public int CurrentDay { get; private set; }
    public void NextDay()
    {
        int strikeCount = 8 - CurrentStrikeCount;

        while (strikeCount > 0)
        {
            StrikePassed?.Invoke();
            strikeCount--;
        }

        CurrentStrikeCount = 0;

        CurrentDay++;
        ScreenTransition.Instance.DoTransition(InvokeDayPassed);
    }
    private void InvokeDayPassed() => DayPassed?.Invoke();

    // Time Manager
    public int CurrentStrikeCountTotal
    {
        get
        {
            return CurrentDay * 9 + CurrentStrikeCount;
        }
    }
    public int TimePassed(int since) => CurrentStrikeCountTotal - since;

    // Events
    public static TimeManagerStrike Instance;
    public event Action StrikePassed;
    public event Action PeriodPassed;
    public event Action DayPassed;

    // 

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            DoStrike(StrikeType.Single);
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            NextDay();
        }
    }
    private void OnGUI()
    {
        GUILayout.Label($"Strike Count: {CurrentStrikeCount}");
        GUILayout.Label($"Day Count: {CurrentDay}");
    }
}

public enum DayPeriod
{
    Morning,
    Afternoon,
    Evening
}