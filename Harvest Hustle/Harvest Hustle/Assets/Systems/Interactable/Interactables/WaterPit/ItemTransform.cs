using UnityEngine;

[System.Serializable]
public class ItemTransform
{
    public ItemData from;
    public ItemData to;

    public ItemTransform(ItemData from, ItemData to)
    {
        this.from = from;
        this.to = to;
    }

    public void DoTransform(Container container)
    {
        Debug.Log($"From: {from.itemName} -> To: {to.itemName}");
        container.RemoveItem(from);
        container.AddItem(to);
    }
}