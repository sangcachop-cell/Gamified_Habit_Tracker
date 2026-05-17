# Session Handoff

## Goal

Build a gamified habit tracker RPG where real-life habits (quests) are completed via interactive minigames. Players earn XP, stats, and loot. Core loop: complete quests → get stronger → survive harder forest raids → craft better gear → repeat.

Pillars not yet finished: more quests/minigames, craft timer, stamina cost, boss encounters, market/economy, chest loot in location interiors.

---

## Current Code State (branch: `fix-branch`)

Last commit still on repo: `ea3ceb1` — all session work is uncommitted local changes.

### What works end-to-end

- **Fantasy UI** — full dark theme (Cinzel font, gold/purple palette) across all views
- **Forest raid** — 128×128 map, WASD-only movement (click-to-move disabled), A* still used internally for post-combat path continuation
- **Location interiors** — stepping into Cave/Warehouse/Lake zone on world map shows "Enter" button → transitions to 64×64 sub-map with unique terrain, stronger monsters (always "rare"), exit zones, and treasure chest markers (visual only)
- **Pity system** — `StepsSinceLastCombat` in session; minimum 10 steps guaranteed between any two encounters; resets to 0 on combat trigger and on interior entry
- **Combat** — turn-based fight, path animates to combat cell before redirecting (was teleporting before fix)
- **Loot staging (Pouch)** — items picked up during loot screen go to `session.Pouch`, NOT DB; only committed on successful Extract; death/server restart = loot lost
- **Inventory** — grid drag-drop, equipment slots (Backpack/Armor/Rig), rotate (AJAX, no reload), discard; item tiles render +1px inset / -2px size so border never clips at grid edge
- **Rig slot constraint** — rotate blocked server-side if rotated size violates `SlotConstraint` (Rig requires W=1,H=2)
- **Hideout** — 7 facilities (stat buffs + Storage Room + Workbench), instant craft (Wood/Stone raw → material), craft/rotate no longer reload page
- **Minigames** — QTE circle + Dino runner, both functional; daily cooldown currently disabled for testing

### What is intentionally disabled for testing

| Flag | Location | Notes |
|------|----------|-------|
| Daily cooldown | `MinigameController.cs` ~L42-46 (GET) ~L63-67 (POST) | Commented out |
| Craft timer | `CraftController.cs` | Instant; no `UserCraftSlot` table yet |
| Workbench upgrade | `HideoutController.cs` | Endpoint is placeholder |
| Chest loot | `ForestController.cs` / `ForestMap.cs` | Chests visible on canvas but no interaction |

### DB state

- 2 quests seeded: Id=1 Tập thể dục (QTE), Id=2 Chạy bộ (Dino)
- 7 facilities seeded: 1=Training Grounds, 2=Meditation Hall, 3=Archive, 4=Agility Course, 5=Barracks, 6=Storage Room, 7=Workbench
- `User.Wood` and `User.Stone` = integer material counts (NOT inventory items)
- Raw wood/stone from forest = `UserInventoryItem` entries

---

## Key Files

| File | Purpose |
|------|---------|
| `Models/ForestSession.cs` | Session state; holds `CurrentMapId`, `WorldReturnX/Y`, `StepsSinceLastCombat`, `Pouch` |
| `Constants/ForestMap.cs` | Zones, spawns, extracts, monster factory, loot tables, **`Interiors[]`** (3 location interior defs) |
| `Controllers/ForestController.cs` | All forest actions: Map, Move, MoveInterior, EnterLocation, ExitLocation, Combat, Loot, Extract, Dead |
| `Views/Forest/Map.cshtml` | Canvas map; dynamic `MAP_W/H/WATER_BORDER/MAP_MODE`; interior terrain/EXIT_ZONES/CHESTS rendering; WASD only |
| `Controllers/InventoryController.cs` | Inventory CRUD; `RotateItemAsync` checks `SlotConstraint` before bounds |
| `Services/Implementations/InventoryService.cs` | `RotateItemAsync` — slot constraint check added this session |
| `Views/Inventory/Index.cshtml` | Grid drag-drop; items at `+1px` inset; Pocket uses bg-image grid; Rig uses tall slot-cells |
| `Views/Hideout/Index.cshtml` | Craft (AJAX, no reload), storage DnD, workbench |
| `Controllers/CraftController.cs` | POST /Craft/Start → returns `removedItemId`, `inputItemId`, `newWood`, `newStone` |
| `Constants/WorkbenchCatalogue.cs` | Craft recipes, slots per level |
| `Constants/ItemCatalogue.cs` | Item defs + Rarity |
| `GAME_CONTENT.md` | Design doc: items, monsters, quests, loot tables, facilities. Edit here → say "sync" → Claude updates seed + migration |
| `TODO.md` | Priority queue + disabled flags table |

