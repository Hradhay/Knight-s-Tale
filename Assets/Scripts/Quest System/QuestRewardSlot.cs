using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class QuestRewardSlot : MonoBehaviour
{
    public Image rewardimage;
    public TMP_Text rewardQuantity;

    public void DisplayReward(Sprite sprite, int quantity)
    {
        rewardimage.sprite = sprite;
        rewardQuantity.text = quantity.ToString();
    }
}
