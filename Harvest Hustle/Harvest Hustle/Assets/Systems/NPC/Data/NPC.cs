using UnityEngine;

public class NPC : Interactable
{
    [Header("Data")]
    public NPCData data;

    protected override void Start()
    {
        base.Start();
        data.CurrentDialogue = data.DefaultDialogue;
    }

    public override void DoInteraction()
    {
        base.DoInteraction();

        data.CurrentDialogue.Initialize();
    }
}