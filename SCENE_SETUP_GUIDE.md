# Scene Setup Guide - Niall Test Scene

This guide walks you through setting up a minimal working game with:
- Player (hermit crab)
- Rock pools (tilemaps)
- One enemy
- Seagull threat (when out of water too long)

---

## Scene Hierarchy Overview

Here's what your finished scene hierarchy should look like:

```
Niall Test Scene
├── Environment
│   ├── RockPool (Tilemap)
│   └── Water (Tilemap with collider)
├── Entities
│   ├── Player (prefab or GameObject)
│   └── BasicEnemy (prefab or GameObject)
├── Systems
│   ├── GameManager
│   ├── WaveManager
│   └── Camera
└── UI
    └── Canvas
        └── TimerText (TextMeshPro)
```

---

## Step-by-Step Setup

### STEP 1: Set Up Game Systems (One-Time Setup)

Create these GameObject hierarchies in your scene. You only need ONE of each system.

#### 1A. Create GameManager

1. Right-click in Hierarchy → Create Empty → Name: `GameManager`
2. Drag `GameManager.cs` onto it
3. That's it! No configuration needed in Inspector

**GameManager is ready to go.**

---

#### 1B. Create WaveManager

1. Right-click in Hierarchy → Create Empty → Name: `WaveManager`
2. Drag `WaveManager.cs` onto it
3. Configure in Inspector:
   - Wave Frequency: `5` (spawn waves every 5 seconds - adjust later)
   - Wave Force: `10` (leave for now)
   - Wave Affect Range: `5` (leave for now)
4. Leave the events empty for now (you can hook them up later)

**WaveManager is ready to go.**

---

#### 1C. Set Up Camera

