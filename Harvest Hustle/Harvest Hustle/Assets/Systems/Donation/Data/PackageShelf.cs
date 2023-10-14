using System.Collections.Generic;

public class PackageShelf : ItemListener
{
    public ItemData PackageDefault;
    private int idCount = 0;

    protected override void Start()
    {
        base.Start();
        RegisterEffectsName(CreatePackage);
    }

    public override void DoInteraction()
    {
        base.DoInteraction();

        List<ItemData> displayItems = new List<ItemData>();

        foreach (var item in GameplayManager.instance.playerInventory.container.UniqueItemsInContainer())
        {
            if(itemsToDisplay.Contains(item))
            {
                displayItems.Add(item);
            }
        }

        GameplayManager.instance.modalManager.packageCreateModal.ModalConfirmed += CreatePackage;
        GameplayManager.instance.ChangeFreeLookCamera(FreeLookCameraType.Bottom);
        GameplayManager.instance.modalManager.OpenPackageCreateModal(displayItems, PackageDefault.itemIcon, Name);
    }

    public void CreatePackage(List<ItemData> items)
    {
        if (items.Count == 0)
            return;

        GameplayManager.instance.modalManager.packageCreateModal.ModalConfirmed -= CreatePackage;
        int packageValue = 0;

        foreach (var item in items)
        {
            packageValue += GameplayManager.instance.globalConfigs.GetDonationValue(item);
        }

        string packageId = $"ID{idCount}A{items.Count}V{packageValue}";
        idCount++;

        ItemData package = ItemData.CreateInstance(PackageDefault);
        package.SetSecretID(packageId);

        GameplayManager.instance.playerInventory.container.RemoveItem(items);
        GameplayManager.instance.playerInventory.container.AddItem(package);

        // Register the package on DonationBox
        GameplayManager.instance.donationManager.RegisterPackage(packageId, packageValue);
    }

    public void CreatePackage(ItemData itemData)
    {
        // See how many items you have to make this package;
        int itemAmount = GameplayManager.instance.playerInventory.container.FindItemSlot(itemData).Count;

        // Set the package value
        int packageValue = GameplayManager.instance.globalConfigs.GetDonationValue(itemData) * itemAmount;

        // Set the packageId
        string packageId = $"{idCount}{itemData.itemName}A{itemAmount}V{packageValue}";
        idCount++;

        ItemData package = ItemData.CreateInstance(PackageDefault);
        package.itemName = $"Pacote de {itemData.itemName}";
        package.SetSecretID(packageId);

        // Change the items to create the package
        GameplayManager.instance.playerInventory.container.RemoveItem(itemData, itemAmount);
        GameplayManager.instance.playerInventory.container.AddItem(package);

        // Register the package on DonationBox
        GameplayManager.instance.donationManager.RegisterPackage(packageId, packageValue);
    }
}