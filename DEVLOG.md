# Devlog

## Step 1 — Scene Bootstrap
- Set up `GameScene` with Player, orthographic Camera, and Directional Light
- Player tagged as `"Player"`, Rigidbody2D set to Dynamic, gravity scale 0
- Created `CameraFollow.cs` — smooth follow attached to Main Camera targeting Player
- Filled in `PlayerInputActions.inputactions` — `Player` action map with `Move` action (WASD + gamepad)
- Added `PlayerInput` component to Player, linked to `PlayerInputActions`, default map `"Player"`
- Camera background color set to dark green, orthographic size 3

## Planned
- Step 2 — Player health & stats (`PlayerStats.cs`, `IDamageable`)
- Step 3 — Enemy base (`EnemyBase.cs`, chase AI, contact damage)
- Step 4 — Enemy spawner (off-screen spawning, difficulty ramp)
- Step 5 — Weapon system (auto-attack, projectile)
- Step 6 — Game Manager (survival timer, game over, restart)
- Step 7 — UI (health bar, timer, game over screen)
- Step 8 — Polish (SFX, VFX, screen shake)
