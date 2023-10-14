using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkbenchModal_UI : Modal
{
    public ItemCraftUnlock craftsUnlocked;

    public List<WorkbenchSlot_UI> slots = new List<WorkbenchSlot_UI>();

    public void SetupModal(ItemCraftUnlock unlocks)
    {
        craftsUnlocked = unlocks;

        List<ItemData> itemsToDisplay = new List<ItemData>();

        foreach(var trade in craftsUnlocked.unlockedTrades)
        {
            itemsToDisplay.Add(trade.itemsToRecieve.First().itemToRecieve);
        }

        slots.ForEach(s => s.ResetSlot());

        foreach(var item in itemsToDisplay) 
        { 
           slots.Find(s => s.IsEmpty).SetDisplaySlot(item);
        }
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