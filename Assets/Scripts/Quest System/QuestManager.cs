using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();
    private  List<QuestSO> completedQuests = new();

    private void OnEnable()
    {
        QuestEvents.IsQuestComplete += IsQuestComplete;
    }

    private void OnDisable()
    {
        QuestEvents.IsQuestComplete -= IsQuestComplete;
    }

    #region Quest Accept Logic
    #region Quest Complete Logic

    public bool IsQuestComplete(QuestSO questSO)
    {
        if (questSO == null) return false;
        if (!questProgress.TryGetValue(questSO, out var progressDict))
        {
            return false;
        }
        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
        foreach (var objective in questSO.objectives)
        {
            if (!progressDict.TryGetValue(objective, out int prog) || prog < objective.requiredAmount)
            {
                return false;
            }
        }
        return true;
    }

    public void CompleteQuest(QuestSO questSO)
    {
        questProgress.Remove(questSO);
        completedQuests.Add(questSO);
        foreach (var objective in questSO.objectives)
        {
            if (objective.targetItem != null && objective.requiredAmount > 0)
            {
                InventoryManager.Instance.RemoveItem(objective.targetItem, objective.requiredAmount);
            }
        }
        foreach (var reward in questSO.rewards)
        {
            InventoryManager.Instance.AddItem(reward.itemSO, reward.quantity, true);

        }
        QuestEvents.OnQuestCompleted?.Invoke(questSO);
    }

    public bool GetCompleteQuest(QuestSO questSO)
    {
        return completedQuests.Contains(questSO);
    }
    #endregion

    public bool IsQuestAccepted(QuestSO questSO)
    {
        return questProgress.ContainsKey(questSO);
    }

    public List<QuestSO> GetActiveQuests()
    {
        return new List<QuestSO>(questProgress.Keys);
    }

    public void AcceptQuest(QuestSO questSO)
    {
        questProgress[questSO] = new Dictionary<QuestObjective, int>();
        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
        QuestEvents.OnQuestAccepted?.Invoke(questSO);
    }
    #endregion

    public void UpdateObjectiveProgress(QuestSO questSO, QuestObjective objective)
    {
        if (questSO == null || objective == null || !questProgress.ContainsKey(questSO))
        {
            return;
        }
        var progressDictionary = questProgress[questSO];
        int newAmount = 0;
        if (objective.targetItem != null && InventoryManager.Instance != null)
        {
            newAmount = InventoryManager.Instance.GetItemQuantity(objective.targetItem);
        }
        else if (objective.targetNPC != null && GameManager.Instance != null && GameManager.Instance.DialogueHistoryTracker != null && GameManager.Instance.DialogueHistoryTracker.HasSpokenWith(objective.targetNPC))
        {
            newAmount = objective.requiredAmount;
        }
        else if (objective.targetLocation != null && GameManager.Instance != null && GameManager.Instance.LocationHistoryTracker != null && GameManager.Instance.LocationHistoryTracker.HasVisited(objective.targetLocation))
        {
            newAmount = objective.requiredAmount;
        }
        progressDictionary[objective] = newAmount;
    }

    public string GetProgressText(QuestSO questSO, QuestObjective objective)
    {
        int currentAmount = GetCurrentAmount(questSO, objective);

        if (currentAmount >= objective.requiredAmount)
        {
            return "Completed";
        }
        else if (objective.targetItem != null)
        {
            return $"{currentAmount}/{objective.requiredAmount} {objective.targetItem.itemName}";
        }
        else if (objective.targetNPC != null || objective.targetLocation != null)
        {
            return "In progress";
        }
        else
        {
            return $"{currentAmount}/{objective.requiredAmount}";
        }
    }

    public int GetCurrentAmount(QuestSO questSO, QuestObjective objective)
    {
        if (questProgress.TryGetValue(questSO, out var objectiveDictionary))
        {
            if (objectiveDictionary.TryGetValue(objective, out int amount))
            {
                return amount;
            }
        }
        return 0;
    }

    public List<QuestSO> GetAvailableQuests(QuestBoard board)
    {
        if (board == null) return new List<QuestSO>();
        return board.GetAvailableQuests().Where(q => !IsQuestAccepted(q) && !GetCompleteQuest(q)).ToList();
    }
}
