# Collaboration & Architecture Guide for Waves Game Jam

This document provides recommendations for improving code structure, asset organization, and team collaboration based on your game concept and current codebase.

## Overview of Your Game

**Hermit Crab Racing Game** - Players race across rock pools to reach home, avoiding enemies and environmental hazards:
- **Player**: Hermit crab that moves faster in water, slower on sand
- **Enemies**: Seagulls that appear when out of water too long (3 seconds)
- **Environmental Threat**: Waves that push/pull entities and can sweep players out to sea
- **Core Mechanic**: Time-based survival (water dependency) + wave navigation

---

## Current Structure Analysis

### ✅ Strengths
1. **Clear component separation** - Scripts are organized by domain (Player, Enemies, Managers, Environment)
2. **UnityEvents for decoupling** - Good event-driven architecture for collaboration
3. **Prefabs exist** - Seagull prefab setup for instantiation
4. **Singleton GameManager** - Centralized game state

### ⚠️ Issues for Team Collaboration

1. **No Prefab-Based Scene Organization**
   - Scene likely has loose GameObjects instead of reusable prefabs
   - Makes it hard to iterate independently without merge conflicts

2. **Missing Enemy Architecture**
   - Only seagull timeout, no generic enemy system
   - No enemy prefabs or patrolling enemies yet
   - SeagullTimer couples too many concerns (timer, UI, spawning, player feedback)

3. **Missing Wave System**
   - WaveManager is empty placeholder
   - Core mechanic for your jam theme not implemented
   - No wave-entity interaction system

4. **Script Location Issues**
   - CameraFollow.cs is in root instead of `Scripts/Camera/`
   - Makes the folder structure misleading

5. **No Configuration/Settings System**
   - Hardcoded values scattered across scripts (speeds, timers, spawn positions)
   - Makes tweaking gameplay difficult without editing code

6. **Limited Scalability for Level Design**
   - No reusable rock pool system
   - Difficulty spawning multiple waves or managing level progression

---

## Recommended Improvements

### 1. Restructure Scripts Folder (Organization)

**Current:**
```
Scripts/
├── Player/
├── Enemies/
├── Environment/
└── Managers/
```

**Recommended:**
```
Scripts/
├── Player/
│   ├── PlayerMovement.cs
│   ├── PlayerHealth.cs
│   └── WaterDetector.cs
├── Enemies/
│   ├── Enemy.cs (abstract base class)
│   ├── Seagull.cs
│   └── SeagullSpawner.cs
├── Environment/
│   ├── WaveManager.cs
│   ├── WaveAffected.cs (component for wave interaction)
│   └── WaveVisuals.cs (optional: animation)
├── Camera/
│   └── CameraFollow.cs
├── UI/
│   ├── HealthDisplay.cs
│   └── TimerDisplay.cs
└── Managers/
    ├── GameManager.cs
    └── LevelManager.cs (for level progression)
```

**Benefit**: Clear ownership domains; easier to assign team members without conflicts.

---

### 2. Organize Public Fields with Inspector Attributes

**Keep using public fields for fast game jam iteration**, but add organization:

```csharp
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Range(1f, 10f)]
    public float waterSpeed = 5f;

    [Range(0.5f, 5f)]
    public float sandSpeed = 2f;

    private Rigidbody2D rb;
    private WaterDetector waterDetector;
    // ... rest of code
}
```

**Useful Inspector Attributes:**
- `[Header("Section Name")]` - Groups related fields visually
- `[Range(min, max)]` - Prevents invalid values with a slider
- `[Tooltip("Description")]` - Shows helpful text when hovering
- `[SerializeField]` private - Keep internal vars out of Inspector

**Example: SeagullTimer**
```csharp
public class SeagullTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [Range(1f, 10f)]
    public float timeBeforeDeath = 3f;

    [Range(0f, 2f)]
    public float seagullWarningTime = 1f;

    [Header("Seagull Spawning")]
    public GameObject seagullPrefab;
    public Vector3 spawnOffset = new Vector3(3f, 5f, 0f);
    [Range(1f, 10f)]
    public float seagullApproachSpeed = 5f;
    // ... rest of code
}
```

**Benefit**:
- Fast tweaking without recompiling
- Team members can test difficulty without touching code
- Inspector stays organized and readable

---

### 3. Refactor Seagull System (Separation of Concerns)

**Problem**: `SeagullTimer` does too much:
- Tracks time out of water
- Manages UI display
- Spawns/destroys seagulls
- Calls death

