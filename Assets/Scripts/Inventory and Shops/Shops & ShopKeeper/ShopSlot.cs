using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ItemSO itemSO;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public Image itemImage;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ShopInfo shopInfo;
    public int price;

    private void OnValidate()
    {
        if (shopManager == null)
        {
            shopManager = GetComponentInParent<ShopManager>();
        }
        if (shopInfo == null)
        {
            shopInfo = GetComponentInParent<ShopInfo>();
        }
    }

    public void Initialize(ItemSO newitemSO, int price)
    {
        itemSO = newitemSO;
        if (itemSO != null)
        {
            itemImage.sprite = itemSO.icon;
            itemNameText.text = itemSO.itemName;
        }
        else
        {
            Debug.LogWarning("Initialize: newitemSO is null");
        }
        this.price = price;
        priceText.text = price.ToString();

        Debug.Log($"ShopSlot [{gameObject.name}].Initialize: itemSO.name = {(itemSO != null ? itemSO.itemName : "null")}, price={price}, shopInfo={(shopInfo != null ? "assigned" : "null")}");

        if (shopInfo == null)
        {
            shopInfo = Object.FindFirstObjectByType<ShopInfo>();
            Debug.Log($"ShopSlot [{gameObject.name}].Initialize: shopInfo found by type = {(shopInfo != null ? "success" : "failed")}");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"ShopSlot OnPointerEnter: {gameObject.name}, item={itemSO?.itemName}, shopInfo={(shopInfo!=null)}, eventData={(eventData!=null)}");
        if (shopInfo != null && itemSO != null)
        {
            shopInfo.ShowItemInfo(itemSO);
        }
        else
        {
            Debug.LogWarning($"OnPointerEnter failed: shopInfo={shopInfo}, itemSO={itemSO}");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (shopInfo != null)
        {
            shopInfo.HideItemInfo();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"ShopSlot OnPointerClick: {gameObject.name}, button={eventData.button}, shopManager={(shopManager!=null)}, item={itemSO?.itemName}");
        if (eventData.button == PointerEventData.InputButton.Left && shopManager != null && itemSO != null)
        {
            shopManager.TryBuyItem(itemSO, price);
        }
        else
        {
            Debug.LogWarning($"OnPointerClick failed: button={eventData.button}, shopManager={shopManager}, itemSO={itemSO}");
        }
    }
}
