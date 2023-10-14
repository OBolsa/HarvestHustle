using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DialogueTriggerCollider : MonoBehaviour
{
    public List<Triggers> triggers = new List<Triggers>();

    private void OnTriggerEnter(Collider other)
    {
        triggers.ForEach(t => t.SetDialogue());
    }

    [System.Serializable]
    public class Triggers
    {
        public NPCData NPC;
        public Dialogue Dialogue;

        public void SetDialogue() => NPC.CurrentDialogue = Dialogue;
    }
}