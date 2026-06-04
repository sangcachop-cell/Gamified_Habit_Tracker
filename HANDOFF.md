# Handoff — Gamified Habit Tracker

**Session date:** 2026-06-03
**Session goal:** Phase 13 — Admin Expansion

---

## What Was Done This Session (2026-06-03, session 12)

### Phase 13 — Admin Expansion

**User model additions (`Models/User.cs`):**
- `IsMuted` (bool, default false) — chat privilege revoked globally
- `IsBanned` (bool, default false) — account banned; login blocked

**New model (`Models/AdminBlocklistEntry.cs`):**
- `Id`, `Type` ("email"|"ip"), `Value`, `Note`, `AddedAt`, `AddedByAdminId`

**AdminController additions (`Controllers/AdminController.cs`):**
- `GET /Admin/Users?q=` — search users by username/email/ID (up to 200 results)
- `GET /Admin/UserDetail/{id}` — full profile: stats, tasks (last 50), inventory, gear, badges
- `POST /Admin/MuteUser/{id}` — toggle `IsMuted`
- `POST /Admin/BanUser/{id}` — toggle `IsBanned` (cannot ban self)
- `GET /Admin/Blocklist` — email/IP blocklist
- `POST /Admin/AddBlocklist` — add entry
- `POST /Admin/RemoveBlocklist/{id}` — remove entry
- `GET /Admin/Groups` — all guilds + parties with member counts
- Existing `Users` action replaced with searchable version

**New views:**
- `Views/Admin/Users.cshtml` — searchable user table with mute/ban/admin-toggle actions
- `Views/Admin/UserDetail.cshtml` — full user history (stats, tasks, inventory, gear, badges)
- `Views/Admin/Blocklist.cshtml` — email/IP blocklist management
- `Views/Admin/Groups.cshtml` — guilds + parties overview

**Integrations:**
- `MessageService.SendAsync` — checks `sender.IsBanned` and `sender.IsMuted` before send
- `GuildService.SendMessageAsync` — checks `IsBanned`/`IsMuted`
- `PartyService.SendMessageAsync` — checks `IsBanned`/`IsMuted`
- `AccountController.Login` — checks `IsBanned`, returns error if banned
- `Views/Shared/_Layout.cshtml` — admin dropdown item "🛡️ Admin Panel" shown only for `IsAdmin` session

**Migration:** `AddPhase13Admin` applied — `IsMuted`, `IsBanned` on Users; `AdminBlocklistEntries` table.

**Files changed:**
- `Models/User.cs` — 2 new fields
- `Models/AdminBlocklistEntry.cs` — NEW
- `Data/AppDbContext.cs` — `AdminBlocklistEntries` DbSet
- `Controllers/AdminController.cs` — replaced Users + 7 new actions
- `Controllers/AccountController.cs` — ban check on Login
- `Services/Implementations/MessageService.cs` — mute/ban check
- `Services/Implementations/GuildService.cs` — mute/ban check
- `Services/Implementations/PartyService.cs` — mute/ban check
- `Views/Admin/Users.cshtml` — NEW
- `Views/Admin/UserDetail.cshtml` — NEW
- `Views/Admin/Blocklist.cshtml` — NEW
- `Views/Admin/Groups.cshtml` — NEW
- `Views/Shared/_Layout.cshtml` — admin panel link
- `Migrations/20260603095528_AddPhase13Admin` — NEW + applied

---

## Current State (end of 2026-06-03 session 12)

- **Build:** 0 errors
- **Phase 13:** COMPLETE ✓
- **Migration applied:** `AddPhase13Admin`
- **Next:** Phase 14 (TBD) or runtime testing

---

## What Was Done This Session (2026-06-03, session 11)

### Phase 12 — Settings & Auth Polish

**User model additions (`Models/User.cs`):**
- `DayStart` (int, 0–23, default 0) — custom cron hour
- `ApiToken` (string?, max 64) — generated on first request
- `PMPermission` (string, "everyone" | "nobody", default "everyone")
- `ProfileVisibility` (string, "public" | "private", default "public")
- `SuppressNotifications` (bool, default false)

**New files:**
- `Controllers/SettingsController.cs` — 8 endpoints:
  - `GET /Settings` — load user → settings view
  - `POST /Settings/SaveDayStart` — update DayStart (0–23)
  - `POST /Settings/SavePreferences` — update SuppressNotifications
  - `POST /Settings/SavePrivacy` — update PMPermission + ProfileVisibility
  - `GET /Settings/GetApiToken` — show/generate token (lazy init)
  - `POST /Settings/RegenerateToken` — regenerate (invalidates old)
  - `GET /Settings/ExportData` — download JSON (account + character + tasks + badges)
  - `POST /Settings/ResetProgress` — wipe level/XP/gold/HP/stats/tasks; requires password
- `Views/Settings/Index.cshtml` — 6-tab UI: Account | Day Start | Preferences | Privacy | API Token | Data
- `Views/Friend/ProfilePrivate.cshtml` — shown when viewer hits a private profile

**AccountController additions (`Controllers/AccountController.cs`):**
- `POST /Account/ChangeUsername` — requires current password; checks uniqueness; updates session
- `POST /Account/ChangeEmail` — requires current password; checks uniqueness
- `POST /Account/DeleteAccount` — requires current password; hard-deletes user row + clears session
- `ChangeCredentialRequest` DTO added (bottom of file, same namespace)

**Integrations:**
- `MessageService.SendAsync` — after block check, rejects if `receiver.PMPermission == "nobody"`
- `TaskService.RunCronAsync` — cron "today" now respects `user.DayStart`: if `now.Hour < DayStart`, still counts as yesterday
- `FriendController.ViewProfile` — returns `ProfilePrivate` view if `target.ProfileVisibility == "private"` and viewer ≠ target
- `Views/Shared/_Layout.cshtml` — `⚙️ Settings` link added to user dropdown (above Change Password)

