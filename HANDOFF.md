# Handoff — Gamified Habit Tracker

**Session date:** 2026-05-30
**Session goal:** Stable UX polish — pet feeding flow, tab restructure, text visibility fixes

---

## What Was Done This Session (2026-05-30, session 8)

### Stable — Feed UX Redesign

**Old flow (removed):** Click pet → offcanvas panel → select food → feed button. Broken: pet onclick never fired.

**Root cause of broken onclick:** Razor `@(condition ? $"onclick=\"handler('{key}')\"" : "")` HTML-encodes the output → `onclick` attribute rendered as `onclick=&quot;...&quot;` → browser never registers it as an event handler. Same bug affected mount slots.

**Failed attempt:** First added offcanvas with inline onclick — onclick never fired, nothing happened on pet click.

**Fix:** Removed all inline Razor onclicks from pet/mount slots. Replaced with JavaScript event delegation:
```javascript
document.getElementById('tab-my-pets').addEventListener('click', function(e) {
    var card = e.target.closest('.owned-pet-card');
    ...
});
```
Also added `.catch()` to all feed/hatch fetch calls to surface silent failures.

**New feed flow:**
- Food bar at top of My Pets tab shows owned food items
- Click food → orange highlight + dashed outline on feedable pets + hint text
- Click pet → feeds with selected food, updates progress bar + qty inline
- ESC or click same food or ✕ Cancel → exits feed mode
- Click pet without food selected → toggle active / unequip

**Files changed:**
- `Views/Stable/Index.cshtml` — full rewrite

---

### Stable — Tab Restructure (5 tabs)

**Old:** 3 tabs — Pets (full catalog + food bar), Mounts (full catalog), Hatch

**New:**
| Tab | Content |
|-----|---------|
| 🐾 My Pets | Owned pets only (flat grid), food bar, active pet banner, feed/activate |
| 🐴 My Mounts | Owned mounts only (flat grid), active mount banner, ride/dismount |
| 📖 Pet Collection | Full catalog read-only (was old Pets tab content, minus food bar) |
| 📖 Mount Collection | Full catalog read-only (was old Mounts tab content) |
| 🥚 Hatch | Unchanged |

Owned pet/mount cards use new `.owned-pet-card` / `.owned-mount-card` CSS classes (80px wide, progress bar, label).

**Files changed:**
- `Views/Stable/Index.cshtml`

---

### Pet/Mount Mutual Exclusivity

**Problem:** User could have both active pet AND active mount simultaneously.

**Fix:** `StableService.SetActivePetAsync` clears `user.ActiveMountKey` when equipping a pet. `SetActiveMountAsync` clears `user.ActivePetKey` when equipping a mount. Unequipping (empty key) leaves the other unchanged.

**Files changed:**
- `Services/Implementations/StableService.cs`

---

### UI Fixes

**Pet position in avatar:** Was `bottom:0;left:0` — overlapped player sprite. Changed to `bottom:-10px;left:-14px` in all 3 avatar views (Character/Index, Equipment/Index, Character/Customize).

**Gear card name text invisible:** `bg-dark` cards didn't set text color → names invisible (dark text on dark bg). Added `text-white` to name divs in:
- `Views/Market/Index.cshtml` (gear shop cards)
- `Views/Inventory/Index.cshtml` (item cards + gear cards, 2 occurrences)

**Footer "5 Features" text removed:**
- `Views/Shared/_Layout.cshtml`

---

## Current State (end of 2026-05-30 session 8)

- **Build:** 0 errors
- **Stable UX:** fully redesigned — 5 tabs, food bar → click-to-feed, event delegation, pet/mount mutual exclusivity
- **Avatar pet offset:** `bottom:-10px;left:-14px` in all 3 avatar views
- **Card text:** `text-white` added to gear/item name divs in Market + Inventory
- **Next:** Phase 9 — Challenges

---

## Architecture Notes Added This Session

- **Razor inline onclick encoding bug:** `@(condition ? "onclick=\"handler()\"" : "")` HTML-encodes the output. The `onclick` attribute is never registered. Use JavaScript event delegation instead: `element.addEventListener('click', e => { var target = e.target.closest('.selector'); ... })`. This applies to ALL dynamically-conditioned onclick attributes in Razor views.
- **Event delegation pattern for Stable:** `#tab-my-pets` listens for `.owned-pet-card` clicks; `#tab-my-mounts` listens for `.owned-mount-card` clicks. Collection tabs (`#tab-pet-collection`, `#tab-mount-collection`) are read-only — no click handlers.
- **Pet/mount mutual exclusivity:** Enforced in service layer. `SetActivePetAsync` sets `ActivePetKey` + clears `ActiveMountKey`. `SetActiveMountAsync` sets `ActiveMountKey` + clears `ActivePetKey`. Clearing (empty key) does NOT affect the other field.
- **Stable tab IDs:** `tab-my-pets`, `tab-my-mounts`, `tab-pet-collection`, `tab-mount-collection`, `tab-hatch`.
- **`feed-targeting` CSS class:** Applied to `#tab-my-pets` (not body). Targets `.owned-pet-card` inside it with crosshair cursor + dashed orange outline.
- **bg-dark card text rule:** Always add `text-white` to name/title text inside `bg-dark` Bootstrap cards. Bootstrap 5 does not auto-invert text color for bg-* utilities unless you also add `text-*`.

---

## What Was Done This Session (2026-05-30, session 7)

### Feature — Equipment Slots: back / eyewear / headAccessory

**Root cause:** `primarySlots` in `Views/Equipment/Index.cshtml` only listed 4 slots. All backend (controller, SetSlot, EquippedKeys dict) already handled 8 slots. Pure view bug.

**Fix — 3 view files:**

`Views/Equipment/Index.cshtml`:
- Added `"back"`, `"eyewear"`, `"headAccessory"` to `primarySlots` array (line 7)
- Added `backGear`, `eyewearGear`, `headAccessoryGear` vars from `equippedGear.GetValueOrDefault(...)`
- Added `id="layer-back"` img before `layer-skin` (after mount-body)
- Added `id="layer-eyewear"` img after `layer-beard`
- Added `id="layer-headAccessory"` img after `layer-head`
- All 3 new layers: `onerror` tries `.gif` before hiding (see bug fix below)

`Views/Character/Customize.cshtml`:
- Added `backGear`, `eyewearGear`, `headAccessoryGear` via existing `FindGear()`
- Added `id="layer-back-gear"`, `id="layer-eyewear-gear"`, `id="layer-headaccessory-gear"` imgs at same positions
- Same `onerror` pattern

`Views/Character/Index.cshtml`:
- Added same 3 gear vars + conditional `@if (backGear != null)` img blocks at correct z-positions
- Same `onerror` pattern

JS note: `updateAvatarLayer` in Equipment already uses `'#layer-' + slot` → back/eyewear/headAccessory automatically supported with no JS changes.

---

### Feature — Background Customization

**Files changed:**

`Models/User.cs` — added:
```csharp
[StringLength(100)]
public string? Background { get; set; }
```

`Models/ViewModels/CustomizeViewModel.cs` — added:
```csharp
public List<string> Backgrounds { get; set; } = new();
```

`Controllers/CharacterController.cs`:
- In `Customize()`: enumerate `background_*.png` from `wwwroot/images/habitica/backgrounds/`, extract key after `background_` prefix, add to vm as `Backgrounds`
- Added `POST /Character/SetBackground` endpoint: sets `user.Background = key` (null if empty/whitespace), `SaveChangesAsync()`

`Views/Character/Customize.cshtml`:
- Added `🌄 Background` tab button
- Background tab pane: `None` option div + grid of all 395 `background_*.png` thumbnails (60×40px, `object-fit:cover`)
- Avatar-preview div: `background-image` CSS from `u.Background` on initial load
- JS `setBackground(key)`: updates `#avatar-preview` `style.backgroundImage` inline; highlights selected thumbnail; fires `POST /Character/SetBackground` AJAX (uses existing CSRF token from customize-form)

`Views/Equipment/Index.cshtml` and `Views/Character/Index.cshtml`:
- Avatar div: `background-image` CSS from `u.Background` on initial load (static, no live selection needed)

**Migration:** `20260530053510_AddUserBackground` — adds `Background NVARCHAR(100) NULL` to `Users` table. Applied.

**Background image notes:**
- No `icon_background_*` files exist locally — using full `background_*.png` at 60×40 display size
- 395 backgrounds enumerated from filesystem at request time (no DB, no seed needed)

---

### Bug Fix — Back Gear Image Renders Broken (`heroicAureole.gif`)

**Problem:** `back_special_heroicAureole` equipped → broken image icon in avatar preview.

**Root cause:** `GearItem.GetWornImagePath()` always returns `.png` extension. The actual local file is `back_special_heroicAureole.gif` — only `.gif` file in all gear folders (confirmed by glob). So the `.png` path 404s.

**Failed attempt:** First `onerror` added was `"this.onerror=null;this.style.display='none'"` — this hides the gear entirely instead of showing it.

**Fix:** Changed `onerror` on all 3 new gear layers in all 3 views to try `.gif` before hiding:
```javascript
onerror="if(!this.src.endsWith('.gif')){this.src=this.src.replace('.png','.gif');}else{this.onerror=null;this.style.display='none';}"
```
Logic: PNG fails → retry as `.gif` (second onerror fires) → if `.gif` also fails → `this.onerror=null` + `display:none`.

**Also fixed:** `overflow:hidden` on avatar-preview divs — initial implementation used `overflow:hidden` to contain background image. Wrong: `background-image` CSS clips to element box naturally (no overflow), and `overflow:hidden` clipped mount heads that extend beyond the 90×90 inner div. Reverted all 3 avatar divs back to `overflow:visible`.

---

### UX — Stable Page: Unmount + Feed Discoverability

**Problem:** No visible button to dismount, no obvious entry point to feed pets. Feed is inside offcanvas panel (opened by clicking a pet slot) — not discoverable. Dismount required knowing to click the active mount slot again.

`Views/Stable/Index.cshtml`:

**Pets tab** — added above the pet grid:
- If `u.ActivePetKey` set: blue banner showing pet icon + name + **✕ Unequip** button (calls `setActivePet('')`)
- Hint text: "Click any owned pet to feed or manage it."

**Mounts tab** — added above the mount grid:
- If `u.ActiveMountKey` set: blue banner showing mount icon + name + **🏠 Dismount** button (calls `setActiveMount('', null)`)
- Hint text: "Click any owned mount to ride it. Click again to dismount."

Note: `setActiveMount('', null)` works because the function checks `isActive = slotEl && slotEl.classList.contains('slot-active-mount')` → `null && ...` = false → `newKey = '' ` (dismount path).

---

