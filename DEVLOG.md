# Devlog

## Step 1 — Scene Bootstrap
- Set up `GameScene` with Player, orthographic Camera, and Directional Light
- Player tagged as `"Player"`, Rigidbody2D set to Dynamic, gravity scale 0
- Created `CameraFollow.cs` — smooth follow attached to Main Camera targeting Player
- Filled in `PlayerInputActions.inputactions` — `Player` action map with `Move` action (WASD + gamepad)
- Added `PlayerInput` component to Player, linked to `PlayerInputActions`, default map `"Player"`
- Camera background color set to dark green, orthographic size 3

## Step 2 — Player Health & Stats
- Created `IDamageable.cs` interface — `TakeDamage(float)` in `Scripts/Systems/`
- Created `PlayerStats.cs` — max health, current health, `Heal()`, `OnHealthChanged(current, max)` and `OnDeath` UnityEvents, implements `IDamageable`
- Attached `PlayerStats` to Player in `GameScene`

## Step 3 — Enemy Base
- Created `EnemyBase.cs` — abstract MonoBehaviour implementing `IDamageable`, chase AI via `Rigidbody2D.linearVelocity`, contact damage on `OnCollisionStay2D`
- Created `SlimeEnemy.cs` — first concrete subclass of `EnemyBase`
- Created `SlimeEnemy.prefab` in `Prefabs/Enemies/` — green rectangle, stats tweakable in Inspector

## Step 4 — Tilemap (chunk-based infinite ground)
- Created `GroundTile.png` (64x64, grid pattern) and `GroundTile.asset` in `Art/Sprites/Tiles/`
- Set up `Grid` + `Ground` Tilemap GameObject in scene, `TilemapRenderer` sorting order -1
- Created `TilemapChunkLoader.cs` — tracks player chunk every frame, loads/unloads 16x16 tile chunks within a radius of 3 around the player
- **Bug fixed:** `_lastPlayerChunk` initialized to `int.MaxValue` caused integer overflow on first `RefreshChunks()` call, loading garbage chunks — fixed by initializing it to the actual player chunk in `Start()`
- **Bug fixed:** tiles set via `SetTile()` at runtime were not rendering until `RefreshAllTiles()` was called after each chunk load

## Step 5 — Enemy Spawner & Wave System
- Created `EnemySpawner.cs` — spawns enemies off-screen at random positions around the camera, interval decreases over time (3s → 0.5s, ramps every 10s)
- Created `IWaveEvent.cs` — interface for special wave events (`Execute(EnemySpawner)`)
- Created `WaveEventManager.cs` — fixed-timeline event system, events disabled until Horde/Elite/Boss types are ready
- Added `EnemySpawner` and `WaveEventManager` GameObjects to scene, SlimeEnemy prefab wired up

## Step 6 — Weapon System
- Created `IWeapon.cs` interface — `Activate()`
- Created `WeaponManager.cs` — holds list of equipped weapons on Player
- Created `OrbProjectile.cs` — moves in a direction, triggers `TakeDamage` on enemy contact, self-destructs on hit or max range
- Created `OrbShooter.cs` — finds nearest enemy via `Physics2D.OverlapCircleAll`, fires an `OrbProjectile` toward it on cooldown
- Created `OrbProjectile.prefab` — yellow square, Dynamic Rigidbody2D (gravity 0), trigger CircleCollider2D
- Added `WeaponManager` + `OrbShooter` to Player, `OrbProjectile` prefab wired up
- Added `"Enemy"` tag and applied it to `SlimeEnemy` prefab
- **Bug fixed:** orb spawning at player position triggered `OnTriggerEnter2D` on player's own `IDamageable` — fixed by checking `Enemy` tag only
- **Bug fixed:** orb speed inconsistent due to physics depenetration when spawning inside player's collider — fixed by offsetting spawn position 0.8 units in fire direction
- Slime collider radius reduced to 0.4 to match sprite visual bounds
- `OrbShooter` cooldown set to 1.5s, only targets enemies visible in camera viewport, falls back to last known direction when none visible
- `OrbProjectile` collider radius reduced from 0.2 to 0.15 to match sprite size (scale 0.3 → radius 0.15)
- Player move speed reduced from 5 to 2.5
- **Bug fixed:** orb disappearing before hitting enemy — `detectionRadius` (15) was larger than `maxRange` (10), orb targeted enemies it couldn't reach — fixed by setting detectionRadius=12, maxRange=15
- **Bug fixed:** SlimeEnemy using CircleCollider2D which was much larger than rectangular sprite — switched to BoxCollider2D sized to sprite bounds (0.16 x 0.16 local)
- **Bug fixed:** player CircleCollider2D too large (default 0.5 radius * 1.5 scale = 0.75 world units) — reduced to 0.1 local radius (0.15 world units) to match sprite
- Orb size increased to scale 0.75 (half of player visual size), collider radius matched (0.075)
- Orb spawn offset reduced from 0.8 to 0.2 — just past player collider edge to avoid physics depenetration while fixing close-range misses

