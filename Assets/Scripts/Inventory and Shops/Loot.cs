using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Loot : MonoBehaviour

{
    public ItemSO itemSO;
    [SerializeField] private SpriteRenderer sr;
    public Animator anim;
    public bool canBePickedUp = true;
    public int quantity;
    public static event Action<ItemSO, int> OnItemLooted;

    private void OnValidate()
    {
        if (itemSO == null) return;

        // Auto-assign sr if not set
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (sr == null) {
            Debug.LogError($"Loot on {gameObject.name}: No SpriteRenderer component found! Add one to this GameObject.", this);
            return;
        }

        UpdateAppearance();
    }
    public void Initialize(ItemSO itemSO, int quantity )
    {
        this.itemSO = itemSO;
        this.quantity = quantity;
        canBePickedUp = false;

        UpdateAppearance();
        StartCoroutine(DelayPickup());
    }
    
    private IEnumerator DelayPickup() {
        yield return new WaitForSeconds(1f);
        canBePickedUp = true;
        Debug.Log($"Loot {itemSO.itemName} pickup enabled at {transform.position}");
    }
    private void UpdateAppearance()
    {
        if (sr == null || itemSO == null || itemSO.icon == null) return;

        sr.sprite = itemSO.icon;
        name = itemSO.itemName;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canBePickedUp)
        {
            Debug.Log($"Picking up {itemSO.itemName} from {transform.position}");
            if (anim != null && anim.runtimeAnimatorController != null && anim.HasState(0, Animator.StringToHash("Lookup"))) {
                anim.Play("Lookup", 0);
            }
            OnItemLooted?.Invoke(itemSO, quantity);
            Destroy(gameObject, 0.5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBePickedUp = true;
        }
    }
}
