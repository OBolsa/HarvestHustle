using System;
using UnityEngine;

[System.Serializable]
public abstract class ClimateData
{
    public string climateName;
    public int climateChance;
    public Sprite climateIcon;
    protected string ClimateType { get => "D"; }

    protected int climateDay_Start;
    protected int climateDay_End;

    protected ClimateData() { }

    protected ClimateData(int daysToEnd)
    {
        StartClimate(daysToEnd);
        //TimeManager.Instance.OnStrike += UpdateClimate;
        TimeManagerStrike.Instance.StrikePassed += UpdateClimate;
    }

    public virtual void StartClimate(int daysToEnd)
    {
        //climateDay_Start = TimeManager.Instance.GetCurrentDay();
        climateDay_Start = TimeManagerStrike.Instance.CurrentDay;
        climateDay_End = climateDay_Start + daysToEnd;
        GameplayManager.instance.climateManager.climateIcon.sprite = climateIcon;
    }
    public abstract void UpdateClimate();
    public abstract void UpdateClimate(int strikes);
    public void Dispose()
    {
        //TimeManager.Instance.OnStrike -= UpdateClimate;
        TimeManagerStrike.Instance.StrikePassed -= UpdateClimate;
        GameplayManager.instance.climateManager.OnEndClimate?.Invoke();
    }
    public static ClimateData CreateClimateDataInstance(Type climateType, ClimateData data, int daysToEnd)
    {
        if (typeof(Rain_ClimateData).IsAssignableFrom(climateType))
        {
            Rain_ClimateData passedData = (Rain_ClimateData)data;
            return new Rain_ClimateData(passedData.rainParticle, passedData.climateName, passedData.climateChance, passedData.climateIcon, passedData.moistureOverTime, passedData.plantConditionDecay, daysToEnd);
        }
        else if (typeof(Dry_ClimateData).IsAssignableFrom(climateType))
        {
            Dry_ClimateData passedData = (Dry_ClimateData)data;
            return new Dry_ClimateData(passedData.light, passedData.climateName, passedData.climateChance, passedData.climateIcon, passedData.moistureOverTime, daysToEnd);
        }
        // Add more conditions for other subclasses if needed

        return null;
    }
}