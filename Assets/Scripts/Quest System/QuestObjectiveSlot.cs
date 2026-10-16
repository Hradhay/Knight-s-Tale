using UnityEngine;
using TMPro;

public class QuestObjectiveSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text trackingText;
    
    public void RefreshObjectives(string description, string progressText, bool isComplet)
    {
        objectiveText.text = description;
        trackingText.text = progressText;
        Color color = isComplet ? Color.gray : Color.white;
        objectiveText.color = color;
        trackingText.color = color;
    }
}

