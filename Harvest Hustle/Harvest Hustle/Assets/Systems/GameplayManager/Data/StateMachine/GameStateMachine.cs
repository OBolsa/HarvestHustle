using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateMachine : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    public GameState LastState { get; private set; }

    public Action GameStateChanged;

    private void OnEnable()
    {
        PlayerInputManager.PlayerInput.World.PrimaryButton.performed += PrimaryButton;
        PlayerInputManager.PlayerInput.World.SecondaryButton.performed += SecondaryButton;
        PlayerInputManager.PlayerInput.World.Run.performed += RunButton;
        PlayerInputManager.PlayerInput.World.Inventory.performed += CallForInventory;
    }

    private void OnDisable()
    {
        PlayerInputManager.PlayerInput.World.PrimaryButton.performed -= PrimaryButton;
        PlayerInputManager.PlayerInput.World.SecondaryButton.performed -= SecondaryButton;
        PlayerInputManager.PlayerInput.World.Run.performed -= RunButton;
        PlayerInputManager.PlayerInput.World.Inventory.performed -= CallForInventory;
    }

    public void ChangeState(GameState state)
    {
        if(state == CurrentState) return;

        CurrentState?.LeaveState();
        LastState = CurrentState;
        CurrentState = state;
        GameStateChanged?.Invoke();
        StartCoroutine(CurrentState.StartState());
    }

    public void PrimaryButton(InputAction.CallbackContext context) => StartCoroutine(CurrentState.PrimaryButton());
    public void SecondaryButton(InputAction.CallbackContext context) => StartCoroutine(CurrentState.SecondaryButton());
    public void RunButton(InputAction.CallbackContext context) => StartCoroutine(CurrentState.RunButton());
    public void CallForInventory(InputAction.CallbackContext context) => StartCoroutine(CurrentState.CallForInventory());
}