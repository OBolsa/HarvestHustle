using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingSpotR : ItemListener
{
    public int MoistureLevel
    {
        get => _moistureLevel;
        set => _moistureLevel = Mathf.Clamp(value, 0, 100);
    }
    private int _moistureLevel;

    public int FertilizationLevel
    {
        get => _fertilizationLevel;
        set => Mathf.Clamp(value, 0, 100);
    }
    private int _fertilizationLevel;

    public override void DoInteraction()
    {
        base.DoInteraction();
    }
}