using UnityEngine;

public class CollectableWithCooldown : Interactable
{
    public ItemData itemToDelivery;
    public Vector2Int collectionAmount;
    private int CollectionAmount { get => Random.Range(collectionAmount.x, collectionAmount.y); }

    public Vector2Int nextCollectInHours;
    private int NextCollectInHours { get => Random.Range(nextCollectInHours.x, nextCollectInHours.y); }
    private int nextCollect;
    private bool isInCooldown = false;

    public override void DoInteraction()
    {
        base.DoInteraction();

        if (isInCooldown)
        {
            Debug.Log("Ainda não tem bananas nesta planta");
            return;
        }

        GameplayManager.instance.playerInventory.container.ItemAdded += OnCollectItem;
        GameplayManager.instance.playerInventory.container.AddItem(itemToDelivery, CollectionAmount);
    }

    public void OnCollectItem(bool collected)
    {
        if(collected)
        {
            StartCooldown();
        }
        else
        {
            Debug.Log("Sem espaço na mochila");
        }
    }

    private void StartCooldown()
    {
        isInCooldown = true;
        nextCollect = TimeManagerStrike.Instance.CurrentStrikeCountTotal + NextCollectInHours;
        TimeManagerStrike.Instance.StrikePassed += CheckCooldown;
    }

    private void CheckCooldown()
    {
        if(TimeManagerStrike.Instance.CurrentStrikeCountTotal >= nextCollect)
        {
            isInCooldown = false;
            TimeManagerStrike.Instance.StrikePassed -= CheckCooldown;
        }
    }
}