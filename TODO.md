# Fix GameController MissingReferenceException

- [x] Analyze root cause (DontDestroyOnLoad + stale scene references)
- [x] Read dependent scripts (GameManager, GameStateManager, GameInitializer)
- [x] Remove DontDestroyOnLoad from GameController.Awake
- [x] Add null checks in ActivateCutscene
- [x] Add null checks in OnCutsceneFinished
- [x] Fix Update() race condition (return after PlayCutscene3)
- [x] Clean up end-game transition

