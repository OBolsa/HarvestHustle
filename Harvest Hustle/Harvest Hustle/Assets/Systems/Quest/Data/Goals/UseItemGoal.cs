public class UseItemGoal : Quest.QuestGoal
{
    public ItemData itemToUse;
    public string desiredInteractionName;

    public override string GetDescription()
    {
        string description;

        if (CustomDescription == string.Empty)
        {
            if (RequiredAmount > 1)
            {
                description = $"Colete {RequiredAmount} {itemToUse.itemName}. {CurrentAmount}/{RequiredAmount}";
            }
            else
            {
                description = $"Colete {itemToUse.itemName}.";
            }
        }
        else
        {
            description = CustomDescription;
            description = description.Replace("{0}", itemToUse.itemName);
            description = description.Replace("{1}", CurrentAmount.ToString());
            description = description.Replace("{2}", RequiredAmount.ToString());
            description = description.Replace("{3}", desiredInteractionName);
        }

        return description;
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<UseItemGameEvent>(OnFarm);
    }

    private void OnFarm(UseItemGameEvent eventInfo)
    {
        if (eventInfo.UsedItem.itemName == itemToUse.itemName && eventInfo.InteractableUsedName == desiredInteractionName)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}