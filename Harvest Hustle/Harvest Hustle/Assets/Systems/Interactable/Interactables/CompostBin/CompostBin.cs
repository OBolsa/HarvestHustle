using UnityEngine;

public class CompostBin : ItemListener
{
    public ItemData fertilizer;
    public CompostBinTooltip_UI feedbacks;

    public int BrownSubstractAmount 
    {
        get => _brownSubstractAmount;
        set => _brownSubstractAmount = Mathf.Clamp(value, 0, GameplayManager.instance.globalConfigs.BrownSubstractToMakeFertilization_Amount);
    }
    public int GreenSubstractAmount 
    {
        get => _greenSubstractAmount;
        set => _greenSubstractAmount = Mathf.Clamp(value, 0, GameplayManager.instance.globalConfigs.GreenSubstractToMakeFertilization_Amount);
    }
    [Header("Substract Count")]
    public int _brownSubstractAmount;
    public int _greenSubstractAmount;

    private int startProcessHour;
    private int endProcessHour;
    public bool IsProcessing { get; set; }
    public bool CanCollectFertilization { get; set; }
    public bool IsEmpty { get => GreenSubstractAmount == 0 &&  BrownSubstractAmount == 0; }

    protected override void Start()
    {
        base.Start();
        RegisterEffectsName(AddSubstract);
        //RegisterToolEffectHandler(ToolType.Substrato, AddSubstract);
    }

    public override void DoInteraction()
    {
        base.DoInteraction();

        if (!IsProcessing)
        {
            // Isn't processing, so can do something.
            feedbacks.DisplayFeedback(!IsEmpty);

            if(CanCollectFertilization)
            {
                // Can collect fertilizer, so give the items to the player
                GameplayManager.instance.playerInventory.container.ItemAdded += OnCollectItem;
                GameplayManager.instance.playerInventory.container.AddItem(fertilizer, GameplayManager.instance.globalConfigs.FertilizationProduction_Amount);

                return;
            }

            GameplayManager.instance.ChangeFreeLookCamera(FreeLookCameraType.Top);
            GameplayManager.instance.modalManager.OpenItemDisplayerModal(itemsToDisplay);
        }

        feedbacks.DisplayFeedback(false);
    }

    public void OnCollectItem(bool collected) 
    {
        if (collected)
        {
            Debug.Log($"Produto - {fertilizer.itemName} - Coletado");

            // Change State to start.
            BrownSubstractAmount = 0;
            GreenSubstractAmount = 0;
            IsProcessing = false;
            CanCollectFertilization = false;
        }
        else
        {
            Debug.Log("Sem espaço na mochila");
        }
    }

    public void AddSubstract(ItemData item)
    {
        int index = 0;
        for (int i = 0; i < itemsToDisplay.Count; i++)
        {
            if (itemsToDisplay[i].itemName == item.itemName)
            {
                index = i; break;
            }
        }

        int itemAmount = Mathf.Clamp(GameplayManager.instance.playerInventory.container.FindItemSlot(itemsToDisplay[index]).Count, 0, GameplayManager.instance.globalConfigs.BrownSubstractToMakeFertilization_Amount);

        switch (index)
        {
            case 0:
                GreenSubstractAmount += itemAmount;
                break;
            case 1:
                BrownSubstractAmount += itemAmount;
                break;
        }

        GameplayManager.instance.playerInventory.container.RemoveItem(itemsToDisplay[index], itemAmount);

        while(itemAmount > 0)
        {
            EventManager.Instance.QueueEvent(new UseItemGameEvent(item, Name));
            itemAmount--;
        }

        feedbacks.DisplayFeedback(!IsEmpty);
        feedbacks.UpdateValues(BrownSubstractAmount, GreenSubstractAmount);
        CheckSubstracts();
    }

    public void CheckSubstracts()
    {
        if (HaveSubstracts())
        {
            // Change State
            IsProcessing = true;
            startProcessHour = TimeManagerStrike.Instance.CurrentStrikeCountTotal;
            endProcessHour = startProcessHour + GameplayManager.instance.globalConfigs.HoursToProduceFertilization_Amount;
            TimeManagerStrike.Instance.StrikePassed += CheckTime;
            feedbacks.DisplayFeedback(false);
        }
        else
        {
            IsProcessing = false;
            // Updates Feedbacks
        }
    }

    private void CheckTime()
    {
        if (TimeManagerStrike.Instance.CurrentStrikeCountTotal > endProcessHour)
        {
            // Change State to collect
            IsProcessing = false;
            CanCollectFertilization = true;
            TimeManagerStrike.Instance.StrikePassed -= CheckTime;
        }
    }

    public bool HaveSubstracts()
    {
        return BrownSubstractAmount >= GameplayManager.instance.globalConfigs.BrownSubstractToMakeFertilization_Amount &&
            GreenSubstractAmount >= GameplayManager.instance.globalConfigs.GreenSubstractToMakeFertilization_Amount;
    }
}