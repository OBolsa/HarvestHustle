using System;
using UnityEngine;

public class FarmingSpot : ItemListener
{
    [Header("Farming Spot State")]
    public SoilState initFarmingSpotState;
    public SoilStateData CurrentFarmingSpotStateData { get { return _currentFarmingSpotStateData; } private set { _currentFarmingSpotStateData = value; } }
    private SoilStateData _currentFarmingSpotStateData;

    [Header("Farming Spot Properties")]
    [Range(0, 100)] public float moistureLevel = 80;
    [Range(0, 100)] public float fertilizationLevel = 25;

    [Header("Farming Spot Components")]
    public Renderer farmingSpotRenderer;
    public TerrainTooltip feedbacks;

    [Header("Plantation Components")]
    private Plantation plantation;
    public Plantation Plantation { get => plantation; }
    public MeshFilter plantationMeshFilter;
    public Renderer plantationMeshRenderer;
    private PlantCollectData collectData;
    private ItemData itemUsed;

    private int nextCollect;
    private int collectionState = 0;

    private void Awake()
    {
        ChangeFarmingSpotStateData(GameplayManager.instance.farmingManager.GetSoilStateData(initFarmingSpotState));
    }
    protected override void Start()
    {
        base.Start();

        RegisterToolEffectHandler(ToolType.Facao, DoPrune);
        RegisterToolEffectHandler(ToolType.Regador, DoWatering);
        RegisterToolEffectHandler(ToolType.Moringa, DoWatering);
        RegisterToolEffectHandler(ToolType.Adubo, DoFertilization);
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        TimeManagerStrike.Instance.StrikePassed += ChangeMoistureOverTime;
        TimeManagerStrike.Instance.DayPassed += CheckMoistureReset;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        TimeManagerStrike.Instance.StrikePassed -= ChangeMoistureOverTime;
        TimeManagerStrike.Instance.DayPassed -= CheckMoistureReset;
    }

    public void InitFarm(PlantData plant)
    {
        plantation = new Plantation(plant, this);
    }
    public void ChangeFarmingSpotStateData(SoilStateData newState)
    {
        CurrentFarmingSpotStateData = newState;
        farmingSpotRenderer.material = newState.material;
    }

    public override void DoInteraction()
    {
        base.DoInteraction();

        if (Plantation != null && Plantation.CurrentStateIndex == 1)
        {
            if(collectionState >= Plantation.plant.collectData.Count)
            {
                FinishPlantation();
            }

            collectData = Plantation.plant.collectData[collectionState];

            GameplayManager.instance.playerInventory.container.ItemAdded += OnCollectOutcome;
            GameplayManager.instance.playerInventory.container.AddItem(Plantation.plant.itemOutcome, collectData.amount);
            return;
        }

        GameplayManager.instance.ChangeFreeLookCamera(FreeLookCameraType.Top);
        GameplayManager.instance.modalManager.OpenItemDisplayerModal(CurrentFarmingSpotStateData.displayData);
    }

    public void OnCollectOutcome(bool collect)
    {
        if (collect)
        {
            Debug.Log($"Produto - {Plantation.plant.itemOutcome.itemName} - Coletado");
            EventManager.Instance.QueueEvent(new FarmGameEvent(Plantation.plant));
            GameplayManager.instance.playerInventory.container.ItemAdded -= OnCollectOutcome;

            if (collectData.timeToNextCollect == 0)
            {
                FinishPlantation();
            }
            else
            {
                NextCollectSetup(collectData);
            }
            return;
        }
        else
        {
            Debug.Log("Sem espaço na mochila");
        }
    }
    public void OnCollectPruneOutcome(bool collect)
    {
        if (collect)
        {
            plantation.ChangePlantCondition(float.MaxValue);
            EventManager.Instance.QueueEvent(new UseItemGameEvent(itemUsed, Name));
            // Apply effects of cleaning the spotTransform
        }
        else
        {
            Debug.Log("Sem espaço na mochila");
        }
    }

    private void NextCollectSetup(PlantCollectData data)
    {
        nextCollect = TimeManagerStrike.Instance.CurrentStrikeCountTotal + data.timeToNextCollect;
        Plantation.ChangeCurrentState(2);
        Plantation.StopGrowth();
        TimeManagerStrike.Instance.StrikePassed += UpdateNextCollect;
    }

    private void UpdateNextCollect()
    {
        if (TimeManagerStrike.Instance.CurrentStrikeCountTotal < nextCollect)
            return;

        Plantation.ChangeCurrentState(1);
        collectionState++;
        TimeManagerStrike.Instance.StrikePassed -= UpdateNextCollect;
    }

    public void FinishPlantation()
    {
        _interactableName = "Terreno Limpo";

        feedbacks.UpdateDegradationFeedback(false);
        ChangeFertilization(fertilizationLevel - (plantation.plant.fertilizationDrain * 25));
        plantationMeshFilter.mesh = null;

        // Clear the materials by providing an empty array
        plantationMeshRenderer.materials = new Material[0];

        ChangeFarmingSpotStateData(GameplayManager.instance.farmingManager.GetSoilStateData(SoilState.Default));
        plantation = null;
    }

    #region Moisture
    private void ChangeMoistureOverTime()
    {
        if (GameplayManager.instance.globalConfigs.MoistureDecayOverStrike_Active)
            ChangeMoisture(moistureLevel - GameplayManager.instance.globalConfigs.MoistureDecayOverStrike_Amount);
    }

    private void CheckMoistureReset()
    {
        if (GameplayManager.instance.globalConfigs.MoistureHaveDayReset_Active)
            moistureLevel = GameplayManager.instance.globalConfigs.MoistureDayResetValue_Amount;
    }
    public void ChangeMoisture(float change)
    {
        moistureLevel = Mathf.Clamp(change, 0, 100);

        if(plantation != null)
            feedbacks.UpdateMoistureFeedback(moistureLevel <= GameplayManager.instance.globalConfigs.MoistureValueToStartBeDry_Amount);
    }
    #endregion

    #region Fertilization
    public void ChangeFertilization(float change)
    {
        fertilizationLevel = Mathf.Clamp(change, 0, 100);
        feedbacks.UpdateFertilizationFeedback(fertilizationLevel < GameplayManager.instance.globalConfigs.FertilizationMinimunToStartDeath_Amount);
    }
    #endregion

    #region ToolEffects
    private void DoPrune(ItemData item)
    {
        itemUsed = item;
        GameplayManager.instance.playerInventory.container.ItemAdded += OnCollectPruneOutcome;
        GameplayManager.instance.playerInventory.container.AddItem(plantation.plant.pruneItem, plantation.plant.pruneItemAmount);
    }
    private void DoWatering(ItemData item)
    {
        itemUsed = item;
        ChangeMoisture(moistureLevel < 80 ? 80 : 100);
        EventManager.Instance.QueueEvent(new UseItemGameEvent(item, Name));
        // Apply effects of watering the spotTransform
    }
    private void DoFertilization(ItemData item)
    {
        itemUsed = item;
        ChangeFertilization(fertilizationLevel + GameplayManager.instance.globalConfigs.FertilizationIncreaseWithItem_Amount);
        ChangeFarmingSpotStateData(GameplayManager.instance.farmingManager.GetSoilStateData(SoilState.Fertilized));
        EventManager.Instance.QueueEvent(new UseItemGameEvent(item, Name));
        // Apply effects of the spotTransform fertilizer
    }
    #endregion
}