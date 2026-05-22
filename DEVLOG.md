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

## Planned
- Step 6 — Weapon system (auto-attack, projectile)
- Step 7 — Game Manager (survival timer, game over, restart)
- Step 8 — UI (health bar, timer, game over screen)
- Step 9 — XP system (XP drops from enemies, XP bar, level up trigger)
- Step 10 — Perk system (perk definitions, perk pool, perk selection on level up)
- Step 11 — Gold, merchant & gear (drops, shop, equipment)
- Step 12 — Polish (SFX, VFX, screen shake)