**Migration:** `AddPhase12Settings` applied — 5 new columns on `Users`. Existing rows backfilled: `PMPermission='everyone'`, `ProfileVisibility='public'`.

**Note — Google OAuth:** Already wired in `Program.cs` (`AddGoogle`). No code change needed; requires `Authentication:Google:ClientId` + `Authentication:Google:ClientSecret` in user secrets/config. Link-social-to-existing-account deferred (out of ROADMAP scope).

**Files changed:**
- `Models/User.cs` — 5 new fields
- `Controllers/SettingsController.cs` — NEW
- `Controllers/AccountController.cs` — 3 new actions + DTO
- `Views/Settings/Index.cshtml` — NEW
- `Views/Friend/ProfilePrivate.cshtml` — NEW
- `Services/Implementations/MessageService.cs` — PMPermission check
- `Services/Implementations/TaskService.cs` — DayStart cron
- `Controllers/Friendcontroller .cs` — ProfileVisibility gate
- `Views/Shared/_Layout.cshtml` — Settings link
- `Migrations/20260603094000_AddPhase12Settings` — NEW + applied

---

## Current State (end of 2026-06-03 session 12)

- **Build:** 0 errors
- **Phase 12:** COMPLETE ✓
- **Phase 13:** COMPLETE ✓
- **Migrations applied:** `AddPhase12Settings`, `AddPhase13Admin`
- **Next:** Phase 14 (TBD) or runtime testing Phases 5–8

---

## What Was Done This Session (2026-06-03, session 10)

### Phase 11 — Notification UI

**New files:**
- `Controllers/NotificationsController.cs` — Razor page, `GET /Notifications`, loads `GetNotificationsAsync(userId, 100)`, redirects to login if no session.
- `Views/Notifications/Index.cshtml` — full notifications page.

**Notifications page:**
- All / Unread tab pills with live counts
- Per-row: icon, title (bold if unread), type badge (color-coded), message, time ago, mark-read ✓ (if unread), delete ✕
- Left border color by type (unread only): Achievement=#f0a500, Streak=#fc9044, LevelUp=#4e9af1, Quest=#a680e8, quest_rage=#e8436a, Guild=#26a69a, Party=#42a5f5, Social=#e76e78
- "Mark all read" button (disabled when none unread)
- Tab filter: JS show/hide by `data-read` — no reload
- All mutations: AJAX to existing `/api/notification` endpoints

**Bell dropdown enhancements (`Views/Shared/_Layout.cshtml`):**
- Header "Notifications" → link to `/Notifications`
- "See all notifications →" footer link
- Per-item: left colored border (type), faint bg tint, bold title if unread
- Click row → `notifDropClick()`: marks read via `PUT /api/notification/{id}/read`, decrements badge, navigates to link

**User dropdown:** Added `🔔 Notifications → /Notifications`.

**Files changed:**
- `Controllers/NotificationsController.cs` — NEW
- `Views/Notifications/Index.cshtml` — NEW
- `Views/Shared/_Layout.cshtml` — bell dropdown HTML + JS + user dropdown link

---

## Bug Fixes (2026-06-03, session 10 continued)

### Notifications Page — Color Bug
**Problem:** `body { background: #f8fafc }` (light). All nav/text colors white → invisible.
**Fix (`Views/Notifications/Index.cshtml`):** Tab text → `rgba(0,0,0,0.6)`, active tab → `#4f46e5`. Rows → `background: #fff`, `border: rgba(0,0,0,0.09)`, title `color: #1a1a2e`. Empty state → `rgba(0,0,0,0.35)`.

### Message Compose Button — Icon Invisible
**Problem:** `<i class="bi bi-pencil-square">` — Bootstrap Icons never loaded. Button = solid purple square, no content.
**Fix (`Views/Message/Index.cshtml`):** Replace `<i class="bi ...">` with inline SVG pencil icon.

