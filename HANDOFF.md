# Session Handoff

## Goal

Build a gamified habit tracker RPG where real-life habits (quests) are completed via interactive minigames. Players earn XP, stats, and loot. Core loop: complete quests → get stronger → survive harder forest raids → craft better gear → repeat.

Pillars not yet finished: craft timer, stamina cost, chest loot in interiors, boss encounters, market/economy, more minigames/quests.

---

## Current Code State (branch: `fix-branch`)

Last commits this session:
- `37d1279` — feat: localize all core UI to Vietnamese
- Several uncommitted changes from this session (Tetris minigame, monster overhaul, item expansion, dark theme fixes, inventory/loot bug fixes)

### What works end-to-end

- **Full Vietnamese UI** — all major pages dark-themed and translated: navbar, forest, inventory, hideout, leaderboard, friend, battle, character, badges, statistics, register, login, dashboard
- **Tetris minigame** — `Views/Minigame/Tetris.cshtml`; 10×20 board, 7 tetrominoes, CW rotation with wall kicks, ghost piece, hard drop (SPACE), R-key rotate mid-drag (via hover), difficulty 3/8/15 lines
- **Quest "Đọc sách"** — Id=3, Category="Học tập", FacilityId=3 (Thư Viện), MinigameType="Tetris"; migrated
- **11 monsters** with scaled stats — see GAME_CONTENT.md. Each has per-monster loot table + exclusive Mythic drop
- **53 new fantasy items** in `ItemCatalogue.cs` — Common through Mythic, Vietnamese names + descriptions, placeholder ❓ icon
- **Facility names/descriptions** in DB → Vietnamese (migration `VietnameseFacilities` applied)
- **Forest inventory panel** — shows both DB items AND session Pouch items (merged in Map GET action)
- **Loot screen** — pouch items moveable via `POST /Forest/Loot/MovePouch`; rotatable via `POST /Forest/Loot/RotatePouch`; body items rotate via `POST /Forest/Loot/RotateBody`
- **R-key rotate** — hover over item → press R → rotates. Works in all 3 inventory surfaces (Inventory/Index, Forest Map panel, Loot screen). Uses `mouseover` tracking because HTML5 DnD suppresses `keydown` mid-drag
- **Dark theme** — all pages fixed: Leaderboard, Friend, Statistics, Badges, Register, Hideout, Battle, Character all use dark backgrounds, no white cards
- **Forest combat log** — fully Vietnamese (attack/defend/flee/speed messages)
- **Monster descriptions** — all 11 monsters have Vietnamese names + flavour text in `ForestMap.MakeMonster()`

### Intentionally disabled

| Flag | Location | Notes |
|------|----------|-------|
| Daily cooldown | `MinigameController.cs` ~L42-46, ~L63-67 | Commented out |
| Craft timer | `CraftController.cs` | Instant; no `UserCraftSlot` table |
| Workbench upgrade | `HideoutController.cs` | Placeholder endpoint |
| Chest loot | `ForestMap.cs` / `ForestController.cs` | Visual only |

### DB state

- 3 quests seeded: Id=1 Tập thể dục (QTE), Id=2 Chạy bộ (Dino), Id=3 Đọc sách (Tetris)
- 7 facilities seeded with Vietnamese names: Sân Tập Luyện / Thiền Đường / Thư Viện / Đường Chướng Ngại / Doanh Trại / Phòng Kho / Bàn Thợ
- Migrations applied: `AddDocSachQuest`, `VietnameseFacilities`
- `User.Wood` / `User.Stone` = integer material counts (NOT inventory items)

---

## Key Files (actively edited this session)

| File | What changed |
|------|-------------|
| `Constants/ItemCatalogue.cs` | Added 53 new items (35 fantasy + 18 monster-unique); all Vietnamese |
| `Constants/ForestMap.cs` | 11 monsters with `MakeMonster(id, lvl)`; `GetMonsterId(rng, locationId)`; per-monster `LootTables.ByMonster`; location names/exits in Vietnamese |
| `Models/ForestSession.cs` | Added `PendingMonsterId` field |
| `Models/ForestCombatState.cs` | Added `MonsterId`, `MonsterDescription` fields |
| `Controllers/ForestController.cs` | 7 call sites updated for `PendingMonsterId`; added `LootMovePouch`, `LootRotateBody`, `LootRotatePouch` actions; Map GET merges Pouch items into panel data; combat log → Vietnamese |
| `Data/AppDbContext.cs` | Facility names + descriptions → Vietnamese; quest Id=3 added |
| `Views/Minigame/Tetris.cshtml` | New file — full canvas Tetris |
| `Views/Forest/Map.cshtml` | Inventory panel: pouch-aware move/rotate/drag; R-key via hover; `pointer-events:none` on item spans |
| `Views/Forest/Loot.cshtml` | `MovePouch` / `RotatePouch` / `RotateBody` wired; R-key via `MutationObserver` + hover; body rotate button removed; border-offset fix on drop |
| `Views/Inventory/Index.cshtml` | Rotate buttons removed; R-key via hover; `pointer-events:none` on spans |
| `Views/Account/Leaderboard.cshtml` | Dark theme, "Level" → "Cấp" |
| `Views/Friend/Index.cshtml` | Dark theme, "Level" → "Cấp" |
| `Views/Dashboard/Statistics.cshtml` | Dark theme, English labels → Vietnamese |
| `Views/Dashboard/Badges.cshtml` | Dark theme, rarity labels → Vietnamese, "NEW" → "MỚI" |
| `Views/Account/Register.cshtml` | Right panel dark, labels → Vietnamese |
| `Views/Battle/Index.cshtml` | "Battle Arena" → "Đấu Trường", wave labels, all English → Vietnamese |
| `Views/Dashboard/Character.cshtml` | Class names → Vietnamese, table headers, stat names, dark `var(--ink)` color bug fixed |
| `Views/Hideout/Index.cshtml` | Dark body bg, facility names, English strings → Vietnamese |
| `Views/Shared/_Layout.cshtml` | `lang="en"` → `lang="vi"` |

