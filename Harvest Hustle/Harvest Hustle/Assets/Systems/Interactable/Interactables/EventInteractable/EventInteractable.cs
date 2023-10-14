using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventInteractable : ItemListener
{
    public StrikeType strikeType;
    public List<ItemData> itemsOutcomes = new List<ItemData>();
    public bool HaveOutcome { get => itemsOutcomes.Count > 0; }
    private ItemData itemUsed;

    public UnityEvent OnInteract;
    public UnityEvent OnUseItem;

    protected override void Start()
    {
        base.Start();

        RegisterEffectsName(ItemUsed);
        //foreach (var item in itemsToDisplay)
        //{
        //    RegisterToolEffectHandler(item.toolType, ItemUsed);
        //}
    }

    public override void DoInteraction()
    {
        base.DoInteraction();

        OnInteract?.Invoke();
        GameplayManager.instance.modalManager.OpenItemDisplayerModal(itemsToDisplay);
    }

    public void ItemUsed(ItemData itemData)
    {
        GameplayManager.instance.progressBar.StartProgress(strikeType, InvokeUseItemEvents);
        itemUsed = itemData;
    }

    private void InvokeUseItemEvents()
    {
        if (HaveOutcome)
        {
            GameplayManager.instance.playerInventory.container.ItemAdded += OnCollectOutcome;
            GameplayManager.instance.playerInventory.container.AddItem(itemsOutcomes);
        }
        else
        {
            OnUseItem?.Invoke();
        }
    }

    public void OnCollectOutcome(bool collected)
    {
        if (collected)
        {
            void DoStrike()
            {
                TimeManagerStrike.Instance.DoStrike(strikeType);
                GameplayManager.instance.interactableInstigator.SetClosestInteractable();
                OnUseItem?.Invoke();
                EventManager.Instance.QueueEvent(new UseItemGameEvent(itemUsed, Name));
            }

            ScreenTransition.Instance.DoTransition(DoStrike);
            GameplayManager.instance.playerInventory.container.ItemAdded -= OnCollectOutcome;
        }
        else
        {
            Debug.Log("Sem espaço na mochila");
        }
    }
}