using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container_UI : MonoBehaviour
{
    public Container container;
    private List<ContainerSlot_UI> _slots = new List<ContainerSlot_UI>();

    private void Awake()
    {
        List<ContainerSlot_UI> childSlots = GetComponentsInChildren<ContainerSlot_UI>(true).ToList();
        childSlots.ForEach(c => c.gameObject.SetActive(false));

        for (int i = 0; i < container.slots.Count; i++) 
        {
            _slots.Add(childSlots[i]);
            _slots[i].gameObject.SetActive(true);
        }

        SetupSlots();
    }

    private void OnEnable()
    {
        SetupSlots();
    }

    public void SetupSlots(Container newContainer)
    {
        container = newContainer;

        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetSlot(container.slots[i]);
        }
    }

    public void SetupSlots()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetSlot(container.slots[i]);
        }
    }
}