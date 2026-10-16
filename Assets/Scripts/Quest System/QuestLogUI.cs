using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class QuestLogUI : MonoBehaviour
{
    void Start()
    {
        Debug.Log("QuestLogUI Start called. GameObject active: " + gameObject.activeInHierarchy);
        if (UnityEngine.Object.FindFirstObjectByType<EventSystem>() == null)
        {
            Debug.LogWarning("No EventSystem found! Create one: GameObject > UI > EventSystem");
        }
    }

    [SerializeField] private QuestManager questManager;
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questDescriptionText;
    [SerializeField] private QuestObjectiveSlot[] objectiveSlots;
    [SerializeField] private QuestRewardSlot[] rewardSlots;

    private QuestSO questSO;
    [SerializeField] private QuestSO noAvailableQuestSO;
    [SerializeField] private QuestLogSlot[] questSlots;

    [SerializeField] private CanvasGroup questcanvas;
    [SerializeField] private CanvasGroup acceptcanvas;
    [SerializeField] private CanvasGroup declinecanvas;
    [SerializeField] private CanvasGroup completecanvas;

    [SerializeField] private QuestBoard questBoard;

    private void OnEnable()
    {
        QuestEvents.OnQuestOfferRequested += ShowQuestOffer;
        QuestEvents.OnQuestTurnInRequested += ShowQuestTurnIn;
        QuestEvents.OnQuestAccepted += OnQuestStateChanged;
        QuestEvents.OnQuestCompleted += OnQuestStateChanged;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestOfferRequested -= ShowQuestOffer;
        QuestEvents.OnQuestTurnInRequested -= ShowQuestTurnIn;
        QuestEvents.OnQuestAccepted -= OnQuestStateChanged;
        QuestEvents.OnQuestCompleted -= OnQuestStateChanged;
    }

    #region Show Quest Methods
    public void ShowQuestOffer(QuestSO incomingQuestSO)
    {
        Debug.Log("ShowQuestOffer: " + incomingQuestSO.questName);

        if(questManager.IsQuestAccepted(incomingQuestSO) || questManager.GetCompleteQuest(incomingQuestSO))
        {
            // Show quest log for accepted/completed
            SetCanvasGroup(acceptcanvas, false);
            SetCanvasGroup(declinecanvas, false);
            SetCanvasGroup(completecanvas, false);
            RefreshQuestList();
            List<QuestSO> activeQuests = questManager.GetActiveQuests();
            QuestSO toShow;
            if (activeQuests.Count > 0)
            {
                toShow = activeQuests[0];
            }
            else
            {
            List<QuestSO> availableQuests = new List<QuestSO>();
            foreach(var board in UnityEngine.Object.FindObjectsByType<QuestBoard>(FindObjectsSortMode.None))
            {
                availableQuests.AddRange(questManager.GetAvailableQuests(board));
            }
                toShow = availableQuests.Count > 0 ? availableQuests[0] : noAvailableQuestSO;
            }
            HandleQuestClicked(toShow);
            SetCanvasGroup(questcanvas, true);
            return;
        }
        questSO = incomingQuestSO;
        SetCanvasGroup(acceptcanvas, true);
        SetCanvasGroup(declinecanvas, true);
        SetCanvasGroup(completecanvas, false);
        SetCanvasGroup(questcanvas, true);
        HandleQuestClicked(questSO);
    }
    public void ShowQuestTurnIn(QuestSO incomingQuestSO)
    {
        if (incomingQuestSO == null) return;
        questSO = incomingQuestSO;

        HandleQuestClicked(questSO);

        SetCanvasGroup(acceptcanvas, false);
        SetCanvasGroup(declinecanvas, false);
        SetCanvasGroup(completecanvas, true);
        SetCanvasGroup(questcanvas, true);
        
    }
    #endregion

    #region On Button Clicked Methods
    public void OnAcceptQuestClicked()
    {
        QuestSound.Instance?.PlayAcceptSound();
        Debug.Log("Accepting quest: " + questSO.questName);
        QuestEvents.OnQuestAccepted?.Invoke(questSO);
        questManager.AcceptQuest(questSO);
        SetCanvasGroup(acceptcanvas, false);
        SetCanvasGroup(completecanvas, false);
        SetCanvasGroup(declinecanvas, false);
        RefreshQuestList();
        Debug.Log("Showing accepted quest: " + questSO.questName);
        questSO = questSO; // ensure pin
        HandleQuestClicked(questSO);
    }
    public void OnDeclineQuestClicked()
    {
        QuestSound.Instance?.PlayDeclineSound();
        Debug.Log("DeclineQuestClicked: " + questSO?.questName + " (hiding UI, quest intact)");
        SetCanvasGroup(questcanvas, false);
        SetCanvasGroup(acceptcanvas, false);
        SetCanvasGroup(declinecanvas, false);
        SetCanvasGroup(completecanvas, false);
        // Note: QuestLogUI QuestUIActive = false automatically
    }
    public void OnCompleteQuestClicked()
    {
        QuestSound.Instance?.PlayCompleteSound();
        Debug.Log("CompleteQuestClicked: " + questSO.questName);
        questManager.CompleteQuest(questSO);
        RefreshQuestList();
        List<QuestSO> activeQuests = questManager.GetActiveQuests();
        QuestSO toShow;
        if (activeQuests.Count > 0)
        {
            toShow = activeQuests[0];
        }
        else
        {
            List<QuestSO> fallbackQuests = new List<QuestSO> { noAvailableQuestSO };
            var available = questBoard != null ? questManager.GetAvailableQuests(questBoard) : fallbackQuests;
            toShow = available.Count > 0 ? available[0] : noAvailableQuestSO;
        }
        HandleQuestClicked(toShow);
        SetCanvasGroup(completecanvas, false);

        // Auto-offer next quest from board after completion
        {
            if (questBoard != null)
            {
                QuestSO nextQuest = questBoard.GetNextAvailableQuest();
                if (nextQuest != null)
                {
                    ShowQuestOffer(nextQuest);
                }
            }
        }
    }
#endregion

    public void OnCloseButtonClicked()
    {
        Debug.Log("OnCloseButtonClicked (hiding UI, quest intact)");
        SetCanvasGroup(questcanvas, false);
        SetCanvasGroup(acceptcanvas, false);
        SetCanvasGroup(declinecanvas, false);
        SetCanvasGroup(completecanvas, false);
    }

    public static bool QuestUIActive = false;
    private void SetCanvasGroup(CanvasGroup canvasGroup, bool activate)
    {
        canvasGroup.alpha = activate ? 1 : 0;
        canvasGroup.blocksRaycasts = activate;
        canvasGroup.interactable = activate;
        if (canvasGroup == questcanvas)
        {
            QuestUIActive = activate;
        }
    }
    private void RefreshQuestList()
    {
        List<QuestSO> activeQuests = questManager.GetActiveQuests();
        List<QuestSO> availableQuests = new List<QuestSO>();
        foreach(var board in UnityEngine.Object.FindObjectsByType<QuestBoard>(FindObjectsSortMode.None))
        {
            availableQuests.AddRange(questManager.GetAvailableQuests(board));
        }
        List<QuestSO> allQuests = activeQuests.Concat(availableQuests).ToList();
        
        for (int i = 0; i < questSlots.Length; i++)
        {
            if (i < allQuests.Count)
            {
                questSlots[i].SetQuest(allQuests[i]);
            }
            else
            {
                questSlots[i].ClearSlot();
            }
        }
    }

    private void OnQuestStateChanged(QuestSO quest)
    {
        if (questcanvas.alpha > 0)
        {
            RefreshQuestList();
        }
    }

    public void HandleQuestClicked(QuestSO questSO)
    {
        if (questSO == null || questManager == null) return;
        this.questSO = questSO;
        questNameText.text = questSO.questName;
        questDescriptionText.text = questSO.questDescription;

        DisplayObjectives();
        DisplayRewards();

        bool isActive = questManager.IsQuestAccepted(questSO);
        bool isActiveAndComplete = isActive && questManager.IsQuestComplete(questSO);
        
        SetCanvasGroup(completecanvas, isActiveAndComplete);
        SetCanvasGroup(acceptcanvas, !isActive);
        SetCanvasGroup(declinecanvas, !isActive);
    }

    private void DisplayObjectives()
    {
        for (int i = 0; i < objectiveSlots.Length; i++)
        {
            if (i < questSO.objectives.Count)
            {
                var objective = questSO.objectives[i];
                questManager.UpdateObjectiveProgress(questSO, objective);

                int currentAmount = questManager.GetCurrentAmount(questSO, objective);
                string progress = questManager.GetProgressText(questSO, objective);
                bool isComplete = currentAmount >= objective.requiredAmount;

                objectiveSlots[i].gameObject.SetActive(true);
                objectiveSlots[i].RefreshObjectives(objective.description, progress, isComplete);
            }
            else
            {
                objectiveSlots[i].gameObject.SetActive(false);
            }
        }
    }
    public void DisplayRewards()
    {
        for (int i = 0; i < rewardSlots.Length; i++)
        {
            if (i < questSO.rewards.Count)
            {
                var reward = questSO.rewards[i];
                rewardSlots[i].DisplayReward(reward.itemSO.icon, reward.quantity);
                rewardSlots[i].gameObject.SetActive(true);
            }
            else
            {
                rewardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        
        if (questcanvas.alpha > 0f && questSO != null)
        {
            bool showComplete = questManager.IsQuestAccepted(questSO) && questManager.IsQuestComplete(questSO);
            SetCanvasGroup(completecanvas, showComplete);
        
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Z))
            {
                OnAcceptQuestClicked();
            }
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.X))
            {
                OnDeclineQuestClicked();
            }
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.C))
            {
                OnCompleteQuestClicked();
            }
        }

        // Keyboard to open quest log near board (kept as requested)
        if (questBoard != null && questBoard.PlayerInRange && questcanvas.alpha < 1f && Input.GetKeyDown(KeyCode.I))
        {
            QuestSO nextQuest = questBoard.GetNextAvailableQuest();
            if (nextQuest != null)
            {
                QuestEvents.OnQuestOfferRequested?.Invoke(nextQuest);
            }
            else if (questBoard.questToOffer != null)
            {
                QuestEvents.OnQuestOfferRequested?.Invoke(questBoard.questToOffer);
            }
        }
    }
}
