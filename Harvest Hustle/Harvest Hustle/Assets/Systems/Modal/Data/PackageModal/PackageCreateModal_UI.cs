using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackageCreateModal_UI : Modal
{
    // Package
    public List<PackageCreateSlot> PackageSlots = new List<PackageCreateSlot>();
    public List<PackageCreateSlot> FilledPackageSlots { get => PackageSlots.Where(s => !s.IsEmpty).ToList(); }
    public List<PackageCreateSlot> EmptyPackageSlots { get => PackageSlots.Where(s => s.IsEmpty).ToList(); }

    // Player
    public List<PackageCreateSlot> PlayerSlots = new List<PackageCreateSlot>();
    public List<PackageCreateSlot> FilledPlayerSlots { get => PlayerSlots.Where(s => !s.IsEmpty).ToList(); }
    public List<PackageCreateSlot> EmptyPlayerSlots { get => PlayerSlots.Where(s => s.IsEmpty).ToList(); }

    public Image MainIcon;
    public TMP_Text ModalTitle;

    public Action<List<ItemData>> ModalConfirmed;

    public override void OpenModal()
    {
        ShowModal(true);
    }

    public override void CloseModal()
    {
        ShowModal(false);
    }

    public override void ShowModal(bool active)
    {
        if(active == false)
        {
            foreach (var slot in PlayerSlots)
            {
                slot.ResetSlot();
            }
        }

        elements.SetActive(active);
    }

    public void SetupModal(Sprite icon, string title)
    {
        MainIcon.sprite = icon;
        ModalTitle.text = title;
    }

    public void CreatePackage()
    {
        if (FilledPackageSlots.Count == 0)
            return;

        List<ItemData> items = new List<ItemData>();

        foreach (var slot in FilledPackageSlots)
        {
            int amount = slot.amount;

            while (amount > 0)
            {
                items.Add(slot.itemInSlot);
                amount--;
            }
        }

        ModalConfirmed?.Invoke(items);
    }

    public void RemoveListeners()
    {
        List<ItemData> items = new List<ItemData>();
        ModalConfirmed?.Invoke(items);
    }

    public void SetupPackageSlots(List<ItemData> items)
    {
        PackageSlots.ForEach(s => s.ResetSlot());

        foreach (var item in items)
        {
            PackageSlotFor(item).SetupSlot(item);
        }
    }

    public void SetupPlayerSlots(List<ItemData> itemsToDisplay, Sprite icon, string title, bool cleanPackage)
    {
        SetupModal(icon, title);
        PlayerSlots.ForEach(s => s.ResetSlot());

        foreach (var playerItem in GameplayManager.instance.playerInventory.container.ItemsInContainer())
        {
            foreach (var item in itemsToDisplay)
            {
                if (playerItem.itemName == item.itemName)
                {
                    PlayerSlotFor(playerItem).SetupSlot(playerItem);
                }
            }
        }

        foreach (var slot in PackageSlots)
        {
            slot.ResetSlot();
        }
    }

    public List<ItemData> AdditionalItems(List<ItemData> previousItems)
    {
        List<ItemData> itemsInFilledSlots = new List<ItemData>();

        foreach (var slot in FilledPlayerSlots)
        {
            itemsInFilledSlots.Add(slot.itemInSlot);
        }

        List<ItemData> extraItems = itemsInFilledSlots.Except(previousItems).ToList();
        return extraItems;
    }

    private PackageCreateSlot PlayerSlotFor (ItemData item) => PlayerSlots.Find(s => !s.IsEmpty && s.itemInSlot.itemName == item.itemName) ?? PlayerSlots.Find(s => s.IsEmpty);
    private PackageCreateSlot PackageSlotFor (ItemData item) => PackageSlots.Find(s => !s.IsEmpty && s.itemInSlot.itemName == item.itemName) ?? PackageSlots.Find(s => s.IsEmpty);

    private void SetSlotWithItem(ItemData itemToFind, List<PackageCreateSlot> slot)
    {
        PackageCreateSlot playerSlot = slot.FirstOrDefault();

        playerSlot.SetupSlot(itemToFind);
    }

    private void SetAmountOfItems(ItemData itemToSet)
    {
        foreach (var slot in FilledPlayerSlots)
        {
            if(slot.itemInSlot.itemName == itemToSet.itemName)
            {
                slot.ChangeAmount(slot.amount + 1);
            }
        }
    }

    public void ChangeItem(PackageCreateSlot passedSlot, ItemData itemToChange)
    {
        PackageCreateSlot slot = PlayerSlots.Find(s => s == passedSlot);

        // Cliquei um PackageSlot
        if (slot == null)
        {
            slot = PackageSlots.Find(s => s == passedSlot);
            slot.ChangeAmount(slot.amount - 1);
            PackageCreateSlot playerSlot = PlayerSlots.Find(s => !s.IsEmpty && s.itemInSlot.itemName == itemToChange.itemName);

            // Nao achei um PlayerSlot que tem este item
            if (playerSlot == null)
            {
                playerSlot = PlayerSlots.Find(s => s.IsEmpty);
                playerSlot.SetupSlot(itemToChange);
            }// Achei um PlayerSlot que tem este item
            else
            {
                playerSlot.ChangeAmount(playerSlot.amount + 1);
            }
        }// Cliquei num PlayerSlot
        else
        {
            slot.ChangeAmount(slot.amount - 1);
            PackageCreateSlot packageSlot = PackageSlots.Find(s => !s.IsEmpty && s.itemInSlot.itemName == itemToChange.itemName);

            // Nao achei um PackageSlot que ja tem este item
            if(packageSlot == null)
            {
                packageSlot = PackageSlots.Find(s => s.IsEmpty);
                packageSlot.SetupSlot(itemToChange);
            }// Achei um PackageSlot que tem este item
            else
            {
                packageSlot.ChangeAmount(packageSlot.amount + 1);
            }
        }
    }
}