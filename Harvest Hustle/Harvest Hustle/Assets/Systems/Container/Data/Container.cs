using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Container", fileName = "Container_")]
public class Container : ScriptableObject
{
    public bool IsPlayerContainer;
    public List<ContainerSlot> slots = new List<ContainerSlot>();
    public int ContainerSize { get { return slots.Count; } }
    public int UsedSlots { get { return slots.FindAll(c => !c.IsEmpty).Count; } }
    public int EmptySlots { get { return slots.FindAll(c => c.IsEmpty).Count; } }

    public Action<bool> ItemAdded;

    //private void OnDisable()
    //{
    //    ClearInventory();
    //}

    [ContextMenu("Clear Inventory")]
    void ClearInventory()
    {
        if (!slots[0].IsEmpty) slots.Clear();
        slots = new List<ContainerSlot>(new ContainerSlot[9]);
    }

    #region ContainerCheckage

    #region HaveItem
    public bool HaveItem(ItemData item)
    {
        ContainerSlot slot = FindItemSlot(item);
        if(slot == null) return false;

        bool haveItem = !slot.IsEmpty;

        if (!haveItem) { Debug.Log($"You don't have <{item.itemName}>."); }

        return haveItem;
    }

    public bool HaveItem(ItemData item, int amount)
    {
        int amountFounded = 0;

        foreach (var slot in slots)
        {
            if (slot.IsSlotForItem(item))
            {
                amountFounded += slot.Count;
            }
        }

        bool haveItem = amountFounded >= amount;

        if (!haveItem) { Debug.Log($"You don't have <{item.itemName}> enough. \n- You need: {amount}\n- You have: {amountFounded}"); }

        return haveItem;
    }

    public bool HaveItem(List<ItemData> itemList)
    {
        Dictionary<ItemData, List<ItemData>> splitedItemList = SplitItems(itemList);
        int amountToFound = 0;
        int amountFounded = 0;

        foreach (KeyValuePair<ItemData,List<ItemData>> pair in splitedItemList)
        {
            amountToFound += pair.Value.Count;

            ContainerSlot slot = FindItemSlot(pair.Key);

            amountFounded += slot == null ? 0 : slot.Count;
        }

        bool haveItem = amountFounded == amountToFound;

        if (!haveItem)
        {
            Debug.Log($"You don't have the items required.\n- You need: {amountToFound}\n- You Have: {amountFounded}");
        }

        return haveItem;
    }
    #endregion

    #region HaveSpaceForItem
    public bool HaveSpaceForItem(ItemData item)
    {
        ContainerSlot slot = FindItemSlotWithSpace(item, 1);

        slot ??= FindEmptySlot();

        if (slot == null)
        {
            Debug.Log($"You're out of space.");
            return false;
        }

        return true;
    }

    public bool HaveSpaceForItem(ItemData item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            ContainerSlot slot = FindItemSlotWithSpace(item, 1);
            slot ??= FindEmptySlot();

            if (slot == null)
            {
                Debug.Log($"You're out of space.");
                return false;
            }
        }

