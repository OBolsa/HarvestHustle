using System.Collections;
using UnityEngine;

public class Modal_GameState : GameState
{
    public Modal_GameState(GameplayManager gameplayManager) : base(gameplayManager)
    {
        Manager.modalManager.OnCloseModal += BackToState;
    }

    private void BackToState(Modal modal)
    {
        Manager.ChangeState(Manager.LastState);
        Manager.modalManager.OnCloseModal -= BackToState;
    }

    public override IEnumerator StartState()
    {
        Debug.Log("-- CURRENT STATE: MODAL --");

        Manager.globalConfigs.Player_Active = false;
        Manager.globalConfigs.CanMakeInteractions_Active = false;
        Manager.tooltip.HideTooltip();
        Cursor.lockState = CursorLockMode.None;

        return base.StartState();
    }

    public override IEnumerator CallForInventory()
    {
        // Display a Feedback text for the player that it can't open the inventory in a modal
        //Manager.ChangeState(new Inventory_GameState(Manager)); // TEMPORARY
        Manager.ChangeState(new Gameplay_GameState(Manager));

        yield return null;
    }

    public override void LeaveState()
    {
        Manager.modalManager.OnCloseModal -= BackToState;
        Manager.modalManager.CloseModal();
        Manager.globalConfigs.Player_Active = true;
        Manager.globalConfigs.CanMakeInteractions_Active = true;
        Manager.interactableInstigator.SetClosestInteractable();
    }
}