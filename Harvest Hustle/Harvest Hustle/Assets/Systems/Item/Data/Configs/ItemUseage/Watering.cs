using UnityEngine;

[CreateAssetMenu(menuName = "Item/Useage", fileName = "Watering")]
public class Watering : ScriptableObject, IUseage
{
    public void OnUse()
    {
        FarmingSpot spot = GameplayManager.instance.farmingManager.CurrentFarmSpot;

        if (spot != null && InteractableInstigator.ClosestInteractable == spot as Interactable)
            spot.ChangeMoisture(spot.moistureLevel < 80 ? 80 : 100);
    }
}