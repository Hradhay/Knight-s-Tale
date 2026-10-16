using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopInfo : MonoBehaviour
{
    public CanvasGroup infoPanel;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    [Header ("Stat Field")]
public TMP_Text[] statTexts;

    [Header("Positioning")]
    public Vector2 mouseOffset = new Vector2(10, -10);

    private RectTransform infoPanelRect;
private Canvas canvas;
    private RectTransform parentRect;
    private bool isShowing;

    private void Awake()
    {
        infoPanelRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        parentRect = infoPanelRect.parent as RectTransform;
    }

    private void Update()
    {
        if (isShowing)
        {
            FollowMouse();
        }
    }

    public void ShowItemInfo(ItemSO itemSO)
    {
        Debug.Log($"ShopInfo.ShowItemInfo called with {(itemSO != null ? itemSO.itemName : "null")}");
        if (itemSO == null)
        {
            Debug.LogWarning("itemSO is null!");
            return;
        }
        if (infoPanel == null || itemNameText == null || itemDescriptionText == null)
        {
            Debug.LogError("ShopInfo UI refs null in Inspector: panel=" + (infoPanel != null) + ", name=" + (itemNameText != null) + ", desc=" + (itemDescriptionText != null) + ", stats.Length=" + statTexts.Length);
            return;
        }
        if (statTexts == null || statTexts.Length == 0)
        {
            Debug.LogWarning("statTexts array empty - no stats shown");
        }

        isShowing = true;
        infoPanel.alpha = 1;
        itemNameText.text = itemSO.itemName;
        itemDescriptionText.text = itemSO.itemDescription;

        List<string> stats = new List<string>();
        if (itemSO.currentHealth > 0)
            stats.Add("Health: " + itemSO.currentHealth);
        if (itemSO.damage > 0)
            stats.Add("Damage: " + itemSO.damage);
        if (itemSO.speed > 0)
            stats.Add("Speed: " + itemSO.speed);
        if (itemSO.Duration > 0)
            stats.Add("Duration: " + itemSO.Duration);

        for (int i = 0; i < statTexts.Length; i++)
        {
            if (i < stats.Count)
            {
                statTexts[i].text = stats[i];
                statTexts[i].gameObject.SetActive(true);
            }
            else
            {
                statTexts[i].gameObject.SetActive(false);
            }
        }
        Debug.Log($"ShowItemInfo complete for {itemSO.itemName}, stats shown: {stats.Count}");
    }

    public void HideItemInfo()
    {
        Debug.Log("ShopInfo.HideItemInfo called");
        isShowing = false;
        if (infoPanel != null) infoPanel.alpha = 0;
        if (itemNameText != null) itemNameText.text = "";
        if (itemDescriptionText != null) itemDescriptionText.text = "";
    }

    public void FollowMouse()
    {
        if (infoPanelRect == null || parentRect == null) 
        {
            Debug.LogWarning("FollowMouse: RectTransforms null");
            return;
        }

        Vector2 localPoint;
        Camera cam = canvas != null && canvas.worldCamera != null ? canvas.worldCamera : null;
        bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, cam, out localPoint);
        if (success)
        {
            Vector2 newPos = localPoint + mouseOffset;
            newPos = ClampToRect(parentRect.rect, newPos);
            infoPanelRect.localPosition = newPos;
        }
        else
        {
            Debug.LogWarning("FollowMouse: ScreenPointToLocalPointInRectangle failed");
        }
    }

    private Vector2 ClampToRect(Rect rect, Vector2 position)
    {
        return new Vector2(
            Mathf.Clamp(position.x, rect.xMin, rect.xMax),
            Mathf.Clamp(position.y, rect.yMin, rect.yMax)
        );
    }
}
