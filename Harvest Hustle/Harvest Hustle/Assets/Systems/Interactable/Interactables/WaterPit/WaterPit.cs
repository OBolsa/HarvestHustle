using System;
using System.Collections.Generic;
using UnityEngine;

public class WaterPit : ItemListener
{
    public List<ItemTransform> transformations = new List<ItemTransform>();

    protected override void Start()
    {
        base.Start();
        RegisterEffectsName(FillWater);
        //RegisterToolEffectHandler(ToolType.Regador, FillWater);
    }

    private void FillWater(ItemData item)
    {
        ItemTransform transformation = transformations.Find(t => t.from.itemName == item.itemName);

        if(transformation == null)
        {
            return;
        }

        transformation.DoTransform(GameplayManager.instance.interactableInstigator.holderContainer);
        EventManager.Instance.QueueEvent(new UseItemGameEvent(item, Name));
    }

    public override void DoInteraction()
    {
        base.DoInteraction();

        GameplayManager.instance.ChangeFreeLookCamera(FreeLookCameraType.Bottom);
        GameplayManager.instance.modalManager.OpenItemDisplayerModal(itemsToDisplay);
    }
}