using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable, CreateAssetMenu(menuName = "CraftUnlocks", fileName = "PlayerCraftUnlocks")]
public class ItemCraftUnlock : ScriptableObject
{
    public List<TradeData> unlockedTrades;

    public void UnlockTrade(TradeData trade)
    {
        unlockedTrades.Add(trade);
    }

    public TradeData TradeByItem(ItemData item)
    {
        TradeData trade = unlockedTrades.Find(t => t.itemsToRecieve.First().itemToRecieve.itemName == item.itemName);

        return trade;
    }
}