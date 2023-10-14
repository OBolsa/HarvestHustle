public class ClimateGameEvent : GameEvent
{
    public string ClimateName;

    public ClimateGameEvent(string climateName)
    {
        ClimateName = climateName;
    }
}