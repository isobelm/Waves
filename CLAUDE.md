# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Waves** is a 2D Unity game where players control a character that must stay in water to survive. The player dies if they remain out of water for more than 3 seconds and gets attacked by a seagull as a warning when time is running out.

- **Engine**: Unity 6000.0.61f1
- **Platform**: 2D
- **Render Pipeline**: Universal Render Pipeline (URP)

## Project Structure

**Development is focused exclusively on the Niall Test Scene and Niall Test Assets folder. Do not modify other test areas (Rory, Isobel) or their scenes.**

Active development area:
- **Niall Test Assets** - Primary development folder with organized scripts
  - `Scripts/Player/` - Player movement and health mechanics
  - `Scripts/Enemies/` - Enemy behavior (seagulls)
  - `Scripts/Environment/` - Environmental systems (waves, water)
  - `Scripts/Managers/` - Game-wide managers (GameManager)
  - `Scenes/Niall Test Scene.unity` - **Primary development scene**

Other folders (do not edit):
- **RoryTestAssests** - Independent development area
- **Isobel Test Assets** - Independent development area

## Architecture & Key Systems

### Player System
The player uses a component-based architecture with three main scripts:

1. **PlayerMovement** (`Assets/Niall Test Assets/Scripts/Player/PlayerMovement.cs`)
   - Handles input and character movement
   - Uses Rigidbody2D for physics
   - Different speeds in water vs sand (waterSpeed=5, sandSpeed=2)
   - Requires `WaterDetector` component to determine current terrain

2. **WaterDetector** (`Assets/Niall Test Assets/Scripts/Player/WaterDetector.cs`)
   - Detects when player enters/exits water via 2D collider triggers
   - Uses "Water" tag to identify water colliders
   - Fires `OnEnterWater` and `OnExitWater` UnityEvents

3. **PlayerHealth** (`Assets/Niall Test Assets/Scripts/Player/PlayerHealth.cs`)
   - Tracks player death state
   - Fires `OnDeath` event with reason string
   - Detects enemy collisions (uses "Enemy" tag)

### Survival Mechanic - SeagullTimer
**SeagullTimer** (`Assets/Niall Test Assets/Scripts/Enemies/SeagullTimer.cs`) is the core survival mechanic:
- Player has 3 seconds (`timeBeforeDeath`) out of water before dying
- Seagull spawns and pursues player in the final 1 second (`seagullWarningTime`)
- Displays countdown timer via TextMeshPro UI
- Coordinates with `PlayerHealth` to trigger death
- Spawns/destroys seagull prefab dynamically

### Game Management
**GameManager** (`Assets/Niall Test Assets/Scripts/Managers/GameManager.cs`):
- Singleton pattern for global game state
- Subscribes to `PlayerHealth.OnDeath` event
- Restarts the level 1 second after player death
- Can load arbitrary scenes via `LoadLevel()`

### Environment
**WaveManager** (`Assets/Niall Test Assets/Scripts/Environment/WaveManager.cs`):
- Currently placeholder - intended for wave animation and effects

## Build & Development

### Opening the Project
1. Open Unity Hub and select Unity 6000.0.61f1
2. Open the Waves project at `/home/niall/Programming/Waves`
3. Main solution file: `Waves.slnx`

### Running the Game
- Open a test scene from `Assets/Scenes/` (e.g., "Niall Test Scene")
- Press Play in the Unity Editor

### Scripts
All gameplay scripts are C# and located in:
- `Assets/Niall Test Assets/Scripts/` (main development area)

### Version Control
- Active branch: `niall/you-die-out-of-water`
- Recent changes involve seagull behavior and water survival mechanics
- Standard Unity .gitignore is configured

## Key Dependencies & Tags

### Tags
Scripts reference these tags - ensure they exist in Project Settings:
- **"Water"** - Used by WaterDetector to identify water colliders
- **"Enemy"** - Used by PlayerHealth to detect enemy collisions

### Components & Requirements
- **PlayerMovement**: Requires `Rigidbody2D` and `WaterDetector`
- **SeagullTimer**: Requires `WaterDetector` and `PlayerHealth`
- TextMeshPro is integrated (Unity 6 default)

## UnityEvents Used
Scripts use UnityEvents for loose coupling:
- `WaterDetector.OnEnterWater` / `OnExitWater` - Water state changes
- `PlayerHealth.OnDeath(string reason)` - Player death notification
- These are configurable in Inspector for non-code event binding

## Common Development Tasks

### Adding New Player Mechanics
1. Create new script in `Assets/Niall Test Assets/Scripts/Player/`
2. Use existing `WaterDetector.IsInWater()` to check terrain
3. Subscribe to `WaterDetector.OnEnterWater`/`OnExitWater` if needed

### Modifying Survival Time
- Edit `SeagullTimer.timeBeforeDeath` field (currently 3 seconds)
- Edit `SeagullTimer.seagullWarningTime` field (currently 1 second)

### Enemy Behavior
- Seagull prefab reference in SeagullTimer inspector
- Spawn position: 3 units right, 5 units up from player
- Target position during warning: 2 units above player
- Movement speed: 5 units/second toward target

### Active Scene
- **Niall Test Scene** - The only scene being developed; located at `Assets/Scenes/Niall Test Scene.unity`

Other scenes exist but are not part of current development:
- RoryTestScene, Isobel Test, SampleScene - Leave untouched
