using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Cutscenes")]
    public GameObject cutscene1;
    public GameObject cutscene2;
    public GameObject cutscene3;
    public GameObject cutscene4;

    [Header("Directors")]
    public PlayableDirector director1;
    public PlayableDirector director2;
    public PlayableDirector director3;
    public PlayableDirector director4;

    [Header("Game Objects")]
    public GameObject gameObjectToEnable;
    public GameObject player;
    public GameObject boss;

    private bool hasStarted = false;
    private bool cutscene2Played = false;
    private bool cutscene3Played = false;
    private bool cutscene4Played = false;
    private bool playerDeadHandled = false;

    // 🔥 Track current cutscene
    private GameObject currentCutscene;
    private PlayableDirector currentDirector;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    void Start()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            PlayCutscene1();
        }
    }

    // 🎬 CUTSCENE 1
    public void PlayCutscene1()
    {
        ActivateCutscene(cutscene1, director1);
    }

    // 🎬 CUTSCENE 2 (Trigger manually via collider)
    public void PlayCutscene2()
    {
        if (cutscene2Played) return;

        cutscene2Played = true;
        ActivateCutscene(cutscene2, director2);
    }

    // 🎬 CUTSCENE 3 (Player Death)
    public void PlayCutscene3()
    {
        if (cutscene3Played) return;

        cutscene3Played = true;
        ActivateCutscene(cutscene3, director3);
    }

    // 🎬 CUTSCENE 4 (Boss Death)
    public void PlayCutscene4()
    {
        if (cutscene4Played) return;

        cutscene4Played = true;
        ActivateCutscene(cutscene4, director4);
    }

    // 🔥 COMMON CUTSCENE HANDLER
    void ActivateCutscene(GameObject cutscene, PlayableDirector director)
    {
        // 🔥 Remove old listener (VERY IMPORTANT)
        if (currentDirector != null)
        {
            currentDirector.stopped -= OnCutsceneFinished;
        }

        // Disable gameplay
        if (gameObjectToEnable != null)
            gameObjectToEnable.SetActive(false);
        else
            Debug.LogWarning("GameController: gameObjectToEnable reference is missing!");

        // Enable cutscene
        if (cutscene != null)
            cutscene.SetActive(true);
        else
            Debug.LogWarning("GameController: cutscene GameObject is missing!");

        currentCutscene = cutscene;
        currentDirector = director;

        if (director != null)
        {
            director.stopped += OnCutsceneFinished;
            director.Play();
        }
    }

    // 🎯 CUTSCENE END HANDLER
    void OnCutsceneFinished(PlayableDirector director)
    {
        director.stopped -= OnCutsceneFinished;

        if (currentCutscene != null)
            currentCutscene.SetActive(false);

        Time.timeScale = 1f;

        // 🎬 Ending cutscenes → go to Main Menu
        if (director == director3 || director == director4)
        {
            Debug.Log("Game End → Loading Main Menu");
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            // Resume gameplay
            if (gameObjectToEnable != null)
                gameObjectToEnable.SetActive(true);
        }
    }

    void Update()
    {
        // ⏭️ Skip cutscene
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipCutscene();
        }

        // 🔥 PLAYER DEATH (FIXED CONDITION)
        if (!playerDeadHandled &&
            player != null &&
            StatsManager.Instance != null &&
            StatsManager.Instance.currentHealth <= 0)
        {
            playerDeadHandled = true;

            Debug.Log("Player died → Cutscene 3");

            PlayCutscene3();
            return; // Prevent boss death check in the same frame
        }

        // 🔥 BOSS DEATH
        if (boss == null && !cutscene4Played)
        {
            Debug.Log("Boss died → Cutscene 4");

            PlayCutscene4();
        }
    }

    // ⏭️ SKIP FUNCTION
    void SkipCutscene()
    {
        if (currentDirector != null)
        {
            currentDirector.Stop(); // triggers OnCutsceneFinished
        }
    }
}
// ----------------------------------------------------------------------------------------------------------------------
// using UnityEngine;
// using UnityEngine.Playables;
// using UnityEngine.SceneManagement;

// public class GameController : MonoBehaviour
// {
//     public static GameController instance;

//     [Header("Cutscenes")]
//     public GameObject cutscene1;
//     public GameObject cutscene2;
//     public GameObject cutscene3;
//     public GameObject cutscene4;

//     [Header("Directors")]
//     public PlayableDirector director1;
//     public PlayableDirector director2;
//     public PlayableDirector director3;
//     public PlayableDirector director4;

