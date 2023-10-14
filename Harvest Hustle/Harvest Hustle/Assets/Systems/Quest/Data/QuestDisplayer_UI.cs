using System.Collections.Generic;
using UnityEngine;

public class QuestDisplayer_UI : MonoBehaviour
{
    public List<QuestDisplay_UI> displays = new List<QuestDisplay_UI>();

    private void OnEnable()
    {
        GameplayManager.instance.questManager.OnStartQuest += StartQuestFeedback;
        GameplayManager.instance.questManager.OnCompleteQuest += CompleteQuestFeedback;
    }

    private void OnDisable()
    {
        GameplayManager.instance.questManager.OnStartQuest -= StartQuestFeedback;
        GameplayManager.instance.questManager.OnCompleteQuest -= CompleteQuestFeedback;
    }

    private void StartQuestFeedback(Quest quest)
    {
        QuestDisplay_UI display = displays.Find(d => d.gameObject.activeSelf == false);

        display.gameObject.SetActive(true);
        display.Initialize(quest);
    }

    private void CompleteQuestFeedback(Quest quest)
    {
        QuestDisplay_UI display = displays.Find(d => d.questToDisplay == quest);

        display.gameObject.SetActive(false);
    }
}