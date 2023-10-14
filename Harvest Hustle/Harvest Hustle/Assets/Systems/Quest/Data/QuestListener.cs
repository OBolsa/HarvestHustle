using UnityEngine;
using UnityEngine.Events;

public class QuestListener : MonoBehaviour
{
    public Quest Quest;

    public UnityEvent OnQuestStart;
    public UnityEvent OnQuestComplete;

    private void Start()
    {
        GameplayManager.instance.questManager.OnStartQuest += QuestStart;
        GameplayManager.instance.questManager.OnCompleteQuest += QuestComplete;
    }

    private void QuestStart(Quest quest)
    {
        if(quest == Quest)
            OnQuestStart?.Invoke();
    }

    private void QuestComplete(Quest quest)
    {
        if(quest == Quest)
            OnQuestComplete?.Invoke();
    }
}