using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClimateManager : MonoBehaviour
{
    [Header("Defaults")]
    public Image climateIcon;
    public Sprite default_ClimateIcon;

    public Action OnEndClimate;

    [SerializeReference] public List<ClimateData> ClimateList = new List<ClimateData>();
    [ContextMenu("Add Rain")] private void AddRain() => ClimateList.Add(new Rain_ClimateData());
    [ContextMenu("Add Dry")] private void AddDry() => ClimateList.Add(new Dry_ClimateData());

    public ClimateData CurrentClimate { get; private set; }

    private bool isInCooldown = false;
    private int cooldownDayEnd;

    private void OnEnable()
    {
        //TimeManager.Instance.OnPassDay += CheckClimate;
        TimeManagerStrike.Instance.DayPassed += CheckClimate;
        OnEndClimate += StartCooldown;
    }

    private void OnDisable()
    {
        //TimeManager.Instance.OnPassDay -= CheckClimate;
        TimeManagerStrike.Instance.DayPassed -= CheckClimate;
        OnEndClimate -= StartCooldown;
    }

    private void StartCooldown()
    {
        if (!GameplayManager.instance.globalConfigs.CanChangeClimateDoCooldown_Active) return;

        CurrentClimate = null;
        cooldownDayEnd = TimeManagerStrike.Instance.CurrentDay + GameplayManager.instance.globalConfigs.ChangeClimateVerification_Cooldown;
        isInCooldown = true;
        //TimeManager.Instance.OnPassDay += CheckCooldown;
        TimeManagerStrike.Instance.DayPassed += CheckCooldown;
    }

    private void CheckCooldown()
    {
        if (TimeManagerStrike.Instance.CurrentDay < cooldownDayEnd ) return;

        isInCooldown = false;
        TimeManagerStrike.Instance.DayPassed -= CheckCooldown;
    }
    private void CheckCooldown(int day)
    {
        //if(TimeManager.Instance.GetCurrentDay() < cooldownDayEnd) return;

        isInCooldown = false;
        //TimeManager.Instance.OnPassDay -= CheckCooldown;
    }

    private void CheckClimate()
    {
        if (isInCooldown) return;
        if (CurrentClimate != null) return;

        if (GameplayManager.instance.globalConfigs.CanChangeClimate_Active && UnityEngine.Random.Range(0f, 100f) <= GameplayManager.instance.globalConfigs.SpecialClimate_Chance)
        {
            // Calculate total weight
            int totalWeight = 0;
            foreach (ClimateData climate in ClimateList)
            {
                totalWeight += climate.climateChance;
            }

            // Generate a random value within the total weight range

            int[] climateWheights = new int[ClimateList.Count];
            for (int i = 0; i < ClimateList.Count; i++)
            {
                climateWheights[i] = ClimateList[i].climateChance;
            }

            int randomClimateIndex = GetRandomWheightedIndex(climateWheights);

            // Find the selected climate
            ClimateData selectedClimate = null;
            foreach (ClimateData climate in ClimateList)
            {
                randomClimateIndex -= climate.climateChance;
                if (randomClimateIndex < 0)
                {
                    selectedClimate = climate;
                    break;
                }
            }

            if (selectedClimate != null)
            {
                List<GlobalConfigs.ClimateIntensitySettings> settings = GameplayManager.instance.globalConfigs.ClimateIntensity_Settings;

                int[] durationWheights = new int[settings.Count];
                for (int i = 0; i < settings.Count; i++)
                {
                    durationWheights[i] = settings[i].ClimateEffect_Chance;
                }

                int randomDurationIndex = GetRandomWheightedIndex(durationWheights);
                int timeToEnd = settings[randomDurationIndex].ClimateEffect_Duration;

                CurrentClimate?.Dispose();
                CurrentClimate = selectedClimate;
                CurrentClimate = ClimateData.CreateClimateDataInstance(selectedClimate.GetType(), selectedClimate, timeToEnd);

                EventManager.Instance.QueueEvent(new ClimateGameEvent(CurrentClimate.climateName));
            }
        }
        else
        {
            StartCooldown();
        }
    }
    private void CheckClimate(int day)
    {
        if (isInCooldown) return;
        if (CurrentClimate != null) return;

        if (GameplayManager.instance.globalConfigs.CanChangeClimate_Active && UnityEngine.Random.Range(0f, 100f) <= GameplayManager.instance.globalConfigs.SpecialClimate_Chance)
        {
            // Calculate total weight
            int totalWeight = 0;
            foreach (ClimateData climate in ClimateList)
            {
                totalWeight += climate.climateChance;
            }

            // Generate a random value within the total weight range

            int[] climateWheights = new int[ClimateList.Count];
            for (int i = 0; i < ClimateList.Count; i++)
            {
                climateWheights[i] = ClimateList[i].climateChance;
            }

            int randomClimateIndex = GetRandomWheightedIndex(climateWheights);

            // Find the selected climate
            ClimateData selectedClimate = null;
            foreach (ClimateData climate in ClimateList)
            {
                randomClimateIndex -= climate.climateChance;
                if (randomClimateIndex < 0)
                {
                    selectedClimate = climate;
                    break;
                }
            }

            if (selectedClimate != null)
            {
                List<GlobalConfigs.ClimateIntensitySettings> settings = GameplayManager.instance.globalConfigs.ClimateIntensity_Settings;

                int[] durationWheights = new int[settings.Count];
                for (int i = 0; i < settings.Count; i++)
                {
                    durationWheights[i] = settings[i].ClimateEffect_Chance;
                }

                int randomDurationIndex = GetRandomWheightedIndex(durationWheights);
                int timeToEnd = settings[randomDurationIndex].ClimateEffect_Duration;

                CurrentClimate?.Dispose();
                CurrentClimate = selectedClimate;
                CurrentClimate = ClimateData.CreateClimateDataInstance(selectedClimate.GetType(), selectedClimate, timeToEnd);

                EventManager.Instance.QueueEvent(new ClimateGameEvent(CurrentClimate.climateName));
            }
        }
        else
        {
            StartCooldown();
        }
    }

    public ClimateData GetClimate()
    {
        int totalWeight = 0;
        foreach (ClimateData climate in ClimateList)
        {
            totalWeight += climate.climateChance;
        }

        // Generate a random value within the total weight range

        int[] climateWheights = new int[ClimateList.Count];
        for (int i = 0; i < ClimateList.Count; i++)
        {
            climateWheights[i] = ClimateList[i].climateChance;
        }

        int randomClimateIndex = GetRandomWheightedIndex(climateWheights);

        // Find the selected climate
        ClimateData selectedClimate = null;
        foreach (ClimateData climate in ClimateList)
        {
            randomClimateIndex -= climate.climateChance;
            if (randomClimateIndex < 0)
            {
                selectedClimate = climate;
                break;
            }
        }

        if(selectedClimate == ClimateList[0])
        {
            selectedClimate.climateName = "Rain";
            return selectedClimate;
        }
        else
        {
            selectedClimate.climateName = "Dry";
            return selectedClimate;
        }
    }

    private int GetRandomWheightedIndex(int[] wheights)
    {
        int weightSum = 0;
        for (int i = 0; i < wheights.Length; ++i)
        {
            weightSum += wheights[i];
        }

        int index = 0;
        int lastIndex = wheights.Length - 1;
        while (index < lastIndex)
        {
            if (UnityEngine.Random.Range(0, weightSum) < wheights[index])
            {
                return index;
            }

            weightSum -= wheights[index++];
        }

        return index;
    }
}