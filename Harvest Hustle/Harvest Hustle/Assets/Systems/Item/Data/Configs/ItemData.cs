using UnityEngine;

[System.Serializable][CreateAssetMenu(menuName = "ItemData", fileName = "ItemData_")]
public class ItemData : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField][TextArea(0, 5)] public string itemDescription;
    [SerializeField] public int maxStack;
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public GameObject itemModel;
    [SerializeField] public ItemType itemType;
    [SerializeField] public ToolType toolType;
    [SerializeField] public string SecretID { get; private set; }
    public void SetSecretID(string secretID) => SecretID = secretID;
    public string Infos
    {
        get => $"Name: {itemName}\nDescription: {itemDescription}\nMaxStack: {maxStack}\nItemType: {itemType}\nToolType: {toolType}\nSecretID: {SecretID}";
    }

    public static ItemData CreateInstance(ItemData item)
    {
        var data = CreateInstance<ItemData>();
        data.itemName = item.itemName;
        data.itemDescription = item.itemDescription;
        data.itemIcon = item.itemIcon;
        data.itemModel = item.itemModel;
        data.maxStack = item.maxStack;
        data.itemType = item.itemType;
        data.toolType = item.toolType;
        return data;
    }
}

[System.Serializable]
public enum ItemType
{
    Generic,
    Tool,
    Seed,
    QuestItem
}

[System.Serializable]
public enum ToolType
{
    None,
    Facao,
    Enxada,
    Machado,
    Picareta,
    Moringa,
    Regador,
    Balde,
    Rastelo,
    Martelo,
    Adubo,
    Substrato,
    Pacote,
    Armadilha
}