## Current State (end of 2026-05-30 session 7)

- **Build:** 0 errors
- **Migration applied:** `AddUserBackground`
- **Equipment tab:** now shows all 7 slots (weapon/armor/head/shield/back/eyewear/headAccessory)
- **Background customization:** fully implemented — Customize tab + live preview + AJAX save + avatar rendering on all 3 pages
- **Gear `.gif` fallback:** onerror chain handles PNG→GIF→hide on all new avatar layers
- **Stable UX:** active pet/mount banners with explicit action buttons; feed discoverability hint added
- **Next:** Phase 9 — Challenges (see ROADMAP.md)

---

## Architecture Notes Added This Session

- **`overflow:visible` on avatar-preview divs:** MUST stay `overflow:visible` (not `hidden`). Mount head images overflow the 90×90 inner container bounds. `background-image` CSS does not need `overflow:hidden` — it clips to element padding-box by default.
- **Gear image `.gif` fallback pattern:** `onerror="if(!this.src.endsWith('.gif')){this.src=this.src.replace('.png','.gif');}else{this.onerror=null;this.style.display='none';}"` — use on all avatar gear layer imgs. Only current `.gif` gear file: `back_special_heroicAureole.gif`.
- **Background key format:** filesystem key stripped from `background_{key}.png`. Path: `/images/habitica/backgrounds/background_{key}.png`. No DB/seed needed — enumerated from `wwwroot/images/habitica/backgrounds/` at request time.
- **`SetBackground` endpoint:** `POST /Character/SetBackground` — form-urlencoded `key` param. Accepts empty string to clear. In `CharacterController`.
- **back/eyewear/headAccessory slots in Equipment view:** avatar layers use IDs `layer-back`, `layer-eyewear`, `layer-headAccessory` (no `-gear` suffix) because `updateAvatarLayer(slot, ...)` in Equipment JS uses `'#layer-' + slot`. Customize.cshtml uses `-gear` suffix IDs since gear isn't swapped dynamically there.

---

## What Was Done This Session (2026-05-30, session 6)

### Fix — Pet Catalog: Remove Entries With No Local Images

**Problem:** Stable page showed broken/empty slots for pet/mount variants with no local image files:
- `Purple` color in PremiumColors → files named `RoyalPurple`, not `Purple` (0 images for plain `Purple`)
- `PolarBear` in QuestAnimals → no `Pet-PolarBear-*.png` or mount icons anywhere
- `Wolf-Cerberus` special → no `Pet-Wolf-Cerberus.png`
- `Gryphon-Gryphatrice` special → no `Pet-Gryphon-Gryphatrice.png` (mount icon exists but no pet image)
- `Gryphatrice-Jubilant` special → no image anywhere

**Fix:** Removed all five from `PetCatalogService`:
- `"Purple"` removed from `PremiumColors` array
- `"PolarBear"` removed from `QuestAnimals` array
- `Wolf-Cerberus`, `Gryphon-Gryphatrice`, `Gryphatrice-Jubilant` removed from `SpecialPets` list

**Files changed:**
- `Services/Implementations/PetCatalogService.cs`

---

### Fix — Pet Collection Count Mismatch (Evolved Pets)

**Problem:** Pet tab badge showed `1/1245` when user had 2 pets (one still as pet, one evolved to mount). The header used `OwnedPets.Count` which only counts `IsMount=false` records, missing evolved pets. Grid showed evolved pets as "owned" (via `IsMount` badge) but count didn't match.

**Failed attempt:** Changed pet grid `IsOwned = ownedPetSet.Contains(s.PetKey)` only — this made evolved pets show as unowned in grid AND count dropped to 1. Wrong: user wants evolved pets counted in pet total.

**Fix:**
1. Reverted pet grid `IsOwned = ownedPetSet.Contains || ownedMountSet.Contains` (original behavior — evolved pets show as collected with 🐴 badge)
2. Added `CollectedPetsCount = OwnedPets.Count + OwnedMounts.Count` computed property to `StableViewModel`
3. Fixed mount grid: `IsOwned = ownedMountSet.Contains` only (was also using petSet — wrong for mounts tab)
4. Updated view to use `Model.CollectedPetsCount` instead of `Model.OwnedPets.Count` in both the header and tab badge

**Files changed:**
- `Models/ViewModels/StableViewModel.cs` — added `CollectedPetsCount` computed property
- `Services/Implementations/StableService.cs` — split pet/mount grid slot merging; mount grid uses mountSet only for IsOwned
- `Views/Stable/Index.cshtml` — replaced `OwnedPets.Count` with `CollectedPetsCount` in header + tab badge (2 occurrences)

---

### Next — Equipment Slots + Background Customization (PLANNED, NOT STARTED)

Plan saved at `PLAN_EQUIPMENT_BACKGROUND.md`. Summary:

**Feature 1 — Expose back/eyewear/headAccessory in Equipment tab:**
- Root cause: `primarySlots` array in `Views/Equipment/Index.cshtml` only has 4 slots. All backend logic already handles 8 slots.
- Fix: add 3 slots to array + gear variable declarations + 3 avatar image layers (back before skin, eyewear after beard, headAccessory after head) in all 3 avatar views

**Feature 2 — Background customization:**
- Add `Background` string field to `User.cs` + EF migration
- Add `Backgrounds` list to `CustomizeViewModel`
- Controller: enumerate `background_*.png` files from wwwroot + add `POST /Character/SetBackground` AJAX endpoint
- Customize view: add Background tab with icon grid (395 backgrounds); avatar preview uses CSS `background-image` on `#avatar-preview` div; AJAX save on click
- All avatar views: apply background CSS on initial load

---

## What Was Done This Session (2026-05-30, session 5)

### Fix — Party Sidebar Quest Panel Clipped

**Problem:** Active boss quest panel (image + HP bars + Abort button) was clipped at bottom of party left sidebar. Members list consumed all flex space, pushing quest panel off-screen.

**Fix:** Quest panel (+ pending invites) now `position:absolute; bottom:0; left:0; right:0; z-index:2; max-height:65%; overflow-y:auto` — pinned to sidebar bottom, scrollable if tall. `adjustMembersListPadding()` runs on load + resize and sets `paddingBottom` on `#membersList` equal to quest panel's actual `offsetHeight`, so member list content never hides behind the panel.

**Files changed:**
- `Views/Party/Index.cshtml` — left sidebar column gets `position-relative`; pending invites + quest panel merged into single absolute-positioned `#questPanel` div; `#membersList` id added; `adjustMembersListPadding()` JS added

---

## What Was Done This Session (2026-05-30, session 4)

### Fix — Party @mention Link Not Rendering Until Reload

**Problem:** Same bug as guild chat (fixed in Phase 7 session 2): `PartyController.SendMessage` returned raw `msg.Body` instead of rendered HTML, so `@username` stayed as plain text until page reload.

**Fix:** Added `RenderBodyAsync` to `IPartyService` / `PartyService` (identical to `GuildService.RenderBodyAsync`, uses GroupBy OrdinalIgnoreCase dedup pattern). `PartyController.SendMessage` now calls `await _party.RenderBodyAsync(msg!.Body)` and returns `renderedBody`.

**Files changed:**
- `Services/IPartyService.cs` — added `Task<string> RenderBodyAsync(string body)`
- `Services/Implementations/PartyService.cs` — implemented `RenderBodyAsync`
- `Controllers/PartyController.cs` — `SendMessage` returns `renderedBody`

---

### Feature — Skills in Task System

**Problem:** Skills were on a separate `/Character/Spells` page (dropdown UX). Goal: 4 skill buttons visible on the task board. Click skill → activates; click task → fires spell.

**What changed:**

**ViewModel + Controller:**
- `TaskBoardViewModel` — added `User`, `EffectiveStats`, `Skills`, `CanUseSkills` (hidden if no class / level < 11)
- `TaskController.Index` — loads user with gear, gets effective stats, gets class spells, populates new fields
- `TaskController.CastSpell` (new `POST /Task/CastSpell`) — delegates to `SpellService.CastAsync`, returns same JSON shape as CharacterController

**SpellService — formula sync + party spells:**
- Added `IBossQuestService` injection (for boss finish on spell kill)
- Fixed `CalculateBonus`: stat term only scaled by crit (matches Habitica exactly: `(val) + stat * 0.5 * critMult`)
- Fixed `smash` crit: now uses CON (was PER) — matches Habitica `crit('con')`
- Fixed `smash` boss damage: `DR(STR * critMult, 55, 70)` now applied to party quest via `ApplySpellBossDamageAsync` (was wrongly awarded as XP)
- Fixed `fireball` formula: `DR(INT * critMult, 75)` XP (was using taskMult hack) + `INT * 0.1` boss damage
- Fixed `defensiveStance`: `DR(CON - BuffCON, 40, 200)` to avoid stacking (was using full es.CON)
- Implemented 7 party spells: `valorousPresence`, `intimidate`, `mpheal`, `earth`, `toolsOfTrade`, `protectAura`, `healAll`
  - Loads all party members, applies buff/heal to each, posts `[SYS]` chat message
  - Solo (no party): applies to self only
  - `mpheal` skips mages; uses simple `target.INT * 2 + 30` for max mana
- Added `ApplySpellBossDamageAsync` — applies spell boss damage to active party quest, adds system chat message, calls `FinishQuestAsync` if HP ≤ 0
- Made `BossQuestService.FinishQuestAsync` public and added to `IBossQuestService`

**UI — Skills bar in task page:**
- Horizontal skills bar between page header and board columns (hidden if `!CanUseSkills`)
- 4 skill cards: image, name, mana cost. Disabled when mana insufficient.
- Active state: blue border + glow
- Task-targeting spells: after clicking skill, body gets `.skill-targeting` class → task cards show crosshair + dashed outline; `.task-card *` gets `pointer-events: none` so click bubbles to card
- Self/party spells: cast immediately on skill click
- `castSkill()` async fetch → updates mana bar + skill disabled states inline + shows toast + calls `refreshHud()`
- ESC or click active skill again = deactivate
- Removed `✨ Cast Spells` button from `Views/Character/Index.cshtml` (Spells page still accessible at `/Character/Spells`)

**Files changed:**
- `Services/IPartyService.cs`
- `Services/Implementations/PartyService.cs`
- `Controllers/PartyController.cs`
- `Services/IBossQuestService.cs` — `FinishQuestAsync` added to interface
- `Services/Implementations/BossQuestService.cs` — `FinishQuestAsync` made public
- `Services/Implementations/SpellService.cs` — full rewrite (formulas + party spells + boss damage)
- `Models/ViewModels/TaskBoardViewModel.cs` — added User/EffectiveStats/Skills/CanUseSkills
- `Controllers/TaskController.cs` — injected ISpellService + ICharacterService + AppDbContext; extended Index; added CastSpell endpoint
- `Views/Task/Index.cshtml` — skills bar HTML + CSS + JS
- `Views/Character/Index.cshtml` — Cast Spells button removed