### Toast Types — Missing CSS Classes
**Problem:** `.success`, `.danger`, `.info`, `.warning` toast classes never written to `_Layout.cshtml` (HANDOFF said done in Phase 7 — wasn't).
**Fix (`Views/Shared/_Layout.cshtml`):** Added `.success` (green), `.danger` (red), `.info` (blue), `.warning` (amber + `color: #1a1a1a`).

### Guild/Party Mention Scoping
**Problem:** `ProcessMentionsAsync` + `RenderBodyAsync` searched all users — not scoped to guild/party members. Could notify non-members. `PartyService.GetMessagesAsync` used `ToDictionaryAsync` directly (missing GroupBy dedup — crash with case-insensitive collation).
**Fix:**
- `IGuildService.RenderBodyAsync(string body, int guildId)` — added `guildId` param
- `IPartyService.RenderBodyAsync(string body, int partyId)` — added `partyId` param
- Both `ProcessMentionsAsync`: fetch `GuildMembers`/`PartyMembers WHERE groupId` → restrict to members; `HashSet<int> notified` prevents duplicate notifications
- Both `GetMessagesAsync`: scoped to member IDs + added GroupBy dedup to PartyService
- `GuildController.SendMessage` → `RenderBodyAsync(msg.Body, id)`
- `PartyController.SendMessage` → `RenderBodyAsync(msg.Body, msg.PartyId)`

### @Mention Autocomplete — Duplicate Username Resolution
**Problem:** No way to pick which "123" to mention when multiple members share username.
**New format:** `@username:userId` (e.g. `@123:42`) stored when names clash. `@username` for unique names — backward compatible.
**Backend (`RenderMentions` both services):** Regex `@(\w+)(?::(\d+))?` — group 2 present → use explicit id; else name lookup.
**Backend (`ProcessMentionsAsync` both services):** Same regex; `@name:id` verified against `memberIdSet`; `@name` resolves by name.
**Frontend (Guild/View.cshtml + Party/Index.cshtml):**
- `const MEMBERS = [...]` injected from Razor (id, username, avatar per member)
- `<div id="mention-dropdown">` above textarea (`position:absolute; bottom:100%`)
- `initMentionAutocomplete(taId, members)`: detects `@` → dropdown with filtered members → avatar + username + `#ID` when shared → Arrow/Enter/Tab/Escape nav
- Duplicate → inserts `@username:id`; unique → `@username`
- `handleKey` yields Enter/Tab/Arrow/Escape to autocomplete when dropdown visible

**Files changed (all bug fixes):**
- `Views/Notifications/Index.cshtml` — color fixes
- `Views/Message/Index.cshtml` — SVG icon
- `Views/Shared/_Layout.cshtml` — toast CSS classes
- `Services/IGuildService.cs` — `RenderBodyAsync(body, guildId)`
- `Services/IPartyService.cs` — `RenderBodyAsync(body, partyId)`
- `Services/Implementations/GuildService.cs` — scoped mentions + `@name:id` format
- `Services/Implementations/PartyService.cs` — scoped mentions + GroupBy fix + `@name:id` format
- `Controllers/GuildController.cs` — pass guildId to RenderBodyAsync
- `Controllers/PartyController.cs` — pass partyId to RenderBodyAsync
- `Views/Guild/View.cshtml` — MEMBERS injection + autocomplete
- `Views/Party/Index.cshtml` — MEMBERS injection + autocomplete

---

## Current State (end of 2026-06-03 session 10)

- **Build:** 0 errors
- **Phase 11:** COMPLETE ✓ — full notifications page + enhanced bell dropdown
- **Bug fixes:** notification colors, compose icon, toast CSS, mention scoping, @mention autocomplete with duplicate-username disambiguation
- **Next:** Phase 12 (TBD) or runtime testing of Phases 5–8

---

## What Was Done This Session (2026-06-03, session 9)

### Phase 10 — Achievements Expansion

**Old system removed:** 3 XP-threshold badges seeded in `AppDbContext`, awarded by `QuestService.AwardBadgesAsync` based on `badge.RequiredXP`.

**New system:**
- `Badge` model: replaced `RequiredXP` with `Key` (string slug) + `TriggerType` (category) + `TriggerValue` (threshold). `RequiredXP` column renamed to `TriggerValue` in DB.
- `User` model: added `TotalTasksCompleted` (int) + `PerfectDayCount` (int).
- `IAchievementService` / `AchievementService` — 7 check methods; each queries earned badges, awards unearned, fires notification.
- 25 achievement badges seeded (IDs 10–34), old 3 deleted.

**25 achievements by category:**

| Category | Keys | Notes |
|---|---|---|
| Streak | streak_7/21/90/180/365 | Checks `CurrentStreak` |
| TaskMilestone | tasks_1/10/50/100/500 | Checks `TotalTasksCompleted` |
| PerfectDay | perfect_1/7/30 | Checks `PerfectDayCount` |
| UltimateGear | ultimate_warrior/mage/rogue/healer | Checks all class GearItems owned |
| Quest | quests_1/10/50 | Counts PartyQuestMembers (accepted+complete) |
| Stable | stable_10/beast_master/mount_master/triad_bingo | Various pet/mount counts |
| Social | joined_guild | First guild join |

**Trigger hooks:**
- `TaskService.ScoreTaskAsync` → `CheckStreakAsync` + `CheckTaskMilestoneAsync` on isUp score
- `TaskService.RunCronAsync` → `PerfectDayCount++` when `anyDailyDue && allDailiesCompleted`, then `CheckPerfectDayAsync`
- `BossQuestService.FinishQuestAsync` → `CheckQuestAsync(user.Id)` per accepted user
- `StableService.FeedAsync` → `CheckStableAsync(userId)` after pet evolves to mount
- `GuildService.JoinPublicAsync` + `AcceptInviteAsync` → `CheckGuildJoinAsync(userId)`
- `EquipmentController.Equip` → `CheckUltimateGearAsync(userId, userClass)` after SetSlot + SaveChanges (non-costume only)

**Perfect Day buff:** `ceil(level/2)` to all stats — also increments `PerfectDayCount` before `CheckPerfectDayAsync`.

**`IQuestService.AwardBadgesAsync`:** removed from interface + QuestService.

**View:** `Views/Dashboard/Badges.cshtml` rewritten — 25 achievements grouped by category, locked state (greyed + 🔒) for unearned, NEW ribbon for earned within 24h, progress bar at top.

**Files changed:**
- `Models/Badge.cs`, `Models/UserBadge.cs`, `Models/User.cs`
- `Services/IAchievementService.cs` — NEW
- `Services/Implementations/AchievementService.cs` — NEW
- `Services/IQuestService.cs`, `Services/Implementations/QuestService.cs` — removed AwardBadgesAsync
- `Services/Implementations/TaskService.cs`, `BossQuestService.cs`, `StableService.cs`, `GuildService.cs`
- `Controllers/EquipmentController.cs`, `Controllers/DashboardController.cs`
- `Data/AppDbContext.cs`, `Program.cs`
- `Views/Dashboard/Badges.cshtml`
- `Migrations/20260603065950_AddPhase10Achievements` — applied

---

## Current State (end of 2026-06-03 session 9)

- **Build:** 0 errors
- **Phase 10:** COMPLETE ✓
- **Migration applied:** `AddPhase10Achievements`
- **Achievements in DB:** 25 (IDs 10–34)

---

## What Was Done This Session (2026-05-30, session 8)

### Stable — Feed UX Redesign

**Root cause of broken onclick:** Razor `@(condition ? $"onclick=\"handler('{key}')\"" : "")` HTML-encodes → `onclick=&quot;...&quot;` → browser never registers handler.

**Fix:** Removed inline Razor onclicks. Replaced with JS event delegation:
```javascript
document.getElementById('tab-my-pets').addEventListener('click', function(e) {
    var card = e.target.closest('.owned-pet-card');
    ...
});
```

**New feed flow:** Food bar at top of My Pets tab → click food → orange highlight + dashed outline on feedable pets → click pet → feeds, updates progress bar + qty inline. ESC/click same food/✕ Cancel → exits feed mode.

**Files changed:** `Views/Stable/Index.cshtml` — full rewrite

---

### Stable — Tab Restructure (5 tabs)

| Tab | Content |
|-----|---------|
| My Pets | Owned only (flat grid), food bar, active pet banner, feed/activate |
| My Mounts | Owned only (flat grid), active mount banner, ride/dismount |
| 📖 Pet Collection | Full catalog read-only |
| 📖 Mount Collection | Full catalog read-only |
| 🥚 Hatch | Unchanged |

**Files changed:** `Views/Stable/Index.cshtml`

---

### Pet/Mount Mutual Exclusivity

`StableService.SetActivePetAsync` clears `user.ActiveMountKey` when equipping pet. `SetActiveMountAsync` clears `user.ActivePetKey`. Unequipping (empty key) leaves other unchanged.

**Files changed:** `Services/Implementations/StableService.cs`

---

### UI Fixes

- **Pet avatar position:** `bottom:-10px;left:-14px` in all 3 avatar views (was `bottom:0;left:0` — overlapped player).
- **Gear card name invisible:** Added `text-white` to name divs in `Views/Market/Index.cshtml` + `Views/Inventory/Index.cshtml`.
- **Footer:** removed "5 Features" text from `Views/Shared/_Layout.cshtml`.

---

## Architecture Notes (session 8)

- **Razor inline onclick encoding bug:** `@(condition ? "onclick=\"handler()\"" : "")` HTML-encodes. Use JS event delegation instead for all conditional onclick attributes.
- **bg-dark card text rule:** Always add `text-white` to text inside `bg-dark` Bootstrap cards.

---

## What Was Done This Session (2026-05-30, session 7)

### Feature — Equipment Slots: back / eyewear / headAccessory

**Root cause:** `primarySlots` in `Views/Equipment/Index.cshtml` only had 4 slots. All backend already handled 8 slots. Pure view bug.

**Fix (3 view files):** Added `"back"`, `"eyewear"`, `"headAccessory"` to `primarySlots`; added gear vars + avatar image layers (`layer-back` before skin, `layer-eyewear` after beard, `layer-headAccessory` after head). All new layers use `.gif` onerror fallback.

---

### Feature — Background Customization

`Models/User.cs` — added:
```csharp
[StringLength(100)]
public string? Background { get; set; }
```

`Models/ViewModels/CustomizeViewModel.cs` — added:
```csharp
public List<string> Backgrounds { get; set; } = new();
```

- Controller: enumerate `background_*.png` from `wwwroot/images/habitica/backgrounds/`; `POST /Character/SetBackground` — sets `user.Background = key`.
- Customize view: `🌄 Background` tab + grid of 395 thumbnails (60×40). JS `setBackground(key)` updates `#avatar-preview` CSS inline + AJAX save.
- Equipment/Character views: apply `background-image` CSS on load.
- **Migration:** `20260530053510_AddUserBackground` — `Background NVARCHAR(100) NULL`.

---

### Bug Fix — Back Gear Image (`heroicAureole.gif`)

**Root cause:** `GearItem.GetWornImagePath()` returns `.png`. Actual file is `.gif` — only `.gif` in all gear folders.

**Fix (onerror on all new gear layers):**
```javascript
onerror="if(!this.src.endsWith('.gif')){this.src=this.src.replace('.png','.gif');}else{this.onerror=null;this.style.display='none';}"
```

**Also fixed:** `overflow:visible` on avatar-preview divs (was `overflow:hidden` — clipped mount heads).

---

## What Was Done This Session (2026-05-30, session 6)

### Fix — Pet Catalog: Remove Entries With No Local Images

**Removed from `PetCatalogService`:** `"Purple"` (files named `RoyalPurple`), `"PolarBear"` (no images), `Wolf-Cerberus`, `Gryphon-Gryphatrice`, `Gryphatrice-Jubilant`.

### Fix — Pet Collection Count Mismatch (Evolved Pets)

`CollectedPetsCount = OwnedPets.Count + OwnedMounts.Count` computed property added to `StableViewModel`. Pet grid `IsOwned = petSet || mountSet`. Mount grid `IsOwned = mountSet` only.

---

## What Was Done This Session (2026-05-30, session 5)

### Fix — Party Sidebar Quest Panel Clipped

Quest panel + pending invites: `position:absolute; bottom:0; left:0; right:0; z-index:2; max-height:65%; overflow-y:auto`. `adjustMembersListPadding()` sets `paddingBottom` on `#membersList` = quest panel `offsetHeight`.

---

## What Was Done This Session (2026-05-30, session 4)

### Fix — Party @mention Link Not Rendering Until Reload

Added `RenderBodyAsync` to `IPartyService` / `PartyService`. `PartyController.SendMessage` now returns `renderedBody`.

### Feature — Skills in Task System

- `TaskBoardViewModel` + `TaskController.Index` loads user/gear/stats/spells. `POST /Task/CastSpell` delegates to `SpellService.CastAsync`.
- SpellService: fixed `CalculateBonus`, `smash` crit (CON), `fireball` formula, `defensiveStance`; implemented 7 party spells; `ApplySpellBossDamageAsync`.
- UI: horizontal skills bar, 4 skill cards, `.skill-targeting` body class on activation, `pointer-events:none` on `.task-card *`.

---

## What Was Done This Session (2026-05-30, session 3)

- **Boss quest avatar:** Active quest renders boss image above HP bars. Collection quests show scroll. `onerror` fallback.
- **Gem cost:** `GEM_GOLD_COST` 25 → 100 GP.
- **Market Inn tab:** Health Potion + Rest Mode moved from Economy page. Posts to existing `/Economy/BuyPotion` + `/Economy/ToggleSleep`.
- **Party chat CSRF:** Added `@Html.AntiForgeryToken()` to in-party DOM branch.
- **Boss damage system messages:** `[SYS]` prefix; `PartyService.GetMessagesAsync` strips prefix, sets `IsSystem=true`; view renders as centered gray badge row.

---

## What Was Done This Session (2026-05-30, session 2)

### Fix — Quest Start Flow

1. **Solo party auto-start:** `BossQuestService.InvitePartyAsync` calls `TryAutoActivateAsync` after creating member rows.
2. **Leader-only panel:** quest "Start a Quest" section gated behind `isLeader`.
3. **Quest Shop UX hints:** info banner + Party page link in toast.

### Fix — Quest Scroll Currency: Gold → Gems

`BossQuest.GemCost` added. Gem pricing tiers (auto-computed from HP + rage):

| Boss HP | No Rage | With Rage |
|---------|---------|-----------|
| ≤ 500   | 4 💎    | 5 💎      |
| 501–1000| 6 💎    | 7 💎      |
| 1001–2000| 8 💎  | 9 💎      |
| > 2000  | 10 💎   | 11 💎     |
| Time Travel | 0 (not for sale) | — |
| Masterclasser | above tiers + 200–700 GP | |
| Collection quest | 4 💎 | — |

`BossQuestSeed` helpers auto-compute GemCost. `BuyScrollAsync` checks `GemCost > 0`.

---

## What Was Done This Session (2026-05-30, session 1)

### Phase 8 — Boss Quests

**New models:** `BossQuest`, `PartyQuest`, `PartyQuestMember`. 115 quests seeded across 7 categories. **Migration:** `AddPhase8BossQuests`.

**Service:** `IBossQuestService` / `BossQuestService` — shop buy, full lifecycle (invite/accept/reject/force-start/cancel/abort), boss damage + collection drops, rage mechanics, quest completion rewards.

**Integration:** `TaskService.ScoreTaskAsync` applies boss damage on task up; `RunCronAsync` applies rage on missed dailies.

**Shop:** `GET /QuestShop`, `POST /QuestShop/Buy`. **Party sidebar:** 3-state quest panel (none/pending/active).

---

### Fix — Guild/Party Invite: Username → User Search

`InviteAsync` → `int targetUserId`. Added `GET /Guild/SearchInvitable?guildId=X&q=` + `GET /Party/SearchInvitable?q=` — partial match, excludes existing members, returns top 8 `{Id, Username, Avatar, Level}`. Views: search-then-select chip UI.

### UX — Navbar Social Dropdown

5 social links (Leaderboard/Friends/Messages/Guilds/Party) grouped under `👥 Social` dropdown. `isSocialActive` bool in `_Layout.cshtml`.

---

## What Was Done This Session (2026-05-29, Phase 7)

### Phase 7.1 — Guilds

**Models:** `Guild`, `GuildMember`, `GuildMessage`, `GuildMessageLike`, `GuildInvite`.
**Service:** `IGuildService` / `GuildService` — full CRUD, chat, invites, kick/promote/demote, @mention.
**Controller:** `GuildController` — 16 endpoints.
**Views:** `Guild/Index.cshtml` (Discover + My Guilds), `Guild/View.cshtml` (two-column), `Guild/Create.cshtml`.

---

### Phase 7.2 — Parties

**Models:** `Party`, `PartyMember`, `PartyMessage`, `PartyMessageLike`, `PartyInvite`. 1-party-per-user enforced at service + DB unique index on `PartyMember.UserId`.
**Service:** `IPartyService` / `PartyService`. **Controller:** `PartyController` — 11 endpoints.

---

### Phase 7.3 — @mention

Both services: `ProcessMentionsAsync` + `RenderMentions`. Views use `@Html.Raw(entry.RenderedBody)`.

**Migration applied:** `20260529061815_AddPhase7GuildsParties` — creates 10 tables.

---

## What Was Done Previous Sessions

### 2026-05-29 — Phase 7 Bug Fixes (session 2)

**Fix 1 — Notifications "Failed to load":** Removed `[Authorize]`. `GetUserId()` uses `GetInt32`. Returns `{count:0}` for unauthenticated.

**Fix 2 — Can't join public guild:** `@Html.AntiForgeryToken()` must render into page body (not Razor variable). Token only existed when pending invites present.

**Fix 3 — Can't invite to private guild:** Same CSRF pattern — `@Html.AntiForgeryToken()` must be actual DOM node.

**Fix 4 — Toast text invisible:** Added `.success`, `.danger`, `.info`, `.warning` CSS (was missing). `success`/`warning` use `color: #1a1a1a`.

**Fix 5 — Guild View crash (duplicate mention key):** `ToDictionaryAsync` on username throws with case-insensitive collation. Use `ToListAsync()` + `GroupBy(OrdinalIgnoreCase).ToDictionary(...)`.

**Fix 6 — @mention not rendered after send:** `SendMessage` returned `msg.Body` (raw). Added `RenderBodyAsync` to `IGuildService`, controller returns `renderedBody`.

**Fix 7 — @mention link invisible:** Added `.mention { color: #4a90d9 }` + `.bg-primary .mention { color: #bde0ff }` to `site.css`.

---

### 2026-05-29 (Phase 6 + Bug Fixes)

**Phase 6 Bug Fix 1 — New Message modal fails on empty inbox:** `@Html.AntiForgeryToken()` inside `#newMsgModal` (token only existed when conversation open).

**Phase 6 Bug Fix 2 — Avatars broken in messages:** `/images/default-avatar.png` → `/images/default.png`.

---

## What Was Done This Session (2026-05-29, Phase 6)

### Phase 6.1 — Private Messages

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
| GET | `/Message?otherId=X&page=N` | View(InboxViewModel) |
| GET | `/Message/Conversation/{id}` | Redirect → Index?otherId={id} |
| GET | `/Message/UnreadCount` | JSON `{count}` |
| POST | `/Message/Send` | JSON `{success, messageId, body, sentAt}` |
| POST | `/Message/Delete/{id}` | JSON `{success, error}` |
| POST | `/Message/Like/{id}` | JSON `{success, liked, count}` |
| POST | `/Message/Block/{id}` | JSON `{success, blocked}` |
| POST | `/Message/Report` | JSON `{success, error}` |

Key patterns: block check before send, soft delete (`DeletedBySender`/`DeletedByReceiver` flags, hard-delete when both true), 24h report duplicate guard.

### Phase 6.2 — Block + Report

`Models/UserBlock.cs`, `Models/Report.cs`. `GET /Admin/Reports`, `POST /Admin/ResolveReport/{id}`.

### Phase 6.3 — Profile Enhancements

`ViewProfile()` queries equipped gear + block state both directions. `FindByUsername(string username)` GET — returns `{Id, Username, Avatar}`. Profile completeness bar (bio+20, avatar+20, location+15, class+15, social+15, badge+15).

### Phase 6.4 — Notification Bell + Message Badge

Bell `#notifBell` + `#notif-badge` in `_Layout.cshtml`. `refreshHud()` fetches `/Message/UnreadCount` + `/api/notification/unread-count`. `show.bs.dropdown` on bell loads notifications.

**Migration applied:** `20260529044742_AddPhase6Social`.

---

## What Was Done Previous Sessions

### 2026-05-29 (session 1) — Bug Fixes + Pet Catalog Sync

- EconomyService: `&& i.Quantity > 0` filter; day-reset before cap check.
- Stable: `NewFoodQuantity` in `StableResult`; food qty syncs inline.
- GameItem seed: 30 → 181 items. Migration `AddWackyPotions` applied.
- `PetCatalogService`: ~1,351-entry singleton. `IsValidHatch`, `CanBecomeMount`, `GetAnimalGroups`.
- Avatar sprite sizing: character=90×90, inner container `width:90px height:90px`, `padding-top` toggle.

### 2026-05-28 (session 2)

- Fixed 85 class gear names. Added 108 special gear items (IDs 552–659). Migration `SyncHabiticaGearCatalog`.
- Fixed task drop toast: `DroppedItemIcon` now `<img>` HTML.

### 2026-05-28 (session 1) — Armoire Sync + Phase 4

- 466 armoire items seeded (IDs 86–551). Fixed `ShopImagePath` + `GetWornImagePath` for armoire.
- Migration `AddArmoireGearItems` + `AddPhase4InventoryShop` applied.

---

## Current State (end of 2026-06-03 session 12)

- **Build:** 0 errors
- **DB migrations applied:** `AddPhase4InventoryShop`, `AddArmoireGearItems`, `SyncHabiticaGearCatalog`, `AddPhase5StablePetMount`, `AddWackyPotions`, `AddPhase6Social`, `AddPhase7GuildsParties`, `AddPhase8BossQuests`, `AddBossQuestGemCost`, `AddUserBackground`, `AddPhase10Achievements`, `AddPhase12Settings`, `AddPhase13Admin`
- **GearItems:** 659 | **GameItems:** 296 | **BossQuests:** 115 | **Achievements:** 25 (IDs 10–34)
- **Phases 1–13:** COMPLETE ✓ (Phases 5–8 UNVERIFIED runtime)
- **Phase 9:** SKIPPED
- **Next:** Phase 14 (TBD) or runtime testing Phases 5–8

---

## Known Gaps / Deferred

| Gap | Notes |
|-----|-------|
| Costume mode UI toggle | Backend done, no UI |
| Rebirth gem cost | Service wired, controller uses free path |
| Phase 5 release pet/mount | No action on Stable page |
| Phase 5 Saddle item | Seeded; `FeedAsync` doesn't handle instant-evolution |
| Phase 6 admin ban/mute | `ResolveReport` only marks resolved |
| Phase 7 guild logo/edit/search pagination | Fields exist, no UI |
| Phase 7 runtime test | See test checklist below |
| Google OAuth link-to-existing | Flow wired; needs `Authentication:Google:ClientId/Secret` in config |
| Phase 12 suppress notifications | Field stored; `AchievementService`/notification creation not gated by it yet |

### Phase 7 Test Checklist

1. `/Guild` → Discover tab loads public guilds grid
2. Create guild (public) → appears in Discover; My Guilds shows
3. Create guild (private) → NOT in Discover
4. Join public guild → member list updates
5. Invite to private guild → invite strip on invitee `/Guild`
6. Accept/decline invite
7. Guild chat: send, like, @mention notification, @username renders as profile link
8. Promote/demote/kick member
9. Leave guild (leader, non-leader, last member)
10. Create party → invite → accept → 1-party constraint blocks second accept
11. Party chat, @mention, kick, leader-leave transfer, last-member disband

### Phase 6 Test Checklist

1. New Message modal on empty inbox (antiforgery fix)
2. Send PM → unread dot + nav badge
3. Like, delete message
4. Block from ViewProfile → blocked user gets error
5. Report → `/Admin/Reports` shows it
6. Notification bell dropdown + mark all read
7. Profile completeness bar, class emoji, gear card

---

## Phase 7 Test Checklist (runtime)

1. `/Guild` → Discover tab loads
2–17. (see above)
18–26. Party flow (see above)

---

## Next Step

Phase 13 complete. Next: Phase 14 (TBD) or runtime testing Phases 5–8.

---

## Source Files Reference

All Habitica source: `D:\Download\habitica-develop\habitica-develop\website\common\`

| Content | Path |
|---------|------|
| Gear sets (class) | `script/content/gear/sets/{warrior,wizard,rogue,healer,base}.js` |
| Gear sets (armoire) | `script/content/gear/sets/armoire.js` |
| Gear sets (special) | `script/content/gear/sets/special/index.js` |
| English locale | `locales/en/gear.json` |
| Pets/stable catalog | `script/content/stable.js` + `petInfo.js` |
| Feed logic | `script/ops/feed.js` |
| Guild/Party model | `server/models/group.js` |

---

## Architecture Notes (all sessions)

- **Session auth:** `HttpContext.Session.GetInt32(AppConstants.SESSION_USER_ID)` — no Identity framework. NO `[Authorize]` attribute — returns 401/redirect, breaks AJAX.
- **CSRF pattern:** `@Html.AntiForgeryToken()` MUST render into page body as HTML. JS reads `document.querySelector('[name=__RequestVerificationToken]')?.value`. Each DOM branch that POSTs needs own token.
- **Toast system:** `showToast(icon, title, msg, type, duration)`. Types: `toast-xp`, `toast-badge`, `toast-streak`, `toast-level`, `toast-gold`, `toast-hp`, `toast-crit`, `toast-drop`, `toast-death`, `toast-rebirth`, `success`, `danger`, `info`, `warning`. `TempData["ToastXxx"]` → hidden div → JS on DOMContentLoaded.
- **HUD refresh:** `refreshHud()` in `_Layout.cshtml` — calls `GET /Economy/GetStats`, `/Message/UnreadCount`, `/api/notification/unread-count`.
- **SaveChanges ownership:** EconomyService Phase 1–3 methods modify user in-memory only — caller saves. Phase 4+ service methods (BuyGem, HatchAsync, FeedAsync, etc.), MessageService, GuildService, PartyService, AchievementService all own their `SaveChangesAsync`.
- **Razor + JS template literals:** avoid `${variable}` inside template literals in `.cshtml` — Razor interprets as C# interpolation. Use string concat `'text ' + var + '.'`.
- **Razor @-symbols in HTML:** `@username` in placeholder/text parsed as C# variable. Escape with `@@username`.
- **Razor inline onclick encoding bug:** `@(condition ? "onclick=\"handler()\"" : "")` HTML-encodes → `onclick` never registers. Use JS event delegation.
- **`overflow:visible` on avatar-preview divs:** NEVER `overflow:hidden` — mount heads overflow 90×90 bounds.
- **Gear image `.gif` fallback:** `onerror="if(!this.src.endsWith('.gif')){this.src=this.src.replace('.png','.gif');}else{this.onerror=null;this.style.display='none';}"`
- **bg-dark card text:** always add `text-white` inside `bg-dark` Bootstrap cards.
- **mentionLookup dedup:** always `ToListAsync()` + `GroupBy(OrdinalIgnoreCase).ToDictionary(...)` — never `ToDictionaryAsync` on username-keyed query.
- **@mention scoping:** `ProcessMentionsAsync` + `RenderBodyAsync` + `GetMessagesAsync` lookups ALL scoped to guild/party member IDs. Non-members can't be notified or linked.
- **@mention format:** `@username` (unique) or `@username:id` (duplicate names). Regex `@(\w+)(?::(\d+))?`. Explicit id skips name lookup.
- **`RenderBodyAsync` signatures:** `IGuildService.RenderBodyAsync(string body, int guildId)`, `IPartyService.RenderBodyAsync(string body, int partyId)`.
- **Notification routes:** `GET /api/notification?limit=N`, `GET /api/notification/unread-count`, `PUT /api/notification/read-all`. NOT `/mark-all-read`.
- **Notifications page:** `GET /Notifications` (NotificationsController, Razor). Light bg (`#f8fafc`) — use dark text colors, not white.
- **Market filter:** `GearClass in [user.Class, "all", "special"]`.
- **GameItem.ImagePath:** special cases: `food_Potato`→`Pet_Food_Potatoe.png`, `egg_Bear`→`Pet_Egg_BearCub.png`.
- **UserPet.PetKey format:** `"{AnimalName}-{PotionColor}"` e.g. `"Wolf-Base"`. No FK to GameItem.
- **GearItem.ShopImagePath:** auto-detects armoire via `Key.Contains("_armoire_")`.
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
- **Avatar layer order:** mount body → BACK → skin → shirt → armor → bangs → hair → mustache → beard → EYEWEAR → head → HEAD_ACCESSORY → shield → weapon → mount head → pet
- **Avatar sprite sizing:** character=90×90, mount=natural, inner container `width:90px height:90px`, `padding-top:0` mounted / `padding-top:24px` unmounted.
- **PetCatalogService:** Singleton. ~1,351 entries. `IsValidHatch` — quest eggs only accept drop colors. `CanBecomeMount` — wacky (except Windup) cannot evolve.
- **PetCatalog image rule:** only add entries with local images. `Purple` ≠ `RoyalPurple`. Verify `Pet-{key}.png` exists before adding special pets.
- **CollectedPetsCount:** `OwnedPets.Count + OwnedMounts.Count`. Evolved pets count in both. Pet grid `IsOwned = petSet || mountSet`; mount grid `IsOwned = mountSet` only.
- **Message block:** `IsBlockedAsync` checks EITHER direction. `IsBlockerAsync` checks specific direction. `SendAsync` uses `IsBlockedAsync`.
- **ViewProfile block state:** `ViewBag.IsBlocked` = viewer blocks target. `ViewBag.IsBlockedBy` = target blocks viewer (hide Send Message).
- **Guild roles:** Leader / Manager / Member. Leader-leave transfers to oldest Manager → oldest Member → disband if last.
- **Party constraint:** 1 party per user — service + DB unique index on `PartyMember.UserId`.
- **Chat pagination:** PAGE_SIZE=20, oldest-first (reverse after OrderByDesc). `HasMore = messages.Count == PAGE_SIZE`.
- **System messages:** `[SYS]` prefix in `PartyMessage.Body`. `GetMessagesAsync` strips prefix, sets `IsSystem=true` on DTO. No DB column — DTO-only.
- **Party CSRF:** `@Html.AntiForgeryToken()` in in-party DOM branch, separate from no-party branch.
- **`back/eyewear/headAccessory` layer IDs:** Equipment view uses `layer-{slot}` (no `-gear` suffix). Customize view uses `layer-{slot}-gear` suffix.
- **BossQuest.GemCost:** 0 = not purchasable. Tiers: HP≤500→4, HP≤1000→6, HP≤2000→8, HP>2000→10 (add 1 with rage). Collection=4.
- **BossQuest.GoldCost:** REPURPOSED = additional gold alongside gems. Non-zero only for masterclasser ("gold" category) quests.
- **Quest scroll keys:** `quest_{questKey}` (e.g. `quest_wolf`). IDs 182–296 in `GameItems`.
- **Quest lifecycle:** Pending → Active → Complete / Aborted. Cancel (Pending, leader) = return scroll. Abort (Active) = no rewards.
- **SpellService formulas:** `DR(bonus, max, halfway)` = `max * bonus / (bonus + halfway)`. `CalcBonus` = `(taskValue<0?1:taskValue+1) + stat * 0.5 * critMult`. `smash` crit uses CON; `fireball`/`pickPocket` PER; `backStab` STR.
- **Achievement system:** `IAchievementService` has 7 `CheckXxxAsync`. Each loads filtered badges, checks threshold, calls `AwardBatchAsync` (queries earned set, inserts new `UserBadge` rows, fires notification), calls `SaveChangesAsync`.
- **Badge.Key format:** `"{triggerType_lc}_{threshold}"` e.g. `streak_7`, `tasks_100`, `perfect_30`, `ultimate_warrior`.
- **`CheckUltimateGearAsync`:** checks all `GearItems WHERE GearClass == userClass` exist in `UserGearItems`. Non-costume only.
- **`CheckStableAsync` triad bingo:** splits PetKey on first `'-'` to get animal. Checks intersection of petAnimals ∩ mountAnimals.
- **`CheckQuestAsync`:** counts `PartyQuestMembers WHERE UserId==X AND Response=="accepted" AND Status=="Complete"`.
- **Effective stats:** `base + gearBonus + classBonus(×0.5 if gearClass==userClass) + floor(level/2) + buff`. MaxMana = `INT*2 + 30`.
- **XP model:** cumulative. `CalculateLevel` iterates thresholds, max 100 loops.
- **CharacterClass:** top-level static class in `HabitTracker.Constants` — NOT nested under `AppConstants`.
- **Mage → "wizard" in images:** GearItem.Key uses `wizard`, GearClass stores `"mage"`.
- **GetEffectiveStatsAsync:** caller MUST load `user.OwnedGear.ThenInclude(ug => ug.GearItem)` first.
- **GetStats() JSON:** `intel` not `int`. Includes `gems`.
- **Guild/Party invite:** `int targetUserId` not username. Search endpoints return partial-match users excluding existing members.
- **Navbar Social dropdown:** `isSocialActive` covers Friend/Message/Guild/Party/Account+Leaderboard. `msg-badge` (toggle) + `msg-badge-dd` (dropdown item) both in `refreshHud()`.
- **GEM_GOLD_COST:** 100 GP. In `Constants/AppConstants.cs`.
- **Market Inn tab:** Potion + sleep. Posts to `/Economy/BuyPotion` + `/Economy/ToggleSleep`.
- **Drop cap reset:** BEFORE `DailyDropCount < DAILY_DROP_CAP` check.
- **DayStart cron logic:** `var today = now.Hour < user.DayStart ? now.Date.AddDays(-1) : now.Date` — uses `DateTime.Now` (local), not `DateTime.Today`.
- **PMPermission:** `"everyone"` (default) or `"nobody"`. Checked in `MessageService.SendAsync` after block check.
- **ProfileVisibility:** `"public"` (default) or `"private"`. Gated in `FriendController.ViewProfile` → returns `ProfilePrivate` view.
- **ApiToken:** 64-char hex string (2× `Guid.NewGuid().ToString("N")`). Generated lazily on first `GET /Settings/GetApiToken`.
- **ChangeCredentialRequest:** DTO class at bottom of `AccountController.cs` namespace block (not inside class).
- **Settings CSRF:** `@Html.AntiForgeryToken()` at bottom of `Views/Settings/Index.cshtml`. JS reads via `document.querySelector('[name=__RequestVerificationToken]')?.value`.
- **SuppressNotifications:** stored in DB, but notification creation in `AchievementService`/`NotificationService` not yet gated by it — deferred.
- **IsMuted:** chat blocked globally (PM, guild chat, party chat). Checked in `MessageService.SendAsync`, `GuildService.SendMessageAsync`, `PartyService.SendMessageAsync`.
- **IsBanned:** login blocked in `AccountController.Login`. Also blocks chat same as mute.
- **AdminBlocklistEntry:** `Type` = "email"|"ip", `Value` = lowercased. No automatic enforcement yet — admin reference list only (not wired into registration/login checks).
- **Admin user search:** max 200 results. Searches `Username.Contains` + `Email.Contains` (case-insensitive via EF). Exact ID match when query is numeric.
- **Admin panel link:** shown in user dropdown only when `Session["IsAdmin"] == "true"`.

---

## Connection String

Stored in .NET User Secrets (never committed):
```
dotnet user-secrets list --project HabitTracker
```
