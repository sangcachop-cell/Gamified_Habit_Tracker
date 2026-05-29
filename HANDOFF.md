# Handoff — Gamified Habit Tracker

**Session date:** 2026-05-29
**Session goal:** Bug fixes (inventory 0-qty, drop cap reset, feed quantity sync, avatar sprite sizing) + full GameItem catalog sync + pet catalog sync (PetCatalogService, full collection grid)

---

## Goal We're Working Toward

Build a Habitica-inspired gamified habit tracker. Full roadmap in `ROADMAP.md`.
Long-term arc: tasks → game economy (HP/mana/gold) → character classes → inventory/shop → pets/mounts → social → guilds/parties → boss quests → challenges.

---

## What Was Done This Session (2026-05-29)

### Bug Fix 1 — Inventory Shows 0-Quantity Items

**File:** `Services/Implementations/EconomyService.cs` — `GetInventoryAsync()`

- **Was:** query had no quantity filter → rows with `Quantity=0` (left by `DeductInventory`) returned
- **Fix:** added `&& i.Quantity > 0` to the WHERE clause
- `SellItemAsync` already guards `Quantity <= 1` → removes row; the display bug was purely the read side

### Bug Fix 2 — Item Drops Permanently Blocked After Hitting Daily Cap

**File:** `Services/Implementations/EconomyService.cs` — `ApplyTaskScoreEconomyAsync()`

- **Was:** day-reset logic (`DailyDropCount = 0`) was INSIDE the `DailyDropCount < DAILY_DROP_CAP` guard
- **Problem:** once cap hit on any day, next day still saw `count >= CAP` → skipped the block entirely → reset never ran → drops blocked forever
- **Fix:** moved day-reset check BEFORE the cap check so count always resets when date changes

### Bug Fix 3 — Stable Feed Page Food Quantity Doesn't Sync

**Files:** `Models/ViewModels/StableResult.cs`, `Services/Implementations/StableService.cs`, `Controllers/StableController.cs`, `Views/Stable/Index.cshtml`

- **Was:** `FeedAsync` response had no food quantity info; JS only updated progress bar, never touched the food `<select>` dropdowns
- **Fix:**
  - Added `NewFoodQuantity` to `StableResult`
  - `FeedAsync` returns `foodInv.Quantity` (after deduct)
  - Controller includes `newFoodQuantity` + `foodGameItemId` in JSON
  - JS after successful feed: updates `×N` text in every pet card's food select; removes option if quantity hits 0

### GameItem Catalog Sync — Full Habitica Stable Catalog Seeded

