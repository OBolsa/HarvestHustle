using UnityEngine;

[CreateAssetMenu(fileName = "NPC")]
public class NPCData : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public Sprite Portrait;
    [TextArea(0, 5)] public string Description;

    public Dialogue CurrentDialogue 
    { 
        get
        {
            if (_currentDialogue == null)
                return DefaultDialogue;
            else return _currentDialogue;
        }
        set => _currentDialogue = value;
    }
    private Dialogue _currentDialogue;
    public Dialogue DefaultDialogue;

    public Sprite GetPortrait() => Portrait == null ? GameplayManager.instance.globalConfigs.DefaultPortrait : Portrait;
    public string GetName() => Name == "" ? "???" : Name;
}