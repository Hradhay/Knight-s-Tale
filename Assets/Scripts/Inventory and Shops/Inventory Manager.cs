using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
public static InventoryManager instance;
    public static InventoryManager Instance => instance;

    public InventorySlot[] itemSlots;
    public UseItem useItem;
    public int gold;
    public TMP_Text goldText;
    public GameObject lootPrefab;
    public Transform player;

    public static event Action<int> OnExperienceGained;

    private void  Awake(){
        if(instance == null){
         instance = this;
         DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject); 
        }
    }
    // Fallback if not assigned
    private Transform GetPlayerTransform() {
        if (player != null) return player;
        player = FindFirstObjectByType<PlayerMovement>()?.transform;
        if (player == null) Debug.LogError("No PlayerMovement found for drop!");
        return player;
    }
    public void Start()
    {
        foreach(var slot in itemSlots)
        {
            slot.UpdateUI();
        }
    }
    private void OnEnable()
    {
        Loot.OnItemLooted += (item, qty) => AddItem(item, qty, false);
    }
    
    private void OnDisable()
    {
Loot.OnItemLooted -= (item, qty) => AddItem(item, qty, false);
    }
    public void AddItem(ItemSO itemSO, int quantity, bool forceDirectAdd = false)
    {
      if (itemSO.isGold)
        {
            gold += quantity;
            goldText.text = gold.ToString();
            return;
        }
        if (itemSO.isEXP)
        {
            OnExperienceGained?.Invoke(quantity);
        }
        foreach(var slot  in itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)
            {
                int availableSpace = itemSO.stackSize - slot.quantity;
                int amountToAdd = Mathf.Min(availableSpace, quantity);

                slot.quantity += amountToAdd;
                quantity -= amountToAdd;

                slot.UpdateUI();

                if (quantity <= 0)
                {
                    return;
                }
            }
        }
        foreach(var slot in itemSlots)
        {
            if(slot.itemSO == null)
            {
                int amountToAdd = Mathf.Min(itemSO.stackSize, quantity);
                slot.itemSO = itemSO;
                slot.quantity = amountToAdd;
                slot.UpdateUI();
                return;
            }
        }
        if (quantity > 0)
        {
            if (forceDirectAdd)
            {
                Debug.LogWarning($"Inventory full for {itemSO.itemName}! Rewards lost (forceDirectAdd). Expand inventory.");
            }
            else
            {
                Droploot(itemSO, quantity);
            }
        }       
    }

    public void SpendGold(int amount)
    {
        if (amount <= 0) return;

        if (gold >= amount)
        {
            gold -= amount;
            goldText.text = gold.ToString();
            Debug.Log($"Spent {amount} gold. Remaining: {gold}");
        }
        else
        {
            Debug.LogWarning($"Cannot spend {amount} gold. Have only {gold}");
        }
    }

    public void RemoveItem(ItemSO itemSO, int quantity)
    {
        for( int i = 0; i < itemSlots.Length; i++)
        {
            var slot = itemSlots[i];
            if(slot.itemSO != itemSO)
            {
                continue;
            }
            if(slot.quantity > quantity)
            {
                slot.quantity -= quantity;
                slot.UpdateUI();
                quantity = 0;
            }
            else
            {
                quantity -= slot.quantity;
                slot.itemSO = null;
                slot.quantity = 0;
                slot.UpdateUI();
            }
        }
    }
    public  void DropItem(InventorySlot slot)
    {
        Debug.Log($"Dropping {slot.itemSO.itemName}");
        if (lootPrefab == null)
        {
            Debug.LogError("lootPrefab not assigned in InventoryManager!");
            return;
        }
        Droploot(slot.itemSO, 1, false);
        slot.quantity--;
        if (slot.quantity <= 0)
        {
            slot.itemSO = null;
        }
        slot.UpdateUI();
    }
    private void Droploot(ItemSO itemSO, int quantity, bool forceDirectAdd = false)
    {
        if (forceDirectAdd) {
            Debug.LogWarning($"Drop blocked: forceDirectAdd for {itemSO.itemName}");
            return;
        }
        Transform p = GetPlayerTransform();
        if (p == null) {
            Debug.LogError("Cannot drop: No player transform!");
            return;
        }
        Vector2 offset2d = ((Vector2)p.right * 1.5f) + (UnityEngine.Random.insideUnitCircle * 0.8f) + (Vector2.up * 0.2f);
        Vector3 spawnPos = p.position + new Vector3(offset2d.x, offset2d.y, 0);
        
        Debug.Log($"Dropping {itemSO.itemName} ({quantity}) at {spawnPos}, player at {p.position}, offset {offset2d}");
        
        
        Loot loot = Instantiate(lootPrefab, spawnPos, Quaternion.identity).GetComponent<Loot>();
        if (loot == null) Debug.LogError("Instantiated lootPrefab has no Loot component!");
        loot.Initialize(itemSO, quantity);
    }
    public void UseItem(InventorySlot slot)
    {
        if (slot.itemSO != null && slot.quantity > 0)
        {
            Debug.Log($"Using {slot.itemSO.itemName}");
            if (useItem == null)
            {
                Debug.LogError("useItem not assigned!");
                return;
            }
            useItem.ApplyItemEffects(slot.itemSO);
            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.itemSO = null;
            }
            slot.UpdateUI();
        }
    }

    void Update()
    {
        for (int i = 0; i < itemSlots.Length && i < 10; i++)
        {
        KeyCode useKey = (KeyCode)((int)KeyCode.Alpha1 + i);

        if (Input.GetKeyDown(useKey))
        {
            if (itemSlots[i] != null && itemSlots[i].itemSO != null && itemSlots[i].quantity > 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DropItem(itemSlots[i]);
                }
                else
                {
                    UseItem(itemSlots[i]);
                }
            }
        }
        }
    }

    public bool HasItem(ItemSO itemSO){
        foreach(var slot in itemSlots){
            if(slot.itemSO == itemSO && slot.quantity > 0){
                return true;
            }
        }
        return false;
    }
    public int GetItemQuantity(ItemSO itemSO)
    {
        int total = 0;
        foreach(var slot in itemSlots)
        {
            if (slot.itemSO == itemSO)
            {
                total += slot.quantity;
            }
        }
        return total;
    }

    public void ResetInventory()
    {
        gold = 0;
        if (goldText != null)
            goldText.text = "0";
        foreach (var slot in itemSlots)
        {
            slot.itemSO = null;
            slot.quantity = 0;
            slot.UpdateUI();
        }
    }
}
