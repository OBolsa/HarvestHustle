using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(menuName = "TradeData", fileName = "Trade_")]
public class TradeData : ScriptableObject
{
    public List<RecieveItems> itemsToRecieve = new List<RecieveItems>();
    public List<GiveItems> itemsToGive = new List<GiveItems>();

    public List<ItemData> ItemsToGive()
    {
        List<ItemData> items = new List<ItemData>();

        foreach (var data in itemsToGive)
        {
            int amount = data.amount;

            while(amount > 0)
            {
                items.Add(data.itemToGive);
                amount--;
            }
        }

        return items;
    }
    public List<ItemData> ItemsToRecieve()
    {
        List<ItemData> items = new List<ItemData>();

        foreach (var data in itemsToRecieve)
        {
            int amount = data.amount;

            while (amount > 0)
            {
                items.Add(data.itemToRecieve);
                amount--;
            }
        }

        return items;
    }

    public int ItemsCount(ItemData itemToCheck, List<ItemData> list)
    {
        int count = 0;

        foreach (var item in list)
        {
            if(item.itemName == itemToCheck.itemName)
                count++;
        }

        return count;   
    }

    [System.Serializable]
    public class GiveItems
    {
        public ItemData itemToGive;
        public int amount;
    }

    [System.Serializable]
    public class RecieveItems
    {
        public ItemData itemToRecieve;
        public int amount;
    }
}