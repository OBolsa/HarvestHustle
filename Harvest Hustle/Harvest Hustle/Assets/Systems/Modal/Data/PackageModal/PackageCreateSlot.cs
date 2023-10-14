using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PackageCreateSlot : MonoBehaviour, IPointerUpHandler
{
    [Header("Item")]
    public ItemData itemInSlot;
    public int amount;
    public string amountString { get => amount > 0 ? amount.ToString() : string.Empty; }

    public Image Icon;
    public TMP_Text ItemAmount;
    public TMP_Text ItemName;

    public bool IsEmpty { get => itemInSlot == null; }

    public void UpdateSlotAmount() => ItemAmount.text = amountString;
    public void UpdateIcon(Sprite newSprite) => Icon.sprite = newSprite;

    public void ChangeAmount(int newAmount)
    {
        amount = newAmount;

        if (amount > 0)
        {
            UpdateSlotAmount();
        }
        else
        {
            ResetSlot();
        }
    }

    public void SetupSlot(ItemData item)
    {
        if (itemInSlot != null && itemInSlot.itemName == item.itemName)
        {
            ChangeAmount(amount + 1);
            return;
        }

        itemInSlot = item;
        ItemName.text = item.itemName;
        ChangeAmount(1);
        UpdateIcon(item.itemIcon);
        UpdateSlotAmount();
    }

    public void SetupSlot(ItemData item, int amount)
    {
        SetupSlot(item);
        ChangeAmount(amount);
    }

    public void SetupSlot(List<ItemData> items)
    {
        SetupSlot(items[0]);
        ChangeAmount(items.Count);
        UpdateSlotAmount();
    }

    public void ResetSlot()
    {
        itemInSlot = null;
        amount = 0;
        UpdateIcon(GameplayManager.instance.globalConfigs.BlankIcon);
        ItemName.text = string.Empty;
        UpdateSlotAmount();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        GameplayManager.instance.modalManager.packageCreateModal.ChangeItem(this, itemInSlot);
    }
}