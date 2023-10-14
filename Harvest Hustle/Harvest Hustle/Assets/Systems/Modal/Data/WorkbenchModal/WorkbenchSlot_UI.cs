using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkbenchSlot_UI : MonoBehaviour, IPointerUpHandler
{
    public ItemData slotItem;
    public Image slotIcon;

    public bool IsEmpty { get => slotItem == null; }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsEmpty)
            return;

        GameplayManager.instance.ChangeFreeLookCamera(FreeLookCameraType.Bottom);
        GameplayManager.instance.modalManager.OpenItemTradeModal(GameplayManager.instance.modalManager.workbenchModal.craftsUnlocked.TradeByItem(slotItem));
    }

    public void ResetSlot()
    {
        slotItem = null;
        slotIcon.sprite = GameplayManager.instance.globalConfigs.BlankIcon;
    }

    public void SetDisplaySlot(ItemData displayItem)
    {
        slotItem = displayItem;
        slotIcon.sprite = displayItem.itemIcon;
    }
}