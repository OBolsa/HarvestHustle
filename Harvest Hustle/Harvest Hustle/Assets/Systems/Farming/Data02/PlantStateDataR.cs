using UnityEngine;

[CreateAssetMenu(menuName = "Plant Data/State", fileName = "PlantState_")]
public class PlantStateDataR : ScriptableObject
{
    public string StateDisplayName;
    public GrowthState SrowthState;
    public GameObject StateModel;
    public Mesh ModelMesh { get { return StateModel.GetComponent<MeshFilter>().sharedMesh; } }
    public Material[] ModelMaterials { get { return StateModel.GetComponent<MeshRenderer>().sharedMaterials; } }
}