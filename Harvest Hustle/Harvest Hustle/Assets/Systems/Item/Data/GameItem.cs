using UnityEngine;

public class GameItem : Interactable
{
    [Header("Item Config")]
    [SerializeField] private ItemData currentItem;

    [Header("Meshs Components")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
    private MeshRenderer _itemMeshRenderer;
    private MeshFilter _itemMeshFilter;

    private void OnValidate()
    {
        name = currentItem != null ? $"GameItem - {currentItem.itemName}" : "GameItem - Null";
    }

    protected override void Start()
    {
        base.Start();

        SetupItem();
    }

    public override void DoInteraction()
    {
        base.DoInteraction();

        Container instigatorContainer = GameplayManager.instance.playerInventory.container;

        if (instigatorContainer.HaveSpaceForItem(currentItem))
        {
            gameObject.SetActive(false);
            instigatorContainer.AddItem(currentItem);
        }
        else
        {
            return;
        }
    }

    public void ChangeItem(ItemData newItem)
    {
        currentItem = newItem;

        SetupItem();
    }

    [ContextMenu("Setup Item")]
    public void SetupItem()
    {
        if (currentItem == null) return;

        _interactableName = currentItem.itemName;

        // Pegando as informacoes do modelo
        _itemMeshRenderer = currentItem.itemModel.GetComponent<MeshRenderer>();
        _itemMeshFilter = currentItem.itemModel.GetComponent<MeshFilter>();

        // Atualizando as meshs do GameItem
        meshRenderer.materials = _itemMeshRenderer.sharedMaterials;
        meshFilter.mesh = _itemMeshFilter.sharedMesh;
        meshCollider.sharedMesh = meshFilter.sharedMesh;

        name = currentItem != null ? $"GameItem - {currentItem.itemName}" : "GameItem - Null";
    }
}