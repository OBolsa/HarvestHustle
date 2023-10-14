using System.Collections;
using UnityEngine;

public class Inventory_GameState : GameState
{
    public Inventory_GameState(GameplayManager gameplayManager) : base(gameplayManager)
    {
    }

    public override IEnumerator StartState()
    {
        Debug.Log("-- CURRENT STATE: INVENTORY --");

        Manager.player.canAnim = false;
        Manager.globalConfigs.Player_Active = false;
        Manager.tooltip.HideTooltip();
        Manager.playerInventory.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Manager.ChangeFreeLookCamera(FreeLookCameraType.Bottom);

        yield return null;
    }

    public override IEnumerator CallForInventory()
    {
        Manager.ChangeState(new Gameplay_GameState(Manager));
        yield return null;
    }

    public override void LeaveState()
    {
        Manager.globalConfigs.Player_Active = true;

        Manager.interactableInstigator.SetClosestInteractable();

        Manager.playerInventory.gameObject.SetActive(false);
    }
}