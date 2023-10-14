using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> CurrentQuests = new List<Quest>();

    public Action<Quest> OnStartQuest;
    public Action<Quest.QuestGoal> OnUpdateQuest;
    public Action<Quest> OnCompleteQuest;

    public void StartQuest(Quest newQuest)
    {
        if (HasQuest(newQuest))
        {
            return;
        }

        newQuest.Initialize();
        CurrentQuests.Add(newQuest);
        newQuest.QuestCompleted.AddListener(OnQuestCompleted);
    }

    public void StartQuest(Quest newQuest, Action<Quest> onQuestComplete)
    {
        if (HasQuest(newQuest))
        {
            return;
        }

        newQuest.Initialize();
        CurrentQuests.Add(newQuest);
        newQuest.QuestCompleted.AddListener((Quest q) => onQuestComplete(newQuest));
        newQuest.QuestCompleted.AddListener(OnQuestCompleted);
    }

    public bool HasQuest(Quest quest)
    {
        Quest questFound = CurrentQuests.Find(q => q.Information.Title == quest.Information.Title);

        if (questFound == null)
        {
            return false;
        }

        bool haveQuest = quest != null && !questFound.Completed;
        return haveQuest;
    }

    public bool HasCompletedQuest(Quest quest)
    {
        Quest questFound = CurrentQuests.Find(q => q.Information.Title == quest.Information.Title);

        if (questFound == null)
        {
            return false;
        }

        bool haveQuest = quest != null && questFound.Completed;
        return haveQuest;
    }

    private void OnQuestCompleted(Quest quest)
    {
        OnCompleteQuest?.Invoke(quest);
    }
}