---

## Goal We're Working Toward

Build a Habitica-inspired gamified habit tracker. Full roadmap in `ROADMAP.md`.
Long-term arc: tasks → game economy (HP/mana/gold) → character classes → inventory/shop → pets/mounts → social → guilds/parties → boss quests → challenges.

---

## What Was Done This Session (2026-05-30, session 3)

### UX — Boss Quest Avatar in Party Panel

**Problem:** Quest panel left sidebar showed blank space above HP bars during active quest.

**Fix:** Active quest renders boss image (`/images/habitica/quests/bosses/quest_{key}.png`, max 110px tall) above the HP/rage bars. Collection quests show scroll image instead. Pending quest state shows scroll image (80px) above member RSVP list. Both states use `onerror` fallback.

**Files changed:**
- `Views/Party/Index.cshtml` — boss/scroll `<img>` added to Active and Pending quest panel blocks

---

### Fix — Gem Cost: 25 GP → 100 GP + Live Gem Display

**Problem:** 25 GP per gem was too cheap. After buying, GP/gem counts in Gem Shop tab didn't update until page reload.

**Fix:**
- `Constants/AppConstants.cs` — `GEM_GOLD_COST`: 25 → 100
- `Views/Market/Index.cshtml` — updated "Costs 25 GP" text and button; added `id="gem-shop-gold"`, `id="gem-shop-gems"`, `id="buyGemBtn"` for live update
- `buyGem()` JS now updates those elements inline from `d.newGold`/`d.newGems` without reload

**Files changed:**
- `Constants/AppConstants.cs`
- `Views/Market/Index.cshtml`

---

### UX — Remove Economy Navbar + Market Inn Tab

**Problem:** Economy page had Health Potion and Rest Mode, but Economy navbar link was cluttering nav (13 items).

**Fix:**
- Removed `💰 Economy` nav link from `Views/Shared/_Layout.cshtml`
- Added `🏨 Inn` tab to Market with Health Potion card + Rest Mode toggle card
- Inn tab reads `Model.User.HP/Gold/IsSleeping` (already in `MarketViewModel.User`); posts to existing `/Economy/BuyPotion` and `/Economy/ToggleSleep` endpoints — no controller changes

**Files changed:**
- `Views/Shared/_Layout.cshtml` — Economy nav item removed
- `Views/Market/Index.cshtml` — Inn tab button + pane + JS handlers added

---

### Fix — Party Chat CSRF + Boss Damage System Messages

**CSRF bug:** All party chat AJAX (SendMessage, Leave, Kick, Invite) was silently failing 400. Root cause: `@Html.AntiForgeryToken()` was never rendered in the in-party DOM branch → `_token` was always `''`. Same pattern as Phase 7 guild/message bug fixes.

**Fix:** Added `<form id="csrf-form" style="display:none">@Html.AntiForgeryToken()</form>` at top of in-party section.

**Boss damage system messages:** When a player completes a task during an active boss quest, `BossQuestService.ApplyTaskDamageAsync` now inserts a `PartyMessage` with body prefix `[SYS]` (e.g. `[SYS]⚔️ tuan2409 dealt 12.3 damage to A Jaded Jinx! (HP: 387/400)`). These render in party chat as centered gray badge rows — no avatar, no like/delete buttons.

**Implementation details:**
- `PartyMessageEntry.IsSystem` — DTO-only bool (no DB column, no migration needed)
- `PartyService.GetMessagesAsync` — strips `[SYS]` prefix from `RenderedBody`, sets `IsSystem=true`
- `PartyController.Messages` — includes `isSystem` in JSON for `loadMore()`
- View detects `entry.IsSystem` (Razor) and `d.isSystem` (JS) to render differently

**Files changed:**
- `Views/Party/Index.cshtml` — CSRF form + system message rendering (Razor + JS `loadMore`)
- `Models/ViewModels/PartyViewModel.cs` — `bool IsSystem` added to `PartyMessageEntry`
- `Services/Implementations/PartyService.cs` — `GetMessagesAsync` strips `[SYS]` prefix, sets flag
- `Services/Implementations/BossQuestService.cs` — `ApplyTaskDamageAsync` inserts system message
- `Controllers/PartyController.cs` — `Messages` endpoint includes `isSystem` in JSON

---

## What Was Done This Session (2026-05-30, session 2)

### Fix — Quest Start Flow

**Problem:** Users could buy scrolls but had no way to start a quest (no UI hint, solo party stuck in Pending, non-leaders saw broken button).

**Fixes:**

1. **Solo party auto-start** — `BossQuestService.InvitePartyAsync` now calls `TryAutoActivateAsync` after creating member rows. Solo party (1 member = leader, already "accepted") auto-activates immediately without needing Force Start.

2. **Leader-only quest panel** — `Views/Party/Index.cshtml` quest panel now only renders "Start a Quest" section for the party leader. Non-leaders see "Only the party leader can start a quest." (previously ALL members with scrolls saw the button, which failed silently).

3. **Quest Shop UX hints** — Added info banner "After buying a scroll, go to your Party page to start the quest" at top of `/QuestShop`. Purchase success toast now also shows Party page link.

**Files changed:**
- `Services/Implementations/BossQuestService.cs` — `TryAutoActivateAsync` call added at end of `InvitePartyAsync`
- `Views/Party/Index.cshtml` — quest panel gated behind `isLeader`
- `Views/QuestShop/Index.cshtml` — info banner + toast with Party link

---

### Fix — Quest Scroll Currency: Gold → Gems

**Problem:** Scrolls cost gold (always 4 GP, trivially cheap). Needed gem-based pricing with difficulty tiers; drop description visible before buying.

**Changes:**

**`BossQuest` model** (`Models/BossQuest.cs`):
- Added `GemCost int` — gem cost to purchase (0 = not purchasable, i.e. time travel quests)
- `GoldCost` repurposed: now means additional gold bonus required alongside gems (only masterclasser quests, 200–700 GP)
- Added `DropDescription` computed property — parses `DropItemsJson` to human-readable string (e.g. `×3 Wolf Egg · 73 GP · 725 XP`)

**Gem pricing tiers** (auto-computed from HP + rage in seed helpers):
| Boss HP | No Rage | With Rage |
|---------|---------|-----------|
| ≤ 500   | 4 💎    | 5 💎      |
| 501–1000| 6 💎    | 7 💎      |
| 1001–2000| 8 💎  | 9 💎      |
| > 2000  | 10 💎   | 11 💎     |
| Time Travel | 0 (not for sale) | — |
| Masterclasser | above tiers + 200–700 GP |  |
| Collection quest | 4 💎 | — |

**`BossQuestSeed.cs`** (`Data/Seeds/BossQuestSeed.cs`):
- Q() helper: ignores old `goldCost` param (was always 4, meaningless); auto-computes `GemCost` via `G(hp, rage)`; sets `GoldCost = 0`
- QB() helper: uses `goldBonus` param for `GoldCost`; auto-computes `GemCost`
- QC() helper: `GemCost = 4` (0 for timeTravelers); `GoldCost = goldCost` only if category == "gold"
- No individual quest call lines changed — only helper bodies

**`BossQuestService.BuyScrollAsync`**: checks `GemCost > 0` (not GoldCost); deducts `user.Gems`; also deducts `user.Gold` if `GoldCost > 0`

**`QuestShopViewModel`**: added `UserGems`

**`QuestShopController`**: passes `UserGems`; buy response includes `newGems`

**`Views/QuestShop/Index.cshtml`**:
- Header shows 💎 gems badge + GP badge
- Each quest card shows drop description (`🎁 ×3 Wolf Egg · 73 GP · 725 XP`)
- Buy button shows `💎 N` (or `💎 N + M GP` for masterclassers)
- `canBuy` logic checks gems (and gold if masterclasser)
- JS updates gem badge after purchase inline

**Migration:** `AddBossQuestGemCost` — adds `GemCost INT` column; EF auto-generated 115 `UpdateData` calls with correct per-quest values

**Files changed:**
- `Models/BossQuest.cs`
- `Data/Seeds/BossQuestSeed.cs`
- `Migrations/20260529230258_AddBossQuestGemCost.cs`
- `Services/Implementations/BossQuestService.cs`
- `Models/ViewModels/QuestShopViewModel.cs`
- `Controllers/QuestShopController.cs`
- `Views/QuestShop/Index.cshtml`

---

## What Was Done This Session (2026-05-30, session 1)

### Phase 8 — Boss Quests

**New models:** `BossQuest`, `PartyQuest`, `PartyQuestMember`

**Seed:** 115 Habitica non-world-boss quests across 7 categories (60 pet, 6 potion boss, 6 potion collection, 7 seasonal, 15 series, 16 masterclasser, 3 time travel, 2 generic). See `PHASE8_PLAN.md`.

**Migration:** `AddPhase8BossQuests` — creates BossQuests, PartyQuests, PartyQuestMembers tables + 115 GameItem scroll seeds (IDs 182-296).

**Service:** `IBossQuestService` / `BossQuestService` — shop buy, full quest lifecycle (invite/accept/reject/force-start/cancel/abort), boss damage + collection item drop, rage mechanics, quest completion with item/gold/XP rewards.

**Integration:** `TaskService.ScoreTaskAsync` applies boss damage on task up; `TaskService.RunCronAsync` applies rage on missed dailies.

**Shop:** `GET /QuestShop` — tabbed grid (6 categories); `POST /QuestShop/Buy` — JSON purchase.

**Party:** `GET/POST /PartyQuest/*` — full lifecycle API; Party page left sidebar shows 3-state quest panel (none/pending/active).

**Key files:**
- `Models/BossQuest.cs`, `Models/PartyQuest.cs`, `Models/PartyQuestMember.cs`
- `Data/Seeds/BossQuestSeed.cs`
- `Services/IBossQuestService.cs`, `Services/Implementations/BossQuestService.cs`
- `Controllers/QuestShopController.cs`, `Controllers/PartyQuestController.cs`
- `Views/QuestShop/Index.cshtml`, `Views/Party/Index.cshtml`
- `Models/ViewModels/QuestShopViewModel.cs` (includes `PartyQuestStatusDto`)
- `Models/ViewModels/PartyViewModel.cs` (added `QuestStatus`, `OwnedScrolls`)

---

### Fix — Guild/Party Invite: Username → User Search

**Problem:** `InviteAsync` searched by exact `Username` match — two users with same username → second user unreachable.

**Solution:** Changed invite flow to search-then-select by unique userId.

