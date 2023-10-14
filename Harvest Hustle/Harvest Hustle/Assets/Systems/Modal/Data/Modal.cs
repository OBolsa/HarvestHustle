using UnityEngine;

public abstract class Modal : MonoBehaviour
{
    public GameObject elements;

    public abstract void OpenModal();
    public abstract void CloseModal();
    public abstract void ShowModal(bool active);
}