**Source:** `D:\Download\habitica-develop\habitica-develop\website\common\script\content\`

Expanded `AppDbContext.cs` GameItem seed from 30 → **174 items** (synced from Habitica source):

- **Food (41 items):** IDs 1–6 (drop, existing) + 22–25 (drop, existing) + 92 (Saddle) + 93–122 (Cake ×10, Candy ×10, Pie ×10)
- **Eggs (71 items):** IDs 7–10 (drop, existing) + 26–30 (drop, existing) + 11, 31–91 (62 quest eggs)
- **Potions (62 items):** IDs 12–21 (drop, existing) + 123–174 (52 premium/wacky potions)

All items in `ITEMS.md`. Migration applied.

---

### Pet Catalog Sync — Full Collection View + Hatching Rule Fixes

**Source:** `D:\Download\habitica-develop\habitica-develop\website\common\script\content\stable.js`

#### New: `PetCatalogService` (singleton, in-memory)

**Files:** `Services/IPetCatalogService.cs`, `Services/Implementations/PetCatalogService.cs`, `Models/ViewModels/StableGridModels.cs`

Built ~1,351-entry catalog from 5 categories:
| Type | Animals | Potions | Mounts? |
|------|---------|---------|---------|
| drop | 9 | 10 drop | YES |
| premium | 9 | 51 premium (excl. TeaShop) | YES |
| quest | 70 | 10 drop ONLY | YES |
| wacky | 9 | 8 (TeaShop/Windup/Veggie/Dessert/VirtualPet/Fungi/Cryptid/Alien) | NO (Windup=YES) |
| special | ~30 hardcoded | — | mostly NO |

Key methods: `IsValidHatch(animalKey, colorKey)`, `CanBecomeMount(petKey)`, `GetAnimalGroups()`, `GetAnimalGroupsForMounts()`. Registered as **Singleton** in `Program.cs`.

**New types in `StableGridModels.cs`:** `PetCategory` enum, `AnimalGroup` record, `PetSlotEntry` record.

#### Bug Fix — HatchAsync: Quest Eggs Could Use Premium Potions

**File:** `Services/Implementations/StableService.cs`

- **Was:** no validation on egg+potion combination
- **Fix:** `_catalog.IsValidHatch(animalKey, colorKey)` called before duplicate check — returns error if quest egg + non-drop potion

#### Bug Fix — FeedAsync: Wacky Pets Could Evolve Into Mounts

**File:** `Services/Implementations/StableService.cs`

- **Was:** always set `IsMount=true` when `FeedingPoints >= 50`
- **Fix:** check `_catalog.CanBecomeMount(petKey)` first; wacky pets (except Windup) cap at 50 points but stay as pets

#### Wacky Potion Seeds + Migration

**File:** `Data/AppDbContext.cs` → added IDs 175–181: potion_Veggie, potion_Dessert, potion_VirtualPet, potion_Fungi, potion_Cryptid, potion_Alien, potion_Windup. All `IsDroppable=false, GoldValue=0`. No potion images exist for most (only TeaShop+Windup have images) but pet images do exist.

**Migration:** `AddWackyPotions` — applied ✓

**GameItems now:** 174 (existing) + 7 = **181 total**

#### Stable Page Redesign — Full Collection Grid

**Files:** `Views/Stable/Index.cshtml`, `wwwroot/css/site.css`

- **Was:** 3-tab page showing ONLY owned pets/mounts as Bootstrap cards
- **Now:** full collection grid (like Habitica) — every animal grouped with all color slots
  - Owned pet = full opacity + thin progress bar + click → Bootstrap Offcanvas feed/active panel
  - Evolved to mount = orange border + 🐴 badge
  - Active pet = green border + ★ badge
  - Unowned = 30% opacity grey
  - Tab badges: `X / Y total` progress counters
  - Hatch tab: quest eggs grey-out non-drop potions in UI (+ server-side enforcement)
- **Offcanvas panel:** feed select + Feed button + Make Active/Unequip; food qty updated in-memory after each feed

**StableViewModel** — added `PetsGrid`, `MountsGrid`, `TotalPetsInCatalog`, `TotalMountsInCatalog`

**GetStableViewModelAsync** — rewritten: single `UserPets` query (was 2), O(1) dict merge with catalog groups

---

### Bug Fix 4 — Avatar Mount/Pet Rendering Wrong (Sprite Sizing)

**Files:** `Views/Character/Index.cshtml`, `Views/Equipment/Index.cshtml`, `Views/Character/Customize.cshtml`

- **Was:** ALL sprites (character skin/gear + mount body/head + pet) forced to `width:140px;height:147px`
- **Problem:** native sizes are character=90×90, mount body/head=105–135px (varies per animal), pet=81×99. Forcing all to 140×147 applies different scale factors → misalignment, mount covers entire frame, character sits wrong relative to mount
- **Fix:** refactored avatar stack in all 3 views to match Habitica's actual layout:
  - Outer wrapper: `padding-top:0px` when mounted, `padding-top:24px` when not (shifts character down = standing stance vs riding stance)
  - Inner `<div style="position:relative; margin-left:24px; width:90px; height:90px;">` — equivalent to Habitica's `.character-sprites`
  - Character sprites (skin/hair/armor/gear): `width:90px;height:90px`
  - Mount body/head: no explicit size → renders at natural pixel dimensions
  - Pet: natural size at `bottom:0;left:0`
  - Removed all explicit `z-index` — DOM order handles stacking (mount body → character layers → mount head → pet)

---

## What Was Done Previous Sessions

### 2026-05-28 (session 2)

### Bug Fix 1 — Armoire Gear Worn Image Path

**File:** `Models/GearItem.cs` — `GetWornImagePath()`

- **Was:** armor path = `/gear/armor/{bodyType}_{key}.png`, other slots = `/gear/{slot}/{key}.png` for ALL gear
- **Problem:** armoire keys (e.g. `armor_armoire_admiralsUniform`) live in `/gear/armoire/` folder, not `/gear/{slot}/`
- **Fix:** added `Key.Contains("_armoire_")` branch → routes to `/gear/armoire/{key}.png` (non-armor) or `/gear/armoire/{bodyType}_{key}.png` (armor)
- Verified by checking actual files: `broad_armor_armoire_admiralsUniform.png` is at `/gear/armoire/`, not `/gear/armor/`

### Bug Fix 2 — Item Drop Never Fires

**File:** `Services/Implementations/EconomyService.cs`

- **Was:** `DayRng(userId)` created `new Random(userId * 397 ^ DateTime.UtcNow.DayOfYear)` — same seed every call within a day
- **Problem:** Every task completion used identical seed → identical `NextDouble()` sequence. If first roll missed, ALL task completions that day missed. Additionally, crit RNG and drop RNG used the same seed → fully correlated (if crit missed, drop missed too).
- **Fix:** replaced `DayRng` in both `RollCrit` and `RollDropAsync` call site with `Random.Shared` (thread-safe, truly random per call)
- Deleted `DayRng` method entirely — no longer referenced

### Phase 5 — Pets & Mounts — COMPLETE ✓

Full implementation per Habitica `hatch.js` + `feed.js` mechanics.

#### 5A — Data Layer
- **`Models/GameItem.cs`:** added `Target` (string?, DB col — food preference potion color, e.g. `"Base"`, `"Golden"`), `AnimalKey` (computed `[NotMapped]` — calls existing `EggKey()`), `PotionColorKey` (computed `[NotMapped]`)
- **`Models/UserPet.cs`:** new model — `Id, UserId, PetKey, FeedingPoints (default 5), IsMount (bool), HatchedAt`. Computed helpers: `AnimalName`, `ColorName`, `PetImagePath`, `MountBodyPath`, `MountHeadPath`, `MountIconPath`
- **`Models/User.cs`:** added `ActivePetKey`, `ActiveMountKey` (string?), `OwnedPets` nav property
- **`Data/AppDbContext.cs`:** added `DbSet<UserPet>`, UserPet relationship config (cascade, unique index on `(UserId,PetKey)`, composite index on `(UserId,IsMount)`), expanded GameItem seed:
  - Food IDs 1–6: added `Target` values (Meat→Base, Strawberry→Red, Potato→Desert, Chocolate→Shade, Fish→Skeleton, Honey→Golden)
  - New food IDs 22–25: Milk(White), RottenMeat(Zombie), CottonCandyPink, CottonCandyBlue
  - New egg IDs 26–30: TigerCub, PandaCub, LionCub, Fox, FlyingPig
- **Migration:** `AddPhase5StablePetMount` — applied ✓

**GameItems initial seed:** 30 total (10 food, 10 eggs, 10 potions) — later expanded to full catalog (see below)

#### 5B — StableService
- **`Services/IStableService.cs`** + **`Services/Implementations/StableService.cs`**
- **`Models/ViewModels/StableResult.cs`** + **`Models/ViewModels/StableViewModel.cs`**
- Methods: `HatchAsync`, `FeedAsync`, `SetActivePetAsync`, `SetActiveMountAsync`, `GetStableViewModelAsync`
- **Hatching:** validates egg+potion in inventory, builds `petKey = "{AnimalKey}-{PotionColorKey}"`, blocks re-hatch, deducts both items, creates UserPet at FeedingPoints=5
- **Feeding:** preferred food (food.Target == pet.ColorName) = +5 pts, other = +2 pts (Habitica exact). Evolution at ≥50 pts: `IsMount=true`, `FeedingPoints=50`, clears `ActivePetKey` if this was active pet
- **Registered in `Program.cs`**

#### 5C — StableController + View
- **`Controllers/StableController.cs`:** GET /Stable, POST /Stable/Hatch, POST /Stable/Feed, POST /Stable/SetActivePet, POST /Stable/SetActiveMount — all POST return JSON
- **`Views/Stable/Index.cshtml`:** 3 tabs
  - **Pets tab:** card grid, 64×64 pet image, feeding progress bar (Bootstrap), feed dropdown (food in inventory with ★ for preferred), Make Active / Unequip button
  - **Mounts tab:** card grid, 64×64 mount icon, Ride / Dismount button
  - **Hatch tab:** egg icon grid + potion icon grid (click to select), live preview `<img>` updates on selection, "Hatch!" AJAX button, already-hatched combos disabled

#### 5D — Avatar Overlays
Added pet/mount layers to **3 files**:
- `Views/Character/Index.cshtml`
- `Views/Equipment/Index.cshtml`
- `Views/Character/Customize.cshtml`

Layer order (each): mount body (z-index:0, first) → all character layers → mount head (z-index:10, last) → pet (z-index:5, bottom-left corner `-8px/-12px`, only shown when no mount active)

Added `overflow:visible` to avatar wrapper div in all 3 files.

#### 5E — Nav + Inventory
- **`Views/Shared/_Layout.cshtml`:** added `🐾 Stable` nav link after Inventory
- **`Views/Inventory/Index.cshtml`:** egg + potion cards now show `🥚 Hatch` link to `/Stable#tab-hatch`

