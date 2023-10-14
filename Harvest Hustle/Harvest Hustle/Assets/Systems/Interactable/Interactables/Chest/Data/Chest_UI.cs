using System.Collections.Generic;
using UnityEngine;

public class Chest_UI : Modal
{
    private Container chestContainer;
    public Container PlayerInventory { get
        {
            if(_playerInventory == null)
                _playerInventory = GameplayManager.instance.playerInventory.container;

            return _playerInventory;
        }
    }
    private Container _playerInventory;

    [Header("Player Inventory")]
    public List<ChestModalSlot_UI> playerInventorySlots = new List<ChestModalSlot_UI>();
    public List<ItemData> itemsInPlayerInventory = new List<ItemData>();

    [Header("Chest")]
    public List<ChestModalSlot_UI> chestSlots = new List<ChestModalSlot_UI>();
    public List<ItemData> itemsInChest = new List<ItemData>();

    // Slots Manipulation
    public void CheckClickedSlot(ChestModalSlot_UI slotClicked, ItemData itemInSlot)
    {
        if (itemInSlot == null)
        {
            return;
        }

        bool isChestSlot = chestSlots.Find(s => s.name == slotClicked.name);

        if (isChestSlot)
        {
            AddToPlayerSlot(itemInSlot);
        }
        else
        {
            AddToChestSlot(itemInSlot);
        }

        slotClicked.RemoveItemInSlot(itemInSlot);
    }
    public void AddToChestSlot(ItemData item)
    {
        ChestModalSlot_UI slot = FindSlot(chestSlots, item);
        slot.AddItemInSlot(item);

        AddToChestContainer(item);
    }
    public void AddToPlayerSlot(ItemData item)
    {
        ChestModalSlot_UI slot = FindSlot(playerInventorySlots, item);
        slot.AddItemInSlot(item);

        RemoveFromChest(item);
    }

    // Container Manipulation
    public void AddToChestContainer(ItemData itemsToAdd)
    {
        chestContainer.AddItem(itemsToAdd);
        PlayerInventory.RemoveItem(itemsToAdd);
    }
    public void RemoveFromChest(ItemData itemsToRemove)
    {
        PlayerInventory.AddItem(itemsToRemove);
        chestContainer.RemoveItem(itemsToRemove);
    }

    // Modal Manipulation
    public override void OpenModal()
    {
        ShowModal(true);
    }
    public override void CloseModal()
    {
        ShowModal(false);
    }
    public override void ShowModal(bool active)
    {
        elements.SetActive(active);
    }

    // Setup Configs
    public void SetupModal(Container newChestContainer)
    {
        chestContainer = newChestContainer;

        // Setup Chest Slots
        chestSlots.ForEach(c => c.ResetSlot());

        itemsInChest.Clear();
        itemsInChest = chestContainer.ItemsInContainer();

        foreach(ItemData item in itemsInChest) 
        {
            ChestModalSlot_UI slot = FindSlot(chestSlots, item);
            slot.AddItemInSlot(item);
        }

        // Setup Player Slots
        playerInventorySlots.ForEach(c => c.ResetSlot());

        itemsInPlayerInventory.Clear();
        itemsInPlayerInventory = PlayerInventory.ItemsInContainer();

        foreach (ItemData item in itemsInPlayerInventory)
        {
            ChestModalSlot_UI slot = FindSlot(playerInventorySlots, item);
            slot.AddItemInSlot(item);
        }
    }

    private ChestModalSlot_UI FindSlot(List<ChestModalSlot_UI> slotList, ItemData item)
    {
        ChestModalSlot_UI slot = slotList.Find(s => s.IsSlotForItem(item));

        if(slot == null)
        {
            slot = slotList.Find(s => s.IsEmpty);
        }

        return slot;
    }
}