**Files changed:**
- `Services/IGuildService.cs` — `InviteAsync(int inviterId, int guildId, string username)` → `InviteAsync(int inviterId, int guildId, int targetUserId)`
- `Services/IPartyService.cs` — `InviteAsync(int inviterId, string username)` → `InviteAsync(int inviterId, int targetUserId)`
- `Services/Implementations/GuildService.cs` — find target by `u.Id == targetUserId` (was `u.Username == username`)
- `Services/Implementations/PartyService.cs` — same change
- `Controllers/GuildController.cs` — `Invite` action now takes `int targetUserId`; added `GET /Guild/SearchInvitable?guildId=X&q=...` — partial match on username/email, excludes existing members, returns top 8 `{Id, Username, Avatar, Level}`
- `Controllers/PartyController.cs` — `Invite` action now takes `int targetUserId`; added `GET /Party/SearchInvitable?q=...` — same pattern, excludes all current party members
- `Views/Guild/View.cshtml` — invite modal: replaced plain text input with search-box → debounced results list → click to select → shows avatar+name chip → Send Invite posts `targetUserId`
- `Views/Party/Index.cshtml` — invite panel: same search-then-select pattern inline (no modal)

**Architecture note:** Search endpoints return empty list for unauthenticated (no error). Results exclude existing members so invite list is always valid. Badge shows avatar + level to disambiguate users with same display name.

---

### UX — Navbar Social Dropdown

**Problem:** 13 nav items; too wide on any screen.

**Solution:** Grouped 5 social nav links under single `👥 Social` dropdown.

**Items moved into dropdown:** Leaderboard, Friends, Messages, Guilds, Party.

**Files changed:**
- `Views/Shared/_Layout.cshtml`:
  - Added `isSocialActive` bool (true when controller is Friend/Message/Guild/Party or Account+Leaderboard action)
  - Replaced 5 `<li class="nav-item">` links with one `<li class="nav-item dropdown">` — toggle highlights when on any social page
  - Each dropdown item gets per-controller `active` class
  - Divider separates Leaderboard/Friends/Messages from Guilds/Party
  - `msg-badge` now appears on toggle (visible when dropdown closed) and `msg-badge-dd` inside Messages item (visible when open); both synced in `refreshHud()`

---

## What Was Done This Session (2026-05-29, Phase 7)

### Phase 7.1 — Guilds

**New model files:**
- `Models/Guild.cs` — `Id`, `Name (max 100, unique)`, `Description (max 500)`, `Summary (max 200)`, `Logo?`, `Privacy ("public"/"private")`, `LeaderId (FK→User Restrict)`, `CreatedAt`, `UpdatedAt?`
- `Models/GuildMember.cs` — `Id`, `GuildId (FK→Guild Cascade)`, `UserId (FK→User Restrict)`, `Role ("Leader"/"Manager"/"Member")`, `JoinedAt`. Unique index `(GuildId, UserId)`
- `Models/GuildMessage.cs` — `Id`, `GuildId (FK→Guild Cascade)`, `AuthorId (FK→User Restrict)`, `Body (max 2000)`, `SentAt`, `IsDeleted`. Index `(GuildId, SentAt)`
- `Models/GuildMessageLike.cs` — composite PK `(GuildMessageId, LikerUserId)`. `GuildMessageId` → Cascade; `LikerUserId` → Restrict
- `Models/GuildInvite.cs` — `Id`, `GuildId (FK→Guild Cascade)`, `InviterId`, `InviteeId` (both Restrict), `Status ("Pending"/"Accepted"/"Declined")`, `CreatedAt`

**New service:**
- `Services/IGuildService.cs` + `Services/Implementations/GuildService.cs` — full CRUD, chat, invites, kick/promote/demote, @mention processing

**New controller:** `Controllers/GuildController.cs` — 16 endpoints

**New views:**
- `Views/Guild/Index.cshtml` — Discover tab (public guilds grid + search + Join button) + My Guilds tab + pending invites strip
- `Views/Guild/View.cshtml` — two-column: left = member list with role badges + kick/promote/demote dropdowns; right = chat thread with load-more, like, delete, compose
- `Views/Guild/Create.cshtml` — name, summary, description, privacy radio

**Key GuildService patterns:**
- `CreateAsync`: name uniqueness check → insert Guild → insert GuildMember(Leader)
- `LeaveAsync`: leader-leave transfers to oldest manager → oldest member → disbands if last
- `InviteAsync`: Leader or Manager only → pending-exists check → CreateNotificationAsync("Guild")
- `SendMessageAsync`: member-only guard → insert GuildMessage → `ProcessMentionsAsync`
- `GetMessagesAsync`: PAGE_SIZE=20 oldest-first; batch-resolves @mention usernames for link rendering
- `RenderMentions`: replaces `@username` → `<a href="/Friend/ViewProfile/{id}">` using pre-built dict
- Kick: Leader/Manager can kick Members; Leader only can kick Managers
- Promote/Demote: Leader only

---

### Phase 7.2 — Parties

**New model files:**
- `Models/Party.cs` — `Id`, `Name (max 100)`, `LeaderId (FK→User Restrict)`, `CreatedAt`
- `Models/PartyMember.cs` — `Id`, `PartyId (FK→Party Cascade)`, `UserId (FK→User Restrict)`, `Role ("Leader"/"Member")`, `JoinedAt`. Unique index `(PartyId, UserId)` AND unique index on `UserId` (1 party per user enforced at DB level)
- `Models/PartyMessage.cs` — same structure as GuildMessage but PartyId
- `Models/PartyMessageLike.cs` — composite PK `(PartyMessageId, LikerUserId)`
- `Models/PartyInvite.cs` — same structure as GuildInvite but PartyId

**New service:**
- `Services/IPartyService.cs` + `Services/Implementations/PartyService.cs` — create, invite, accept/decline, leave (with leadership transfer), kick, chat, @mention

**New controller:** `Controllers/PartyController.cs` — 11 endpoints

**New view:**
- `Views/Party/Index.cshtml` — dual state: (A) no party → create form + pending invites; (B) in party → two-column: left = member list with stats + kick button for leader + invite input; right = party chat with full AJAX

**Key PartyService patterns:**
- `CreateAsync`: checks user not already in party → insert Party + PartyMember(Leader)
- `AcceptInviteAsync`: checks user not in another party before joining (auto-declines if already in one)
- `LeaveAsync`: leader-leave transfers to oldest member → disbands if last
- 1-party-per-user enforced at service level AND DB unique index on `PartyMember.UserId`

---

### Phase 7.3 — @mention

Both GuildService and PartyService implement:
- `ProcessMentionsAsync(body, groupName, link, authorId)` — regex `@(\w+)` scan post-save; skips author; `CreateNotificationAsync` for each mentioned user
- `RenderMentions(body, userLookup)` — batch-resolves usernames to IDs during `GetMessagesAsync`; unknown usernames left as plain `@username` text
- Views render `@@Html.Raw(entry.RenderedBody)` (XSS-safe: only links injected, no user-controlled HTML)

---

### DB + Migration

**Modified `Data/AppDbContext.cs`:**
- Added 10 new DbSets (Guild, GuildMember, GuildMessage, GuildMessageLike, GuildInvite, Party, PartyMember, PartyMessage, PartyMessageLike, PartyInvite)
- Phase 7 OnModelCreating block: Restrict on all user FKs, Cascade on group membership FKs, unique indexes, composite PKs for likes, chat pagination indexes

**Modified `Program.cs`:**
- Added `builder.Services.AddScoped<IGuildService, GuildService>()`
- Added `builder.Services.AddScoped<IPartyService, PartyService>()`

**Modified `Views/Shared/_Layout.cshtml`:**
- Added `⚔️ Guilds` → `/Guild` nav link
- Added `🛡️ Party` → `/Party` nav link
- Economy icon updated from 🛡️ to 💰 (avoid icon collision with Party)

**Migration applied:** `20260529061815_AddPhase7GuildsParties` — creates 10 tables

---

### Files created/modified this session (Phase 7)

| File | What changed |
|------|-------------|
| `Models/Guild.cs` | NEW |
| `Models/GuildMember.cs` | NEW |
| `Models/GuildMessage.cs` | NEW |
| `Models/GuildMessageLike.cs` | NEW |
| `Models/GuildInvite.cs` | NEW |
| `Models/Party.cs` | NEW |
| `Models/PartyMember.cs` | NEW |
| `Models/PartyMessage.cs` | NEW |
| `Models/PartyMessageLike.cs` | NEW |
| `Models/PartyInvite.cs` | NEW |
| `Models/ViewModels/GuildIndexViewModel.cs` | NEW — GuildIndexViewModel + GuildCardModel |
| `Models/ViewModels/GuildViewModel.cs` | NEW — GuildViewModel + GuildMemberEntry + GuildMessageEntry |
| `Models/ViewModels/PartyViewModel.cs` | NEW — PartyViewModel + PartyMemberEntry + PartyMessageEntry |
| `Services/IGuildService.cs` | NEW |
| `Services/Implementations/GuildService.cs` | NEW |
| `Services/IPartyService.cs` | NEW |
| `Services/Implementations/PartyService.cs` | NEW |
| `Controllers/GuildController.cs` | NEW — 16 endpoints |
| `Controllers/PartyController.cs` | NEW — 11 endpoints |
| `Views/Guild/Index.cshtml` | NEW |
| `Views/Guild/View.cshtml` | NEW |
| `Views/Guild/Create.cshtml` | NEW |
| `Views/Party/Index.cshtml` | NEW |
| `Views/Shared/_Layout.cshtml` | Added Guild + Party nav links |
| `Data/AppDbContext.cs` | 10 new DbSets + Phase 7 relationship block |
| `Program.cs` | Registered IGuildService + IPartyService |
| `PHASE7_PLAN.md` | NEW — implementation plan |
| `HANDOFF.md` | This file |

---

## What Was Done Previous Sessions

### 2026-05-29 — Phase 7 Bug Fixes (session 2)

#### Bug Fix 1 — Notifications "Failed to load"
**Files:** `Controllers/NotificationController.cs`
**Cause (A):** Controller had `[Authorize]` attribute, which requires cookie/JWT auth middleware. App uses session-only auth → ASP.NET returns 401/redirect → JS `fetch` receives HTML, not JSON → `r.json()` throws → catch block shows "Failed to load."
**Cause (B):** Controller read userId with `HttpContext.Session.GetString("UserId")` but session stores it as int via `SetInt32`. `GetString` returns null → `int.Parse(null ?? "0")` = 0 → all queries return empty for userId=0.
**Fix:** Removed `[Authorize]`. Added `GetUserId()` helper using `GetInt32`. `unread-count` returns `{count:0}` for unauthenticated (silent) instead of 401.

