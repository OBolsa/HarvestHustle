using System;
using System.Collections.Generic;

public class ItemListener : Interactable
{
    private Dictionary<ToolType, Action<ItemData>> toolEffectHandlers = new Dictionary<ToolType, Action<ItemData>>();
    private Dictionary<string, Action<ItemData>> toolEffectHandlersName = new Dictionary<string, Action<ItemData>>();
    public List<ItemData> itemsToDisplay = new List<ItemData>();

    protected virtual void OnEnable()
    {
        GameplayManager.instance.itemManager.OnUseTool += HandleToolEffect;
    }

    protected virtual void OnDisable()
    {
        GameplayManager.instance.itemManager.OnUseTool -= HandleToolEffect;
    }

    protected virtual void RegisterEffects(Action<ItemData> onUse)
    {
        foreach (var item in itemsToDisplay)
        {
            RegisterToolEffectHandler(item.toolType, onUse);
        }
    }
    protected virtual void RegisterEffectsName(Action<ItemData> onUse)
    {
        foreach (var item in itemsToDisplay)
        {
            RegisterToolEffectHandlerName(item.itemName, onUse);
        }
    }

    protected void RegisterToolEffectHandler(ToolType toolType, Action<ItemData> handler)
    {
        toolEffectHandlers[toolType] = handler;
    }
    protected void RegisterToolEffectHandlerName(string name, Action<ItemData> handler)
    {
        toolEffectHandlersName[name] = handler;
    }
    protected void HandleToolEffect(ItemData toolItem, Interactable interactable)
    {
        if (interactable.ID != ID)
            return;

        if (toolEffectHandlers.TryGetValue(toolItem.toolType, out Action<ItemData> handler))
        {
            handler.Invoke(toolItem);
        }
    }
    protected void HandleToolEffectName(ItemData toolItem, Interactable interactable)
    {
        if (interactable.ID != ID)
            return;

        if (toolEffectHandlersName.TryGetValue(toolItem.itemName, out Action<ItemData> handler))
        {
            handler.Invoke(toolItem);
        }
    }
}