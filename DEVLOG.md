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

## Planned
- Step 3 — Enemy base (`EnemyBase.cs`, chase AI, contact damage)
- Step 4 — Enemy spawner (off-screen spawning, difficulty ramp)
- Step 4 — Enemy spawner (off-screen spawning, difficulty ramp)
- Step 5 — Weapon system (auto-attack, projectile)
- Step 6 — Game Manager (survival timer, game over, restart)
- Step 7 — UI (health bar, timer, game over screen)
- Step 8 — Polish (SFX, VFX, screen shake)
