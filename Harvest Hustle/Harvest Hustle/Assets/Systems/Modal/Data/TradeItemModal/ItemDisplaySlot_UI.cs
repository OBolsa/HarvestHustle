using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplaySlot_UI : MonoBehaviour
{
    public ItemData slotItem;
    public int amount;
    public Image slotIcon;
    public TMP_Text slotAmount;

    public void SetDisplaySlot(ItemData displayItem, int amount)
    {
        slotItem = displayItem;
        this.amount = amount;
        slotIcon.sprite = displayItem.itemIcon;
        slotIcon.color = Color.white;
        slotAmount.text = amount.ToString();
    }

    public void SetDisplaySlot(ItemData displayItem, int amount, Container playerContainer)
    {
        slotItem = displayItem;
        this.amount = amount;
        slotIcon.sprite = displayItem.itemIcon;
        slotIcon.color = Color.white;
        slotAmount.text = amount.ToString();

        bool playerHaveItem = playerContainer.HaveItem(displayItem);
        int playerAmount = playerContainer.CountItemsInContainer(slotItem);
        bool playerHaveAmount = playerHaveItem && playerAmount >= this.amount;

        slotIcon.color = playerHaveAmount ? Color.white : Color.gray;
        slotAmount.text = $"{playerAmount}/{this.amount}";
    }
}