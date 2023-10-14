using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ContainerSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Item")]
    public ItemData itemInSlot;
    public int amount;

    [Header("Components")]
    public Image icon;
    public TMP_Text amountText;
    public TMP_Text itemName;
    private Sprite _defaultIcon;
    public GameObject nameBox;

    private void Awake()
    {
        _defaultIcon = icon.sprite;
    }

    public void SetSlot(ContainerSlot slot)
    {
        if(slot.DefaultItem == null)
        {
            icon.sprite = _defaultIcon;
            amountText.text = "";
            itemName.text = "";
        }
        else
        {
            itemInSlot = slot.DefaultItem;
            amount = slot.Count;
            icon.sprite = slot.DefaultItem.itemIcon;
            amountText.text = slot.Count > 1 ? slot.Count.ToString() : "";
            itemName.text = slot.DefaultItem.itemName;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nameBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nameBox.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(itemName.text != "")
        {
            //GameplayManager.instance.playerInventory.container.RemoveAndDropItem(itemInSlot, amount);
            GameplayManager.instance.playerInventory.SetupSlots();
        }
    }
}