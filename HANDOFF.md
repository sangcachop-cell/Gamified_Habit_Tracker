# Session Handoff

## Goal

Build a gamified habit tracker RPG where real-life habits (quests) are completed via interactive minigames. Players earn XP, stats, and loot. Core loop: complete quests → get stronger → survive harder forest raids → craft better gear → repeat.

Pillars not yet finished: more quests/minigames, craft timer, stamina cost, boss encounters, chest loot in location interiors, market/economy.

---

## Current Code State (branch: `fix-branch`)

Last commit on repo: `c872322`. Current session work (Việt hóa UI) is **uncommitted local changes**.

### What works end-to-end

- **Fantasy UI + Full Vietnamese** — dark theme (Cinzel font, gold/purple), toàn bộ text đã dịch sang tiếng Việt: navbar, forest views (Rừng Sâu, Chiến Đấu, Chiến Lợi Phẩm, Rút Lui, Tử Vong), inventory, login, dashboard
- **Forest raid** — 128×128 map, WASD-only movement (click-to-move removed), A* used internally for post-combat path continuation
- **Location interiors** — Cave/Warehouse/Lake → 64×64 sub-map with unique terrain, rare encounters (always Forest Brute), exit zones, chest markers (visual only)
- **Pity system** — `StepsSinceLastCombat`: minimum 10 steps between encounters; resets on combat and on interior entry
- **Combat** — turn-based, path animates to combat cell before redirecting (no teleport)
- **Loot staging (Pouch)** — items go to `session.Pouch` on pickup, NOT DB; committed only on successful Extract; death/server restart = loot lost
- **Inventory** — grid drag-drop, equipment slots (Ba Lô/Giáp/Rig Chiến Thuật), rotate (AJAX), items render +1px inset so borders never clip
- **Rig slot constraint** — rotate blocked server-side if violates W=1,H=2
- **Hideout** — 7 facilities, instant craft (AJAX, no reload), storage DnD
- **Minigames** — QTE circle + Dino runner; daily cooldown disabled for testing

### Intentionally disabled

| Flag | Location | Notes |
|------|----------|-------|
| Daily cooldown | `MinigameController.cs` ~L42-46, ~L63-67 | Commented out |
| Craft timer | `CraftController.cs` | Instant; no `UserCraftSlot` table |
| Workbench upgrade | `HideoutController.cs` | Placeholder endpoint |
| Chest loot | `ForestMap.cs` / `ForestController.cs` | Visual only |

### DB state

- 2 quests seeded: Id=1 Tập thể dục (QTE), Id=2 Chạy bộ (Dino)
- 7 facilities seeded: 1=Training Grounds … 7=Workbench
- `User.Wood` / `User.Stone` = integer material counts (NOT inventory items)
- Raw wood/stone = `UserInventoryItem` entries

---

## Key Files

| File | Purpose |
|------|---------|
| `Models/ForestSession.cs` | Session state; `CurrentMapId`, `WorldReturnX/Y`, `StepsSinceLastCombat`, `Pouch` |
| `Constants/ForestMap.cs` | Zones, spawns, extracts, `Interiors[]` (3 location interior defs with exits/chests) |
| `Controllers/ForestController.cs` | Map, Move, MoveInterior, EnterLocation, ExitLocation, Combat, Loot, Extract, Dead |
| `Views/Forest/Map.cshtml` | Canvas map; dynamic `MAP_W/H/WATER_BORDER/MAP_MODE`; full Vietnamese UI |
| `Views/Forest/Combat.cshtml` | Tấn Công / Phòng Thủ / Bỏ Chạy |
| `Views/Forest/Loot.cshtml` | Chiến Lợi Phẩm screen |
| `Views/Forest/Dead.cshtml` | Tử Vong screen |
| `Views/Forest/Result.cshtml` | Rút Lui / Mất Tích result |
| `Views/Forest/Index.cshtml` | Rừng Sâu entry page |
| `Controllers/InventoryController.cs` | `RotateItemAsync` checks `SlotConstraint` |
| `Services/Implementations/InventoryService.cs` | Slot constraint check on rotate |
| `Views/Inventory/Index.cshtml` | Kho Đồ; +1px inset items; Pocket bg-image; Rig tall slot-cells |
| `Views/Shared/_Layout.cshtml` | Navbar toàn tiếng Việt |
| `Views/Account/Login.cshtml` | Bước Vào Vương Quốc |
| `Views/Hideout/Index.cshtml` | Craft AJAX, DnD storage |
| `Controllers/CraftController.cs` | Returns `removedItemId`, `inputItemId`, `newWood`, `newStone` |
| `Constants/WorkbenchCatalogue.cs` | Craft recipes, slots per level |
| `Constants/ItemCatalogue.cs` | Item defs + Rarity |
| `GAME_CONTENT.md` | Design doc. Edit here → say "sync" → Claude updates seed + migration |
| `TODO.md` | Priority queue + disabled flags |

---

## What Failed / Watch Out For

### Razor / C# gotchas
- **`@` in JS** → RZ1003 parser error. Use `//` without `@`; write "at" in strings.
- **`@@keyframes`** in Razor `<style>` → must be `@@keyframes` not `@keyframes`.
- **C# in tag helper attribute** → `<form asp-action="X" @(condition ? ...)>` is RZ1031. Use `@{ bool flag = ...; }` then `style="@(flag ? "display:none" : "")"`.
- **Hex opacity in Razor** → `@(color)22` not `@color22` (Razor reads `color22` as variable name).

