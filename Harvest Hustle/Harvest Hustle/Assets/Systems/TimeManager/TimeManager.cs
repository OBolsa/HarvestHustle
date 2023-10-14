using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    private int currentHours; // The current hours of the day
    private int currentMinutes; // The current minutes of the day
    private int totalTimeInMinutes; // The accumulated minutes in the game
    private int totalHours; // Total accumulated hours in the game
    public bool IsRaining { get; set; }

    // Event for notifying objects about time pass
    public event Action<int, int> OnTimePass;
    public event Action<int> OnStrike;
    public event Action<int> OnPassDay;
    public Vector2Int LastTime { get; private set; }
    public Vector2Int TimePassed { get; private set; }

    // Singleton instance of TimeManager
    private static TimeManager instance;
    public static TimeManager Instance => instance;

    private void Awake()
    {
        // Ensure there's only one instance of TimeManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            DoSingleStrike();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            DoDoubleStrike();
        }
    }

    #region TimePassage
    public void DoStrike(StrikeType strike)
    {
        switch (strike)
        {
            default:
            case StrikeType.None:
                return;
            case StrikeType.Single:
                DoSingleStrike();
                return;
            case StrikeType.Double:
                DoDoubleStrike();
                return;
        }
    }
    public void DoSingleStrike() => PassTime(Strike.Single);
    public void DoDoubleStrike() => PassTime(Strike.Double);
    public void PassTime(Vector2Int timeToPass)
    {
        int hoursToAdd = timeToPass.x;
        int minutesToAdd = timeToPass.y;

        int currentDay = GetCurrentDay();
        int totalMinutes = hoursToAdd * 60 + minutesToAdd;
        int remainingMinutes = totalMinutes;

        // Use a while loop to tick time in 30-minute intervals
        while (remainingMinutes >= 30)
        {
            TickTime(30);
            remainingMinutes -= 30;
        }

        // Handle remaining minutes
        if (remainingMinutes > 0)
        {
            TickTime(remainingMinutes);
        }

        Debug.Log($"Hours to Add: {hoursToAdd}");
        OnStrike?.Invoke(hoursToAdd == 1 ? 1 : 2);

        // Notify day change if applicable
        if (GetCurrentDay() > currentDay)
        {
            OnPassDay?.Invoke(GetCurrentDay());
        }
    }
    public void PassTime(int hoursToAdd, int minutesToAdd)
    {
        int currentDay = GetCurrentDay();
        int totalMinutes = hoursToAdd * 60 + minutesToAdd;
        int remainingMinutes = totalMinutes;

        // Use a while loop to tick time in 30-minute intervals
        while (remainingMinutes >= 30)
        {
            TickTime(30);
            remainingMinutes -= 30;
        }

        // Handle remaining minutes
        if (remainingMinutes > 0)
        {
            TickTime(remainingMinutes);
        }

        // Notify day change if applicable
        if (GetCurrentDay() > currentDay)
        {
            OnPassDay?.Invoke(GetCurrentDay());
        }
    }
    private void TickTime(int minutes)
    {
        LastTime = GetCurrentTime();

        // Store the previous time
        int prevHours = currentHours;
        int prevMinutes = currentMinutes;

        // Update current time
        currentMinutes += minutes;

        // Adjust hours if minutes exceed 60
        if (currentMinutes >= 60)
        {
            int extraHours = currentMinutes / 60;
            currentMinutes %= 60;
            totalHours += extraHours;
        }

        // Update the current hour & the accumulated time in minutes
        currentHours = totalHours - (24 * (totalHours / 24));
        totalTimeInMinutes = totalHours * 60 + currentMinutes;

        // Notify time pass event
        int hoursPassed = currentHours - prevHours;
        int minutesPassed = currentMinutes - prevMinutes;
        if (minutesPassed < 0)
        {
            minutesPassed += 60;
            hoursPassed--;
        }

        // Invoke the time pass event with the time passed
        NotifyTimePass(hoursPassed, minutesPassed);
    }
    private void NotifyTimePass(int hours, int minutes)
    {
        // Notify subscribers of the OnTimePass event
        OnTimePass?.Invoke(hours, minutes);
    }
    #endregion

    #region GetMethods
    public int GetCurrentHours() => currentHours;
    public int GetCurrentMinutes() => currentMinutes;
    public int GetTotalCurrentTimeInMinutes() => currentHours * 60 + currentMinutes;
    public int GetTotalHours() => totalHours;
    public int GetTotalTimeInMinutes() => totalTimeInMinutes;

    public int GetCurrentDay()
    {
        // Assuming each day is 24 hours long, calculate the current day based on the total accumulated hours
        int day = totalHours / 24 + 1; // Add 1 since day count starts from 1
        return day;
    }

    public Vector2Int GetTotalTime() => new Vector2Int(GetTotalHours(), GetCurrentMinutes());
    public Vector2Int GetCurrentTime() => new Vector2Int(GetCurrentHours(), GetCurrentMinutes());
    public bool CanDoStrike() => GetCurrentHours() >= 6 && GetCurrentHours() < 18;
    #endregion

    private void OnGUI()
    {
        GUILayout.Label(string.Format("Day {0}, {1:00}:{2:00}", GetCurrentDay(), GetCurrentHours(), GetCurrentMinutes()));
    }
}

public enum StrikeType
{
    None,
    Single,
    Double
}