using System;
using System.Collections.Generic;
using UnityEngine;

public class ModalManager : MonoBehaviour
{
    public ItemDisplayModal_UI itemDisplayerModal;
    public PackageCreateModal_UI packageCreateModal;
    public Chest_UI chestModal;
    public ItemTradeModal_UI itemTradeModal;
    public WorkbenchModal_UI workbenchModal;
    public Modal CurrentModal { get; private set; }

    public Action<Modal> OnOpenModal;
    public Action<Modal> OnCloseModal;

    public void OpenItemDisplayerModal(List<ItemData> itemsToDisplay)
    {
        itemDisplayerModal.SetupSlots(itemsToDisplay);
        GameplayManager.instance.CallForModal(itemDisplayerModal);
    }

    public void OpenPackageCreateModal(List<ItemData> itemsToDisplay, Sprite icon, string modalTitle)
    {
        packageCreateModal.SetupPlayerSlots(itemsToDisplay, icon, modalTitle, true);
        GameplayManager.instance.CallForModal(packageCreateModal);
    }

    public void OpenChestModal(Container chestContainer)
    {
        chestModal.SetupModal(chestContainer);
        GameplayManager.instance.CallForModal(chestModal);
    }

    public void OpenItemTradeModal(TradeData tradeData)
    {
        itemTradeModal.SetupModal(tradeData);
        GameplayManager.instance.CallForModal(itemTradeModal);
    }

    public void OpenWorkbenchModal(ItemCraftUnlock unlocks)
    {
        workbenchModal.SetupModal(unlocks);
        GameplayManager.instance.CallForModal(workbenchModal);
    }

    public void OpenModal(Modal modal)
    {
        if (modal == null) return;

        CloseModal();
        CurrentModal = modal;
        CurrentModal.OpenModal();
        OnOpenModal?.Invoke(CurrentModal);
    }

    public void CloseModal()
    {
        CurrentModal?.CloseModal();
        OnCloseModal?.Invoke(CurrentModal);
        CurrentModal = null;
    }
}