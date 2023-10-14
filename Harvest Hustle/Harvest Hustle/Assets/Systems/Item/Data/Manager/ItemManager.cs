using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject defaultGameItem;
    public Action<ItemData, Interactable> OnUseTool;

    public void UseTool(ItemData item, Interactable interactable)
    {
        OnUseTool?.Invoke(item, interactable);
        GameplayManager.instance.modalManager.CloseModal();
    }

    public void DropItem(ItemData data)
    {
        GameItem itemToSpawn = GetItem();

        if (itemToSpawn == null)
            return;

        itemToSpawn.ChangeItem(data);
        itemToSpawn.transform.position = GameplayManager.instance.player.transform.position + new Vector3(0f, 0.5f, 0f);
        itemToSpawn.gameObject.SetActive(true);
    }

    public GameItem GetItem()
    {
        GameItem itemToSpawn;

        GameObject spawnedItem = Instantiate(defaultGameItem, transform);
        itemToSpawn = spawnedItem.GetComponent<GameItem>();
        return itemToSpawn;
    }
}