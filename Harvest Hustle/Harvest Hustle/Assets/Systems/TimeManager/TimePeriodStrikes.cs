using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimePeriodStrikes : MonoBehaviour
{
    public Image periodFill;
    public Image periodIcon;
    public TMP_Text dayDisplay;

    public float nextFill;
    private bool isEvening = false;

    private void Start()
    {
        NextPeriod();
    }

    private void OnEnable()
    {
        TimeManagerStrike.Instance.StrikePassed += UpdateFill;
        TimeManagerStrike.Instance.DayPassed += StartNewDay;
    }

    private void OnDisable()
    {
        TimeManagerStrike.Instance.StrikePassed -= UpdateFill;
        TimeManagerStrike.Instance.DayPassed -= StartNewDay;
    }

    public void UpdateFill()
    {
        if (isEvening)
            return;

        // Define the next fill;
        nextFill += 0.25f;

        // Do the fillAmount
        DOTween.To(() => periodFill.fillAmount, x => periodFill.fillAmount = x, nextFill, 0.2f).OnComplete(() =>
        {
            // When end fill amount, check if the fill amount is 1. If it is the case, go to the next Period.
            if (IsFilled()) NextPeriod();
        });
    }

    private void UpdateVisualElements()
    {
        DayPeriodInfo infos = TimeManagerStrike.Instance.CurrentDayPeriodInfo;

        periodFill.color = infos.Color;
        periodIcon.sprite = infos.Icon;

        if (infos.Period == DayPeriod.Evening)
        {
            periodFill.fillAmount = 1;
            isEvening = true;
        }

        string dayToDisplay = (TimeManagerStrike.Instance.CurrentDay + 1).ToString();
        dayDisplay.text = $"Dia {dayToDisplay}";
    }

    private void StartNewDay()
    {
        periodFill.fillAmount = 0;
        nextFill = 0;
        isEvening = false;
        UpdateVisualElements();
    }

    public void NextPeriod()
    {
        // Resets the FillAmount to 0 and the nextFill
        periodFill.fillAmount = 0;
        nextFill = 0;

        if(TimeManagerStrike.Instance.DayPeriod == DayPeriod.Evening)
            isEvening = true;

        // And Update The Image;
        UpdateVisualElements();
    }
    private bool IsFilled() => periodFill.fillAmount == 1;
}