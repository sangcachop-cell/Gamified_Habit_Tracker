# Gamified Habit Tracker — CLAUDE.md

## Project Overview

ASP.NET Core 8 MVC web app. Habit tracking as RPG. Users complete quests (habits/tasks) → earn XP, level up, gain stats, battle monsters.

## Tech Stack

- **Backend**: ASP.NET Core 8 MVC, Entity Framework Core 9, SQL Server
- **Frontend**: Razor Views, Bootstrap 5, Bootstrap Icons, vanilla JS
- **Auth**: Session-based (no Identity framework) — `SESSION_USER_ID` in `AppConstants.cs`
- **State**: Battle state = session JSON (no DB table)

## Architecture

- Controllers use PRG pattern
- Services via DI; interfaces in `Services/`, implementations in `Services/Implementations/`
- Static item/level data in `Constants/` (no DB tables)
- `[NotMapped]` computed props on `User` for derived RPG stats
- Migrations in `HabitTracker/Migrations/`

## Key Systems

### RPG Stats (`Models/User.cs`)
Stored base stats: `STR`, `WILL`, `INT`, `AGL`, `END` (int, default 0)
Computed (NotMapped):
- `HP = (80 + Level*5) + END*10 + WILL*5`
- `AttackDamage = (5 + Level) + STR*2 + AGL`
- `Armor = (int)(Level*0.5 + END*1.5 + WILL*0.5)`
- `XPGainPercent = Level*0.2 + INT*0.5`
- `Stamina = (50 + Level*2) + WILL*5 + END*3`
- `Speed = Min(100, (int)(5.0 + Sqrt(Level)*3.0 + Sqrt(AGL)*5.0))`

### Level System (`Constants/AppConstants.cs` → `LevelSystem`)
- Max level 100
- Total XP for level N: `25*(N-1)*(N+2)`
- `CalculateLevel(xp)`: closed-form quadratic — `floor((-1 + sqrt(9 + 4*xp/25)) / 2) + 1`
- +1 all base stats per level-up (`STAT_POINTS_PER_LEVEL = 1`)

### Battle System (`Controllers/BattleController.cs`)
- 5-wave gauntlet; waves scale with player level × multipliers (1.0/1.3/1.6/1.9/2.2)
- Speed-based turn order; 1.5× speed advantage → bonus strike
- Defend: full block, consecutive failure chance (0/35/65/85%)
- Flee disabled — requires Escape Scroll (placeholder)
- XP per wave; state in session JSON as `BattleState`

### Inventory System (`Controllers/InventoryController.cs`, `Services/IInventoryService.cs`)
- Storage: 5×4 grid (20 cells), Backpack: 4×2 (8 cells), 64px cells
- Items in `Constants/ItemCatalogue.cs` (static dict, no DB)
- `UserInventoryItem` model: UserId, ItemId, ContainerType, GridX, GridY, IsRotated
- Drag-and-drop: HTML5 DnD, grab-offset, optimistic DOM + server rollback
- Move: POST `/Inventory/Move` → JSON `{ success, error }`
- AABB overlap check in `InventoryService.Overlaps()`

### Hideout (`Controllers/HideoutController.cs`, `Services/IHideoutService.cs`)
- 5 facilities seeded in DB, each gives passive stat buff
- Quest completion available from hideout (same logic as TaskController)
- Upgrade: placeholder only

### Stat Gains (`Constants/AppConstants.cs` → `RpgStats`)
- `ComputeStatGains(category, difficulty, frequency)` → per-stat gains on quest cards
- Quest cards show colored pills with exact stat gains

## Razor / CSS Gotchas

- `@keyframes` in Razor → must be `@@keyframes` (@ is Razor directive prefix)
- C# code blocks inside `else {}`: do NOT wrap in `@{ }` — already in code block
- Append hex opacity: use `@(color)22` not `@color22`
- Items positioned with `position: absolute` over CSS `background-image` grid lines

## Session Keys (all in `AppConstants.cs`)
- `SESSION_USER_ID` — int, primary auth check
- `IsAdmin` — string "true"
- `Username`, `Avatar`

## DB Context (`Data/AppDbContext.cs`)
- Seeded: 5 Facilities, 11 Quests (with FacilityId assignments), Badges
- FK rules: Quest→Facility = SetNull, UserFacility/UserInventoryItem = Cascade

## Commit Convention

No commit unless user asks. Separate commits per feature. No `--no-verify`. Branch: `fix-branch`, main: `main`.