### Inventory / Grid
- **Item border clipping** — tried `box-shadow: inset` first (didn't fix all edges). Final fix: render items at `GridX*C+1, GridY*C+1` size `W*C-2, H*C-2`; update drag JS and rotate JS too.
- **Double grid on Pocket/Rig** — CSS `background-image` + slot-cell `<div>` both showed. Fix: Pocket = bg-image only; Rig = tall slot-cells + `background-image:none`.
- **Slot constraint rotate bypass** — `RotateItemAsync` only checked bounds, not `SlotConstraint`. Fixed: check `SlotConstraint` before bounds.

### Forest / Interior
- **Combat teleport** — JS was immediately jumping player position and redirecting without animating. Fixed: run partial path animation loop before the 400ms redirect.
- **World zones in interior** — `LOCATIONS` JS constant always serialized all world zones. Fixed: serialize `[]` when `MapMode == "interior"`.
- **Exit zones unreachable** — exits at y=1/x=1 are inside `WATER_BORDER=2` → impassable. Fixed: north exits y=2, west x=2, east x=60, south y=60.
- **Loot double-fire** — fixed in earlier session with `busy` flag + `e.stopPropagation()`.

### Việt hóa
- **Login page** — was already edited to English (fantasy copy) in UI overhaul session. Had to re-translate back to Vietnamese this session.
- **Dashboard** — was mostly already Vietnamese from previous dev; just fixed "Welcome back" → "Chào mừng trở lại" and "Level" → "Cấp".
- **Task/Index** — also mostly Vietnamese already; only needed "Other Quests" → "Nhiệm Vụ Khác".

---

## Interior System Reference

```
ForestSession.CurrentMapId = null       → bản đồ thế giới 128×128
ForestSession.CurrentMapId = "cave"     → Hang Động 64×64
                           = "warehouse" → Kho Hàng 64×64
                           = "lake"     → Hồ 64×64

StepsSinceLastCombat → pity counter (tối thiểu 10 bước giữa 2 trận)
WorldReturnX/Y       → vị trí thế giới để khôi phục khi ExitLocation
```

Interior specs (ForestMap.cs `Interiors[]`):
- All 64×64, border=2 (passable area x/y=[2..61])
- Cave: 40% encounter, exits N(y=2) W(x=2) E(x=60), chests: (18,20)(42,38)(10,50)
- Warehouse: 35% encounter, exits N(y=2) S(y=60) W(x=2), chests: (10,12)(50,14)(32,48)
- Lake: 45% encounter, exits N(y=2) S(y=60) E(x=60), chests: (15,30)(48,18)(30,52)
- Always spawns Forest Brute (rare tier) inside

---

## Vietnamese Translation Coverage

Đã dịch xong:
- ✅ Navbar + dropdown + footer
- ✅ Forest: Index, Map (panel + log + buttons + JS strings), Combat, Loot, Dead, Result
- ✅ Inventory: header, equipment panel (Ba Lô/Giáp/Rig), container labels, info panel
- ✅ Dashboard: greeting, level label
- ✅ Task: "Other Quests" fallback
- ✅ Login: hero copy + button

Chưa dịch (ít ảnh hưởng hoặc chưa quan trọng):
- ⚠️ `Views/Account/Register.cshtml` — còn một số label tiếng Anh
- ⚠️ `Views/Account/Leaderboard.cshtml` — "Top XP", "Top Streak", column headers
- ⚠️ `Views/Friend/Index.cshtml` — search, accept/reject buttons
- ⚠️ `Views/Battle/Index.cshtml` — wave labels, stats
- ⚠️ `Views/Dashboard/Character.cshtml` — stat formulas, class names (Warrior/Monk/Scholar...)
- ⚠️ `Views/Dashboard/Badges.cshtml`, `Statistics.cshtml`
- ⚠️ Error messages từ controllers (TempData["Error"], validation strings)
- ⚠️ Item descriptions trong `ItemCatalogue.cs` (tiếng Anh)
- ⚠️ Facility descriptions trong `AppDbContext.cs` seed

---

## Next Steps (priority order)

1. **Commit Việt hóa** — `git add` các view files đã dịch → commit.

2. **Chest loot trong interior** — định nghĩa loot table per location trong `ForestMap.cs`. Click vào chest cell → POST `/Forest/OpenChest` → validate position, award loot to `session.Pouch`.

3. **Re-enable daily cooldown** — bỏ comment 2 block trong `MinigameController.cs` khi xong test.

4. **Loot tiers** — thêm items vào `ForestMap.LootTables.Forest[Rarity.Uncommon]` và cao hơn.

5. **Stamina cost** — thêm `PlayerStamina` vào `ForestSession`; trừ mỗi bước trong `MoveInterior()` và `Move()`; hết stamina → buộc rút lui hoặc chết.

6. **Craft timer** — `UserCraftSlot` model (UserId, SlotIndex, RecipeId, StartedAt, CompletesAt), migration, `CraftService` collect on Hideout load.

7. **Dịch nốt các trang còn lại** — Register, Leaderboard, Friend, Battle, Character, Badges, Statistics.
