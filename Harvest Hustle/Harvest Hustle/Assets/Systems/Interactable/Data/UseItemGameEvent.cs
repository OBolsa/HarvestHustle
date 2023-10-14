public class UseItemGameEvent : GameEvent
{
    public ItemData UsedItem;
    public string InteractableUsedName;

    public UseItemGameEvent(ItemData usedItem, string interactableUsed)
    {
        UsedItem = usedItem;
        InteractableUsedName = interactableUsed;
    }
}