using UnityEngine;

[System.Serializable]
public class Rain_ClimateData : ClimateData
{
    public ParticleSystem rainParticle;
    public float moistureOverTime;
    public float plantConditionDecay;

    public Rain_ClimateData() { }

    public Rain_ClimateData(int daysToEnd) : base(daysToEnd)
    {
    }

    public Rain_ClimateData(ParticleSystem rainParticle, string climateName, int climateChance, Sprite climateIcon, float moistureOverTime, float plantConditionDecay, int daysToEnd)
    {
        Debug.Log($"Rain Climate Starting!  It will run for {daysToEnd} days");
        this.climateName = climateName;
        this.climateChance = climateChance;
        this.climateIcon = climateIcon;
        this.moistureOverTime = moistureOverTime;
        this.plantConditionDecay = plantConditionDecay;
        this.rainParticle = rainParticle;

        StartClimate(daysToEnd);
        //TimeManager.Instance.OnStrike += UpdateClimate;
        TimeManagerStrike.Instance.StrikePassed += UpdateClimate;
    }

    public override void StartClimate(int daysToEnd)
    {
        base.StartClimate(daysToEnd);
        DoRain(true);
        GameplayManager.instance.farmingManager.farmingSpots.ForEach(f => f.Plantation?.ChangePlantCondition(f.Plantation.PlantCondition - plantConditionDecay));
        // Aplicar Efeitos de Post Processing de Chuva
    }

    public override void UpdateClimate()
    {
        if (TimeManagerStrike.Instance.CurrentStrikeCountTotal >= climateDay_End)
        {
            DoRain(false);
            // Retirar Efeitos de Post Processing de Chuva
            GameplayManager.instance.climateManager.climateIcon.sprite = GameplayManager.instance.climateManager.default_ClimateIcon;
            Dispose();
            return;
        }

        GameplayManager.instance.farmingManager.farmingSpots.ForEach(f => f.ChangeMoisture((int)moistureOverTime));
    }
    public override void UpdateClimate(int strikes)
    {
        //if(TimeManager.Instance.GetCurrentDay() >= climateDay_End) 
        //{
        //    DoRain(false);
        //    // Retirar Efeitos de Post Processing de Chuva
        //    GameplayManager.instance.climateManager.climateIcon.sprite = GameplayManager.instance.climateManager.default_ClimateIcon;
        //    Dispose();
        //    return;
        //}
        if (TimeManagerStrike.Instance.CurrentDay >= climateDay_End)
        {
            DoRain(false);
            // Retirar Efeitos de Post Processing de Chuva
            GameplayManager.instance.climateManager.climateIcon.sprite = GameplayManager.instance.climateManager.default_ClimateIcon;
            Dispose();
            return;
        }

        GameplayManager.instance.farmingManager.farmingSpots.ForEach(f => f.ChangeMoisture((int)moistureOverTime));
    }

    public void DoRain(bool play)
    {
        if (!play)
        {
            rainParticle.Stop();
            TimeManager.Instance.IsRaining = false;
        }
        else
        {
            rainParticle.Play();
            TimeManager.Instance.IsRaining = true;
        }
    }
}