        return true;
    }

    public bool HaveSpaceForItem(List<ItemData> itemList)
    {
        Dictionary<ItemData, List<ItemData>> listedItems = SplitItems(itemList);

        foreach(KeyValuePair<ItemData, List<ItemData>> pair in listedItems)
        {
            ContainerSlot slot = FindItemSlotWithSpace(pair.Key, 1);
            slot ??= FindEmptySlot();

            if(slot == null)
            {
                Debug.Log($"You're out of space.");
                return false;
            }
        }

        return true;
    }
    #endregion

    #endregion

    #region ContainerManipulation

    #region AddItem
    public void AddItem(ItemData item)
    {
        if (!HaveSpaceForItem(item))
        {
            Debug.Log("Don't have space for the item");
            ItemAdded?.Invoke(false);
            //GameplayManager.instance.itemManager.DropItem(item);
            return;
        }

        ContainerSlot slot = FindItemSlotWithSpace(item, 1);
        slot ??= FindEmptySlot();

        if (slot.IsEmpty)
            slot.maxStacks = item.maxStack;

        if (slot.CanAddMoreItem(1))
        {
            slot.items.Add(item);
            EventManager.Instance.QueueEvent(new ItemCollectGameEvent(item));
        }
        ItemAdded?.Invoke(true);
    }

    public void AddItem(ItemData item, int amount)
    {
        if (!HaveSpaceForItem(item, amount))
        {
            for (int i = 0; i < amount; i++)
            {
                Debug.Log("Don't have space for the item");
                ItemAdded?.Invoke(false);
                //GameplayManager.instance.itemManager.DropItem(item);
            }
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            ContainerSlot slot = FindItemSlotWithSpace(item, 1);
            slot ??= FindEmptySlot();

            if (slot.IsEmpty) slot.maxStacks = item.maxStack;

            slot.items.Add(item);
            EventManager.Instance.QueueEvent(new ItemCollectGameEvent(item));
        }
        ItemAdded?.Invoke(true);
    }

    public void AddItem(List<ItemData> itemList)
    {
        if (!HaveSpaceForItem(itemList))
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Debug.Log("Don't have space for the item");
                ItemAdded?.Invoke(false);
                //GameplayManager.instance.itemManager.DropItem(itemList[i]);
            }
            return;
        }

        Dictionary<ItemData, List<ItemData>> splitedItemList = SplitItems(itemList);

        foreach(KeyValuePair<ItemData, List<ItemData>> pair in splitedItemList )
        {
            ContainerSlot slot = FindItemSlot(pair.Key);
            slot ??= FindEmptySlot();

            if (slot.IsEmpty)
                slot.maxStacks = pair.Key.maxStack;

            foreach (ItemData item in pair.Value)
            {
                slot.items.Add(item);
                EventManager.Instance.QueueEvent(new ItemCollectGameEvent(item));
            }
        }

        ItemAdded?.Invoke(true);
    }
    #endregion

    #region RemoveAndDrop
    public void RemoveAndDropItem(ItemData item)
    {
        if (!HaveItem(item)) return;

        ContainerSlot slot = FindItemSlot(item);

        slot.items.Remove(item);
        //GameplayManager.instance.itemManager.DropItem(item);
    }

    public void RemoveAndDropItem(ItemData item, int amount)
    {
        if (!HaveItem(item, amount)) return;

        for (int i = 0; i < amount; i++)
        {
            ContainerSlot slot = FindItemSlot(item);

            slot.items.Remove(item);
            //GameplayManager.instance.itemManager.DropItem(item);
        }
    }

    public void RemoveAndDropItem(List<ItemData> itemList)
    {
        if (!HaveItem(itemList)) return;

        Dictionary<ItemData, List<ItemData>> splitedItemList = SplitItems(itemList);

        foreach (KeyValuePair<ItemData, List<ItemData>> pair in splitedItemList)
        {
            ContainerSlot slot = FindItemSlot(pair.Key);

            foreach (ItemData item in pair.Value)
            {
                slot.items.Remove(item);
                //GameplayManager.instance.itemManager.DropItem(item);
            }
        }
    }
    #endregion

    #region RemoveItem
    public void RemoveItem(ItemData item)
    {
        if (!HaveItem(item)) return;

        ContainerSlot slot = FindItemSlot(item);

        slot.items.Remove(item);
    }

    public void RemoveItem(ItemData item, int amount)
    {
        if(!HaveItem(item, amount)) return;

        for (int i = 0; i < amount; i++)
        {
            ContainerSlot slot = FindItemSlot(item);

            slot.items.Remove(item);
        }
    }

    public void RemoveItem(List<ItemData> itemList)
    {
        foreach (var item in itemList)
        {
            RemoveItem(item, 1); // Call the existing RemoveItemInSlot method for each item
        }
    }
    #endregion

    #endregion

    #region Utilities
    public ContainerSlot FindItemSlot(ItemData item)
    {
        ContainerSlot itemSlot = slots.Find(c => c.IsSlotForItem(item));
        return itemSlot;
    }

    public ContainerSlot FindItemSlotWithSpace(ItemData item, int amount)
    {
        var itemSlot = slots.Find(c => c.IsSlotForItem(item) && c.Count +  amount <= c.maxStacks);
        return itemSlot;
    }

    public ContainerSlot FindEmptySlot()
    {
        var emptySlot = slots.Find(c => c.Count == 0);
        return emptySlot;
    }

    private Dictionary<ItemData, List<ItemData>> SplitItems(List<ItemData> itemList)
    {
        Dictionary<ItemData, List<ItemData>> itemDictionary = new Dictionary<ItemData, List<ItemData>>();

        // Iterate through the Item list
        foreach (ItemData item in itemList)
        {
            // Check if the Item already exists in the dictionary
            if (itemDictionary.ContainsKey(item))
            {
                // If it exists, add the Item to its corresponding list
                itemDictionary[item].Add(item);
            }
            else
            {
                // If it doesn't exist, create a new list and add the Item
                List<ItemData> newList = new List<ItemData> { item };
                itemDictionary.Add(item, newList);
            }
        }

        return itemDictionary;
    }

    public List<ItemData> FindItems(string itemName)
    {
        var list = new List<ItemData>();

        foreach (var slot in slots)
        {
            if (slot.IsEmpty || slot.DefaultItem.itemName != itemName)
            {
                continue;
            }

            foreach (var item in slot.items)
            {
                list.Add(item);
            }
        }

        return list;
    }

    public List<ItemData> FindItems(ToolType toolType)
    {
        var list = new List<ItemData>();

        foreach (var slot in slots)
        {
            if (slot.IsEmpty || slot.DefaultItem.toolType != toolType)
            {
                continue;
            }

            foreach (var item in slot.items)
            {
                list.Add(item);
            }
        }

        return list;
    }

    public List<ItemData> ItemsInContainer()
    {
        var list = new List<ItemData>();

        foreach (var slot in slots)
        {
            if (slot.IsEmpty) continue;

            foreach (var item in slot.items)
            {
                list.Add(item);
            }
        }

        return list;
    }

    public List<ItemData> UniqueItemsInContainer()
    {
        HashSet<ItemData> unique = ItemsInContainer().ToHashSet();

        return unique.ToList();
    }

    public int CountItemsInContainer(ItemData item)
    {
        int itemCount = 0;

        foreach (var slot in slots)
        {
            if (slot.IsSlotForItem(item))
            {
                itemCount += slot.Count;
            }
        }

        return itemCount;
    }
    #endregion
}