//     [Header("Game Objects")]
//     public GameObject gameObjectToEnable;
//     public GameObject player;
// public GameObject boss;
// public GameObject pauseMenu;

//     private bool hasStarted = false;
//     private bool cutscene2Played = false;
//     private bool cutscene3Played = false;
//     private bool cutscene4Played = false;
//     private bool playerDeadHandled = false;

//     // 🔥 Track current cutscene
//     private GameObject currentCutscene;
//     private PlayableDirector currentDirector;

//     void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     void OnDestroy()
// {
//     // 🔥 Clear singleton reference when object is destroyed
//     if (instance == this)
//     {
//         instance = null;
//     }
// }

//     void Start()
//     {
//         if (!hasStarted)
//         {
//             hasStarted = true;
//             PlayCutscene1();
//         }
//     }

//     // 🎬 CUTSCENE 1
//     public void PlayCutscene1()
//     {
//         ActivateCutscene(cutscene1, director1);
//     }

//     // 🎬 CUTSCENE 2
//     public void PlayCutscene2()
//     {
//         if (cutscene2Played) return;

//         cutscene2Played = true;
//         ActivateCutscene(cutscene2, director2);
//     }

//     // 🎬 CUTSCENE 3 (Player Death)
//     public void PlayCutscene3()
//     {
//         if (cutscene3Played) return;

//         cutscene3Played = true;
//         ActivateCutscene(cutscene3, director3);
//     }

//     // 🎬 CUTSCENE 4 (Boss Death)
//     public void PlayCutscene4()
//     {
//         if (cutscene4Played) return;

//         cutscene4Played = true;
//         ActivateCutscene(cutscene4, director4);
//     }

//     // 🔥 COMMON FUNCTION (FIXED)
//     void ActivateCutscene(GameObject cutscene, PlayableDirector director)
//     {
//         // Disable gameplay
//         gameObjectToEnable.SetActive(false);

//         // Enable cutscene
//         cutscene.SetActive(true);

//         currentCutscene = cutscene;
//         currentDirector = director;

//         if (director != null)
//         {
//             // 🔥 Listen for timeline end
//             director.stopped += OnCutsceneFinished;
//             director.Play();
//         }
//     }

//     // 🎯 AUTOMATIC END (MAIN FIX)
// void OnCutsceneFinished(PlayableDirector director)
// {
//     director.stopped -= OnCutsceneFinished;

//     if (currentCutscene != null)
//         currentCutscene.SetActive(false);

//     Time.timeScale = 1f;

//     // 🎯 END GAME → LOAD MENU
//     if (director == director3 || director == director4)
//     {
//         Debug.Log("Destroying GameController and loading Main Menu");

//         // 🔥 DESTROY THIS OBJECT FIRST
//         Destroy(gameObject);

//         // 🔥 LOAD MENU SCENE
//         SceneManager.LoadScene("Main Menu");
//     }
//     else
//     {
//         gameObjectToEnable.SetActive(true);
//     }
// }


//    void Update()
// {
//     // ⏭️ Skip
//     if (Input.GetKeyDown(KeyCode.Space))
//     {
//         SkipAll();
//     }

//     // 🔥 Player death via health system
//     if (!playerDeadHandled && StatsManager.Instance.currentHealth <= 0 || !player.activeSelf)
//     {
//         playerDeadHandled = true;

//         Debug.Log("Player died via health system");

//         PlayCutscene3();
//     }

//     // 🔥 Boss destroyed → Cutscene 4
//     if (boss == null && !cutscene4Played)
//     {
//         PlayCutscene4();
//     }
// }

//     void SkipAll()
//     {
//         currentDirector?.Stop();
//         gameObjectToEnable.SetActive(true);
//     }
// }

// ------------------------------------------------------------------------------------
// using UnityEngine;
// using UnityEngine.Playables;

// public class GameController : MonoBehaviour
// {
//     public static GameController instance;

//     [Header("Cutscenes")]
//     public GameObject cutscene1;
//     public GameObject cutscene2;
//     public GameObject cutscene3;
//     public GameObject cutscene4;

//     [Header("Directors")]
//     public PlayableDirector director1;
//     public PlayableDirector director2;
//     public PlayableDirector director3;
//     public PlayableDirector director4;

//     [Header("Game Objects")]
//     public GameObject gameObjectToEnable;
//     public GameObject player;
//     public GameObject boss;
//     public GameObject pauseMenu;

