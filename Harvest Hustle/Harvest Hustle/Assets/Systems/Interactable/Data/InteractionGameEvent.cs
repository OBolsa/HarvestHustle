public class InteractionGameEvent : GameEvent
{
    public string InteractableName;

    public InteractionGameEvent(string interactableName)
    {
        this.InteractableName = interactableName;
    }
}