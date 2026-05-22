# Test Survivor — Claude Context

## Project

2D survival game (Vampire Survivors-style) built in Unity 6 to re-learn Unity.
Target platform: PC (prototype).

## Stack

- Unity 6 (6000.4.7f1)
- Universal Render Pipeline (URP)
- Unity Input System (new)
- MCP for Unity (editor automation)

## Scene

`Assets/_Game/Scenes/GameScene.unity`
- Main Camera — orthographic size 3, dark green background, CameraFollow targeting Player
- Player — SpriteRenderer, Rigidbody2D (Dynamic, gravity 0), CircleCollider2D, PlayerMovement, PlayerInput
- Directional Light

## Working Style

- Do **one action at a time** — create or edit one file, then wait for the next instruction before continuing.
- **Update `DEVLOG.md`** at the end of every step — add a completed section with bullet details AND remove it from the Planned list. Always double-check the Planned list has no completed steps.

## Scripts

| File | Purpose |
|------|---------|
| `Scripts/Player/PlayerMovement.cs` | WASD + gamepad movement via Input System |
| `Scripts/Systems/CameraFollow.cs` | Smooth LateUpdate camera follow |
| `Scripts/Player/PlayerInputActions.inputactions` | Input map: Move action, Keyboard&Mouse + Gamepad schemes |
| `Scripts/Systems/IDamageable.cs` | Interface: `TakeDamage(float)` |
| `Scripts/Player/PlayerStats.cs` | Health, `Heal()`, 1s invulnerability after hit, `OnHealthChanged` / `OnDeath` UnityEvents, implements IDamageable |
| `Scripts/Systems/GameManager.cs` | Singleton, `GameState` enum, `TriggerGameOver()`, `RestartGame()`, `OnGameOver` UnityEvent |
| `Scripts/Systems/SurvivalTimer.cs` | Elapsed time tracker, fires `OnTimerUpdated(float)` while Playing |
| `Scripts/Systems/GameOverHandler.cs` | Bridges `PlayerStats.OnDeath` → `GameManager.TriggerGameOver()` |
| `Scripts/UI/UIManager.cs` | Timer text, game over panel, restart button — all wired at runtime in `Start()` |

## Folder Structure

```
Assets/_Game/
  Scenes/
  Scripts/     Player/ Enemies/ Weapons/ Systems/ UI/
  Prefabs/     Player/ Enemies/ Weapons/ VFX/
  Art/         Sprites/ Materials/
  Audio/       SFX/ Music/
  UI/          Fonts/ Sprites/
```

## Roadmap

- [x] Step 1 — Scene bootstrap (player movement, camera follow, input)
- [x] Step 2 — Player health & stats (`PlayerStats.cs`, `IDamageable`)
- [x] Step 3 — Enemy base (`EnemyBase.cs`, `SlimeEnemy.cs`, `SlimeEnemy.prefab`)
- [x] Step 4 — Tilemap (chunk-based infinite ground, `TilemapChunkLoader.cs`)
- [x] Step 5 — Enemy spawner + wave event system (`EnemySpawner.cs`, `IWaveEvent.cs`, `WaveEventManager.cs`)
- [x] Step 6 — Weapon system (`IWeapon`, `WeaponManager`, `OrbShooter`, `OrbProjectile`)
- [x] Step 7 — Game Manager (survival timer, game over, restart)
- [x] Step 8 — UI (main menu scene, in-game HUD with health bar/timer/kill counter, pause menu, run summary screen on game over)
- [ ] Step 9 — XP system (XP drops from enemies, XP bar, level up trigger)
- [ ] Step 10 — Perk system (perk definitions, perk pool, perk selection on level up)
- [ ] Step 11 — Object pooling (pool enemies and projectiles for performance)
- [ ] Step 12 — Damage numbers (floating combat text, pooled, shown on hit)
- [ ] Step 13 — Weapon variety (new weapon types, passive items, evolution system)
- [ ] Step 14 — Enemy variety (elites and bosses via WaveEventManager, scaling stats)
- [ ] Step 15 — Save system (persist gold and gear across runs via JSON save file)
- [ ] Step 16 — Run history (best time, most kills, stored in save file)
- [ ] Step 17 — Gold, merchant & gear (drops, shop, equipment)
- [ ] Step 18 — Polish (SFX, screen shake, enemy death VFX)
