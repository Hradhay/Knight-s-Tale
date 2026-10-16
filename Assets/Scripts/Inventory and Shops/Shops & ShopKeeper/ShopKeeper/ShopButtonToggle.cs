using UnityEngine;

public class ShopButtonToggle : MonoBehaviour
{
    public void OpenItemShop()
    {
        if (ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenItemShop();
        }
    }
    public void OpenWeaponShop()
    {
        Debug.Log("OpenWeaponShop button pressed. currentShopKeeper: " + (ShopKeeper.currentShopKeeper != null));
        if (ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenWeaponShop();
        }
        else
        {
            Debug.LogWarning("currentShopKeeper is null! Shop not opened?");
        }
    }
    public void OpenArmourShop()
    {
        Debug.Log("OpenArmourShop button pressed. currentShopKeeper: " + (ShopKeeper.currentShopKeeper != null));
        if (ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenArmourShop();
        }
        else
        {
            Debug.LogWarning("currentShopKeeper is null! Shop not opened?");
        }
    }
}
