using UnityEngine;

[System.Serializable]
public class Plantation
{
    public PlantData plant;
    public FarmingSpot farmingSpot;

    // == Plantation Name ==
    public string PlantationName
    {
        get
        {
            switch(currentState)
            {
                default: return plant.plantName;
                case 0: return $"{plant.plantName} {CurrentPlantState.stateName}";
                case 1: return $"{CurrentPlantState.stateName} {plant.plantName}";
                case 2: return $"{plant.plantName} {CurrentPlantState.stateName}";
                case 3: return $"{CurrentPlantState.stateName} de {plant.plantName}";
                case 4: return $"{plant.plantName} em Crescimento";
                case 5: return $"{plant.plantName} em Maturação";
            }
        }
    }

    // == PlantCondition Scheme ==
    public float PlantCondition { get => _plantCondition;  /*private*/ set => _plantCondition = Mathf.Clamp(value, 0f, 100f); }
    private float _plantCondition;
    public int Integrity { get => _integrity; private set => _integrity = Mathf.Clamp(value, 0, 3); }
    private int _integrity;
    // Integrity 0 = Muito Degradada;
    // Integrity 1 = Degradada;
    // Integrity 2 = Pouco Degradada;
    // Integrity 3 = Íntegra;

    // == Growth Scheme ==
    //public bool CanGrow { get => }
    public int NextTimeToGrow { get => plant.timeToGrow / (plant.states.Count - 3); }
    public int GrowCount { get => _growCount; private set => _growCount = Mathf.Clamp(value, 0, NextTimeToGrow); }
    private int _growCount = 0;
    public int GrowthStartTime { get; private set; }

    // == Death Scheme ==
    public int HoursToDead { get => GameplayManager.instance.globalConfigs.TimeForPlantationDeathInStrikes_Amount; }
    public bool HaveDeathConditions
    {
        get
        {
            bool isDry = farmingSpot != null && farmingSpot.moistureLevel < GameplayManager.instance.globalConfigs.MoistureValueToStartBeDry_Amount;
            bool isSoaked = farmingSpot != null && farmingSpot.moistureLevel > GameplayManager.instance.globalConfigs.MoistureValueToStartBeSoak_Amount;
            bool isTooDegradated = Integrity < (int)plant.care;

            return isDry || isSoaked || isTooDegradated;
        }
    }
    public int DeathCount { get => _deathCount; private set => _deathCount = Mathf.Clamp(value, 0, HoursToDead); }
    public bool IsDying { get => _isDying; private set => _isDying = value; }
    private bool _isDying;
    public bool IsDead { get => DeathCount == HoursToDead; }
    private int _deathCount;

    // == Nutrition Scheme ==
    public bool HaveTerrainConditions
    {
        get
        {
            switch ((int)plant.fertilization)
            {
                default: return false;
                case 0: return farmingSpot.fertilizationLevel >= 25;
                case 1: return farmingSpot.fertilizationLevel >= 50;
                case 2: return farmingSpot.fertilizationLevel >= 75;
            }
        }
    }

    // == PlantData State Control ==
    public bool IsGrowing { get => currentState > 2; }
    public PlantState CurrentPlantState { get { return plant.states[currentState]; } }
    public int CurrentStateIndex { get => currentState; }
    private int currentState;
    // State 0 = Morta;
    // State 1 = Colheita;
    // State 2 = SemFruto;
    // State 3 = Broto;
    // State 4 = Vegetativo;
    // State 5 = Maturacao;

    public Plantation(PlantData plant, FarmingSpot spot) 
    {
        this.plant = plant;
        farmingSpot = spot;
        GrowthStartTime = TimeManagerStrike.Instance.CurrentStrikeCountTotal;
        ChangePlantCondition(100);
        StartGrowth();
    }

    public Plantation(FarmingSpot spot)
    {
        farmingSpot = spot;
        GrowthStartTime = TimeManagerStrike.Instance.CurrentStrikeCountTotal;
        ChangePlantCondition(100);
        StartGrowth();
    }

    public void UpdatePlantation()
    {
        if(HaveDeathConditions)
        {
            if (IsDying)
            {
                UpdatesDeathCounter();
            }
            else
            {
                StartDeath();
            }
        }
        else
        {
            IsDying = false;
            ChangePlantCondition(PlantCondition - GameplayManager.instance.globalConfigs.DegradationDecayPerStrike_Amount);
            DoGrowth();
        }
    }

    //public void UpdatePlantation(int hours, int minutes)
    //{
    //    bool hourPassed = TimeManager.Instance.GetCurrentHours() > TimeManager.Instance.LastTime.x;

    //    if (hourPassed)
    //    {
    //        Debug.Log($"Plant Integrity = {Integrity}");
    //        if (HaveDeathConditions)
    //        {
    //            if (!IsDying)
    //            {
    //                StartDeath();
    //            }
    //            else
    //            {
    //                UpdatesDeathCounter();
    //            }
    //        }
    //        else
    //        {
    //            if(IsDying) IsDying = false;
    //            ChangePlantCondition(PlantCondition - 1.7f);
    //            DoGrowth();
    //        }
    //    }
    //}
    public void FinishPlantation()
    {
        TimeManagerStrike.Instance.StrikePassed -= UpdatePlantation;
        farmingSpot.FinishPlantation();
        Debug.Log($"End {plant.plantName} Plantation");
    }

