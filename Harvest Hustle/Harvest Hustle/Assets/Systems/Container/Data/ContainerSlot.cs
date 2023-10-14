using System.Collections.Generic;

[System.Serializable]
public class ContainerSlot
{
    public List<ItemData> items = new List<ItemData>();
    public int maxStacks;
    public int Count { get { return items.Count; } }
    public int EmptySpace { get { return maxStacks - Count; } }
    public bool IsEmpty { get { return items.Count == 0; } }
    public bool IsFull { get { return items.Count >= maxStacks; } }
    public ItemData DefaultItem { get { return Count > 0 ? items[0] : null; } }

    public bool CanAddMoreItem(int amount) => Count + amount <= maxStacks;
    public bool IsSlotForItem(ItemData item)
    {
        if (DefaultItem == null || item == null)
            return false;

        return item.itemName == DefaultItem.itemName;
    }
}