//     private bool hasStarted = false;
//     private bool cutscene2Played = false;
//     private bool cutscene3Played = false;
//     private bool cutscene4Played = false;

//     void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     void Start()
//     {
//         if (!hasStarted)
//         {
//             hasStarted = true;
//             PlayCutscene1();
//         }
//     }

//     // 🎬 CUTSCENE 1
//     public void PlayCutscene1()
//     {
//         ActivateCutscene(cutscene1, director1);
//     }

//     // 🎬 CUTSCENE 2 (Trigger)
//     public void PlayCutscene2()
//     {
//         if (cutscene2Played) return;

//         cutscene2Played = true;
//         ActivateCutscene(cutscene2, director2);
//     }

//     // 🎬 CUTSCENE 3 (Player Death)
//     public void PlayCutscene3()
//     {
//         if (cutscene3Played) return;

//         cutscene3Played = true;
//         ActivateCutscene(cutscene3, director3);
//     }

//     // 🎬 CUTSCENE 4 (Boss Death)
//     public void PlayCutscene4()
//     {
//         if (cutscene4Played) return;

//         cutscene4Played = true;
//         ActivateCutscene(cutscene4, director4);
//     }

//     // 🔥 COMMON FUNCTION
//     void ActivateCutscene(GameObject cutscene, PlayableDirector director)
//     {
//         gameObjectToEnable.SetActive(false); // pause gameplay
//         cutscene.SetActive(true);

//         if (director != null)
//             director.Play();
//     }

//     // 🎯 Normal End (Cutscene 1 & 2)
//     public void EndCutscene(GameObject cutscene)
//     {
//         cutscene.SetActive(false);
//         gameObjectToEnable.SetActive(true);
//     }

//     // 🎯 Special End (Cutscene 3 & 4)
//     public void EndCutsceneWithPause(GameObject cutscene)
//     {
//         cutscene.SetActive(false);
//         gameObjectToEnable.SetActive(true);

//         // Disable player
//         if (player != null)
//             player.SetActive(false);

//         // Show pause menu
//         if (pauseMenu != null)
//         {
//             pauseMenu.SetActive(true);
//             Time.timeScale = 0f; // pause game
//         }
//     }

//     void Update()
//     {
//         // ⏭️ Skip
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             SkipAll();
//         }

//         // 🔥 Player destroyed → Cutscene 3
//         if (player == null && !cutscene3Played)
//         {
//             PlayCutscene3();
//         }

//         // 🔥 Boss destroyed → Cutscene 4
//         if (boss == null && !cutscene4Played)
//         {
//             PlayCutscene4();
//         }
//     }

//     void SkipAll()
//     {
//         director1?.Stop();
//         director2?.Stop();
//         director3?.Stop();
//         director4?.Stop();

//         gameObjectToEnable.SetActive(true);
//     }
// }
// ------------------------------------------------------------------------------------
// using UnityEngine;
// using UnityEngine.Playables;

// public class GameController : MonoBehaviour
// {
//     public static GameController instance;

//     [Header("Scene References")]
//     public GameObject cutsceneObject;
//     public GameObject gameObjectToEnable;

//     [Header("Timeline")]
//     public PlayableDirector director;

//     private bool hasStarted = false;

//     void Awake()
//     {
//         // ✅ Prevent duplicates
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
//     }

//     void Start()
//     {
//         if (!hasStarted)
//         {
//             hasStarted = true;
//             StartCutscene();
//         }
//     }

//     // 🎬 Start Cutscene
//     public void StartCutscene()
//     {
//         if (cutsceneObject == null || gameObjectToEnable == null)
//         {
//             Debug.LogError("Assign Cutscene and Game objects!");
//             return;
//         }

//         cutsceneObject.SetActive(true);
//         gameObjectToEnable.SetActive(false);

//         Debug.Log("Cutscene Started");

//         // ▶️ Play Timeline
//         if (director != null)
//         {
//             director.Play();
//         }
//     }

//     // 🎯 THIS IS CALLED FROM TIMELINE SIGNAL
//     public void EndCutscene()
//     {
//         Debug.Log("Cutscene Ended via Timeline");

//         // Stop timeline (optional safety)
//         if (director != null)
//         {
//             director.Stop();
//         }

//         cutsceneObject.SetActive(false);
//         gameObjectToEnable.SetActive(true);

//         Debug.Log("Game Started");
//     }

//     void Update()
//     {
//         // ⏭️ Optional skip
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             EndCutscene();
//         }
//     }
// }