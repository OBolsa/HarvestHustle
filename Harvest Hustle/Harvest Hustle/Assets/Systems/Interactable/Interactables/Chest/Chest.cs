using System.Collections.Generic;
using UnityEngine;

public class Chest : ItemListener
{
    public Container ChestContainer;
    public Sprite ChestIcon;

    public override void DoInteraction()
    {
        base.DoInteraction();

        List<ItemData> displayItems = new List<ItemData>();

        foreach (var item in GameplayManager.instance.playerInventory.container.UniqueItemsInContainer())
        {
            if (itemsToDisplay.Contains(item))
            {
                displayItems.Add(item);
            }
        }

        GameplayManager.instance.ChangeFreeLookCamera(FreeLookCameraType.Bottom);
        GameplayManager.instance.modalManager.OpenChestModal(ChestContainer);
    }
}