#### Bug Fix 2 — Can't join public guild (Index page)
**File:** `Views/Guild/Index.cshtml`
**Cause:** `const _token = document.querySelector('[name=__RequestVerificationToken]')?.value ?? ''` only finds a token when pending invites exist (only those forms render `@Html.AntiForgeryToken()`). No pending invites → empty token → `[ValidateAntiForgeryToken]` returns 400 (HTML) → `res.json()` throws → `joinGuild` silently fails with no error shown.
**Fix:** Moved `@Html.AntiForgeryToken()` from a dead Razor variable (`var token = Html.AntiForgeryToken()` was never rendered into HTML) to the page body, so the hidden input is always in DOM.

#### Bug Fix 3 — Can't invite to private guild / join from View page
**File:** `Views/Guild/View.cshtml`
**Cause:** JS fallback `const _token = ... ?? '@Html.AntiForgeryToken()'` — Razor expands `@Html.AntiForgeryToken()` to the full `<input name="__RequestVerificationToken" type="hidden" value="TOKEN" />` HTML string. The full markup was used as the token value → CSRF validation rejects it.
**Fix:** Added `@Html.AntiForgeryToken()` to the View page body so the hidden input is always in DOM. Simplified JS to `document.querySelector('[name=__RequestVerificationToken]')?.value ?? ''`.

#### Bug Fix 4 — Toast text invisible (white text on white background)
**File:** `Views/Shared/_Layout.cshtml`
**Cause:** Guild/Party views call `showToast(..., 'success'/'danger'/'info'/'warning')` but the custom toast CSS only defines backgrounds for `toast-xp`, `toast-badge`, `toast-streak`, etc. No background for `success`/`danger`/`info`/`warning` → browser background stays white, `.toast-noti { color: white }` → invisible.
**Fix:** Added CSS classes `.success`, `.danger`, `.info`, `.warning` with colored gradient backgrounds. `success`/`warning` use `color: #1a1a1a` (dark text; light background). `danger`/`info` keep `color: white` (inherited from `.toast-noti`).

#### Bug Fix 5 — Guild View crash: duplicate key in mention lookup
**File:** `Services/Implementations/GuildService.cs` — `GetMessagesAsync`, line ~360
**Cause:** `ToDictionaryAsync(u => u.Username, u => u.Id)` throws `ArgumentException: An item with the same key has already been added` when DB collation is case-insensitive and `mentionedNames.Contains("tuantran")` matches multiple rows with different casings (e.g., `tuantran` and `TuanTran`).
**Fix:** Changed to `ToListAsync()` + `GroupBy(u.Username, OrdinalIgnoreCase).ToDictionary(g => g.Key, g => g.First().Id, OrdinalIgnoreCase)`. Same pattern applied to `RenderBodyAsync` added in Bug Fix 6.

#### Bug Fix 6 — @mention not rendered immediately after sending
**Files:** `Services/IGuildService.cs`, `Services/Implementations/GuildService.cs`, `Controllers/GuildController.cs`
**Cause:** `SendMessage` AJAX endpoint returned `body = msg.Body` (raw text). The JS `appendMessage` function used `d.body` directly — so `@username` stayed as plain text, not a clickable link. Only page-load messages went through `RenderMentions`.
**Fix:** Added `Task<string> RenderBodyAsync(string body)` to `IGuildService` and implemented it in `GuildService` (resolves mentions for a single body; reuses existing `RenderMentions` private static). Controller now calls `await _guilds.RenderBodyAsync(msg.Body)` and returns `renderedBody` in the JSON response. JS `appendMessage` already uses `d.body` — which is now the rendered HTML.

#### Bug Fix 7 — @mention link invisible (no .mention CSS)
**File:** `wwwroot/css/site.css`
**Cause:** `RenderMentions` injects `<a class="mention fw-semibold">` links but `.mention` had no CSS definition. Inside own messages (`bg-primary text-white`), Bootstrap's default anchor color (blue `#0d6efd`) blends into the blue background. On other messages, anchor color may still be hard to read.
**Fix:** Added `.mention { color: #4a90d9; font-weight: 600; text-decoration: none; }` and `.bg-primary .mention, .text-white .mention { color: #bde0ff; }` (light blue on blue background).

---

### Architecture Notes Added This Session
- **`NotificationController`:** NO `[Authorize]`. Uses `Session.GetInt32(SESSION_USER_ID)` same as all other controllers. Returns `{count:0}` for unauthenticated on `unread-count` (silent, no error in navbar).
- **CSRF token pattern:** `@Html.AntiForgeryToken()` MUST be rendered into the page body as HTML (not assigned to a Razor variable, not used as a JS string fallback). JS reads it with `document.querySelector('[name=__RequestVerificationToken]')?.value`.
- **Toast type names:** custom system uses `toast-xp`, `toast-badge`, `toast-streak`, `toast-level`, `toast-gold`, `toast-hp`, `toast-crit`, `toast-drop`, `toast-death`. Bootstrap names `success`/`danger`/`info`/`warning` now also work (CSS added this session) but are second-class.
- **`GuildService.RenderBodyAsync(body)`:** public method for rendering @mentions in a single message body. Use after `SendMessageAsync` to get the HTML to return to the client. Internally calls `RenderMentions(body, lookup)`.
- **`mentionLookup` dedup pattern:** always use `ToListAsync()` + `GroupBy(..., OrdinalIgnoreCase).ToDictionary(...)` — never `ToDictionaryAsync` directly on a username-keyed query. Case-insensitive DB collation can return duplicate rows.

---

### 2026-05-29 (Phase 6 + Bug Fixes)

### Phase 6 Bug Fixes (session 2)

#### Bug Fix 1 — New Message modal fails silently on empty inbox
**File:** `Views/Message/Index.cshtml`
**Cause:** `sendNewMessage()` grabs antiforgery token via `document.querySelector('[name=__RequestVerificationToken]')`, but the token is only rendered inside `#sendForm` and `#reportModal`, which only exist when an active conversation is open. On empty inbox → no token → POST rejected with 400 (HTML response) → `res.json()` throws → silent crash, message never sent, no conversation appears.
**Fix:** Added `@Html.AntiForgeryToken()` inside `#newMsgModal` body so token always exists on page regardless of conversation state.

#### Bug Fix 2 — All avatars broken in message thread and conversation list
**File:** `Views/Message/Index.cshtml`
**Cause:** All avatar `src` and `onerror` fallbacks referenced `/images/default-avatar.png`, which does not exist. Actual default avatar file is `/images/default.png`.
**Fix:** Replaced all occurrences of `/images/default-avatar.png` → `/images/default.png`.

---

## What Was Done This Session (2026-05-29, Phase 6)

### Phase 6.1 — Private Messages

**New files:**
- `Models/Message.cs` — entity: `Id`, `SenderId` (FK Restrict), `ReceiverId` (FK Restrict), `Body (max 2000)`, `SentAt`, `IsRead`, `ReadAt?`, `DeletedBySender`, `DeletedByReceiver`. Navigation: `Sender?`, `Receiver?`, `Likes` collection.
- `Models/MessageLike.cs` — composite PK `(MessageId, LikerUserId)`. `MessageId` → Cascade; `LikerUserId` → Restrict.
- `Models/ViewModels/InboxViewModel.cs` — `ConversationSummary` (OtherUser, LastMessage, UnreadCount, IsBlocked) + `InboxViewModel` (Conversations, TotalUnread, ActiveConversationUserId?, ActiveConversation?).
- `Models/ViewModels/ConversationViewModel.cs` — `MessageEntry` (Message, LikedByMe, Count) + `ConversationViewModel` (OtherUser, Messages, IsBlocked, IsBlockedBy, Page, HasMore).
- `Services/IMessageService.cs` + `Services/Implementations/MessageService.cs` — full implementation (see interface below).
- `Controllers/MessageController.cs` — 8 endpoints (see below).
- `Views/Message/Index.cshtml` — two-column inbox/thread UI with full JS.

**`IMessageService` interface:**
```csharp
Task<(bool Success, string? Error, Message? Msg)> SendAsync(int senderId, int receiverId, string body);
Task<List<ConversationSummary>> GetConversationsAsync(int userId);
Task<List<MessageEntry>> GetConversationAsync(int userId, int otherId, int page = 0);
Task MarkReadAsync(int userId, int otherId);
Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId);
Task<int> GetUnreadCountAsync(int userId);
Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId);
Task<bool> IsBlockedAsync(int userA, int userB);    // EITHER direction
Task<bool> IsBlockerAsync(int blockerId, int blockedId); // specific direction
Task<bool> ToggleBlockAsync(int blockerId, int blockedId);
Task<(bool Success, string? Error)> ReportUserAsync(int reporterId, int reportedUserId, string reason);
Task<(bool Success, string? Error)> ReportMessageAsync(int reporterId, int messageId, string reason);
```

**MessageController endpoints:**
| Method | Route | Returns |
|--------|-------|---------|
| GET | `/Message?otherId=X&page=N` | View(InboxViewModel) — unified inbox; calls MarkReadAsync if otherId set |
| GET | `/Message/Conversation/{id}` | Redirect → Index?otherId={id} |
| GET | `/Message/UnreadCount` | JSON `{count}` |
| POST | `/Message/Send` | JSON `{success, messageId, body, sentAt}` |
| POST | `/Message/Delete/{id}` | JSON `{success, error}` |
| POST | `/Message/Like/{id}` | JSON `{success, liked, count}` |
| POST | `/Message/Block/{id}` | JSON `{success, blocked}` |
| POST | `/Message/Report` | JSON `{success, error}` |

**MessageService patterns:**
- `SendAsync`: block check → create Message → `NotificationService.CreateNotificationAsync(receiverId, "New message from X", preview, "Social", "/Message/Conversation/{senderId}", "✉️")`
- Conversation list: two separate EF queries (sent + received), merged in-memory dict by partnerId. GroupBy only for unread counts on receive side.
- Soft delete: `DeletedBySender`/`DeletedByReceiver` flags. Hard-deletes row when BOTH are true.
- `ToggleLikeAsync`: insert or delete `MessageLike` row; returns (wasInserted, newCount).
- `ToggleBlockAsync`: insert or delete `UserBlock` row; returns new blocked state.
- Report duplicate guard: 24h window checked before insert.

**`Views/Message/Index.cshtml`:**
- Left col (col-md-4): conversation list — avatar, username, last message preview, timestamp, unread dot badge.
- Right col (col-md-8): thread header with Block/Unblock + Report + ViewProfile buttons; message bubbles (own = right/blue, other = left/grey); ❤️ like + delete + flag per message; compose textarea at bottom.
- Blocked states: `IsBlockedBy` → "cannot send" notice; `IsBlocked` → "unblock to send" notice.
- New Message modal: username input → `fetch('/Friend/FindByUsername')` lookup → compose + send.
- All mutations via AJAX (no reload). `appendMessage()` inserts new bubble inline after send.

