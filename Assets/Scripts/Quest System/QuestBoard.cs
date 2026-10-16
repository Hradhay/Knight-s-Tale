using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private List<QuestSO> availableQuests = new List<QuestSO>();
    [SerializeField] public QuestSO questToOffer; // Fallback single quest
    [SerializeField] private QuestManager questManager;
    public bool PlayerInRange { get; private set; }

    private QuestSO GetReadyToTurnInQuest()
    {
        if (questManager == null) return null;
        var activeQuests = questManager.GetActiveQuests();
        foreach (var quest in activeQuests)
        {
            if (questManager.IsQuestComplete(quest))
            {
                return quest;
            }
        }
        return null;
    }

    public QuestSO GetNextAvailableQuest()
    {
        if (questManager == null) return null;
        foreach (var quest in availableQuests)
        {
            if (!questManager.IsQuestAccepted(quest) && !questManager.GetCompleteQuest(quest))
            {
                return quest;
            }
        }
        return null;
    }

    public List<QuestSO> GetAvailableQuests()
    {
        return availableQuests;
    }

    private void Update()
    {
        if (QuestLogUI.QuestUIActive) return;
        if (Input.GetButton("Quest"))
        {
            var turnIn = GetReadyToTurnInQuest();
            if (turnIn != null)
            {
                QuestEvents.OnQuestTurnInRequested?.Invoke(turnIn);
            }
            else
            {
                var activeQuests = questManager.GetActiveQuests();
                if (activeQuests.Count > 0)
                {
                    QuestEvents.OnQuestOfferRequested?.Invoke(activeQuests[0]);
                }
                else
                {
                    QuestSO nextOffer = GetNextAvailableQuest();
                    if (nextOffer == null)
                    {
                        nextOffer = questToOffer;
                    }
                    if (nextOffer != null)
                    {
                        QuestEvents.OnQuestOfferRequested?.Invoke(nextOffer);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}

