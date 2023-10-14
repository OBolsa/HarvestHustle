using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemDisplayContainerSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Slot Item")]
    public ItemData slotItem;
    public bool HaveItem { get; private set; }

    [Header("Settings")]
    public Image slotIcon;
    public Sprite defaultSprite;
    public TMP_Text nameBox;

    private void OnDisable()
    {
        nameBox.gameObject.SetActive(false);
    }

    public void Setup(ItemData item, bool haveItem)
    {
        slotItem = item;
        HaveItem = haveItem;
        nameBox.text = HaveItem ? slotItem.itemName : "???";
        slotIcon.sprite = slotItem.itemIcon;
        slotIcon.color = HaveItem ? Color.white : Color.grey;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HaveItem)
        {
            // Faz sonzinho de feedback e nada acontece.
            return;
        }

        switch (slotItem.itemType)
        {
            default:
            case ItemType.Generic:
                GameplayManager.instance.itemManager.UseTool(slotItem, InteractableInstigator.ClosestInteractable);
                break;
            case ItemType.Tool:
                GameplayManager.instance.itemManager.UseTool(slotItem, InteractableInstigator.ClosestInteractable);
                break;
            case ItemType.Seed:
                GameplayManager.instance.farmingManager.DoFarm(slotItem);
                break;
            case ItemType.QuestItem:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nameBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nameBox.gameObject.SetActive(false);
    }
}