---

### Phase 6.2 — Safety: Block + Report

**New files:**
- `Models/UserBlock.cs` — `Id`, `BlockerId` (FK Restrict), `BlockedId` (FK Restrict), `CreatedAt`. Unique index `(BlockerId, BlockedId)`.
- `Models/Report.cs` — `Id`, `ReporterId` (FK Restrict), `ReportedUserId` (FK Restrict), `ReportedMessageId?` (FK SetNull), `Reason (max 500)`, `CreatedAt`, `IsResolved`, `ResolvedAt?`, `ResolvedByAdminId?`.

**Modified `Controllers/AdminController.cs`:**
- Added `GET /Admin/Reports?showResolved=false` — queries Reports with all includes, ordered `CreatedAt DESC`.
- Added `POST /Admin/ResolveReport/{id}` — sets `IsResolved=true`, `ResolvedAt=UtcNow`, `ResolvedByAdminId` from session.

**New `Views/Admin/Reports.cshtml`:**
- Table: Id, Reporter (link), Reported User (link, red bold), Type badge (Message/User), Reason, Message excerpt (truncated 60 chars), Date, Resolve button.
- Toggle link to switch open ↔ resolved queue.

---

### Phase 6.3 — Profile Enhancements

**Modified `Controllers/Friendcontroller .cs` — `ViewProfile()`:**
- Queries `GearItems` for all 4 equipped slots (`EquippedWeapon/Armor/Head/Shield`) → `ViewBag.EquippedGear`.
- Checks `UserBlocks` in both directions → `ViewBag.IsBlocked` (viewer blocks target), `ViewBag.IsBlockedBy` (target blocks viewer).

**Added `FriendController.FindByUsername(string username)`** — GET, returns `{Id, Username, Avatar}` JSON. Used by compose modal. Excludes admin accounts.

**Rewritten `Views/Friend/ViewProfile.cshtml`:**
- **Profile completeness bar:** bio+20, avatar not default+20, location+15, class selected+15, social link+15, any badge+15. Bootstrap progress bar with % label.
- **Class emoji:** computed from `Model.Class` string (`warrior`/`mage`/`rogue`/`healer`).
- **Pet/mount tiles:** `Pet-{ActivePetKey}.png` + `Mount_Icon_{ActiveMountKey}.png` with `onerror` hidden.
- **Send Message button:** hidden when `IsBlockedBy`.
- **Block/Unblock button:** `id="blockBtn"` → `toggleBlock(userId)` AJAX → updates button state inline.
- **Report button:** opens `#reportUserModal` with reason textarea → `submitReport()` AJAX.
- **Equipped gear card:** 4 thumbnails using `gear.ShopImagePath` (48×48), `onerror` → armoire fallback.

---

### Phase 6.4 — Notification Bell + Message Badge

**Modified `Views/Shared/_Layout.cshtml`:**
- Messages nav link with `<span id="msg-badge">` positioned-absolute badge (d-none when count=0).
- Notification bell `<li class="nav-item dropdown">` BEFORE user dropdown: 🔔 icon, `#notif-badge`, `#notif-dropdown` with `#notif-list`.
- `refreshHud()`: added `GET /Message/UnreadCount` → toggles `#msg-badge`; `GET /api/notification/unread-count` → toggles `#notif-badge`.
- `show.bs.dropdown` on `#notifBell` loads `GET /api/notification?limit=8` into `#notif-list`.
- `markAllRead(e)` calls `PUT /api/notification/read-all`.

---

### DB + Migration

**Modified `Data/AppDbContext.cs`:**
- Added `DbSet<Message>`, `DbSet<MessageLike>`, `DbSet<UserBlock>`, `DbSet<Report>`.
- Phase 6 relationship block: Message → Sender/Receiver Restrict; indexes `(SenderId,ReceiverId)` and `(ReceiverId,IsRead)`; MessageLike composite PK + Cascade; UserBlock unique index + Restrict; Report → SetNull on `ReportedMessageId`.

**Modified `Program.cs`:**
- Added `builder.Services.AddScoped<IMessageService, MessageService>();`

**Migration applied:** `20260529044742_AddPhase6Social` — creates `Messages`, `UserBlocks`, `MessageLikes`, `Reports` tables.

---

### Files actively edited this session (Phase 6)

| File | What changed |
|------|-------------|
| `Models/Message.cs` | NEW — entity |
| `Models/MessageLike.cs` | NEW — entity |
| `Models/UserBlock.cs` | NEW — entity |
| `Models/Report.cs` | NEW — entity |
| `Models/ViewModels/InboxViewModel.cs` | NEW — ConversationSummary + InboxViewModel |
| `Models/ViewModels/ConversationViewModel.cs` | NEW — MessageEntry + ConversationViewModel |
| `Services/IMessageService.cs` | NEW — interface |
| `Services/Implementations/MessageService.cs` | NEW — full implementation |
| `Controllers/MessageController.cs` | NEW — 8 endpoints |
| `Views/Message/Index.cshtml` | NEW — two-column inbox UI; Bug fix: antiforgery token in newMsgModal; Bug fix: default avatar path |
| `Views/Admin/Reports.cshtml` | NEW — admin report queue |
| `Controllers/AdminController.cs` | Added Reports() + ResolveReport() |
| `Controllers/Friendcontroller .cs` | ViewProfile: gear + block queries; added FindByUsername endpoint |
| `Views/Friend/ViewProfile.cshtml` | Profile bar, class icon, pet/mount tiles, block/report buttons, gear card |
| `Views/Shared/_Layout.cshtml` | Message badge, notification bell, unread fetch in refreshHud |
| `Data/AppDbContext.cs` | 4 new DbSets + Phase 6 relationship config |
| `Program.cs` | Registered IMessageService |
| `HANDOFF.md` | This file |

---

## What Was Done Previous Sessions

### 2026-05-29 (session 1) — Bug Fixes + Pet Catalog Sync

#### Bug Fix 1 — Inventory Shows 0-Quantity Items
**File:** `Services/Implementations/EconomyService.cs` — added `&& i.Quantity > 0` filter to `GetInventoryAsync()`.

#### Bug Fix 2 — Item Drops Permanently Blocked After Daily Cap
**File:** `Services/Implementations/EconomyService.cs` — moved day-reset of `DailyDropCount` BEFORE the cap check (was inside the guard → reset never ran next day).

#### Bug Fix 3 — Stable Feed Food Quantity Doesn't Sync
Added `NewFoodQuantity` to `StableResult`; controller returns `newFoodQuantity`+`foodGameItemId`; JS updates qty text + removes option when 0.

#### GameItem Catalog Sync
Expanded seed: 30 → **181 items** (41 food, 71 eggs, 62 potions + 7 wacky potions). Migration `AddWackyPotions` applied.

#### PetCatalogService — Full Collection Grid
- `Services/IPetCatalogService.cs` + `Services/Implementations/PetCatalogService.cs` — ~1,351-entry in-memory catalog. Singleton. Methods: `IsValidHatch`, `CanBecomeMount`, `GetAnimalGroups`.
- Fixed `HatchAsync`: quest egg + premium potion → server blocks.
- Fixed `FeedAsync`: wacky pets cap at 50 pts, no evolution (except Windup).
- `Views/Stable/Index.cshtml` redesigned: full animal-grouped collection grid, offcanvas feed panel, quest-egg potion gating.

#### Bug Fix 4 — Avatar Sprite Sizing
Refactored avatar stack in Character/Index, Equipment/Index, Customize: character=90×90, mount body/head = natural size, inner container `width:90px height:90px`, `padding-top` toggle for mounted/unmounted stance.

---

### 2026-05-28 (session 2)

- Fixed 85 class gear names (placeholder → real Habitica locale names)
- Added 108 special gear items (IDs 552–659), `GearClass="special"`
- Fixed task drop toast: `DroppedItemIcon` now `<img>` HTML
- Fixed armoire gear toast in Market
- Migration `SyncHabiticaGearCatalog` applied

### 2026-05-28 (session 1) — Armoire Sync + Phase 4

- 466 real Habitica armoire items seeded (IDs 86–551)
- Fixed `ShopImagePath` + `GetWornImagePath` for armoire items
- Fixed `_armoire_` path routing in `GearItem.GetWornImagePath()`
- Removed wrong `IsArmoire=true` from 4 tier-0 class starter weapons
- Migration `AddArmoireGearItems` applied
- Phase 4 (Market, Gem Shop, Armoire, Inventory) — COMPLETE ✓
- Migration `AddPhase4InventoryShop` applied

---

## Current State (end of 2026-05-30 session 8)

- **Build:** 0 errors
- **Skills system:** fully implemented in task page — 4 skill buttons, task-targeting UX, party spells live, formulas Habitica-synced
- **DB migrations applied:** `AddPhase4InventoryShop`, `AddArmoireGearItems`, `SyncHabiticaGearCatalog`, `AddPhase5StablePetMount`, `AddWackyPotions`, `AddPhase6Social`, `AddPhase7GuildsParties`, `AddPhase8BossQuests`, `AddBossQuestGemCost`, `AddUserBackground`
- **GearItems in DB:** 659 total
- **GameItems in DB:** 296 (41 food, 71 eggs, 62 potions + 7 wacky + 115 quest scrolls)
- **BossQuests in DB:** 115 — all have `GemCost` set (4–11 💎 based on HP/rage tiers); masterclasser quests also have `GoldCost` (200–700 GP bonus)
- **PetCatalog (in-memory):** ~1,329 entries
- **Gem cost:** 100 GP
- **Equipment tab:** 7 slots (weapon/armor/head/shield/back/eyewear/headAccessory) — was 4
- **Background customization:** COMPLETE ✓ — Customize tab, live preview, AJAX save, renders on Character/Equipment/Customize pages
- **Gear .gif fallback:** onerror chain on all new avatar layers (PNG→GIF→hide)
- **Stable:** 5 tabs — My Pets (owned + food bar feeding), My Mounts (owned), Pet Collection (catalog), Mount Collection (catalog), Hatch
- **Pet/mount mutual exclusivity:** enforced in StableService — equipping one unequips the other
- **Avatar pet offset:** `bottom:-10px;left:-14px` (no longer clips player)
- **Card name text:** `text-white` in Market gear shop + Inventory items/gear cards
- **Footer:** "5 Features" text removed
- **Phase 4:** COMPLETE ✓
- **Phase 5:** COMPLETE ✓
- **Phase 6:** COMPLETE ✓ (UNVERIFIED runtime)
- **Phase 7:** COMPLETE ✓ (UNVERIFIED runtime — test checklist below)
- **Phase 8:** COMPLETE ✓ (UNVERIFIED runtime — test checklist in PHASE8_PLAN.md)
- **Next:** Phase 9 — Challenges

