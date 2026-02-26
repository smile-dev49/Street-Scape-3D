# Street Escape 3D - Script Architecture

## Folder Structure

```
Scripts/
  Core/         - Shared systems (events, constants)
  Configs/      - ScriptableObjects for design-time configuration
  Player/       - Player movement and control
  Vehicles/     - Vehicle prefabs, pool, spawner
  Level/        - Street, lanes, level progression
  Economy/      - Currency and rewards
  Shop/         - (Placeholder for Step 6)
  UI/           - (Placeholder for Step 9)
  Managers/     - Game flow managers
  Services/     - (Placeholder for IAP, leaderboard)
```

## Script Overview

### Core
- **GameEvents.cs** - Central event bus. Systems subscribe/unsubscribe to avoid tight coupling. Raise events instead of direct method calls.

### Configs
- **GameConfig.cs** - ScriptableObject for level, player, economy tuning. Create via: Create > Street Escape > Game Config
- **VehicleSpawnConfig.cs** - Per-vehicle spawn config (prefab, speed range, spawn weight)

### Player
- **PlayerController.cs** - Lane-based movement (left/right), forward step (Space/W/Up), Rigidbody physics, collision with vehicles, death animation trigger

### Vehicles
- **Vehicle.cs** - Base component for Car/Bus/Motorcycle. Movement, pooling lifecycle
- **VehiclePool.cs** - Object pooling for vehicles. Reduces GC on mobile
- **VehicleSpawner.cs** - Spawns vehicles on lanes with random timing and speed. Difficulty scales with level

### Level
- **Street.cs** - Single street with lanes. Holds spawn points and VehicleSpawner
- **StreetSegment.cs** - Single lane/segment building block
- **LevelManager.cs** - Level progression, street count scaling, restart on death, unlock next level, PlayerPrefs persistence

### Economy
- **CurrencyManager.cs** - Coins, earn per street crossed, perfect run bonus, PlayerPrefs save

### Managers
- **GameManager.cs** - Singleton. Start/pause/resume/restart. Orchestrates game flow. Subscribes to PlayerDeath and LevelComplete

## Setup Instructions

1. Create GameConfig: Right-click > Create > Street Escape > Game Config. Assign to GameManager.
2. Create Street prefab with Lane transforms and VehicleSpawner.
3. Create Vehicle prefabs (Car, Bus, etc.) with Vehicle component and tag "Vehicle".
4. Assign prefabs to VehiclePool and VehicleSpawnConfigs.
5. Assign references in GameManager, LevelManager, PlayerController, CurrencyManager.

## Next Steps (as per spec)

- Shop system (Step 6)
- IAP (Step 7)
- Leaderboard (Step 8)
- UI system (Step 9)
- Advanced features (Step 10)
- Optimization (Step 11)
