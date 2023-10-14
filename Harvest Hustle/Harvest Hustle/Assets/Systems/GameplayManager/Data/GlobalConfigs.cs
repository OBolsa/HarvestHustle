using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GlobalConfigs", fileName = "GlobalConfigs")]
public class GlobalConfigs : ScriptableObject
{
    #region WorldSettings
    [Header("Player Settings")]
    public bool Player_Active;
    public bool CanMakeInteractions_Active;

    [Header("Text Settings")]
    public float SlowText_Speed = 15f;
    public float MediumText_Speed = 60f;
    public float FastText_Speed = 120f;
    public float GetTextSpeed(TextSpeedType type)
    {
        switch (type)
        {
            case TextSpeedType.Slow:
                return SlowText_Speed;
            default:
            case TextSpeedType.Medium:
                return MediumText_Speed;
            case TextSpeedType.Fast:
                return FastText_Speed;
        }
    }

    [Header("Sprite Settings")]
    public Sprite DefaultPortrait;
    public Sprite BlankIcon;

    [Header("Camera Settings")]
    public bool Camera_Active;

    [Header("Time Settings")]
    public List<DayPeriodInfo> DayPeriodInfos = new List<DayPeriodInfo>();
    public DayPeriodInfo GetDayPeriodInfo(DayPeriod period) => DayPeriodInfos.Find(p => p.Period == period);
    #endregion
    [Space(24)]

    #region ClimateSettings
    [Header("Climate Settings")]
    public bool CanChangeClimate_Active;
    public bool CanChangeClimateDoCooldown_Active;
    public int ChangeClimateVerification_Cooldown = 3;
    [Range(0f, 100f)] public float SpecialClimate_Chance = 25f;
    public List<ClimateIntensitySettings> ClimateIntensity_Settings = new List<ClimateIntensitySettings>();

    [Header("Rain Settings")]
    // Moisture
    public bool MoistureStaticOnRain_Active;
    public float MoistureStaticOnRain_Amount = 80f;
    // Degradation
    public bool DegradationDecreaseOnRain_Active;
    public float DegradationDecreaseOnRainPerStrike_Amount = 15f;

    [System.Serializable]
    public class ClimateIntensitySettings
    {
        public ClimateIntensity intensity;
        [Range(0, 100)] public int ClimateEffect_Chance = 50;
        public int ClimateEffect_Duration = 3;
    }
    #endregion
    [Space(24)]

    #region PlantationSettings
    [Header("Plantation Settings")]
    public int TimeForPlantationDeathInHours_Amount = 24;
    public int TimeForPlantationDeathInStrikes_Amount = 8;

    [Header("Moisture Settings")]
    // Decay Over Strike
    public bool MoistureDecayOverStrike_Active;
    public float MoistureDecayOverStrike_Amount = 7.5f;
    public float MoistureValueToStartBeDry_Amount = 5f;
    public float MoistureValueToStartBeSoak_Amount = 80f;
    // Reset Value
    public bool MoistureHaveDayReset_Active;
    public float MoistureDayResetValue_Amount = 5f;

    [Header("Fertilization Settings")]
    public bool FertilizationDecayOverEndPlantation_Active;
    public float FertilizationMinimunToStartDeath_Amount = 25f;
    public float FertilizationIncreaseWithItem_Amount = 50f;

    [Header("Plant Degradation Settings")]
    public bool DegradatePlantationPerStrike_Active;
    public float DegradationDecayPerStrike_Amount = 1.5f;
    public float DegradationIntegrityMinValue_Amount = 15f;
    public float DegradationIntegrityMediumValue_Amount = 30f;
    public float DegradationIntegrityMaxValue_Amount = 50f;
    #endregion
    [Space(24)]

    #region Donation
    [Header("Donations Settings")]
    public List<DonationInfo> DonationInfos = new List<DonationInfo>();
    public int GetDonationValue(ItemData item) => DonationInfos.Find(d => d.item.itemName == item.itemName).donationValue;
    #endregion
    [Space(24)]

    #region ActionsSettings
    [Header("Time to Execute Actions Settings")]
    public float BarFillTimeInSecondsForNoneStrike = 1f;
    public float BarFillTimeInSecondsForSingleStrike = 2f;
    public float BarFillTimeInSecondsForDoubleStrike = 5f;
    public float GetBarFillTimeInSeconds(StrikeType type)
    {
        switch(type)
        {
            default:
            case StrikeType.None:
                return BarFillTimeInSecondsForNoneStrike;
            case StrikeType.Single:
                return BarFillTimeInSecondsForSingleStrike;
            case StrikeType.Double:
                return BarFillTimeInSecondsForDoubleStrike;
        }
    }
    #endregion
    [Space(24)]

    #region SystemsSettings
    [Header("Compost Bin Settings")]
    public int BrownSubstractToMakeFertilization_Amount;
    public int GreenSubstractToMakeFertilization_Amount;
    public int HoursToProduceFertilization_Amount;
    public int FertilizationProduction_Amount;
    #endregion
}

[System.Serializable]
public class DayPeriodInfo
{
    public DayPeriod Period;
    public Sprite Icon;
    public Color Color;
}

[System.Serializable]
public class DonationInfo
{
    public ItemData item;
    public int donationValue;
}

public enum ClimateIntensity
{
    Strong,
    Medium,
    Weak,
}

public enum ClimateType
{
    Default,
    Rain,
    Dry
}