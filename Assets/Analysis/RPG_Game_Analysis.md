# GAME OVERVIEW

**Genre**: 2D Top-Down Action RPG built in Unity (URP, InputSystem, Timeline for cutscenes).  
**Core Gameplay Loop**: Main Menu → Intro Cutscene → Tilemap exploration & real-time combat → Enemy kills for EXP/loot → NPC dialogue & quests → Inventory/shop management → Leveling & skill tree → Boss fight → Victory/Death cutscene → Main Menu.  
**Player Abilities**: WASD movement (speed-scaled, sprite flip), melee attack (cooldown+AOE damage/knockback via StatsManager), bow (partial), item hotkeys (1-0 use/drop), knockback resilience.  
**Enemy Types**: Patrol AI (Idle/Chase/Attack FSM), basic melee counter-attack, knockback/stun, boss variants (music trigger, prelude dialogue).  
**Boss Mechanics**: Trigger zone (dialogue+music), larger HP/EXP, death triggers Cutscene4.  
**Health/Stats/Leveling**: StatsManager singleton (health=100, damage=1, speed=5, etc.), EXP slider (1.2x growth), levels grant skill points.  
**Inventory**: Fixed slots, stackable ItemSOs (Sword/Shield/Meat/Gold/EXP potions), gold currency, auto-drop on full.  
**UI**: Health/EXP/Level HUD, StatsUI, QuestLog, SkillTree, Dialogue canvas (portrait+choices), Inventory slots, Pause/MainMenu.  
**Cutscenes**: 4 Timeline-based (PlayableDirector): C1=Intro, C2=Trigger, C3=Player Death, C4=Boss Death → Main Menu.

# CORE SYSTEMS

## Player System

**Purpose**: Handles movement, combat, health, stats integration.  
**Key Scripts**:

- `PlayerMovement.cs`: WASD input, Rigidbody2D velocity (Stats.speed), sprite flip, knockback coroutine, attack input trigger.
- `Player_Combat.cs`: Attack() on input (anim trigger→DealDamage() after delay), Physics2D.OverlapCircleAll(enemies), apply Stats.damage+knockback.
- `PlayerHealth.cs`: ChangeHealth() clamps 0-Stats.maxHealth, updates TMP UI, death disables movement/collider→GameController.PlayCutscene3().
- `StatsManager.cs`: Singleton DontDestroy, combat/movement/health stats w/ static fallbacks, UI updates.
- `ExpManager.cs`: EXP gains (enemy death/EXP items), level up (1.2x req), skill points event.
  **Interactions**: Stats drive all values dynamically; knockback↔enemy; health→cutscene.

## Combat System

**Purpose**: Real-time ARPG combat with stats, knockback, cooldowns.  
**Key Scripts**: Player_Combat.cs, Enemy_Combat.cs, StatsManager.cs.  
**Mechanics**: Player melee AOE (weaponRange), enemy FSM→attack when close. Bidirectional knockback/stun. Damage via Stats.damage. Cooldowns prevent spam. Gizmos visualize ranges.

## Enemy System

**Purpose**: Basic AI foes for EXP/loot grinding.  
**Key Scripts**:

- `Enemy_Movements.cs`: FSM (Idle/Chase/Attack/Knockback), Physics2D detect player (detectionPoint+range), Chase() normalized velocity, Attack→Enemy_Combat.Attack().
- `Enemy_Combat.cs`: Attack() AOE player damage+knockback.
- `Enemy_Health.cs`: ChangeHealth(), 0HP→Destroy() + OnMonsterDefeated(expReward).
- `Enemy_Knockback.cs`: Apply velocity+stun→Idle.
- `Healthbar.cs`: UI (likely follows enemy).
  **Behaviors**: Patrol? → Detect → Chase (speed) → Attack range → Hit → Knockback stun → resume. BossMusic/BossTrigger extras.

## Boss System

**Purpose**: Climax encounters w/ prelude/special effects.  
**Key Scripts**: BossTrigger.cs (player enter→music/dialogue?), BossMusic.cs, Enemy variants. Death null→GameController.PlayCutscene4().

## Inventory System

**Purpose**: Loot collection, consumables, shopping.  
**Key Scripts**:

- `Inventory Manager.cs`: Singleton, fixed InventorySlot[] (stacks), AddItem() fill/overflow drop Loot prefab near player, hotkeys 1-0 (use/drop w/Shift), gold UI.
- `ItemSO.cs`: Stackable data (Sword/Shield/Meat/Gold/EXP), UseItem.ApplyEffects().
  **Interactions**: Loot.OnItemLooted→add, quests consume, shops spend gold.

## UI System

