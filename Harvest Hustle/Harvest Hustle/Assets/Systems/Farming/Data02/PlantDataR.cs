using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plant Data/Plant", fileName = "Plant_")]
public class PlantDataR : PlantDataRStateMachine
{
    [Header("Configs")]
    public PlantDataConfig Configs;

    [Header("General Infos")]
    public GrowthState CurrentGrowthState;
    public PlantStateDataR CurrentStateData
    {
        get => States.Find(s => s.SrowthState == CurrentGrowthState);
    }
    public int Integrity = 100;
    public int NextGrowthInStrikes = 0;
    private FarmingSpotR spot;

    //[Header("Checkages")]
    public bool NeedsCare 
    {
        get
        {
            int care = 0;
            if (Integrity < GameplayManager.instance.globalConfigs.DegradationIntegrityMinValue_Amount)
            {
                care = 0;
            }
            else if (Integrity < GameplayManager.instance.globalConfigs.DegradationIntegrityMediumValue_Amount)
            {
                care = 1;
            }
            else if (Integrity < GameplayManager.instance.globalConfigs.DegradationIntegrityMaxValue_Amount)
            {
                care = 2;
            }
            else
            {
                care = 3;
            }

            return care < (int)Configs.care;
        }
    }
    public bool NeedsWater
    {
        get
        {
            return spot.MoistureLevel < GameplayManager.instance.globalConfigs.MoistureValueToStartBeDry_Amount;
        }
    }
    public bool IsSoaked
    {
        get => spot.MoistureLevel > 80;
    }
    public bool IsInfertile
    {
        get => 3 * spot.FertilizationLevel / 100 < (int)Configs.fertilization;
    }
    public bool HaveDeathConditions
    {
        get => NeedsWater || IsSoaked || NeedsCare;
    }
    public bool IsDying
    { 
        get; 
        private set; 
    }
    public bool IsDead
    {
        get => DeathStrikeCount >= GameplayManager.instance.globalConfigs.TimeForPlantationDeathInStrikes_Amount;
    }
    public bool CanCollect
    {
        get; private set;
    }

    [Header("Counters")]
    public int DeathStrikeCount;
    public int GrowthStrikeCount;

    public void Initialize(FarmingSpotR spot)
    {
        CanCollect = false;
        CurrentGrowthState = GrowthState.Broto;
        GrowthStrikeCount = 0;
        NextGrowthInStrikes = TimeManagerStrike.Instance.CurrentStrikeCountTotal + Configs.TimeToGrowth / 3;
        this.spot = spot;

        // Vai morrer
        if (IsInfertile)
        {
            IsDying = true;
        } // Vai viver. Por enquanto
        else
        {
            // Agora ele cresce a cada strike
            TimeManagerStrike.Instance.StrikePassed += DoGrowth;
        }
    }

    public void DoGrowth()
    {
        if (IsDying)
        {
            DeathStrikeCount++;

            if (IsDead)
            {
                // Mostra que ta morta;
            }
        }
        else
        {
            if (HaveDeathConditions)
            {
                StartDeath();
                return;
            }// Vai crescer
            else
            {
                GrowthStrikeCount++;

                if(GrowthStrikeCount >= NextGrowthInStrikes)
                {
                    NextState();
                }
            }
        }
    }

    private void StartDeath()
    {
        IsDying = true;
        DeathStrikeCount = 0;
        // Invocar feedback de que esta morrendo;
    }

    public void NextState()
    {
        GrowthStrikeCount = 0;

        // Determine the next state based on the current state
        GrowthState nextState = GetNextState();

        // Handle specific behaviors for each state
        switch (nextState)
        {
            case GrowthState.Morto:
                // Encerrou o ciclo
                // Add any specific logic for this state
                break;
            case GrowthState.Colheita:
                // Agora pode ser colhida
                CanCollect = true;
                // Add any specific logic for this state
                break;
            default:
                // Handle any other intermediate States
                // Add any logic for these States
                break;
        }

        // Change the current growth state
        CurrentGrowthState = nextState;

        // Here you can call the method to change the state based on the GrowthState
        ChangeStateBasedOnGrowthState();
    }

    public void GiveProducts()
    {
        // Tentar adicionar os itens pro jogador.
    }

    private GrowthState GetNextState()
    {
        return CurrentGrowthState switch
        {
            GrowthState.SemFruto => GrowthState.Colheita,
            GrowthState.Broto => GrowthState.Vegetativo,
            GrowthState.Vegetativo => GrowthState.Maturacao,
            GrowthState.Maturacao => GrowthState.Colheita,
            GrowthState.Colheita => Configs.CollectData.Count > 1 ? GrowthState.SemFruto : GrowthState.Morto,
            _ => GrowthState.Morto,
        };
    }

    public void ChangeStateBasedOnGrowthState()
    {
        TransitionToState(CurrentGrowthState);
    }

    [System.Serializable]
    public class PlantDataConfig
    {
        [Header("Indentifiers")]
        public string PlantName;
        public string PlantDescription;
        public ItemData Seed;
        public ItemData Product;
        public List<ItemData> PruneProduct = new List<ItemData>();

        [Header("Growing Info")]
        public int TimeToGrowth;
        [Range(0, 3)] public int fertilizationDrain;
        public IrrigationType irrigation;
        public FertilizationType fertilization;
        public CareType care;

        [Header("States")]
        public List<PlantCollectData> CollectData = new List<PlantCollectData>();
    }
}

public enum GrowthState
{
    Morto,
    SemFruto,
    Broto,
    Vegetativo,
    Maturacao,
    Colheita
}