    public void StopGrowth()
    {
        TimeManagerStrike.Instance.StrikePassed -= UpdatePlantation;
    }

    #region PlantGrowth
    private void StartGrowth()
    {
        if (!HaveTerrainConditions)
        {
            Debug.Log("Don't have the terrain conditions");
            StartDeath();
            return;
        }

        GrowCount = 0;
        currentState = 3;
        Integrity = int.MaxValue;

        // Aqui eu tenho que entrar no evento para começar a dar update na plantação.
        //OLD -- TimeManager.Instance.OnTimePass += UpdatePlantation; -- OLD
        TimeManagerStrike.Instance.StrikePassed += UpdatePlantation;

        Debug.Log($"Start Planting -{plant.plantName}- / Start State -{CurrentPlantState.stateName}- / - CareType: {(int)plant.care}");
        ChangeModelState();
    }

    private void UpdateTerrainName()
    {
        farmingSpot._interactableName = CurrentPlantState.stateName;
    }

    private void DoGrowth() 
    {
        Debug.Log($"Growing -{plant.plantName}- / GrowCount -{_growCount}-");
        GrowCount++;

        if(GrowCount >= NextTimeToGrow)
        {
            string d = $"State Changed From -{CurrentPlantState.stateName}- /";

            GrowCount = 0;
            ChangeCurrentState(currentState + 1 >= plant.states.Count ? 1 : currentState + 1);
            ChangeModelState();

            d += $" To -{CurrentPlantState.stateName}-";
            Debug.Log(d);
        }
    }
    public void ChangeCurrentState(int newState)
    {
        currentState = newState;
        GameplayManager.instance.tooltip.UpdateText();
        ChangeModelState();
    }
    #endregion

    #region PlantStateManager
    private void ChangeModelState()
    {
        farmingSpot.plantationMeshFilter.mesh = CurrentPlantState.ModelMesh;
        farmingSpot.plantationMeshRenderer.materials = CurrentPlantState.ModelMaterials;
        UpdateTerrainName();
    }
    #endregion

    #region PlantDegradation
    private void UpdateDegradation()
    {
        if (PlantCondition < GameplayManager.instance.globalConfigs.DegradationIntegrityMinValue_Amount)
        {
            Integrity = 0;
        }
        else if (PlantCondition < GameplayManager.instance.globalConfigs.DegradationIntegrityMediumValue_Amount)
        {
            Integrity = 1;
        }
        else if (PlantCondition < GameplayManager.instance.globalConfigs.DegradationIntegrityMaxValue_Amount)
        {
            Integrity = 2;
        }
        else
        {
            Integrity = 3;
        }

        farmingSpot.feedbacks.UpdateDegradationFeedback(Integrity < (int)plant.care);
    }
    public void ChangePlantCondition(float newCondition)
    {
        if (!GameplayManager.instance.globalConfigs.DegradatePlantationPerStrike_Active)
            return;

        PlantCondition = newCondition;
        UpdateDegradation();
    }
    #endregion

    #region PlantDeath
    private void StartDeath()
    {
        StartDeathTimer();
        DeathCount = 0;
    }
    private void UpdatesDeathCounter()
    {
        DeathCount += 1;
        Debug.Log($"A plantação de {plant.name} está 1 ponto mais próximo da morte. Morte em {HoursToDead - DeathCount} horas.");

        if(IsDead) 
        {
            ChangeCurrentState(0);
            Debug.Log($"A plantação de {plant.name} morreu.");
            FinishPlantation(); 
        }
    }
    private void StartDeathTimer()
    {
        IsDying = true;
    }
    #endregion

    private void SaveDataState()
    {
        PlantationSaveData savedData = new PlantationSaveData()
        {
            Plant = plant,
            PlantCondition = PlantCondition,
            Integrity = Integrity,
            GrowCount = GrowCount,
            GrowthStartTime = GrowthStartTime,
            DeathCount = DeathCount,
            IsDying = IsDying,
            FarmingSpotID = farmingSpot.ID
        };

        GameplayManager.instance.sceneSaver.DataFromPlantations.Add(savedData);
    }

    private void RestoreDataState()
    {
        PlantationSaveData savedData = GameplayManager.instance.sceneSaver.DataFromPlantations.Find(d => d.FarmingSpotID == farmingSpot.ID);

        if (savedData == null)
            return;

        plant = savedData.Plant;
        PlantCondition = savedData.PlantCondition;
        Integrity = savedData.Integrity;
        GrowCount = savedData.GrowCount;
        GrowthStartTime = savedData.GrowthStartTime;
        DeathCount = savedData.DeathCount;
        IsDying = savedData.IsDying;
    }
}

[System.Serializable]
public class PlantationSaveData
{
    public string FarmingSpotID;
    public PlantData Plant;
    public float PlantCondition;
    public int Integrity;
    public int GrowCount;
    public int GrowthStartTime;
    public int DeathCount;
    public bool IsDying;
}