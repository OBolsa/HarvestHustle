using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmingManager : MonoBehaviour
{
    [Header("Soil Stages")]
    public List<SoilStateData> soilStages = new List<SoilStateData>();
    public List<FarmingSpot> farmingSpots = new List<FarmingSpot>();
    public FarmingSpot CurrentFarmSpot 
    { 
        get
        {
            if(InteractableInstigator.ClosestInteractable.gameObject.TryGetComponent<FarmingSpot>(out FarmingSpot spot))
            {
                return spot;
            }
            else
            {
                return null;
            }
        }
    }

    [Header("Plants")]
    public List<PlantData> possiblePlants = new List<PlantData>();

    [Header("Farming Modals")]
    public ItemDisplayModal_UI farmingModal;

    private void Awake()
    {
        SetupSpots();
    }

    private void Start()
    {
        SetupPossiblePlants();
    }

    // New Code

    public void DoFarm(ItemData item)
    {
        PlantData plantToFarm = GetPlant(item);

        EventManager.Instance.QueueEvent(new UseItemGameEvent(item, CurrentFarmSpot.Name));
        CurrentFarmSpot.ChangeFarmingSpotStateData(GetSoilStateData(SoilState.Planted));
        Debug.Log($"{CurrentFarmSpot.name} - {CurrentFarmSpot.GetInstanceID()}");
        CurrentFarmSpot.InitFarm(plantToFarm);
        GameplayManager.instance.interactableInstigator.holderContainer.RemoveItem(item);
        GameplayManager.instance.modalManager.CloseModal();
    }

    public SoilStateData GetSoilStateData(SoilState state) => soilStages.Find(s => s.state == state);

    private void SetupSpots()
    {
        FarmingSpot[] farmingSpotsInScene = FindObjectsOfType<FarmingSpot>();

        foreach (FarmingSpot item in farmingSpotsInScene)
        {
            farmingSpots.Add(item);
        }
    }
    private void SetupPossiblePlants()
    {
        PlantData[] plants = Resources.LoadAll<PlantData>("Plantations");

        foreach (PlantData plant in plants)
        {
            possiblePlants.Add(plant);
        }
    }

    public PlantData GetPlant(ItemData item) => possiblePlants.Find(p => p.itemSeed.itemName == item.itemName);
}

[System.Serializable]
public struct SoilStateData
{
    public string soilStateName;
    public SoilState state;
    public Material material;
    public List<ItemData> displayData;
}

public enum SoilState
{
    Default,
    Fertilized,
    Wet,
    Planted
}