using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Modal_UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent OnClickEvent;
    public UnityEvent OnPointerEnterEvent;
    public UnityEvent OnPointerExitEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke();
    }
}