**Purpose**: HUD, menus, dialogue, logs.  
**Key Scripts**: StatsUI.cs, healthText TMP, expSlider, Pause Menu.cs, MainMenu.cs (reset+load), QuestLogUI.cs, SkillTreeManager.cs (buttons+points).

## Dialogue System

**Purpose**: Branching NPC conversations, quest gating.  
**Key Scripts**:

- `DialogueManager.cs`: CanvasGroup fade, DialogueSO.lines (speaker/portrait/text), choices→next SO, integrates quests (offer/turn-in).
- `DialogueTrigger.cs`: Collider→simple message fade or fallback Manager.
- Trackers: DialogueHistoryTracker (NPC spoken), LocationHistoryTracker/VisitedTracker.
  **Flow**: Trigger→Show lines→Choices→Next/End/Quest event.

## Cutscene System

**Purpose**: Narrative breaks, intro/outro.  
**Key Scripts**: `GameController.cs` singleton: PlayCutscene1-4 (PlayableDirector+signals), disable gameplay→Timeline→stopped event→resume or MainMenu. Space skip. Triggers: auto/start, collider(C2), health=0(C3), boss=null(C4).

## Camera System

**Purpose**: Follow player (likely Cinemachine implied, CameraFix.cs exists but content unknown - probably orthographic fix).

## GameController / Game Flow

**Singletons** (DontDestroy): GameController (cutscenes), StatsManager, InventoryManager, GameManager (dialogue/quest refs).  
**Progression**: SceneChanger (fade+teleport position), RoadUnlocker/RoadTrigger (path gating), Elevation_Entry/Exit. GameInitializer resets stats/inv. MainMenu→reset→load.

# GAME FLOW

Main Menu (Play/Quit, reset stats)  
↓  
RPG Scene Load + GameInitializer reset  
↓  
Cutscene1 (intro)  
↓  
**Exploration Loop**: Move/fight enemies (EXP/loot drop) → Inventory hotkeys → NPC talk (quests/dialogue tree) → QuestBoard check → Shop (gold spend) → SkillTree spend points → Stats grow  
↓ Triggers  
Elevation/Road unlocks → SceneChanger teleports → Next area (Scene1→2→...6?)  
↓ Boss area  
BossTrigger (music+dialogue C2) → Boss fight  
↓  
**Win**: Boss death → Cutscene4 → Main Menu  
**Lose**: Player HP=0 → Cutscene3 → Main Menu

# GAME WORLD & LORE

**Setting**: Generic fantasy overworld - tilemap areas w/ elevation changes, roads (unlockable gates), multiple scenes suggest villages/forests/dungeons progression.  
**Characters**: Color-coded Bobs (Blue/Yellow/Purple), Soldiers, Unknown_Yellow, Night Runner - villagers/guards/quest givers.  
**Enemies**: Generic melee mobs (claw/bite implied by SFX), bosses thematic (earth/fire?).  
**Environments**: RPG Scene1-6 (team-built variants), elevation layers, loot drops, quest boards/shops.  
**Themes**: Hero rises via grinding → quests → boss climax (narrative cutscenes).

# POSSIBLE STORY (INFERRED)

Lonely wanderer (player) enters village (Scene1) → meets Bobs/Soldiers → quests against Night Runners/thieves → unlocks roads to boss lair → defeats evil boss → credits (C4) or retry (C3). Lore via dialogue SOs (greetings, hidden continues).

# QUEST IDEAS (BASED ON SYSTEMS)

1. **Collect X Meat** (kill animals, inventory track). Reward: Gold/EXP pot.
2. **Talk to Yellow Bob** (history tracker gate). Reward: Sword.
3. **Visit SoldierMain location** (location tracker). Reward: Shield.
4. **Kill 5 enemies** (manual? extend QuestObjective). Reward: Skill points.
5. **Boss prelude chain** (dialogue→quest→elevation unlock).

# GAMEPLAY FEATURES SUMMARY

| Feature       | Status   | Details                                                                            |
| ------------- | -------- | ---------------------------------------------------------------------------------- |
| Movement      | Complete | Top-down, stats-speed, flip, knockback.                                            |
| Combat        | Core+    | Melee AOE, FSM enemy AI, bidirectional KB.                                         |
| Progression   | Strong   | EXP→level→skills/stats, quests (multi-type).                                       |
| Inventory     | Full     | Slots+stacks+hotkeys+drop+shop.                                                    |
| Dialogue      | Advanced | Branching+history+quests.                                                          |
| Cutscenes     | Polished | Timeline w/ triggers/signals.                                                      |
| Polish        | Medium   | SFX packs, URP, InputSystem, but needs polish (bow complete, more quests/enemies). |
| Extensibility | High     | SO-driven (items/quests/dialogue), events clean.                                   |
