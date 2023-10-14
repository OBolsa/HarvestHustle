using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class CollectableSpot : ItemListener
{
    public StrikeType strikeType;
    public GameObject elements;
    public List<ItemData> itemToGive = new List<ItemData>();
    public bool IsActive { get; private set; }
    private ItemData itemUsed;

    protected override void Start()
    {
        base.Start();
        IsActive = true;
        elements.SetActive(IsActive); // Preciso salvar isso no inicio da cena, pra nao retornar os matinhos

        RegisterEffectsName(StartCollect);
    }

    public override void DoInteraction()
    {
        base.DoInteraction();

        GameplayManager.instance.modalManager.OpenItemDisplayerModal(itemsToDisplay);
    }

    private void StartCollect(ItemData itemData)
    {
        itemUsed = itemData;
        GameplayManager.instance.progressBar.StartProgress(strikeType, Collect);
    }

    private void Collect()
    {
        GameplayManager.instance.playerInventory.container.ItemAdded += OnCollectItem;
        GameplayManager.instance.playerInventory.container.AddItem(itemToGive);
    }

    public void OnCollectItem(bool collected)
    {
        if (collected)
        {
            void DoStrike()
            {
                IsActive = false;
                //TimeManager.Instance.DoStrike(strikeType);
                TimeManagerStrike.Instance.DoStrike(strikeType);
                gameObject.SetActive(IsActive);
                GameplayManager.instance.interactableInstigator.SetClosestInteractable();
                EventManager.Instance.QueueEvent(new UseItemGameEvent(itemUsed, Name));
            }

            ScreenTransition.Instance.DoTransition(DoStrike);
            GameplayManager.instance.playerInventory.container.ItemAdded -= OnCollectItem;
        }
        else
        {
            Debug.Log("Sem espaço na mochila");
        }
    }
}