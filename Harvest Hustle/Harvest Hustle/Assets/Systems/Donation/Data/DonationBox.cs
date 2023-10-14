using System.Collections.Generic;
using UnityEngine;

public class DonationBox : ItemListener
{
    public ItemData packageDefault;

    public override void DoInteraction()
    {
        base.DoInteraction();

        List<ItemData> packages = new List<ItemData>();
        packages = GameplayManager.instance.playerInventory.container.FindItems(packageDefault.toolType);

        itemsToDisplay.Clear();
        itemsToDisplay = packages;
        RegisterEffectsName(DoDonation);

        GameplayManager.instance.modalManager.OpenItemDisplayerModal(itemsToDisplay);
    }

    public void DoDonation(ItemData item)
    {
        Debug.Log(item.SecretID);
        GameplayManager.instance.donationManager.ComputePackage(item.SecretID);
        GameplayManager.instance.playerInventory.container.RemoveItem(item);
    }
}