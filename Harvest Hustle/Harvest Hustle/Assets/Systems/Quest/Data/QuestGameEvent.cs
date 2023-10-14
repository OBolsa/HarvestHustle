public class QuestGameEvent : GameEvent
{
    public string QuestName;

    public QuestGameEvent(string questName)
    {
        QuestName = questName;
    }
}