## Step 7 — Game Manager (survival timer, game over, restart)
- Created `GameManager.cs` — singleton, `GameState` enum (Playing/GameOver/Paused), `TriggerGameOver()` (sets timeScale=0), `RestartGame()` (reloads scene), `OnGameOver`/`OnGameRestart` UnityEvents
- Created `SurvivalTimer.cs` — tracks elapsed time, fires `OnTimerUpdated(float)` every frame while state is Playing
- Created `GameOverHandler.cs` — subscribes to `PlayerStats.OnDeath` at runtime, calls `GameManager.TriggerGameOver()`
- Created `UIManager.cs` — wires timer text and game over panel; subscribes to `SurvivalTimer.OnTimerUpdated` and `GameManager.OnGameOver` at runtime in `Start()`; wires restart button `onClick` at runtime
- Added `EventSystem` + `InputSystemUIInputModule` to scene (required for UI button clicks — was missing)
- Added 1-second invulnerability window to `PlayerStats` after each hit (`invulnerabilityDuration` tweakable in Inspector) to prevent damage spam
- **Bug fixed:** duplicate `UIManager` component on `UICanvas` — removed extra instance
- **Bug fixed:** `SurvivalTimer.OnTimerUpdated` had no listeners — moved wiring to `UIManager.Start()` as runtime `AddListener`
- **Bug fixed:** restart button unresponsive — root cause was missing `EventSystem` in scene, not listener persistence

## Step 8 — UI (main menu, in-game HUD, pause menu, run summary)
- Created `MainMenuScene` — black background canvas, Play button (loads GameScene), Quit button (exits play mode in editor / quits in build), `MainMenuManager.cs`
- Added `EventSystem` + `InputSystemUIInputModule` to both scenes
- In-game HUD: health bar (top-left, green fill shrinks via `RectTransform` width), timer (top-center), kill counter (between health and timer, font 40)
- Kill tracking: `GameManager.RegisterKill()` called from `EnemyBase.Die()`, `KillCount` property exposed
- Pause menu: ESC toggles pause, window frame (border + dark bg), Resume and Main Menu buttons, calls `GameManager.PauseGame()`/`ResumeGame()`
- Run summary screen: GAME OVER title, time survived, kill count, Restart and Main Menu buttons, window frame
- Both scenes added to Build Settings (MainMenuScene index 0, GameScene index 1)
- **Bug fixed:** `Image.Type.Filled` didn't clip without a sprite — switched to `RectTransform.SetSizeWithCurrentAnchors` for health bar
- **Bug fixed:** `UIManager.Update()` null ref when `GameManager.Instance` not yet initialised — added null guard
- **Bug fixed:** `UIManager` accidentally added to `MainMenuScene` canvas — removed; duplicate `MainMenuManager` also cleaned up
- **Bug fixed:** serialized refs (killText, healthBarFill, summaryTexts) lost on script rewrites — rewired via `SerializedObject` after each compile

