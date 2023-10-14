using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestModalSlot_UI : MonoBehaviour, IPointerClickHandler
{
    [Header("Info")]
    public ItemData itemInSlot;
    public int amount;
    public string amountString { get => amount.ToString(); }

    public Image slotIcon;
    public TMP_Text slotName;
    public TMP_Text slotAmount;

    public bool IsEmpty { get => itemInSlot == null; }

    public bool IsSlotForItem(ItemData item) => itemInSlot != null && itemInSlot.itemName == item.itemName;
    private void UpdateAmount(int newAmount)
    {
        amount = newAmount;
        slotAmount.text = amountString;

        if(amount <= 0)
        {
            ResetSlot();
        }
    }

    public void SetupSlot(ItemData item)
    {
        itemInSlot = item;
        UpdateAmount(1);
        slotIcon.sprite = item.itemIcon;
        slotName.text = item.itemName;
    }
    public void ResetSlot()
    {
        itemInSlot = null;
        slotIcon.sprite = GameplayManager.instance.globalConfigs.BlankIcon;
        slotName.text = string.Empty;
    }

    public void AddItemInSlot(ItemData item)
    {
        if (IsSlotForItem(item))
        {
            UpdateAmount(amount + 1);
        }
        else
        {
            SetupSlot(item);
        }
    }
    public void RemoveItemInSlot(ItemData item)
    {
        if(!IsSlotForItem(item))
        {
            Debug.Log("Estou tentando tirar um item de um slot que não tem este item.");
            return;
        }
        else
        {
            UpdateAmount(amount - 1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameplayManager.instance.modalManager.chestModal.CheckClickedSlot(this, itemInSlot);
    }
}