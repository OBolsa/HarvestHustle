using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeSpot : Interactable
{
    public TradeData TradeData;

    public override void DoInteraction()
    {
        base.DoInteraction();

        GameplayManager.instance.ChangeFreeLookCamera(FreeLookCameraType.Bottom);
        GameplayManager.instance.modalManager.OpenItemTradeModal(TradeData);
    }
}