---

## Current State (end of 2026-05-29 session)

- **Build:** 0 errors, ~47 warnings (all pre-existing nullability noise in SearchService.cs)
- **DB migrations applied:** `AddPhase4InventoryShop`, `AddArmoireGearItems`, `SyncHabiticaGearCatalog`, `AddPhase5StablePetMount`, `AddWackyPotions`
- **GearItems in DB:** 659 total
- **GameItems in DB:** 181 (41 food, 71 eggs, 62 potions + 7 wacky potions) — full catalog synced 2026-05-29
- **PetCatalog (in-memory):** ~1,351 entries (Drop 90 + Premium 459 + Quest 700 + Wacky 72 + Special 30)
- **Phase 4:** COMPLETE ✓
- **Phase 5:** COMPLETE ✓ (bugs fixed + pet catalog sync + full collection grid)
- **Runtime:** Phase 5 features still UNVERIFIED end-to-end — test checklist below still applies

### Files actively edited this session (2026-05-29)

| File | What changed |
|------|-------------|
| `Data/AppDbContext.cs` | GameItem seed: 30 → 174 items (full catalog); +7 wacky potions IDs 175–181 |
| `Services/Implementations/EconomyService.cs` | `GetInventoryAsync`: added `Quantity > 0` filter; drop cap reset moved before cap check |
| `Models/ViewModels/StableResult.cs` | Added `NewFoodQuantity` |
| `Models/ViewModels/StableViewModel.cs` | Added `PetsGrid`, `MountsGrid`, `TotalPetsInCatalog`, `TotalMountsInCatalog` |
| `Models/ViewModels/StableGridModels.cs` | NEW — `PetCategory` enum, `AnimalGroup` record, `PetSlotEntry` record |
| `Services/IPetCatalogService.cs` | NEW — interface for pet catalog singleton |
| `Services/Implementations/PetCatalogService.cs` | NEW — ~1,351-entry in-memory catalog; `IsValidHatch`, `CanBecomeMount`, `GetAnimalGroups` |
| `Services/Implementations/StableService.cs` | Injected `IPetCatalogService`; fixed HatchAsync (quest-egg rule) + FeedAsync (wacky mount block); rewrote `GetStableViewModelAsync` (single query + catalog merge) |
| `Controllers/StableController.cs` | Feed: returns `newFoodQuantity` + `foodGameItemId` in JSON |
| `Program.cs` | Registered `IPetCatalogService` as Singleton |
| `Views/Stable/Index.cshtml` | Full redesign: animal-grouped collection grid, offcanvas feed panel, quest-egg potion gating |
| `wwwroot/css/site.css` | Added `.pet-slot` styles (owned/unowned/active/mount states, progress bar) |
| `Views/Character/Index.cshtml` | Avatar stack: correct sprite sizes (90×90 char, natural mount/pet), inner container + padding-top |
| `Views/Equipment/Index.cshtml` | Avatar stack: same fix |
| `Views/Character/Customize.cshtml` | Avatar stack: same fix |
| `HANDOFF.md` | This file |

