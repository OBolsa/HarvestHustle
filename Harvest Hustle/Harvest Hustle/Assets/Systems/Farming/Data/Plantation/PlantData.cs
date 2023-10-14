using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(menuName = "Plant/New Plant", fileName = "Plant_")]
public class PlantData : ScriptableObject
{
    [Header("Settings")]
    public string plantName;
    public int timeToGrow;
    [Range(0, 3)] public int fertilizationDrain;
    public IrrigationType irrigation;
    public FertilizationType fertilization;
    public CareType care;

    [Header("States")]
    public List<PlantState> states = new List<PlantState>();

    [Header("Items related")]
    public ItemData itemSeed;
    public ItemData itemOutcome;
    public ItemData pruneItem;
    public int pruneItemAmount;

    [Header("Collect Infos")]
    public List<PlantCollectData> collectData;
}

[System.Serializable]
public struct PlantState
{
    public string stateName;
    public GameObject stateModel;
    public Mesh ModelMesh { get { return stateModel.GetComponent<MeshFilter>().sharedMesh; } }
    public Material[] ModelMaterials { get { return stateModel.GetComponent<MeshRenderer>().sharedMaterials; } }
}

public enum IrrigationType
{
    Single,
    Double
}

public enum FertilizationType
{
    Simple, // Needs between 25-49 Fertilization Level (0)
    Moderate, // Needs between 50-74 Fertilization Level (1)
    Complete // Needs between 75-100 Fertilization Level (2)
}

public enum CareType
{
    Null, // Don't Check Integrity -- Integrity Range = 0~3;
    Simple, // Degradarion needs to be at least 1 -- Integrity Range = 1~3;
    Moderate, // Integrity needs to be at least 2 -- Integrity Range = 2~3;
    Complex // Degradations needs to be at 3 -- Integrity Range = 3;
}