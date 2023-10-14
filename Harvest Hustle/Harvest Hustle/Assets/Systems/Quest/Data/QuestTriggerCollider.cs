using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestTriggerCollider : MonoBehaviour
{
    public Quest questToTrigger;

    private void OnTriggerEnter(Collider other)
    {
        GameplayManager.instance.questManager.StartQuest(questToTrigger);
    }
}