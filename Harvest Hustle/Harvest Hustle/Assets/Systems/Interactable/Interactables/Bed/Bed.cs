public class Bed : Interactable
{
    public override void DoInteraction()
    {
        base.DoInteraction();

        TimeManagerStrike.Instance.NextDay();
    }
}