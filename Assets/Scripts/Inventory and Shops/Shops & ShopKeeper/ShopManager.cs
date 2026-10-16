using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopSlot[] shopSlots;
[SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private ShopInfo shopInfo;

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindFirstObjectByType<InventoryManager>();
        }
        if (shopInfo == null)
        {
            shopInfo = FindFirstObjectByType<ShopInfo>();
        }
    }

    private void OnValidate()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindFirstObjectByType<InventoryManager>();
        }
    }
    public void PopulateShopItems(List<ShopItems> shopsItems)
    {
        // Hide previous info
        if (shopInfo != null)
        {
            shopInfo.HideItemInfo();
        }

        Debug.Log($"ShopManager PopulateShopItems: received {shopsItems.Count} items, shopSlots length={shopSlots?.Length ?? 0}");
        if (shopsItems == null || shopSlots == null)
        {
            Debug.LogWarning("shopsItems or shopSlots is null!");
            return;
        }
        for (int i = 0; i < shopsItems.Count && i < shopSlots.Length; i++)
        {
           ShopItems shopItem = shopsItems[i];
           shopSlots[i].Initialize(shopItem.itemSO, shopItem.price); 
           shopSlots[i].gameObject.SetActive(true);
        }
        for (int i = shopsItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }

        // Show info for first active slot
        if (shopSlots.Length > 0 && shopInfo != null)
        {
            var firstSlot = shopSlots[0];
            if (firstSlot.gameObject.activeSelf && firstSlot.itemSO != null)
            {
                Debug.Log($"Auto showing info for first slot: {firstSlot.itemSO.itemName}");
                shopInfo.ShowItemInfo(firstSlot.itemSO);
            }
            else
            {
                Debug.LogWarning("No first slot item to auto-show info");
            }
        }
        else
        {
            Debug.LogWarning("No shopSlots or shopInfo for auto-show");
        }
    }
    public void TryBuyItem(ItemSO itemSO, int price)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager not assigned in " + gameObject.name);
            return;
        }

        Debug.Log($"TryBuyItem called for {(itemSO != null ? itemSO.itemName : "null")} (price: {price}, gold: {inventoryManager.gold})");

        if (itemSO == null)
        {
            Debug.LogWarning("ItemSO is null!");
            return;
        }

        if (inventoryManager.gold < price)
        {
            Debug.LogWarning($"Insufficient gold: {inventoryManager.gold} < {price}");
            return;
        }

        if (!HasSpaceForItem(itemSO))
        {
            Debug.LogWarning($"No space for {itemSO.itemName}");
            return;
        }

        Debug.Log($"Buying {itemSO.itemName}: spending {price} gold");

        inventoryManager.SpendGold(price);
        inventoryManager.AddItem(itemSO, 1);

        ShopSound.Instance?.PlayTransactionSound();

        Debug.Log("Buy complete");
    }
    private bool HasSpaceForItem(ItemSO itemSO)
    {
        if (inventoryManager == null || inventoryManager.itemSlots == null) return false;
        foreach (var slot in inventoryManager.itemSlots)
        {
            if (slot == null) continue;
            if(slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)
            {
                return true;
            }
            else if (slot.itemSO == null)
            {
                return true;
            }
        }
        return false;
    }



    public void SellItem(ItemSO itemSO)
    {
        if (itemSO == null)
        {
            return;
        }
        foreach (var slot in shopSlots)
        {
            if (slot.itemSO == itemSO)
            {
                inventoryManager.gold += slot.price;
                inventoryManager.goldText.text = inventoryManager.gold.ToString();
                ShopSound.Instance?.PlayTransactionSound();
                return;
            }
        }
    }
}

[System.Serializable]
public class ShopItems
{
    public ItemSO itemSO;
    public int price;
}

