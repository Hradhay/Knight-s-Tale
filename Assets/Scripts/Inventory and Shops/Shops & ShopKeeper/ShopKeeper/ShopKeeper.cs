using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopKeeper : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log($"ShopKeeper Awake: shopWeapons count={shopWeapons.Count}, shopArmour count={shopArmour.Count}, shopManager={(shopManager != null)}");
    }
    public static ShopKeeper currentShopKeeper;
    public Animator anim;
    public CanvasGroup shopCanvasGroup;
    public ShopManager shopManager;
    [SerializeField] private List<ShopItems> shopItems;
    [SerializeField] private List<ShopItems> shopWeapons;
    [SerializeField] private List<ShopItems> shopArmour;
    public static event Action<ShopManager, bool> OnShopStateChanged;
    private bool playerinrange;
    private bool isShopOpen;
void Update()
    {
        if (playerinrange)
        {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.V))
            {



                if (!isShopOpen)
                {
                    Time.timeScale = 0;
                    isShopOpen = true;
                    currentShopKeeper = this;
                    OnShopStateChanged?.Invoke(shopManager, true);
                    shopCanvasGroup.alpha = 1;
                    shopCanvasGroup.blocksRaycasts = true;
                    shopCanvasGroup.interactable = true; 
                    OpenItemShop(); 
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                isShopOpen = false;
                currentShopKeeper = null;
                OnShopStateChanged?.Invoke(shopManager, false);
                shopCanvasGroup.alpha = 0;
                shopCanvasGroup.blocksRaycasts = false;
                shopCanvasGroup.interactable = false;
            }
        }
    }
    public void  OpenItemShop()
    {
        shopManager.PopulateShopItems(shopItems);
    }
    public void OpenWeaponShop()
    {
        Debug.Log($"OpenWeaponShop called: weapons count={shopWeapons.Count}, shopManager={(shopManager != null)}");
        if (shopManager != null)
        {
            shopManager.PopulateShopItems(shopWeapons);
        }
        else
        {
            Debug.LogWarning("shopManager is null in ShopKeeper!");
        }
    }
    public void OpenArmourShop()
    {
        Debug.Log($"OpenArmourShop called: armour count={shopArmour.Count}, shopManager={(shopManager != null)}");
        if (shopManager != null)
        {
            shopManager.PopulateShopItems(shopArmour);
        }
        else
        {
            Debug.LogWarning("shopManager is null in ShopKeeper!");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerinrange", true);
            playerinrange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerinrange", false);
            playerinrange = false;
        }
    }
}
    