---

## Known Gaps / Deferred

| Gap | Notes |
|-----|-------|
| **Phase 4.4 — Seasonal Shop** | Not implemented. |
| **Equipment back/eyewear/headAccessory UI** | DONE ✓ (session 7) — 3 slots added to Equipment view, avatar layers in all 3 views. |
| **Background customization** | DONE ✓ (session 7) — User.Background field + migration + Customize tab + AJAX save. |
| **Costume mode UI toggle** | Backend supports `mode=costume`, Equipment page always posts `mode=equipped`. UI toggle never built. |
| **Rebirth gem cost** | Currently free. Gem deduction wired in service but controller calls free path. |
| **Phase 5 runtime test** | Collection grid + hatching rules never end-to-end tested. |
| **Phase 5 — Release pet/mount** | No "release" action on Stable page. |
| **Phase 5 — Beast Master / Mount Master achievements** | Deferred to Phase 10. |
| **Phase 5 — Saddle item** | Seeded but `FeedAsync` doesn't handle instant-evolution. |
| **Phase 5 — Avatar partial extraction** | Avatar HTML duplicated in 3 files. Deferred. |
| **Phase 5 — Wacky potion no-image** | IDs 175–181 seeded; no local potion images for Veggie/Dessert/VirtualPet/Fungi/Cryptid/Alien. |
| **Phase 6 — Admin ban/mute action** | `ResolveReport` only marks resolved. No actual user action (ban/silence) taken. |
| **Notification type "Social"** | Added implicitly in `MessageService.SendAsync`. No formal migration needed (stored as string). |
| **Phase 7 — Guild logo upload** | `Logo` field exists on Guild but no upload UI/endpoint built. |
| **Phase 7 — Guild edit** | No edit-guild endpoint. Leader cannot update name/description after creation. |
| **Phase 7 — Guild search pagination** | `GetPublicGuildsAsync` returns all public guilds; no paging for large catalogs. |
| **Phase 7 runtime test** | Guild/party chat, invites, @mention — all unverified. Test checklist below. |
| **Phase 7.4 — Guild/Party Challenges** | Deferred to Phase 9 (challenges are their own phase per ROADMAP). |

### Phase 7 Test Checklist (do this before Phase 8)

1. `/Guild` → Discover tab loads public guilds grid (empty state shows create prompt)
2. Create guild (public) → appears in Discover tab; `⚔️ Guilds` nav shows count
3. Create guild (private) → NOT in Discover tab
4. Join public guild → member list updates; My Guilds tab shows guild
5. Invite to private guild → invite strip appears on invitee's `/Guild` page
6. Accept invite → member appears in guild member list; invite strip disappears
7. Decline invite → invite removed, not a member
8. Send guild chat message → appears in thread right-aligned (own) / left-aligned (other)
9. Like guild message → ❤️ count increments; click again → decrements
10. @mention in guild chat → mentioned user gets notification
11. @username rendered as clickable profile link in chat
12. Promote member to Manager → badge changes to Manager
13. Demote Manager → badge reverts to Member
14. Kick member (as leader/manager) → removed from member list
15. Leave guild (non-leader) → no longer in My Guilds
16. Leave guild (leader, other members exist) → leadership transfers
17. Leave guild (last member) → guild deleted
18. `/Party` → no-party state shows create form + pending invites
19. Create party → party page shows with member list and chat
20. Invite to party by username → invite appears on invitee's `/Party`
21. Accept party invite → member appears; 1-party constraint: second accept blocked if already in party
22. Send party chat → appears in thread
23. @mention in party chat → notification sent
24. Party leader kicks member → removed
25. Leader leaves → next member becomes leader
26. Last member leaves → party deleted; `/Party` shows no-party state

### Phase 6 Test Checklist (do this before Phase 7)

1. `/Message` → inbox loads with conversation list (empty state shows "No conversations yet")
2. New Message modal → type username → Find → compose → Send → thread opens (antiforgery token fix applied — was silently failing on empty inbox)
3. Send PM to User B → User B sees unread dot on conversation; msg-badge count in nav
4. User B opens conversation → unread dot clears + `IsRead=true` in DB
5. Like message → ❤️ count increments; click again → decrements
6. Delete own message → disappears from own view; still visible to other party
7. Block user from ViewProfile → `POST /Message/Block/{id}` → button text toggles
8. Blocked user tries to send PM → error toast "blocked"
9. Report user from ViewProfile → modal → submit → record in `/Admin/Reports`
10. Report message (flag button in thread) → record with `ReportedMessageId` set
11. `/Admin/Reports` → shows open reports; Resolve → moves to resolved queue
12. Notification bell click → dropdown loads recent notifications + marks all read
13. PM triggers notification for receiver → `NotificationService` record in DB
14. Nav msg-badge count updates on `refreshHud()` (after completing task)
15. `/Friend/ViewProfile/{id}` → profile completeness bar renders; class emoji shows; equipped gear card shows thumbnails; pet/mount tiles show if active

### Phase 5 Test Checklist

1. `/Stable` Pets tab → full animal grid; greyed unowned, full-color owned
2. Hatch tab → quest egg → non-drop potions disabled
3. Hatch quest egg + premium potion → server rejects
4. Hatch quest egg + drop potion → slot turns owned
5. Owned pet offcanvas → feed preferred food → progress +5; other → +2
6. Feed to 50 pts → evolution → orange border + 🐴 badge; appears in Mounts grid
7. TeaShop pet to 50 pts → caps, no evolution
8. Make Active pet → `/Character` → pet icon visible
9. Active mount → `/Character` → mount body+head visible; pet hidden

---

## Next Step — Phase 9: Challenges

Phase 8 complete. Run Phase 8 test checklist in `PHASE8_PLAN.md` before starting Phase 9.

### Phase 9 spec (from ROADMAP.md)

Guild/Party Challenges — shared task sets with leaderboards and prize distribution.

---

## Source Files Reference

