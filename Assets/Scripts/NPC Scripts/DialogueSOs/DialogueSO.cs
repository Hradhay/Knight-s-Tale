using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName="DialogueSO",menuName="Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
     public DialogueLine[] lines;     
     public DialogueOption[] options;

     [Header("Quest Offer(Optional)")]
     public QuestSO offerQuestOnEnd;
     [Header ("Completed Quest Requirements(Optional)")] 
     public QuestSO[] requiredCompletedQuests;
     [Header ("Quest Turn In(Optional)")]
     public QuestSO turnInQuestOnEnd;

     [Header("Conditional Requirements (Optional)")]
     public ActorSO[] requiredNPCs;
     public LocationSO[] requiredLocations;
     public ItemSO[] requiredItems;

     [Header("Control Flags")]
     public bool removeAfterPlay;
     public List<DialogueSO> removeTheseOnPlay;


    public bool IsConditionMet()
    {
        if (GameManager.Instance == null) return false;

        if(requiredNPCs != null && requiredNPCs.Length > 0){
            var tracker = GameManager.Instance.DialogueHistoryTracker;
            if (tracker == null) return false;
            foreach(var npc in requiredNPCs){
                if(npc == null || !tracker.HasSpokenWith(npc))
                    return false;
            }
        }

        if(requiredLocations != null && requiredLocations.Length >0){
            var locationTracker = GameManager.Instance.LocationHistoryTracker;
            if (locationTracker == null) return false;
            foreach(var location in requiredLocations){
                if(location == null || !locationTracker.HasVisited(location)){
                    return false;
                }
            }
        }

        if(requiredItems != null && requiredItems.Length > 0){
            var inventory = InventoryManager.Instance;
            if (inventory == null) return false;
            foreach(var item in requiredItems){
                if(item == null || !inventory.HasItem(item)){
                    return false;
                }
            }
        }


        return true;
    }

//     public bool IsConditionMet()
//     {
//         if (GameManager.instance == null) return false;

//         // NPCs
//         if (requiredNPCs != null && requiredNPCs.Length > 0)
//         {
//             var tracker = GameManager.instance.DialogueHistoryTracker;
//             if (tracker == null) return false;

//             foreach (var npc in requiredNPCs)
//             {
//                 if (npc == null) continue;

//                 if (!tracker.HasSpokenWith(npc))
//                     return false;
//             }
//         } 

//         // Locations
//         if (requiredLocations != null && requiredLocations.Length > 0)
//         {
//             var locationTracker = GameManager.instance.LocationHistoryTracker;
//             if (locationTracker == null) return false;

//             foreach (var location in requiredLocations)
//             {
//                 if (location == null) continue;

//                 if (!locationTracker.HasVisited(location))
//                     return false;
//             }
//         }

//         // Items
//         if (requiredItems != null && requiredItems.Length > 0)
//         {
//             var inventory = InventoryManager.instance;
//             if (inventory == null) return false;

//             foreach (var item in requiredItems)
//             {
//                 if (item == null) continue;

//                 if (!inventory.HasItem(item))
//                     return false;
//             }
//         }

//         return true;
//     }

}

[System.Serializable]
public class DialogueLine{
    public ActorSO speaker;
    [TextArea(3,5)] public string text;
}

[System.Serializable]
public class DialogueOption{
    public string optionText;
    public DialogueSO nextDialogue;
    public QuestSO offerQuest;
}