## Step 9 — XP System
- Created `PlayerXP.cs` — tracks current XP, level, `XPToNextLevel` (scales ×1.2 per level), fires `OnXPChanged(float,float)` and `OnLevelUp(int)` UnityEvents
- Created `XPGem.cs` — collectible drop, Kinematic Rigidbody2D + Interpolate for smooth movement; attracted to player within `pickupRadius` (2 units), moves via `MovePosition` at `speed` 3.5 units/s, destroys on contact and calls `PlayerXP.AddXP()`
- Created `XPGem.prefab` — UISprite, blue color, scale 0.75, rotated 45° (diamond shape), isTrigger CircleCollider2D radius 0.15
- Updated `EnemyBase.cs` — `Die()` spawns XP gem with 66% drop chance
- Updated `UIManager.cs` — XP bar (same width-scaling pattern as health bar, starts empty), level text `Lv N`
- **Bug fixed:** XP bar started fully filled — added `xpBarFill.SetSizeWithCurrentAnchors(0f)` in `Start()`
- **Bug fixed:** XP gem invisible — sprite was not set on prefab; set to UISprite (built-in)
- **Bug fixed:** Gem not collecting — `CircleCollider2D.isTrigger` was false; contact detection also added as fallback via distance check in `FixedUpdate`
- **Bug fixed:** Gem slow/inconsistent speed — Dynamic RB2D with physics drag fought velocity; switched to Kinematic + MovePosition for physics-independent smooth movement

## Step 10 — Perk System
- Expanded `IWeapon` interface — added `Damage`, `FireRateBonus`, `ProjectileSpeed`, `Range`, `ProjectileCount` properties
- Updated `OrbShooter` — implements all `IWeapon` properties via public get/set backed by serialized fields; `baseCooldown` + `_fireRateBonus` accumulator with `EffectiveCooldown = baseCooldown / (1 + bonus)` for diminishing returns; multi-shot fires burst via coroutine (0.12s between shots, all in same direction)
- Created `PerkSO.cs` — ScriptableObject with `WeaponStat` enum (Damage, FireRate, ProjectileSpeed, Range, ProjectileCount), `value` field; `Apply(GameObject)` modifies weapon via `IWeapon` interface
- Created 5 perk assets in `Assets/_Game/Perks/` — Damage ×1.2, FireRate +0.2, ProjectileSpeed ×1.2, Range ×1.2, ProjectileCount +1
- Created `PerkManager.cs` — attached to GameManager GO, holds perk pool, picks 3 random perks on level up, pauses game, fires `OnPerkChoicesReady`; `SelectPerk()` applies perk and resumes
- Created `PerkSelectionUI.cs` — subscribes to `OnPerkChoicesReady`, spawns 3 `PerkCard` instances in horizontal layout, calls `PerkManager.SelectPerk` on click
- Created `PerkCard.prefab` — white rectangle, dark Outline component, VerticalLayoutGroup, Name (bold 20pt) + Description (14pt) TMP text, Button component
- Added `PerkSelectionPanel` to GameScene — full-screen dark overlay, "LEVEL UP!" title, CardsContainer with HorizontalLayoutGroup

## Planned
- Step 11 — Object pooling (pool enemies and projectiles for performance)
- Step 12 — Damage numbers (floating combat text, pooled, shown on hit)
- Step 13 — Weapon variety (new weapon types, passive items, evolution system)
- Step 14 — Enemy variety (elites and bosses via WaveEventManager, scaling stats)
- Step 15 — Save system (persist gold and gear across runs via JSON save file)
- Step 16 — Run history (best time, most kills, stored in save file)
- Step 17 — Gold, merchant & gear (drops, shop, equipment)
- Step 18 — Polish (SFX, screen shake, enemy death VFX)