---

## What Failed / Watch Out For

### Razor / C# gotchas
- **`@` in JS** → Razor parser error RZ1003. Use `//` comments without `@`, write "at" literally in strings.
- **`@@keyframes`** in Razor `<style>` → must be `@@keyframes` not `@keyframes`.
- **Tag helper attribute with C#** → `<form asp-action="X" @(condition ? ...)>` is RZ1031. Use `@{ bool flag = ...; }` then `style="@(flag ? "display:none" : "")"` instead.

### Inventory / Grid
- **Item border clipping at grid edges** — tried `box-shadow: inset 0 0 0 2px` first (didn't fix all edges). Final fix: render items at `GridX*C+1, GridY*C+1` with `W*C-2, H*C-2`; also update drag-drop JS and rotate JS to match.
- **Double grid on Pocket/Rig** — CSS `background-image` + slot-cell `<div>` both rendered at once. Fix: Pocket uses bg-image only; Rig uses tall slot-cells only (`background-image: none`).
- **Slot constraint rotate bypass** — `RotateItemAsync` only checked bounds, not `SlotConstraint`. Player could rotate wood (1×2) back to (2×1) inside the Rig if bounds happened to pass. Fixed by checking `SlotConstraint` first.

### Forest / Interior
- **Combat teleport** — when combat triggered mid-path, JS was immediately updating `session.PlayerX/Y` and redirecting without animating. Fixed by running the partial path animation loop before the 400ms redirect.
- **World zones bleeding into interior** — `LOCATIONS` JS constant always serialized all world zones (Cave/Warehouse/Lake) even when inside an interior, causing "Cave" label to appear inside the Warehouse. Fixed: make `LOCATIONS`/`SPAWNS`/`EXTRACTS` serialize to `[]` when `MapMode == "interior"`.
- **Exit zones in impassable border** — original exit zone coords had y=1 / x=1 which are inside `WATER_BORDER=2` → player could never reach them. Fixed: north exits moved to y=2, west to x=2, east to x=60, south to y=60.
- **Loot double-fire** — fixed in previous session with `busy` flag + `e.stopPropagation()`.
- **Continue path teleport** — fixed in previous session by recomputing A* from current player position.

### Theme
- **Hex opacity in Razor** — `@(color)22` not `@color22` (Razor parses the second form as variable `color22`).
- **Item tile opacity** — items had `background: @(TileColor)cc` (80% transparent) letting CSS grid lines bleed through. Fixed by removing the `cc` alpha: `background: @(TileColor)`.

---

## Interior System Reference

```
ForestSession.CurrentMapId  = null       → on 128×128 world map
ForestSession.CurrentMapId  = "cave"     → inside Cave 64×64 interior
                            = "warehouse"
                            = "lake"

ForestSession.WorldReturnX/Y = world coords to restore on ExitLocation
ForestSession.StepsSinceLastCombat = pity counter (min 10 steps between encounters)
```

Interior map constants (ForestMap.cs `Interiors[]`):
- All 64×64, border=2 (passable area x/y=[2..61])
- Cave: 40% encounter, 3 exits (N y=2, W x=2, E x=60), 3 chests
- Warehouse: 35% encounter, 3 exits (N y=2, S y=60, W x=2), 3 chests
- Lake: 45% encounter, 3 exits (N y=2, S y=60, E x=60), 3 chests
- Always spawns "Forest Brute" (rare tier) inside

---

## Next Steps (priority order)

1. **Chest loot in interiors** — define chest loot tables in `ForestMap.cs` per location. Add click interaction on chest cells in `Map.cshtml` (separate from WASD movement). POST to new `/Forest/OpenChest` action → validate chest position, award loot to `session.Pouch`.

2. **Re-enable daily cooldown** — uncomment 2 blocks in `MinigameController.cs` when testing is done.

3. **Loot tiers** — add items to `ForestMap.LootTables.Forest[Rarity.Uncommon]` (and higher tiers) in `ForestMap.cs`. Rare/Elite monsters in interiors use the same pool.

4. **Stamina cost in forest** — add `PlayerStamina` to `ForestSession`; deduct on each Move step in `MoveInterior()` and world `Move()`; if stamina hits 0, force extract or death.

5. **Craft timer** — create `UserCraftSlot` model (UserId, SlotIndex, RecipeId, StartedAt, CompletesAt), migration, update `CraftController` to queue jobs; add `CraftService` to collect finished slots on Hideout load.

6. **More quests** — add rows to `GAME_CONTENT.md`, say "sync" → update `AppDbContext.cs` seed + migration. Each quest needs a `MinigameType` string.

7. **Workbench upgrade costs** — define Wood+Stone cost per facility level in `HideoutController` / new `UpgradeCatalogue.cs`. Currently endpoint returns placeholder.