---

## What Was Done Previous Sessions

### 2026-05-28 session 1 — Gear Sync + Bug Fixes

- Fixed 85 class gear names (placeholder → real Habitica locale names)
- Added 108 special gear items (IDs 552–659), `GearClass="special"`
- Fixed task drop toast: `DroppedItemIcon` now `<img>` HTML, not emoji string
- Fixed armoire gear toast in Market: uses `d.gearImgPath` instead of hardcoded ⚔️
- Migration `SyncHabiticaGearCatalog` applied

### 2026-05-28 — Armoire Sync

- 466 real Habitica armoire items seeded (IDs 86–551)
- Fixed `ShopImagePath` for armoire: uses `/gear/armoire/shop/` folder
- Removed wrong `IsArmoire=true` from 4 tier-0 class starter weapons
- Migration `AddArmoireGearItems` applied

### Phase 4 — Inventory & Shop — COMPLETE ✓

- 4.1 Market, 4.2 Quest Shop stub, 4.3 Gem Shop, 4.5 Armoire, 4.6 Inventory
- Migration `AddPhase4InventoryShop` applied

---

## Known Gaps / Deferred

Check this section each session before starting new work.

| Gap | Notes |
|-----|-------|
| **Phase 4.4 — Seasonal Shop** | Not implemented. No timeline. |
| **Costume mode UI toggle** | Backend supports `mode=costume`, Equipment page always posts `mode=equipped`. UI toggle never built. |
| **Rebirth gem cost** | Currently free. Gem deduction wired in service but controller calls free path. |
| **Phase 5 runtime test** | Collection grid + hatching rules never end-to-end tested. Retest with checklist below. |
| **Phase 5 — Release pet/mount** | Not implemented. No "release" action on Stable page. |
| **Phase 5 — Beast Master / Mount Master achievements** | Deferred to Phase 10. |
| **Phase 5 — Saddle item** | Seeded as `potion_Saddle`-equivalent but `FeedAsync` doesn't handle instant-evolution. Logic not implemented. |
| **Phase 5 — Avatar partial extraction** | Avatar HTML duplicated in 3 files (Character/Index, Equipment/Index, Customize). Deferred. |
| **Phase 5 — Wacky potion no-image** | IDs 175–181 seeded but no local potion images for Veggie/Dessert/VirtualPet/Fungi/Cryptid/Alien. Hatch tab won't show them unless user already has them in inventory (which requires admin grant). Pet images DO exist. |
| **Phase 4 runtime test** | Phase 4 features never manually verified. |
| **Drop rate tuning** | Drop RNG uses `Random.Shared`. Rates from `CalcDropChance`. Never load-tested. |
| **GameItem Quantity=0 rows** | `DeductInventory` leaves rows at `Quantity=0`. Both `GetStableViewModelAsync` and `GetInventoryAsync` filter `Quantity > 0`. FIXED 2026-05-29. |

