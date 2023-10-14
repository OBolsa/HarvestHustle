public class CollectGoal : Quest.QuestGoal
{
    public ItemData collectName;

    public override string GetDescription()
    {
        string description;

        if (CustomDescription == string.Empty)
        {
            if (RequiredAmount > 1)
            {
                description = $"Colete {RequiredAmount} {collectName.itemName}. {CurrentAmount}/{RequiredAmount}";
            }
            else
            {
                description = $"Colete {collectName.itemName}.";
            }
        }
        else
        {
            description = CustomDescription;
            description = description.Replace("{0}", collectName.itemName);
            description = description.Replace("{1}", CurrentAmount.ToString());
            description = description.Replace("{2}", RequiredAmount.ToString());
        }

        return description;
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<ItemCollectGameEvent>(OnCollect);
    }

    public void OnCollect(ItemCollectGameEvent eventInfo)
    {
        if (eventInfo.Item.itemName == collectName.itemName)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}