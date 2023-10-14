public class InteractGoal : Quest.QuestGoal
{
    public string desiredInteractionName;

    public override string GetDescription()
    {
        string description;

        if (CustomDescription == string.Empty)
        {
            if (RequiredAmount > 1)
            {
                description = $"Colete {RequiredAmount} {desiredInteractionName}. {CurrentAmount}/{RequiredAmount}";
            }
            else
            {
                description = $"Colete {desiredInteractionName}.";
            }
        }
        else
        {
            description = CustomDescription;
            description = description.Replace("{0}", desiredInteractionName);
            description = description.Replace("{1}", CurrentAmount.ToString());
            description = description.Replace("{2}", RequiredAmount.ToString());
        }

        return description;
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<InteractionGameEvent>(OnInteract);
    }

    public void OnInteract(InteractionGameEvent eventInfo)
    {
        if (eventInfo.InteractableName == desiredInteractionName)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}