---

## What Failed / Watch Out For

### Monster system
- Old system: `MakeMonster(tier, lvl)` with only 2 monsters. Replaced with `MakeMonster(monsterId, lvl)` with 11 monsters and `GetMonsterId(rng, locationId?)`.
- `ForestMonster` record now has `Id` field — any old session JSON in browser cookies will have stale `PendingMonsterId = ""` (defaults to `"forest_scout"` from model default, safe).

### Inventory drag
- **Root cause of drag not working**: child `<span>` inside `.inv-item` was intercepting mousedown before the parent `draggable=true` div. Fix: `style="pointer-events:none;"` on all item inner spans in all three surfaces.
- **Border offset bug**: `.grid-box` / `.inv-grid` has `border: 2px solid` with `box-sizing: content-box`. Drop target `getBoundingClientRect().left` includes border. Drop position calc must subtract `bw=2` before dividing by `CELL`. Fixed in Loot.cshtml; NOT yet fixed in Map.cshtml panel or Inventory/Index.cshtml (Map uses absolute-positioned items so visual matches; Inventory uses `+1px` inset convention which partially compensates).

### R-key rotate
- **Failed approach**: `keydown` inside HTML5 DnD drag. Browser enters drag-capture mode; `keydown` events are suppressed in most browsers (Chrome, Firefox) while drag is active. `drag !== null` / `dragData !== null` check never passes.
- **Working approach**: `mouseover` tracking. User hovers over any item → `hovItem` / `hovInv` / `hovMapItem` is set → pressing R fires the rotate call. No drag dependency.
- In Loot.cshtml: `MutationObserver` watches grid childList changes to attach hover listeners after `renderBody()` / `renderPlayer()` create items dynamically.

### Razor / C# gotchas (carry-forward)
- **`@` in JS** → RZ1003 parser error. Use `//` without `@`; write "at" in strings.
- **`@@keyframes`** in Razor `<style>` → must be `@@keyframes` not `@keyframes`.
- **C# in tag helper attribute** → `<form asp-action="X" @(condition ? ...)>` is RZ1031. Use `@{ bool flag = ...; }` then `style="@(flag ? "display:none" : "")"`.
- **Hex opacity in Razor** → `@(color)22` not `@color22` (Razor reads `color22` as variable name).
- **`data-pocket='...'` single-quote HTML attr** — `System.Text.Json` does NOT escape `'`. Any item Name/Description with apostrophe breaks the attribute. Current Vietnamese translations have no apostrophes. Watch if adding English names back.

### Loot / Pouch system
- Pouch items have `Id = 0` from `BuildPlacedPouchItem`. Never pass id=0 to `/Inventory/Move` — always route through `/Forest/Loot/MovePouch` instead.
- `MovePouch` finds item by `Container + GridX + GridY` tuple. Unique within a pouch because no overlapping items are allowed.
- Discard of pouch items from Map panel is intentionally blocked — user must abandon at extract or lose on death.

### Interior system (unchanged from before)
- `ForestSession.CurrentMapId = null` → world 128×128
- `= "cave" | "warehouse" | "lake"` → interior 64×64
- Exit zones at border=2 (passable x/y = [2..61])
- Cave exits: N(y=2) W(x=2) E(x=60) — exits must be at ≥2 or they're inside water border

---

## Next Steps (priority order)

1. **Commit all session work** — large diff across many files; commit with meaningful message grouping (monsters + items, UI fixes, inventory bugfixes, Tetris).

2. **Chest loot in interiors** — `ForestMap.cs` already has `ChestPos[]` per interior (visual only). Need:
   - POST `/Forest/OpenChest` — validate player is adjacent to chest cell; award loot to `session.Pouch`
   - Per-location loot tables in `LootTables` (different from monster drops)
   - JS in `Map.cshtml`: detect chest cells on click → show popup → POST

3. **Re-enable daily cooldown** — uncomment 2 blocks in `MinigameController.cs` L42-46, L63-67.

4. **Stamina cost** — add `PlayerStamina` to `ForestSession`; deduct per step in `Move()` and `MoveInterior()`; force exit or death when empty.

5. **Craft timer** — `UserCraftSlot` model (UserId, SlotIndex, RecipeId, StartedAt, CompletesAt); migration; `CraftService` collects on Hideout load.

6. **Item icons** — all new items use ❓ placeholder. Design emoji or custom icons per category.

7. **Loot tier population** — `ForestMap.LootTables.ByMonster` has pools defined per monster per rarity. All non-Common tiers are populated. Common tiers also have monster-specific drops. `heart_of_the_forest` is **Ancient Warden only** — do not add it to any other pool.

8. **Boss trigger system** — `ancient_warden` currently has 0.2% chance in open world. Consider adding a dedicated spawn trigger (rare map event, boss room mechanic, etc.) so it's findable but not just random luck.

9. **Inventory border-offset fix** — Map.cshtml panel and Inventory/Index.cshtml `targetCell()` both skip the 2px border subtraction. Low priority since placement works close enough, but causes 1-cell off-by-one near grid edges.
