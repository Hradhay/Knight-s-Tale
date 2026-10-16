// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     public DialogueManager DialogueManager;
//     public DialogueHistoryTracker DialogueHistoryTracker;
//     public LocationHistoryTracker LocationHistoryTracker;
//     public QuestManager QuestManager;

//     [Header("Persistent Objects")]
//     public GameObject[] persistentObjects;

//     private void Awake()
//     {
//         if (Instance != null)
//         {
//             CleanUpAndDestroy();
//             return;
//         }
//         Instance = this;
//         DontDestroyOnLoad(gameObject);
//         MarkPersistentObjects();
//     }


//     private void MarkPersistentObjects()
//     {
//         foreach (GameObject obj in persistentObjects)
//         {
//             if (obj != null){
//                 DontDestroyOnLoad(obj);
//             }
//         }
//     }


//     private void CleanUpAndDestroy()
//     {
//         foreach (GameObject obj in persistentObjects)
//         {
//             if (obj != null)
//             {
//                 Destroy(obj);
//             }
//         }
//         Destroy(gameObject);
//     }
// }


using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DialogueManager DialogueManager;
    public DialogueHistoryTracker DialogueHistoryTracker;
    public LocationHistoryTracker LocationHistoryTracker;
    public QuestManager QuestManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // ❌ REMOVE DontDestroyOnLoad
        // ❌ REMOVE persistentObjects logic
    }
}