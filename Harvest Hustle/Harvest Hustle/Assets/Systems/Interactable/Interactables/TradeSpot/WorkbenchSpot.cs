using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchSpot : Interactable
{
    public ItemCraftUnlock craftsUnlocked;

    public override void DoInteraction()
    {
        base.DoInteraction();

        GameplayManager.instance.ChangeFreeLookCamera(FreeLookCameraType.Bottom);
        GameplayManager.instance.modalManager.OpenWorkbenchModal(craftsUnlocked);
    }
}