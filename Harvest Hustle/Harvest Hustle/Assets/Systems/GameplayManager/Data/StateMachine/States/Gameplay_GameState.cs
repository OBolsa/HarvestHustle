using System.Collections;
using UnityEngine;

public class Gameplay_GameState : GameState
{
    public Gameplay_GameState(GameplayManager gameplayManager) : base(gameplayManager)
    {
    }

    public override IEnumerator StartState()
    {
        Debug.Log("-- CURRENT STATE: GAMEPLAY --");

        Manager.player.canAnim = true;
        Manager.ChangeCamera("FreeLookCam");
        Manager.ChangeFreeLookCamera(FreeLookCameraType.Middle);
        Manager.globalConfigs.Player_Active = true;
        Manager.globalConfigs.CanMakeInteractions_Active = true;
        Cursor.lockState = CursorLockMode.Locked;

        yield return null;
    }

    public override IEnumerator CallForInventory()
    {
        Manager.ChangeState(new Inventory_GameState(Manager));

        yield return null;
    }

    public override IEnumerator CallForModal(Modal modal)
    {
        Manager.modalManager.OpenModal(modal);
        Manager.ChangeState(new Modal_GameState(Manager));
        yield return null;
    }

    public override IEnumerator CallForCutscene()
    {
        Manager.ChangeState(new Cutscene_GameState(Manager));
        yield return null;
    }
}