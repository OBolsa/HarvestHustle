using System.Collections.Generic;
using UnityEngine;

public class ClimateSorter : MonoBehaviour
{
    public List<ClimateDataEntry> ClimatesByDayList = new List<ClimateDataEntry>();
    public string SaveFilePath;
    public string FileName { get => $"{_fileName}.json"; }
    public string _fileName;
    public int daysToMakeList;
    public int ClimateCooldown { get => GameplayManager.instance.globalConfigs.ChangeClimateVerification_Cooldown; }
    public bool HaveSpecialClimate
    {
        get
        {
            float randomValue = Random.Range(0f, 100f);
            return randomValue <= GameplayManager.instance.globalConfigs.SpecialClimate_Chance;
        }
    }
    public int ClimateDuration
    {
        get
        {
            List<GlobalConfigs.ClimateIntensitySettings> settings = GameplayManager.instance.globalConfigs.ClimateIntensity_Settings;

            int[] durationWheights = new int[settings.Count];
            for (int i = 0; i < settings.Count; i++)
            {
                durationWheights[i] = settings[i].ClimateEffect_Chance;
            }

            int randomDurationIndex = GetRandomWheightedIndex(durationWheights);
            int timeToEnd = settings[randomDurationIndex].ClimateEffect_Duration;
            return timeToEnd;
        }
    }

    [ContextMenu("Make Climate list")]
    public void SortClimate()
    {
        ClimatesByDayList.Clear();

        int dayFill = 1;

        bool isInCooldown = false;
        int endOfCooldown = 0;

        bool isInSpecialClimate = false;
        int endOfSpecialClimate = 0;

        string lastClimate = "";

        while (dayFill < daysToMakeList)
        {
            if (isInCooldown)
            {
                ClimatesByDayList.Add(new ClimateDataEntry { day = dayFill, climate = lastClimate });
                dayFill++;

                isInCooldown = dayFill < endOfCooldown;
                continue;
            }
            else if (isInSpecialClimate)
            {
                ClimatesByDayList.Add(new ClimateDataEntry { day = dayFill, climate = lastClimate });
                dayFill++;

                isInSpecialClimate = dayFill < endOfSpecialClimate;
                continue;
            }
            else
            {
                if (dayFill < 4)
                {
                    lastClimate = "Default";
                    ClimatesByDayList.Add(new ClimateDataEntry { day = dayFill, climate = lastClimate });
                    dayFill++;
                    continue;
                }

                if (HaveSpecialClimate)
                {
                    lastClimate = SortedClimate().climateName;
                    ClimatesByDayList.Add(new ClimateDataEntry { day = dayFill, climate = lastClimate });
                }
                else
                {
                    lastClimate = "Default";
                    ClimatesByDayList.Add(new ClimateDataEntry { day = dayFill, climate = lastClimate });
                    isInCooldown = true;
                }

                dayFill++;
            }
        }

        // Save the data as JSON
        SaveClimatesToJson(SaveFilePath, FileName);

        string debug = "";

        foreach (ClimateDataEntry entry in ClimatesByDayList)
        {
            debug += $"Day: {entry.day} || Climate: {entry.climate}\n";
        }

        Debug.Log(debug);
    }

    public ClimateData SortedClimate() => GameplayManager.instance.climateManager.GetClimate();

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
            if (Random.Range(0, weightSum) < wheights[index])
            {
                return index;
            }

            weightSum -= wheights[index++];
        }

        return index;
    }

    public void SaveClimatesToJson(string folderPath, string fileName)
    {
        // Construct the full file path
        string filePath = System.IO.Path.Combine(folderPath, fileName);

        // Check if the file exists
        if (System.IO.File.Exists(filePath))
        {
            // If it exists, delete the file to replace it
            System.IO.File.Delete(filePath);
        }

        // Create a wrapper object
        ClimateDataWrapper wrapper = new ClimateDataWrapper
        {
            climateDataList = ClimatesByDayList
        };

        // Convert the wrapper object to JSON
        string jsonData = JsonUtility.ToJson(wrapper);
        Debug.Log(jsonData);

        // Save the JSON data to a new file or the replaced file
        System.IO.File.WriteAllText(filePath, jsonData);

        Debug.Log($"Saved climates data to {filePath}");

        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }


    [System.Serializable]
    public class ClimateDataEntry
    {
        public int day;
        public string climate;
    }

    [System.Serializable]
    public class ClimateDataWrapper
    {
        public List<ClimateDataEntry> climateDataList;
    }
}