### Phase 5 Test Checklist (do this before Phase 6)

1. `/Stable` Pets tab → full animal grid visible (greyed unowned slots, owned slots full-color)
2. Hatch tab → select quest egg → non-drop potions are greyed/disabled
3. Hatch quest egg + premium potion → server blocks with error toast
4. Hatch quest egg + drop potion → succeeds → slot turns owned in grid
5. Click owned pet slot → offcanvas panel opens with feed select + Make Active button
6. Feed with preferred food (★ in select) → progress bar increments 5 pts
7. Feed with non-preferred food → increments 2 pts
8. Feed to 50 pts → evolution toast → slot gets orange border + 🐴 badge; appears in Mounts grid
9. Feed TeaShop pet to 50 pts → caps at 50, no evolution (wacky rule)
10. Pets tab → Make Active → `/Character` → pet icon visible
11. Mounts tab → click owned mount → sets active → `/Character` → mount body+head visible
12. Set active mount → active pet should NOT show
13. `/Equipment` and `/Character/Customize` → same overlays
14. `/Inventory` → egg/potion cards show `🥚 Hatch` link

---

## Next Step — Phase 6: Social Features

Phase 5 complete. No known blockers. Start Phase 6.

### Phase 6 spec (from ROADMAP.md)

1. **6.1 Private Messages** — inbox (sent+received), send by username, delete own copy, unread badge
2. **6.2 Safety Features** — block/unblock user, flag/report user, flag chat message
3. **6.3 Profile Enhancements** — full profile view (stats, class, gear, pet/mount, achievements), @mention
4. **6.4 Chat Interactions** — like message, flag for admin review

**DB tables needed:**
- `Messages` (Id, SenderId, ReceiverId, Body, SentAt, DeletedBySender, DeletedByReceiver, IsRead)
- `UserBlocks` (BlockerId, BlockedId)
- `Reports` (Id, ReporterId, ReportedUserId, ReportedMessageId?, Reason, CreatedAt)

