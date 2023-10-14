public class FarmGoal : Quest.QuestGoal
{
    public PlantData plantToFarm;

    public override string GetDescription()
    {
        string description;

        if (CustomDescription == string.Empty)
        {
            if (RequiredAmount > 1)
            {
                description = $"Colete {RequiredAmount} {plantToFarm.plantName}. {CurrentAmount}/{RequiredAmount}";
            }
            else
            {
                description = $"Colete {plantToFarm.plantName}.";
            }
        }
        else
        {
            description = CustomDescription;
            description = description.Replace("{0}", plantToFarm.plantName);
            description = description.Replace("{1}", CurrentAmount.ToString());
            description = description.Replace("{2}", RequiredAmount.ToString());
        }

        return description;
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<FarmGameEvent>(OnFarm);
    }

    private void OnFarm(FarmGameEvent eventInfo)
    {
        if(eventInfo.Plant.plantName == plantToFarm.plantName)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}