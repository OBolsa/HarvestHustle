using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSaver : MonoBehaviour
{
    public List<string> SceneNames = new List<string>();
    public Dictionary<string, bool> InteractableActives = new Dictionary<string, bool>();
    public List<PlantationSaveData> DataFromPlantations = new List<PlantationSaveData>();


}