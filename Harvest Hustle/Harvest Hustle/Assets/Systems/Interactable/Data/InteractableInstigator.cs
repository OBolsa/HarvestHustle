using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableInstigator : MonoBehaviour
{
    private List<Interactable> _interactables = new List<Interactable>();
    public static Interactable ClosestInteractable;
    public static bool HaveClosestInteractable { get => ClosestInteractable != null; }

    public Container holderContainer;
    public Container_UI holderContainer_UI;

    private void OnEnable()
    {
        PlayerInputManager.PlayerInput.World.PrimaryButton.performed += DoInteraction;
        GameplayManager.instance.sceneManager.SceneLoaded += SetClosestInteractable;
    }

    private void OnDisable()
    {
        PlayerInputManager.PlayerInput.World.PrimaryButton.performed -= DoInteraction;
        GameplayManager.instance.sceneManager.SceneLoaded -= SetClosestInteractable;
    }

    public void DoInteraction(InputAction.CallbackContext context)
    {
        if (ClosestInteractable == null || !GameplayManager.instance.globalConfigs.CanMakeInteractions_Active)
            return;

        SetClosestInteractable();
        ClosestInteractable.DoInteraction();
    }

    public void ClearInteractables()
    {
        _interactables.Clear();
    }

    public void SetClosestInteractable()
    {
        List<Interactable> activeInteractables = _interactables.FindAll(i => i.gameObject.activeSelf == true);
        _interactables.Clear();
        _interactables = activeInteractables;
        holderContainer_UI.SetupSlots();

        if (_interactables.Count <= 0)
        {
            GameplayManager.instance.tooltip.HideTooltip();
            ClosestInteractable = null;
            return;
        }

        Interactable closest = _interactables.OrderBy(i => Vector3.Distance(transform.position, i.gameObject.transform.position)).FirstOrDefault();

        ClosestInteractable = closest;

        GameplayManager.instance.tooltip.ShowTooltip(ClosestInteractable.TooltipPosition, ClosestInteractable.Name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if(interactable != null && interactable.enabled)
        {
            _interactables.Add(interactable);
            SetClosestInteractable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable != null)
        {
            _interactables.Remove(interactable);
            SetClosestInteractable();
        }
    }

    public static bool IsInteractingWithMe(string id) => ClosestInteractable.ID == id;
}