**Solution**: Split into three systems:

**A. `OutOfWaterTimer.cs`** - Tracks timer only
```csharp
public class OutOfWaterTimer : MonoBehaviour
{
    private float timeOutOfWater = 0f;
    public UnityEvent<float> OnTimerTick; // Fires every frame with time remaining
    public UnityEvent OnDanger; // Fires when < 1 sec remaining
    public UnityEvent OnTimeout; // Fires when time runs out
}
```

**B. `SeagullSpawner.cs`** - Manages seagull spawning
```csharp
public class SeagullSpawner : MonoBehaviour
{
    public void ShowSeagull() { /* spawn/move seagull */ }
    public void HideSeagull() { /* destroy seagull */ }
}
```

**C. `TimerDisplay.cs`** - Updates UI
```csharp
public class TimerDisplay : MonoBehaviour
{
    public void UpdateDisplay(float timeRemaining) { /* update text */ }
}
```

**Benefit**:
- Each script has one responsibility
- Easy to test independently
- Other features can reuse `OutOfWaterTimer` (e.g., health degradation)
- Team members can work on different systems without blocking each other

---

### 4. Create a Proper Enemy System

**Create: `Scripts/Enemies/Enemy.cs`** (abstract base class)

```csharp
public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected WaveAffected waveAffected;

    public virtual void Die(string reason)
    {
        Destroy(gameObject);
    }
}
```

**Create: `Scripts/Enemies/Seagull.cs`** (extends Enemy)

```csharp
public class Seagull : Enemy
{
    public Transform target;
    public float moveSpeed = 5f;

    void Update()
    {
        if (target) MoveToward(target);
    }
}
```

**Benefit**:
- Easy to add more enemies later (crabs, fish, etc.)
- Consistent interface for all enemies
- Scales with growing game

---

### 5. Implement Wave System Architecture

**Create: `Scripts/Environment/WaveManager.cs`** (non-empty)

```csharp
public class WaveManager : MonoBehaviour
{
    public UnityEvent<Wave> OnWaveIncoming;
    public UnityEvent OnWaveReceding;

    private Wave currentWave;
    private float waveTimer = 0f;

    void Update()
    {
        // Spawn waves periodically
        // Notify all WaveAffected components
    }
}

public class Wave
{
    public float force;
    public float duration;
    public float startTime;
}
```

**Create: `Scripts/Environment/WaveAffected.cs`** (component for any entity affected by waves)

```csharp
public class WaveAffected : MonoBehaviour
{
    private Rigidbody2D rb;
    public float affectRadius = 5f;

    public void ApplyWaveForce(Wave wave)
    {
        float yDistance = transform.position.y;
        float forceMultiplier = Mathf.Max(0, 1f - (yDistance / affectRadius));
        rb.AddForce(wave.direction * wave.force * forceMultiplier);
    }
}
```

**Add to Entities**:
- Attach `WaveAffected` to player, seagulls, and enemies
- Subscribe to `WaveManager.OnWaveIncoming`

**Benefit**:
- Modular wave system you can build incrementally
- Consistent behavior for all entities
- Easy to test and debug independently

---

### 6. Use Prefabs for Everything (Reduce Scene Conflicts)

**Create prefabs for:**
- Player (with all components)
- Enemy types (Seagull, etc.)
- Rock pool zones
- UI panels
- Wave effects (visual)

**Scene should only contain:**
- One instance of each major system prefab (GameManager, WaveManager, etc.)
- Level layout (empty GameObjects or tilemap)
- Spawn points for dynamic entities

**Benefit**:
- Multiple team members can edit prefabs without scene conflicts
- Easy to prototype different level layouts
- Reusable components across levels

---

### 7. Create a Level Manager for Progression

**Create: `Scripts/Managers/LevelManager.cs`**

```csharp
public class LevelManager : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform homePoint;
    public int rockPoolCount;

    public void CheckWin()
    {
        if (Vector3.Distance(player.position, homePoint.position) < 1f)
        {
            GameManager.Instance.LevelComplete();
        }
    }
}
```

**Benefit**:
- Clear win condition
- Easy to add multiple levels later
- Supports spawning enemies and obstacles per level

---

### 8. Create Code Documentation Standard

**Add XML doc comments to public methods:**

```csharp
/// <summary>
/// Moves the entity toward the target position.
/// </summary>
/// <param name="target">Target position</param>
/// <param name="speed">Movement speed in units/sec</param>
public void MoveTo(Vector3 target, float speed) { }
```

