using System.Collections.Generic;
using System.Linq;

public class ItemDisplayModal_UI : Modal
{
    public List<ItemDisplayContainerSlot_UI> slots = new List<ItemDisplayContainerSlot_UI>();
    private List<ItemData> items = new List<ItemData>();

    private void Awake()
    {
        slots.Clear();
        List<ItemDisplayContainerSlot_UI> childrenSlots = GetComponentsInChildren<ItemDisplayContainerSlot_UI>(true).ToList();
        childrenSlots.ForEach(x => slots.Add(x));
    }

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
        elements.SetActive(active);
    }

    public void SetupSlots(List<ItemData> requiredItemList)
    {
        slots.ForEach(s => s.gameObject.SetActive(false));

        for (int i = 0; i < requiredItemList.Count; i++)
        {
            slots[i].Setup(requiredItemList[i], GameplayManager.instance.interactableInstigator.holderContainer.HaveItem(requiredItemList[i]));
            slots[i].gameObject.SetActive(true);
        }
    }
}