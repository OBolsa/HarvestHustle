using UnityEngine;

[System.Serializable]
public class Dry_ClimateData : ClimateData
{
    public Light light;
    public float moistureOverTime;

    public Dry_ClimateData() { }

    public Dry_ClimateData(int daysToEnd) : base(daysToEnd)
    {
    }

    public Dry_ClimateData(Light light, string climateName, int climateChance, Sprite climateIcon, float moistureOverTime, int daysToEnd)
    {
        Debug.Log($"Dry Climate Starting! It will run for {daysToEnd} days");
        this.light = light;
        this.climateName = climateName;
        this.climateChance = climateChance;
        this.climateIcon = climateIcon;
        this.moistureOverTime = moistureOverTime;

        StartClimate(daysToEnd);
        //TimeManager.Instance.OnStrike += UpdateClimate;
        TimeManagerStrike.Instance.StrikePassed += UpdateClimate;
    }

    public override void StartClimate(int daysToEnd)
    {
        base.StartClimate(daysToEnd);
        light.intensity = 1.5f;
        // Aplicar efeitos de Post Processing de Seca
    }

    public override void UpdateClimate()
    {
        if(TimeManagerStrike.Instance.CurrentStrikeCountTotal > climateDay_End)
        {
            // Retirar Efeitos de Post Processing de Seca
            light.intensity = 1;
            GameplayManager.instance.climateManager.climateIcon.sprite = GameplayManager.instance.climateManager.default_ClimateIcon;
            Dispose();
            return;
        }

        foreach (var spot in GameplayManager.instance.farmingManager.farmingSpots)
        {
            spot.ChangeMoisture(spot.moistureLevel - (int)moistureOverTime);
        }
    }
    public override void UpdateClimate(int strikes)
    {
        //if (TimeManager.Instance.GetCurrentDay() >= climateDay_End)
        //{
        //    // Retirar Efeitos de Post Processing de Seca
        //    light.intensity = 1;
        //    GameplayManager.instance.climateManager.climateIcon.sprite = GameplayManager.instance.climateManager.default_ClimateIcon;
        //    Dispose();
        //    return;
        //}
        if (TimeManagerStrike.Instance.CurrentDay >= climateDay_End)
        {
            // Retirar Efeitos de Post Processing de Seca
            light.intensity = 1;
            GameplayManager.instance.climateManager.climateIcon.sprite = GameplayManager.instance.climateManager.default_ClimateIcon;
            Dispose();
            return;
        }

        foreach (var spot in GameplayManager.instance.farmingManager.farmingSpots)
        {
            spot.ChangeMoisture(spot.moistureLevel - (int)moistureOverTime);
        }
        // Beneficiar Caça
    }
}