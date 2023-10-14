public class ItemCollectGameEvent : GameEvent
{
    public ItemData Item;

    public ItemCollectGameEvent(ItemData item)
    {
        Item = item;
    }
}