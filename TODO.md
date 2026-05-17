# TODO — Gamified Habit Tracker

Last synced: 2026-05-17

---

## Recently Completed (this session)

- [x] **Fantasy UI theme** — full dark overhaul: Cinzel font, gold/purple/crimson palette, dark navbar, dark cards across all views (Dashboard, Tasks, Character, Login, Hideout, Inventory, Forest)
- [x] **Loot staging (Pouch fix)** — `LootPickup` now writes to `session.Pouch` instead of DB directly. Extract commits Pouch → DB. Death clears Pouch. Server restart = loot lost. Loot is only permanent after successful extract.
- [x] **Rotate no page reload** — inventory rotate button calls `RotateAjax` via fetch; DOM swaps width/height in place. No `location.reload()`.
- [x] **Craft no page reload** — `doCraft()` updates material chip counts, fades + removes consumed item from grid, refreshes recipe button states. No reload.
- [x] **Rotate slot constraint** — `RotateItemAsync` now checks `SlotConstraint` before bounds check. Rig (W=1,H=2 required) blocks rotation to 2×1.
- [x] **Item border fix** — items rendered +1px inset, -2px size. Border never touches `overflow:hidden` boundary → all 4 edges always visible.
- [x] **Rig visual: tall slot-cells** — 4 distinct tall rectangles (1-col × 2-row each) instead of 4×2 flat grid. `background-image: none` on rig grid.
- [x] **Pocket visual** — CSS `background-image` grid lines (no slot-cells). Matches Equipped Backpack style.
- [x] **Item tiles opaque** — removed `cc` hex alpha from `TileColor` renders. CSS grid lines no longer bleed through items at shared cell boundaries.
- [x] **Craft/rotate DOM updates** — `window.hs_removeItem` exposed from Hideout DnD IIFE so craft can purge item without reload.

---

## Priority — Next Up

### Must fix / core loop broken without these
- [ ] **Re-enable daily cooldown** — 2 commented blocks in `MinigameController.cs` (~lines 42-46 GET, ~63-67 POST). Currently disabled for testing.
- [ ] **Workbench upgrade cost** — `HideoutController` upgrade endpoint is placeholder. Define Wood+Stone cost per level and wire it. (`WorkbenchCatalogue` needs upgrade cost table.)
- [ ] **Loot tiers** — `ForestMap.LootTables.Forest[Rarity.Uncommon]` (and higher) is empty. Add items to Uncommon–Rare pools so rarer monsters drop something interesting.

### Important / next feature sprint
- [ ] **Craft timer** — `WorkbenchCatalogue.CraftTime` is defined (2hrs) but craft is instant. Needs: `UserCraftSlot` model (UserId, SlotIndex, RecipeId, StartedAt, CompletesAt), migration, `CraftController` to queue jobs, `CraftService` to collect finished slots on Hideout load.
- [ ] **Stamina cost in forest** — deduct from `session.PlayerCurrentHP` (or add `PlayerStamina` to `ForestSession`) on each Move step in `ForestController.Move`.
- [ ] **More quests** — add quest rows to `GAME_CONTENT.md`, then run sync → update `AppDbContext.cs` seed + migration. Each quest needs a `MinigameType` string.
- [ ] **More minigame types** — new view at `Views/Minigame/<Type>.cshtml`. Controller auto-routes via `return View(quest.MinigameType, quest)`.

---

## Backlog

### Forest / Raid
- [ ] Boss encounter (named zone trigger)
- [ ] Escape Scroll item (flee from combat — endpoint exists, consumes scroll)
- [ ] Hunger / thirst in raid (session stamina drain)
- [ ] Map polish (more named zones, environmental storytelling)

### Items
- [ ] Food consume logic (Bread: +30 HP in combat; use during battle turn)
- [ ] Water Bottle flee logic (requires re-enabling flee system in BattleController)
- [ ] More item definitions (add to `ItemCatalogue.cs` + `GAME_CONTENT.md`)
- [ ] Real image assets (replace emoji icons)

### Economy
- [ ] Currency / coin system
- [ ] Market / shop view

### Progression
- [ ] Questline (chained quests with story)
- [ ] Difficulty scaling rework
- [ ] Badge system expansion

### Polish
- [ ] Sound + animation pass
- [ ] Mobile responsive fixes

---

## Known Disabled / Testing Flags

| Flag | Location | Notes |
|------|----------|-------|
| Daily cooldown | `MinigameController.cs` ~L42-46, ~L63-67 | Commented out — re-enable for prod |
| Craft timer | `CraftController.cs` | Instant craft; no `CraftSlot` table yet |
| Workbench upgrade | `HideoutController.cs` | POST returns placeholder response |
| Escape Scroll flee | `BattleController.cs` | Endpoint exists, item defined, flee disabled |
