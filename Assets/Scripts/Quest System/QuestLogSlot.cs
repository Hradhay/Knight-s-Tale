using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLogSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questLevelText;
    public QuestSO currentQuest;
    public QuestLogUI questLogUI;

    private void OnValidate()
    {
        if (currentQuest != null)
        {
            SetQuest(currentQuest);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
    public void SetQuest(QuestSO questSO)
    {
        currentQuest = questSO;
        questNameText.text = questSO.questName;
        questLevelText.text = "Lvl" + questSO.questLevel.ToString();
        gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        currentQuest = null;
        //questNameText.text = "";
        //questLevelText.text = "";
        gameObject.SetActive(false);
    }
    public void OnSlotClicked()
    {
        if (currentQuest != null && questLogUI != null)
            questLogUI.HandleQuestClicked(currentQuest);        
    }
}