**Benefit**:
- Hover tooltips in Visual Studio/Rider
- Clearer API for collaboration
- Easier for new team members

---

## Recommended File Structure for Niall Test Assets

```
Assets/Niall Test Assets/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerMovement.cs
│   │   ├── PlayerHealth.cs
│   │   └── WaterDetector.cs
│   ├── Enemies/
│   │   ├── Enemy.cs
│   │   ├── Seagull.cs
│   │   ├── SeagullSpawner.cs
│   │   └── EnemySpawner.cs
│   ├── Environment/
│   │   ├── WaveManager.cs
│   │   └── WaveAffected.cs
│   ├── Camera/
│   │   └── CameraFollow.cs
│   ├── UI/
│   │   ├── TimerDisplay.cs
│   │   └── HealthDisplay.cs
│   └── Managers/
│       ├── GameManager.cs
│       └── LevelManager.cs
├── Prefabs/
│   ├── Player.prefab
│   ├── Seagull.prefab
│   ├── GameManager.prefab
│   ├── WaveManager.prefab
│   └── UI/
│       └── HUD.prefab
└── Scenes/
    └── Niall Test Scene.unity
```

---

## Team Collaboration Workflow

### Before Starting Work
1. Create a branch: `git checkout -b feature/wave-system`
2. Assign one person per system (Player, Enemies, Wave, UI)

### During Development
- **Prefabs**: Edit `.prefab` files (safe to merge with git)
- **Scripts**: Keep scripts focused and small
- **Config**: Use GameConfig.asset to avoid script conflicts
- **Scene**: Minimize direct scene edits; use prefabs instead

### Code Review Checklist
- [ ] Does the script have one responsibility?
- [ ] Are hardcoded values in GameConfig instead?
- [ ] Are public APIs documented with XML comments?
- [ ] Does it follow the naming convention?
- [ ] Are UnityEvents used for loose coupling?

---

## Minimum Viable Game (MVP) Checklist

### Phase 1: Core Loop (Week 1)
- [ ] Player movement (water/sand speeds)
- [ ] Water detection
- [ ] Out-of-water timer + seagull spawning
- [ ] Basic enemy (seagull patrol)
- [ ] Death and level restart

### Phase 2: Wave Mechanic (Week 2)
- [ ] Wave spawning system
- [ ] Wave force application
- [ ] Wave visuals (particle effects or animation)
- [ ] Wave win/lose conditions

### Phase 3: Level Design (Week 3+)
- [ ] Multiple rock pools
- [ ] Enemy variety
- [ ] Difficulty progression
- [ ] Polish and sound

---

## Helpful Unity Practices for Your Team

1. **Use Layers for Collision Detection**
   - Layer: "Water"
   - Layer: "Player"
   - Layer: "Enemies"
   - Makes collision logic cleaner

2. **Use Tags Consistently**
   - Tag: "Water"
   - Tag: "Enemy"
   - Tag: "RockPool"

3. **Organize Scene Hierarchy**
   ```
   Niall Test Scene
   ├── Environment
   │   ├── RockPool_1
   │   ├── Water_Area
   │   └── Hazards
   ├── Entities
   │   ├── Player
   │   └── Enemies
   ├── Systems
   │   ├── GameManager
   │   ├── WaveManager
   │   └── LevelManager
   └── UI
       └── HUD
   ```

4. **Version Control Strategy**
   - Use `.gitignore` for Temp/, Library/, Build/ (already set up)
   - Commit `.prefab` and `.cs` files regularly
   - Use descriptive commit messages: "Add wave force system" not "Update stuff"

---

## Quick Wins to Implement Now

1. **Move CameraFollow.cs to `Scripts/Camera/`** - Organize file structure
2. **Add Inspector attributes** to scripts ([Header], [Range], [Tooltip]) - Makes tweaking easier without code changes
3. **Split SeagullTimer into three systems** (timer, spawner, UI) - Allows parallel dev and reusability
4. **Create Enemy base class** for future extensibility - Prepare for adding more enemy types
5. **Rename scripts clearly**: "OutOfWaterTimer" not "SeagullTimer" - Clearer responsibility

These changes will unlock parallel development and reduce merge conflicts significantly.

---

## Questions to Answer

1. How many rock pools will be in a level?
2. Will there be multiple enemy types, or just seagulls?
3. Should waves be timed events or continuous mechanic?
4. Is the goal to reach a specific location, or survive a timer?
5. Do you want difficulty progression across levels?

Good luck with your jam! 🎮
