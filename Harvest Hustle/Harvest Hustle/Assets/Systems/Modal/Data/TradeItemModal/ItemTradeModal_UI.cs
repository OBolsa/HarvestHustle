using System.Collections.Generic;

public class ItemTradeModal_UI : Modal
{
    public List<ItemDisplaySlot_UI> giveItemsDispaly = new List<ItemDisplaySlot_UI>();
    public List<ItemDisplaySlot_UI> recieveItemsDispaly = new List<ItemDisplaySlot_UI>();

    private TradeData currentData;

    public Container PlayerInventory
    {
        get
        {
            if (_playerInventory == null)
                _playerInventory = GameplayManager.instance.playerInventory.container;

            return _playerInventory;
        }
    }
    private Container _playerInventory;
    public bool PlayerHaveItems
    {
        get
        {
            foreach (var item in currentData.ItemsToGive())
            {
                if (!PlayerInventory.HaveItem(item))
                {
                    return false;
                }

                if(PlayerInventory.CountItemsInContainer(item) < currentData.ItemsCount(item, currentData.ItemsToRecieve())) 
                {
                    return false;
                }
            }

            return true;
        }
    }

    public void SetupModal(TradeData tradeData)
    {
        currentData = tradeData;

        // Clear the display slots
        giveItemsDispaly.ForEach(i => i.gameObject.SetActive(false));
        recieveItemsDispaly.ForEach(i => i.gameObject.SetActive(false));

        // Setup giveItemsDispaly
        Dictionary<ItemData, int> itemCountsToGive = CountItems(tradeData.ItemsToGive());
        int slotIndex = 0;

        foreach (var item in itemCountsToGive.Keys)
        {
            giveItemsDispaly[slotIndex].gameObject.SetActive(true);
            giveItemsDispaly[slotIndex].SetDisplaySlot(item, itemCountsToGive[item], PlayerInventory);
            slotIndex++;
        }

        // Setup recieveItemsDisplay
        Dictionary<ItemData, int> itemCountsToRecieve = CountItems(tradeData.ItemsToRecieve());
        slotIndex = 0;

        foreach(var item in itemCountsToRecieve.Keys)
        {
            recieveItemsDispaly[slotIndex].gameObject.SetActive(true);
            recieveItemsDispaly[slotIndex].SetDisplaySlot(item, itemCountsToRecieve[item]);
            slotIndex++;
        }
    }
    private Dictionary<ItemData, int> CountItems(List<ItemData> items)
    {
        Dictionary<ItemData, int> itemCounts = new Dictionary<ItemData, int>();
        Dictionary<string, int> itemCountsString = new Dictionary<string, int>();

        foreach (var item in items)
        {
            // Use both the item object and its itemName as keys
            if (itemCountsString.ContainsKey(item.itemName))
            {
                itemCounts[item]++;
                itemCountsString[item.itemName]++;
            }
            else
            {
                itemCounts[item] = 1;
                itemCountsString[item.itemName] = 1;
            }
        }

        return itemCounts;
    }

    public void TradeItem()
    {
        if (!PlayerHaveItems)
        {
            return;
        }

        PlayerInventory.RemoveItem(currentData.ItemsToGive());
        PlayerInventory.AddItem(currentData.ItemsToRecieve());

        GameplayManager.instance.modalManager.CloseModal();
    }

    public override void CloseModal()
    {
        ShowModal(false);
    }
    public override void OpenModal()
    {
        ShowModal(true);
    }
    public override void ShowModal(bool active)
    {
        elements.SetActive(active);
    }
}
