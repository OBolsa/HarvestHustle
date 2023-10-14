using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TimePeriod : MonoBehaviour
{
    public Image fill;
    public Image icon;
    public TMP_Text dayDisplay;
    public List<TimeState> states = new List<TimeState>();

    [System.Serializable]
    public class TimeState
    {
        public string name;
        public Sprite icon;
        public Color32 color;
        public Vector2Int hourMin;
        public Vector2Int hourMax;
        private int minutesMin { get => hourMin.x * 60 + hourMin.y; }
        private int minutesMax { get => hourMax.x * 60 + hourMax.y; }
        public List<TimeFill> fills = new List<TimeFill>();

        public bool IsBetween(int currentHour)
        {
            int currentInMinutes = currentHour;
            return currentInMinutes >= minutesMin && currentInMinutes < minutesMax;
        }
    }

    [System.Serializable]
    public class TimeFill
    {
        public Vector2Int time;
        [Range(0, 1)] public float fill;
    }

    private void OnEnable()
    {
        TimeManager.Instance.OnStrike += StartFill;
        TimeManager.Instance.OnPassDay += UpdateDay;
    }
    private void OnDisable()
    {
        TimeManager.Instance.OnStrike -= StartFill;
        TimeManager.Instance.OnPassDay -= UpdateDay;
    }

    private void StartFill()
    {
        int currentTimeInMinutes = TimeManager.Instance.GetTotalCurrentTimeInMinutes();

        TimeState newState = states.Find(s => s.IsBetween(currentTimeInMinutes));
        fill.color = newState.color;
        icon.sprite = newState.icon;
        TimeFill newFill = newState.fills.Find(f => f.time == TimeManager.Instance.GetCurrentTime());

        newFill ??= newState.fills[0];

        fill.fillAmount = newFill.fill;
    }
    private void StartFill(int strikes)
    {
        int currentTimeInMinutes = TimeManager.Instance.GetTotalCurrentTimeInMinutes();

        TimeState newState = states.Find(s => s.IsBetween(currentTimeInMinutes));
        fill.color = newState.color;
        icon.sprite = newState.icon;
        TimeFill newFill = newState.fills.Find(f => f.time == TimeManager.Instance.GetCurrentTime());

        newFill ??= newState.fills[0];

        fill.fillAmount = newFill.fill;
    }

    private void UpdateDay(int day)
    {
        dayDisplay.text = $"dia {day}";
    }
}