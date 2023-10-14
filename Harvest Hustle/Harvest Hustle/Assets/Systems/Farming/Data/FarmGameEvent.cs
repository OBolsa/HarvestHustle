public class FarmGameEvent : GameEvent
{
    public PlantData Plant;

    public FarmGameEvent(PlantData plant)
    {
        Plant = plant;
    }
}