---

## Source Files Reference

All Habitica source at: `D:\Download\habitica-develop\habitica-develop\website\common\`

| Content | Path |
|---------|------|
| Gear sets (class) | `script/content/gear/sets/{warrior,wizard,rogue,healer,base}.js` |
| Gear sets (armoire) | `script/content/gear/sets/armoire.js` |
| Gear sets (special) | `script/content/gear/sets/special/index.js` (+ special-backer.js, special-contributor.js) |
| English locale | `locales/en/gear.json` |
| Pets/stable catalog | `script/content/stable.js` + `petInfo.js` |
| Food catalog | `script/content/food.js` |
| Hatch logic | `script/ops/hatch.js` |
| Feed logic | `script/ops/feed.js` |

---

## Architecture Notes (all sessions)

- **Session auth:** `HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID)` — no Identity framework
- **Toast system:** `showToast(icon, title, msg, type, duration)` in `_Layout.cshtml` — icon is innerHTML, accepts `<img>` tags
- **HUD refresh:** `refreshHud()` in `_Layout.cshtml`, calls `GET /Economy/GetStats`
- **EconomyService contract (Phase 1–3 methods):** modifies `user` in-memory only. Caller calls `SaveChangesAsync`. No double-save.
- **Phase 4 service methods:** BuyGem, PullArmoire, SellItem all own their own `SaveChangesAsync`
- **StableService methods:** HatchAsync, FeedAsync, SetActivePetAsync, SetActiveMountAsync all own their own `SaveChangesAsync`
- **AJAX antiforgery pattern:** `__RequestVerificationToken=` in body (form-urlencoded). NOT header-only.
- **Badge awarding:** Must `.Include(u => u.UserBadges)` before `_questService.AwardBadgesAsync()`
- **GearItem equipped slots:** stored as string Keys on User, NOT FK to GearItem. Validate ownership before equip.
- **Mage → "wizard" in images:** GearItem.Key uses `wizard` (e.g. `weapon_wizard_3`), GearClass stores `"mage"`
- **GetEffectiveStatsAsync caller contract:** MUST load `user.OwnedGear.ThenInclude(ug => ug.GearItem)` first
- **EconomyService caller contract (cron/score):** callers must pre-load OwnedGear
- **GetStats() JSON field:** use `intel` not `int` (C# keyword conflict). Includes `gems`.
- **Armoire contract:** uses `Random.Shared` (not day-seeded) — intentionally unpredictable
- **Drop RNG:** uses `Random.Shared` — random per task completion. NOT day-seeded anymore (was bug).
- **Sell contract:** GoldValue == 0 → not sellable (enforced at service level)
- **Market Buy:** duplicates EquipmentController buy logic (10-line block). NOT delegated cross-controller.
- **Market filter:** `GearClass in [user.Class, "all", "special"]` — special buyable by all
- **GameItem.ImagePath:** computed property, no DB column. Special cases: `food_Potato`→`Pet_Food_Potatoe.png`, `egg_Bear`→`Pet_Egg_BearCub.png`
- **GameItem.AnimalKey / PotionColorKey:** `[NotMapped]` computed props. Used by StableService to build PetKey.
- **GameItem.Target:** DB column on food items. Potion color this food is preferred for. Null on non-food.
- **UserPet.PetKey format:** `"{AnimalName}-{PotionColor}"` e.g. `"Wolf-Base"`, `"BearCub-Golden"`. No FK to GameItem.
- **UserPet feeding:** preferred food (Target == ColorName) = +5 pts; other = +2 pts. Evolution at ≥50.
- **GearItem.ShopImagePath:** auto-detects armoire via `Key.Contains("_armoire_")` → `/gear/armoire/shop/` folder
- **GearItem.GetWornImagePath:** also checks `Key.Contains("_armoire_")` → `/gear/armoire/` folder (armor: `{bodyType}_{key}`, other: `{key}`)
- **Image path rules:**
  ```
  Skill icon:            /images/habitica/skills/shop_{key}.png
  Shop gear (class):     /images/habitica/gear/{slot}/shop/shop_{key}.png
  Shop gear (armoire):   /images/habitica/gear/armoire/shop/shop_{key}.png
  Worn armor (class):    /images/habitica/gear/armor/{broad|slim}_{key}.png
  Worn armor (armoire):  /images/habitica/gear/armoire/{broad|slim}_{key}.png
  Worn other slots:      /images/habitica/gear/{slot}/{key}.png
  Worn armoire other:    /images/habitica/gear/armoire/{key}.png
  Food:                  /images/habitica/stable/food/Pet_Food_{Name}.png
  Egg:                   /images/habitica/stable/eggs/Pet_Egg_{Name}.png
  Potion:                /images/habitica/stable/potions/Pet_HatchingPotion_{Name}.png
  Pet grid:              /images/habitica/stable/pets/Pet-{Animal}-{Color}.png
  Mount icon:            /images/habitica/stable/mounts/icon/Mount_Icon_{Animal}-{Color}.png
  Mount head:            /images/habitica/stable/mounts/head/Mount_Head_{Animal}-{Color}.png
  Mount body:            /images/habitica/stable/mounts/body/Mount_Body_{Animal}-{Color}.png
  ```
- **XP model:** Cumulative. `CalculateLevel` iterates thresholds; max 100 loops.
- **Effective stats formula:** `base + gearBonus + classBonus(×0.5 if gearClass==userClass) + floor(level/2) + buff`
- **MaxMana:** `effectiveINT × 2 + 30`
- **Perfect Day guard:** `anyDailyDue && allDailiesCompleted`. No Dailies → no buff.
- **CharacterClass location:** top-level static class in `HabitTracker.Constants` — NOT nested under `AppConstants`
- **TempData toast pattern:** set `TempData["ToastXxx"]` in controller, `_Layout.cshtml` renders hidden div, JS calls `showToast` on DOMContentLoaded.
- **Avatar layer order:** mount body → skin → shirt → armor → bangs → hair → mustache → beard → head gear → shield → weapon → mount head → pet (bottom-left corner)
- **Avatar sprite sizing:** character sprites = 90×90, mount body/head = natural size (105×105 or 135×135 varies per animal), pet = natural size (81×99 typical). All inside inner `<div style="position:relative; margin-left:24px; width:90px; height:90px;">`. Outer wrapper: `padding-top:0` when mounted, `padding-top:24px` when not. No z-index — DOM order only.
- **Drop cap reset:** must happen BEFORE the `DailyDropCount < DAILY_DROP_CAP` check, not inside it. Otherwise cap persists across days.
- **PetCatalogService:** Singleton. Built once at startup. ~1,351 entries: Drop(90)+Premium(459)+Quest(700)+Wacky(72)+Special(30). NO DB table — pure in-memory like Habitica's JS catalog.
- **PetCatalog hatching rule:** quest eggs (IsDroppable=false) → ONLY drop colors valid. Drop animals → drop | premium | wacky. `IsValidHatch(animalKey, colorKey)` enforces this.
- **PetCatalog mount rule:** wacky colors (TeaShop, Veggie, Dessert, VirtualPet, Fungi, Cryptid, Alien) → `CanBecomeMount=false`. Exception: Windup → `CanBecomeMount=true` (mount images confirmed). `CanBecomeMount(petKey)` enforces this.
- **Wacky potions (IDs 175–181):** seeded in GameItems but no local potion images for most. Pet images DO exist (e.g. `Pet-Wolf-Veggie.png`). `GetStableViewModelAsync` won't show them in Hatch tab unless user has them in inventory.
- **Stable page grid:** `StableViewModel.PetsGrid` / `MountsGrid` are `List<AnimalGroup>`. Each `AnimalGroup` has `IReadOnlyList<PetSlotEntry>` with `IsOwned`/`IsMount`/`FeedingPoints`/`IsActivePet`/`IsActiveMount` merged in from UserPets table. Safe to use directly in view — no second DB call.
- **Offcanvas feed panel:** JS `petDataMap` (serialized from `OwnedPets`) + `foodItems` array (serialized from `FoodInInventory`) embedded inline in page. Feed updates progress bar + food qty in-memory without reload. Evolution triggers page reload.
- **PetFormatAnimalName / FormatColorName:** static helpers in `PetCatalogService` — insert space before each uppercase letter (e.g. `TigerCub` → `Tiger Cub`). Used for display labels.

---

## Connection String

Stored in .NET User Secrets (never committed):
```
dotnet user-secrets list --project HabitTracker
```