All Habitica source at: `D:\Download\habitica-develop\habitica-develop\website\common\`

| Content | Path |
|---------|------|
| Gear sets (class) | `script/content/gear/sets/{warrior,wizard,rogue,healer,base}.js` |
| Gear sets (armoire) | `script/content/gear/sets/armoire.js` |
| Gear sets (special) | `script/content/gear/sets/special/index.js` |
| English locale | `locales/en/gear.json` |
| Pets/stable catalog | `script/content/stable.js` + `petInfo.js` |
| Food catalog | `script/content/food.js` |
| Hatch logic | `script/ops/hatch.js` |
| Feed logic | `script/ops/feed.js` |
| Message schema | `server/models/message.js` |
| Block logic | `common/script/ops/blockUser.js` |
| Report flow | `server/libs/chatReporting/` |
| Guild/Party model | `server/models/group.js` |
| Guild/Party ops | `server/controllers/api-v3/groups.js` |
| Guild/Party invite logic | `server/libs/invites/index.js` |

---

## Architecture Notes (all sessions)

- **Session auth:** `HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID)` — no Identity framework
- **Toast system:** `showToast(icon, title, msg, type, duration)` in `_Layout.cshtml` — icon is innerHTML, accepts `<img>` tags
- **HUD refresh:** `refreshHud()` in `_Layout.cshtml`, calls `GET /Economy/GetStats`; also fetches `/Message/UnreadCount` and `/api/notification/unread-count`
- **EconomyService contract (Phase 1–3 methods):** modifies `user` in-memory only. Caller calls `SaveChangesAsync`. No double-save.
- **Phase 4 service methods:** BuyGem, PullArmoire, SellItem all own their own `SaveChangesAsync`
- **StableService methods:** HatchAsync, FeedAsync, SetActivePetAsync, SetActiveMountAsync all own their own `SaveChangesAsync`
- **MessageService methods:** SendAsync, DeleteMessageAsync, MarkReadAsync, ToggleLikeAsync, ToggleBlockAsync, ReportUserAsync, ReportMessageAsync all own their own `SaveChangesAsync`
- **AJAX antiforgery pattern:** `__RequestVerificationToken=` in body (form-urlencoded). NOT header-only.
- **Badge awarding:** Must `.Include(u => u.UserBadges)` before `_questService.AwardBadgesAsync()`
- **GearItem equipped slots:** stored as string Keys on User, NOT FK to GearItem.
- **Mage → "wizard" in images:** GearItem.Key uses `wizard`, GearClass stores `"mage"`
- **GetEffectiveStatsAsync caller contract:** MUST load `user.OwnedGear.ThenInclude(ug => ug.GearItem)` first
- **GetStats() JSON field:** use `intel` not `int`. Includes `gems`.
- **Drop RNG:** uses `Random.Shared` — random per call, NOT day-seeded.
- **Drop cap reset:** must happen BEFORE the `DailyDropCount < DAILY_DROP_CAP` check.
- **Market filter:** `GearClass in [user.Class, "all", "special"]`
- **GameItem.ImagePath:** computed property. Special cases: `food_Potato`→`Pet_Food_Potatoe.png`, `egg_Bear`→`Pet_Egg_BearCub.png`
- **GameItem.Target:** DB column on food items. Potion color this food prefers. Null on non-food.
- **UserPet.PetKey format:** `"{AnimalName}-{PotionColor}"` e.g. `"Wolf-Base"`. No FK to GameItem.
- **GearItem.ShopImagePath:** auto-detects armoire via `Key.Contains("_armoire_")` → `/gear/armoire/shop/`
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
- **Avatar layer order:** mount body → skin → shirt → armor → bangs → hair → mustache → beard → head gear → shield → weapon → mount head → pet (bottom-left)
- **Avatar sprite sizing:** character=90×90, mount=natural size, pet=natural size. Inner container `width:90px height:90px`. `padding-top:0` mounted, `padding-top:24px` not mounted.
- **PetCatalogService:** Singleton. ~1,351 entries. `IsValidHatch(animalKey, colorKey)` — quest eggs only accept drop colors. `CanBecomeMount(petKey)` — wacky potions (except Windup) cannot evolve.
- **Notification routes:** `GET /api/notification?limit=N`, `GET /api/notification/unread-count`, `PUT /api/notification/read-all`. Note: NOT `/mark-all-read`.
- **Message block check:** `IsBlockedAsync` checks EITHER direction. `IsBlockerAsync` checks specific direction only. SendAsync uses `IsBlockedAsync` (symmetric).
- **Conversation list query:** two EF queries (sent + received) merged in-memory dict by partnerId. Avoids EF GroupBy translation issues. Unread counts via separate GroupBy on received-only messages.
- **Message notification type:** `"Social"` — added in `MessageService.SendAsync`. Stored as string in `Notifications` table, no migration needed.
- **ViewProfile block state:** `ViewBag.IsBlocked` = viewer blocks target (can toggle). `ViewBag.IsBlockedBy` = target blocks viewer (hide Send Message button).
- **Guild privacy:** `"public"` or `"private"` string on Guild entity. Public guilds joinable without invite. Private guilds require GuildInvite with Status="Pending".
- **Guild roles:** `"Leader"` / `"Manager"` / `"Member"`. Leader stored as both `Guild.LeaderId` FK and `GuildMember.Role="Leader"`. Leader-leave transfers to oldest Manager then oldest Member; disbands if last.
- **Party constraint:** 1 party per user enforced at service level (`GetMyPartyAsync` check) AND DB unique index on `PartyMember.UserId`.
- **Chat pagination:** PAGE_SIZE=20. `GetMessagesAsync` returns oldest-first (reversed after OrderByDescending). `HasMore = messages.Count == PAGE_SIZE`. Load-more appends older messages above, preserving scroll offset.
- **@mention rendering:** `GuildService.RenderMentions(body, dict)` / `PartyService.RenderMentions` — batch username→ID lookup during `GetMessagesAsync`. Views use `@Html.Raw(entry.RenderedBody)`. Only `<a href>` tags injected (no user-controlled HTML).
- **Razor + JS template literals:** avoid `${variable}` inside template literals in `.cshtml` — Razor interprets `${}` as C# interpolation. Use string concatenation `'text ' + var + '.'` instead.
- **Razor + @-symbols in HTML attributes:** `@username` in placeholder/text is parsed as C# variable. Escape with `@@username` → renders as `@username` in HTML.
- **GuildService/PartyService:** both own their own `SaveChangesAsync`. `ProcessMentionsAsync` is called post-save inside `SendMessageAsync`.
- **XP model:** Cumulative. `CalculateLevel` iterates thresholds; max 100 loops.
- **Effective stats formula:** `base + gearBonus + classBonus(×0.5 if gearClass==userClass) + floor(level/2) + buff`
- **MaxMana:** `effectiveINT × 2 + 30`
- **TempData toast pattern:** set `TempData["ToastXxx"]` in controller, `_Layout.cshtml` renders hidden div, JS calls `showToast` on DOMContentLoaded.
- **CharacterClass location:** top-level static class in `HabitTracker.Constants` — NOT nested under `AppConstants`
- **Guild/Party invite:** uses `int targetUserId` not `string username`. Search endpoints `GET /Guild/SearchInvitable?guildId=X&q=` and `GET /Party/SearchInvitable?q=` return partial-match users excluding existing members. UI is search-then-select; never posts raw username.
- **Navbar Social dropdown:** `isSocialActive` bool computed in `_Layout.cshtml` from controller/action. Covers Friend, Message, Guild, Party, Account+Leaderboard. `msg-badge` (on toggle) + `msg-badge-dd` (in dropdown item) — both updated by `refreshHud()`.
- **BossQuestService.ApplyTaskDamageAsync:** called from `TaskService.ScoreTaskAsync` after `ApplyTaskScoreEconomyAsync` (isUp + non-Reward). Damage formula: `rawDelta * critMult * (1.0 + STR/200)` for todos/dailies, `* (0.5 + STR/400)` for habits, then `/ BossDef`.
- **BossQuestService.ApplyMissedDailyRageAsync:** called from `TaskService.RunCronAsync` after `ApplyCronDamageAsync` for each missed daily. Rage = `|cronDelta| * PriorityMultiplier`. When `RageMeter >= RageValue`: TriggerRageAsync (heal + mpDrain + progressDrain), meter resets to 0.
- **Quest scroll GameItem keys:** `quest_{questKey}` (e.g. `quest_wolf`). IDs 182–296 in `GameItems` table.
- **BossQuest.Category values:** `"pet"`, `"hatchingPotion"`, `"unlockable"`, `"gold"`, `"seasonal"`, `"timeTravelers"`.
- **Time travel quests:** GemCost=0, not purchasable — buy button disabled. IDs 111–113 (robot, solarSystem, windup). GoldCost also 0.
- **Quest lifecycle states:** `"Pending"` → `"Active"` → `"Complete"` / `"Aborted"`. Cancel (Pending, leader): returns scroll. Abort (Active, leader): no rewards.
- **PartyService.GetPartyViewAsync:** now also populates `QuestStatus` (PartyQuestStatusDto) and `OwnedScrolls` (GameItem list) using direct `_context` queries.
- **Quest shop image onerror fallback:** uses `quest_armadillo.png` as generic fallback (guaranteed to exist locally).
- **BossQuest.GemCost:** gem cost to buy scroll. 0 = not purchasable. Tiers: HP≤500 → 4 (5 w/rage), HP≤1000 → 6 (7), HP≤2000 → 8 (9), HP>2000 → 10 (11). Collection quests = 4 always.
- **BossQuest.GoldCost:** REPURPOSED — now means additional gold required alongside gems. Only non-zero for masterclasser ("gold" category) quests (200–700 GP). All other categories: 0.
- **BossQuest.DropDescription:** computed property, not mapped. Parses `DropItemsJson` → human-readable string. Used in Quest Shop cards.
- **Quest start flow:** Only party leader can start a quest. `Views/Party/Index.cshtml` quest panel (`isLeader` gated). After `InvitePartyAsync`, `TryAutoActivateAsync` is called — solo party (1 member) auto-activates immediately.
- **BossQuestSeed helpers:** Q() ignores goldCost param, auto-computes GemCost, GoldCost=0. QB() uses goldCost as goldBonus (masterclasser gold). QC() uses goldCost only if category=="gold". No individual quest call lines need changing.

- **Party chat CSRF:** Same fix as guild/message — `@Html.AntiForgeryToken()` must be rendered in the in-party DOM branch. The no-party branch forms don't count; when `inParty==true` a separate hidden CSRF form is required at top of that section.
- **Party system messages:** Body prefix `[SYS]` marks server-generated messages (boss damage, quest events). `PartyService.GetMessagesAsync` strips prefix and sets `IsSystem=true` on `PartyMessageEntry`. View renders as centered gray badge row (no avatar, no like/delete). No migration — `IsSystem` is DTO-only. `AuthorId` is still the player who triggered the action.
- **Boss damage message persistence:** System message is added to `_db` before `SaveChangesAsync()` at end of `ApplyTaskDamageAsync`. If quest ends (HP≤0), `SaveChangesAsync()` is called first, then `FinishQuestAsync` — message persists before quest completion.
- **GEM_GOLD_COST:** 100 GP (was 25). In `Constants/AppConstants.cs`. Service error messages auto-update from constant.
- **Market Inn tab:** Potion + sleep moved from Economy page to Market `🏨 Inn` tab. Posts to `/Economy/BuyPotion` and `/Economy/ToggleSleep` — same controller endpoints, no changes needed there.
- **Boss quest avatar in party panel:** `BossImagePath` = `/images/habitica/quests/bosses/quest_{Key}.png` (computed on `BossQuest` model). Rendered via `qs.QuestKey` from `PartyQuestStatusDto`. Fallback = `quest_TEMPLATE_FOR_MISSING_IMAGE.png`.
- **Party sidebar quest panel layout:** `#questPanel` is `position:absolute; bottom:0; left:0; right:0; max-height:65%; overflow-y:auto` inside a `position-relative` flex column. `adjustMembersListPadding()` on load+resize sets `paddingBottom` on `#membersList` = `questPanel.offsetHeight + 4px`. Pattern: use `offsetHeight` not a fixed value since quest panel height varies by state (none/pending/active).
- **Party @mention fix pattern:** `IPartyService.RenderBodyAsync(body)` — same as `IGuildService.RenderBodyAsync`. `PartyController.SendMessage` must call it and return `renderedBody` (not `msg.Body`). Party `appendMessage` JS already uses `${d.body}` as innerHTML — same as guild.
- **PetCatalog image rule:** only add catalog entries that have local image files. `Purple` ≠ `RoyalPurple` — images are named `RoyalPurple`, not `Purple`. `PolarBear` has no images at all (different from `BearCub-Polar` which is a special). Before adding new special pets, verify `Pet-{key}.png` exists in `wwwroot/images/habitica/stable/pets/`.
- **CollectedPetsCount vs OwnedPets.Count:** `OwnedPets` = pets where `IsMount=false`. `CollectedPetsCount = OwnedPets.Count + OwnedMounts.Count` — use this for the "X/Y pets" display. Evolved pets should count toward pet collection total AND show with 🐴 badge in pet grid. Pet grid `IsOwned = petSet || mountSet`; mount grid `IsOwned = mountSet` only.
- **Avatar layer order (complete):** mount body → BACK → skin → shirt → armor → bangs → hair → mustache → beard → EYEWEAR → head → HEAD_ACCESSORY → shield → weapon → mount head → pet. Back/eyewear/headAccessory layers currently missing from all 3 avatar views.
- **Skills in task page:** `TaskBoardViewModel.CanUseSkills` = class set AND level ≥ 11. `TaskController.Index` loads user with gear + EffectiveStats + class spells. `POST /Task/CastSpell` delegates to `SpellService.CastAsync`. Skills bar hidden if `!CanUseSkills`. Task-targeting: `.skill-targeting` body class + `pointer-events:none` on `.task-card *` makes click bubble to card.
- **SpellService formulas (Habitica-synced):** `DR(bonus, max, halfway)` = `max * bonus / (bonus + halfway)`. `CalcBonus(taskValue, stat, critMult)` = `(taskValue < 0 ? 1 : taskValue+1) + stat * 0.5 * critMult` (crit scales ONLY the stat term). `smash` crit uses CON; `fireball`/`pickPocket` crit uses PER; `backStab` crit uses STR at 0.3 base chance. `smash` boss damage = `DR(STR*crit, 55, 70)`; `fireball` boss damage = `INT * 0.1`.
- **Party spells:** All 7 party spells now implemented. Load party members via `_context.PartyMembers.Where(partyId).Include(User)`. Apply buff/heal to each. Solo (no party) applies to self only. `mpheal` skips users with Class=="mage". Buff expiry = `UtcNow.AddDays(1)`. Posts `[SYS]` chat message after. `IBossQuestService.FinishQuestAsync` is now public (was private) — called by `SpellService.ApplySpellBossDamageAsync` if boss HP ≤ 0 from spell damage.

---

## Connection String

Stored in .NET User Secrets (never committed):
```
dotnet user-secrets list --project HabitTracker
```
