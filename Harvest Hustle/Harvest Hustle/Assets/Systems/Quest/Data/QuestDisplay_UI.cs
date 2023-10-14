using TMPro;
using UnityEngine;

public class QuestDisplay_UI : MonoBehaviour
{
    public Quest questToDisplay;

    public TMP_Text TitleDisplay;
    public TMP_Text GoalsDisplay;

    private void OnDisable()
    {
        questToDisplay = null;
        TitleDisplay.text = string.Empty;
        GoalsDisplay.text = string.Empty;
        GameplayManager.instance.questManager.OnUpdateQuest -= UpdateGoal;
    }

    public void Initialize(Quest quest)
    {
        questToDisplay = quest;

        DoDisplay();
        GameplayManager.instance.questManager.OnUpdateQuest += UpdateGoal;
    }

    public void DoDisplay()
    {
        TitleDisplay.text = questToDisplay.Information.Title;
        GoalsDisplay.text = string.Empty;
        for (int i = 0; i < questToDisplay.Goals.Count; i++)
        {
            GoalsDisplay.text += questToDisplay.Goals[i].GetDescription() + "\n\n";
        }
    }

    public void UpdateGoal(Quest.QuestGoal eventGoal)
    {
        Quest.QuestGoal goal = questToDisplay.Goals.Find(g => g.Equals(eventGoal));

        if (goal == null)
        {
            return;
        }

        DoDisplay();
    }
}