1. Find your Main Camera in the hierarchy
2. Drag `CameraFollow.cs` onto it
3. Configure in Inspector:
   - Player: Drag your Player GameObject here (you'll create this in Step 2)
   - Smooth Speed: `0.125` (good default)
   - Offset: `(0, 0, -10)` (standard for 2D top-down)

**Camera is ready, just needs Player reference.**

---

### STEP 2: Create the Player

This is the most important GameObject. It has many components!

#### 2A. Create Player GameObject

1. Right-click Hierarchy → Create 3D Object → Cube
2. Name it: `Player`
3. Delete the cube mesh - we'll add a sprite instead
4. Add Component → Sprite Renderer
   - Select your crab sprite (if you have one)
   - If not, leave as white placeholder
5. Add Component → Circle Collider 2D (or Box Collider 2D)
   - **CHECK:** "Is Trigger" = **FALSE** (player needs physics!)
6. Add Component → Rigidbody 2D
   - Body Type: `Dynamic`
   - Gravity Scale: `0` (top-down game, no gravity)
   - Constraints: Freeze Rotation Z = **TRUE** (prevent spinning)

**Your Player now has physics and collision.**

---

#### 2B. Add Player Scripts

Attach these scripts to the Player GameObject in this order:

1. **PlayerMovement.cs**
   - Water Speed: `5`
   - Sand Speed: `2`
   - (Nothing else to configure)

2. **WaterDetector.cs**
   - Leave the events empty for now
   - (You'll hook them up in Step 3)

3. **PlayerHealth.cs**
   - Leave the event empty for now

4. **OutOfWaterTimer.cs**
   - Time Before Death: `3`
   - Danger Threshold: `1`
   - Leave the events empty for now

**Player now has movement, health, and timer!**

---

### STEP 3: Create Water Areas

These are the safe zones where the player can survive.

#### 3A. Create Water Tilemap

If you already have tilemaps drawn:

1. Select your existing water tilemap in Hierarchy
2. Add Component → Tilemap Collider 2D
   - **CHECK:** "Is Trigger" = **TRUE** (water is a trigger)
3. Tag this tilemap as "Water"
   - Select tilemap → Inspector top-right → Tag dropdown → Add Tag "Water"

**Water zones are ready!**

---

#### 3B. Add Water Visual (Optional)

If you want water to look different:
- Adjust the tilemap's color or transparency in Sprite Renderer
- Or add a semi-transparent overlay

---

### STEP 4: Create the Seagull System

The seagull appears when you're out of water and kills you.

#### 4A. Create SeagullSpawner GameObject

1. Right-click Hierarchy → Create Empty → Name: `SeagullSpawner`
2. **IMPORTANT:** Make it a child of Player
   - Drag SeagullSpawner under Player in hierarchy
   - This makes seagull spawn relative to player position
3. Drag `SeagullSpawner.cs` onto it
4. Configure in Inspector:
   - Seagull Prefab: Drag your Seagull.prefab here
   - Spawn Offset: `(3, 5, 0)` (3 right, 5 up from player)
   - Approach Speed: `5`
   - Target Offset: `(0, 2, 0)` (2 units above player)

**Seagull spawning is ready!**

---

#### 4B. Make Seagull Prefab Correct

1. Open your `Seagull.prefab` in Project
2. Make sure it has:
   - Sprite Renderer with seagull sprite
   - **Circle Collider 2D** or **Box Collider 2D**
     - **CHECK:** "Is Trigger" = **FALSE** (seagull kills you on touch)
   - **Rigidbody 2D**
     - Body Type: `Dynamic`
     - Gravity Scale: `0`
     - Constraints: Freeze Rotation Z = **TRUE**

**Seagull prefab is ready!**

---

### STEP 5: Create UI Canvas & Timer Display

#### 5A. Create Canvas

1. Right-click Hierarchy → UI → Canvas
2. Name it: `Canvas`
3. Make sure Render Mode = `Screen Space - Overlay`

---

#### 5B. Create Timer Text

1. Right-click on Canvas → UI → Text - TextMeshPro
   - If prompted, import TMP resources (click Import)
2. Name it: `TimerText`
3. In Inspector, set:
   - Text: "In water - safe!" (initial text)
   - Font Size: `36` (or whatever looks good)
   - Alignment: Top-Left (or wherever you want)
4. Add Component → `TimerDisplay.cs`
5. Configure in Inspector:
   - Timer Text: Drag the TextMeshPro component here
   - Timer Format: `"Out of water: {0:F1}s / {1:F1}s"`
   - Safe Text: `"In water - safe!"`
   - Normal Color: `White`
   - Danger Color: `Red`

**Timer UI is ready!**

---

### STEP 6: Create a Basic Enemy

For testing, create a simple patrol enemy.

#### 6A. Create Enemy GameObject

1. Right-click Hierarchy → Create 3D Object → Cube
2. Name it: `BasicEnemy`
3. Delete the cube mesh
4. Add Component → Sprite Renderer
   - Add an enemy sprite (placeholder is fine)
5. Add Component → Circle Collider 2D
   - **CHECK:** "Is Trigger" = **FALSE**
6. Add Component → Rigidbody 2D
   - Body Type: `Dynamic`
   - Gravity Scale: `0`
   - Constraints: Freeze Rotation Z = **TRUE**

#### 6B. Add Enemy Script

1. Drag `Seagull.cs` onto BasicEnemy (Seagull extends Enemy, so it works)
2. Configure in Inspector:
   - Move Speed: `2`
   - Dive Speed: `2`

**For now, the enemy just sits there. It won't move until you call `SetTarget()` on it.**

---

### STEP 7: Wire Up Events (Most Important!)

This connects all the systems together using UnityEvents.

#### 7A. Wire OutOfWaterTimer Events

1. Select Player in Hierarchy
2. Find `OutOfWaterTimer` component in Inspector
3. Expand the events section:

**OnDangerStart()** → Show Seagull
- Click `+` button under "On Danger Start"
- Drag SeagullSpawner into the object field
- From dropdown: `SeagullSpawner` → `ShowSeagull()`

**OnDangerEnd()** → Hide Seagull
- Click `+` button under "On Danger End"
- Drag SeagullSpawner into the object field
- From dropdown: `SeagullSpawner` → `HideSeagull()`

**OnTimerTick(float)** → Update Display
- Click `+` button under "On Timer Tick"
- Drag TimerText (with TimerDisplay) into the object field
- From dropdown: `TimerDisplay` → `UpdateDisplay(float)`

---

#### 7B. Wire WaterDetector Events

1. Select Player in Hierarchy
2. Find `WaterDetector` component in Inspector

**OnEnterWater()** → Safe Message
- Click `+` button under "On Enter Water"
- Drag TimerText (with TimerDisplay) into the object field
- From dropdown: `TimerDisplay` → `ShowSafeMessage()`

---

#### 7C. Wire PlayerHealth Events

1. Select Player in Hierarchy
2. Find `PlayerHealth` component in Inspector

**OnDeath(string)** → Restart Level
- Click `+` button under "On Death"
- Drag GameManager into the object field
- From dropdown: `GameManager` → `RestartLevel()`

---

### STEP 8: Set Up Tags

For collision detection to work properly:

1. Select the water tilemap
2. Inspector top-right → Tag dropdown → Add Tag "Water"
3. Select Player
4. Tag dropdown → Add Tag "Player"
5. Select BasicEnemy
6. Tag dropdown → Add Tag "Enemy"

**Tags are now set up for collision detection.**

---

## Quick Checklist Before Testing

- [ ] GameManager in scene (no config needed)
- [ ] WaveManager in scene (basic config done)
- [ ] Camera has CameraFollow.cs with Player reference
- [ ] Player GameObject has:
  - [ ] Sprite Renderer
  - [ ] Circle/Box Collider 2D (trigger = FALSE)
  - [ ] Rigidbody 2D (dynamic, gravity = 0)
  - [ ] PlayerMovement
  - [ ] WaterDetector
  - [ ] PlayerHealth
  - [ ] OutOfWaterTimer
- [ ] Water tilemap has:
  - [ ] Tilemap Collider 2D (trigger = TRUE)
  - [ ] Tag = "Water"
- [ ] SeagullSpawner as child of Player with:
  - [ ] SeagullSpawner.cs component
  - [ ] Seagull.prefab assigned
- [ ] Seagull.prefab has:
  - [ ] Sprite Renderer
  - [ ] Circle/Box Collider 2D (trigger = FALSE)
  - [ ] Rigidbody 2D
- [ ] Canvas with TimerText:
  - [ ] TextMeshPro text component
  - [ ] TimerDisplay.cs attached
- [ ] Events wired:
  - [ ] OutOfWaterTimer → SeagullSpawner
  - [ ] OutOfWaterTimer → TimerDisplay
  - [ ] WaterDetector → TimerDisplay
  - [ ] PlayerHealth → GameManager

---

## Expected Behavior When Testing

Press Play and you should see:

1. **In Water**: Player can move freely, timer says "In water - safe!"
2. **Exit Water**: Timer starts counting down "Out of water: 0.5s / 3.0s"
3. **< 1 Second Left**: Seagull spawns and dives toward player, text turns RED
4. **3 Seconds Up**: Player dies, scene restarts
5. **Re-Enter Water**: Seagull disappears, timer resets, text returns to white

---

## Troubleshooting

### Seagull Never Appears
- Check OutOfWaterTimer is on Player
- Check OnDangerStart event is wired to SeagullSpawner.ShowSeagull()
- Check SeagullSpawner has seagull prefab assigned
- Check seagull spawning messages in Console

### Player Doesn't Move
- Check PlayerMovement is attached
- Check Rigidbody2D Body Type = Dynamic (not Static/Kinematic)
- Check movement input is working (check Console)

### Player Gets Stuck on Water Edge
- This is normal! Water collision is working.
- Adjust water collider shape to be cleaner edges
- Or make water collider bigger to cover more area

### Timer Never Starts
- Check OutOfWaterTimer is on Player
- Check WaterDetector is detecting water (check Console for "Entered water" / "Exited water")
- Check water tilemap has Tag = "Water"

### Game Doesn't Restart
- Check PlayerHealth is on Player
- Check GameManager is in scene (there must be exactly one!)
- Check OnDeath event is wired to GameManager.RestartLevel()

---

## Next Steps After Testing

Once this all works:

1. **Add More Enemies** - Duplicate BasicEnemy, customize behavior
2. **Add More Pools** - Create more water tilemaps
3. **Add Waves** - Implement WaveManager physics
4. **Polish** - Add sounds, effects, better visuals

Good luck! 🎮
