using System.Collections;
using UnityEngine;

public class Cutscene_GameState : GameState
{
    public Cutscene_GameState(GameplayManager gameplayManager) : base(gameplayManager)
    {
    }

    public override IEnumerator StartState()
    {
        Debug.Log("-- CURRENT STATE: INVENTORY --");

        Manager.player.canAnim = false;
        Manager.globalConfigs.Player_Active = false;
        Manager.globalConfigs.CanMakeInteractions_Active = false;
        Manager.tooltip.HideTooltip();
        Manager.ChangeFreeLookCamera(FreeLookCameraType.Bottom);
        //Manager.playerInventory.gameObject.SetActive(true);
        //Cursor.lockState = CursorLockMode.None;

        yield return null;
    }

    public override void LeaveState()
    {
        Manager.globalConfigs.Player_Active = true;
        Manager.interactableInstigator.SetClosestInteractable();
    }

    public override IEnumerator CallForCutscene()
    {
        Manager.ChangeState(new Gameplay_GameState(Manager));
        yield return null;
    }
}