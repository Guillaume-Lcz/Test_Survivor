# Test Survivor — Claude Context

## Project

2D survival game (Vampire Survivors-style) built in Unity 6 to re-learn Unity.
Target platform: Android.

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
- **Update `DEVLOG.md`** at the end of every step — move it from Planned to a completed section with bullet details.

## Scripts

| File | Purpose |
|------|---------|
| `Scripts/Player/PlayerMovement.cs` | WASD + gamepad movement via Input System |
| `Scripts/Systems/CameraFollow.cs` | Smooth LateUpdate camera follow |
| `Scripts/Player/PlayerInputActions.inputactions` | Input map: Move action, Keyboard&Mouse + Gamepad schemes |
| `Scripts/Systems/IDamageable.cs` | Interface: `TakeDamage(float)` |
| `Scripts/Player/PlayerStats.cs` | Health, `Heal()`, `OnHealthChanged` / `OnDeath` UnityEvents, implements IDamageable |

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
- [ ] Step 5 — Enemy spawner (off-screen spawning, difficulty ramp)
- [ ] Step 6 — Weapon system (auto-attack, projectile)
- [ ] Step 7 — Game Manager (survival timer, game over, restart)
- [ ] Step 8 — UI (health bar, timer, game over screen)
- [ ] Step 9 — Polish (SFX, VFX, screen shake)
