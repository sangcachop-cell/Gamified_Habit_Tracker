# Gamified Habit Tracker — Build Roadmap

Generated: 2026-05-27  
Based on: codebase audit vs FEATURES_CHECKLIST.md  
Inspiration: Habitica source at `D:\Download\habitica-develop`

---

## Current State (Audit Summary)

| Category | Status | What Exists |
|---|---|---|
| Tasks | PARTIAL | "Quest" CRUD + XP rewards — NOT proper Habitica task types |
| Character/Avatar | PARTIAL | Level, XP, streak, avatar, bio. No classes/stats/equipment/HP/mana |
| Game Mechanics | MISSING | Nothing |
| Inventory & Shop | MISSING | Nothing |
| Pets & Mounts | MISSING | Nothing |
| Social | PARTIAL | Friends list only. No messages, block, flag, mentions, likes |
| Guilds & Parties | MISSING | Nothing |
| Boss Quests | MISSING | Nothing (different from habit tasks) |
| Challenges | MISSING | Nothing |
| Achievements | PARTIAL | 3 XP-based badges, no dynamic trigger logic |
| Notifications | PARTIAL | API exists (CRUD), zero UI |
| Auth | PARTIAL | Register/login done. Google half-wired. No delete/username-change |
| Settings | MINIMAL | Profile edit only |
| Admin | PARTIAL | Quest CRUD + user role toggle + basic stats |
| Payments | MISSING | Nothing (by design: free sub + gold-for-gems) |

**Overall: ~25-30% of planned features implemented. Core skeleton only.**

---

## Dependency Map

```
Task System (Habits/Dailies/To-Dos/Rewards)
    └── Game Economy (Gold, HP, Mana, Cron)
            ├── Character Stats + Classes
            │       ├── Equipment System
            │       ├── Class Spells
            │       └── Buffs
            ├── Shop (Market, Quest Shop, Gem Shop, Armoire)
            ├── Item Drop System
            └── Health Potions / Revive / Sleep
                    └── Pets & Mounts
                            └── Beast Master achievements

Social (Messages, Block, Flag)
    └── Guilds & Parties
            └── Boss Quests
                    └── Challenges

Achievements (expanded) ← depends on: tasks + character + social + quests
Notifications UI ← depends on: achievements + quests + social triggers
```

---

## Phase 1 — Task System Overhaul (Foundation)
**~2-3 weeks | Blocks everything else**

The current "Quest" table is a simplified task model. Must migrate to proper Habitica task types before adding game mechanics — the scoring formulas, cron logic, and gold/XP rewards all differ per type.

### 1.1 Task Types
- [ ] **Habits** — positive (+), negative (-), or both buttons. Counter tracking. No due date. Score goes up/down.
- [ ] **Dailies** — repeat on schedule, streak tracking, cron resets each day. Damage HP if missed.
- [ ] **To-Dos** — one-time tasks, optional due date, clear completed list.
- [ ] **Rewards** — spend gold to claim. No XP given.

> Habitica ref: `website/common/script/ops/score.js`, `website/common/script/cron.js`

### 1.2 Task Properties (all types)
- [ ] Difficulty levels: Trivial / Easy / Medium / Hard (affects XP + gold scaling)
- [ ] Task notes (text field)
- [ ] Task checklists (sub-items with individual completion)
- [ ] Task tags (create tag, apply to task, filter by tag)
- [ ] Task reordering (drag-and-drop or manual sort order)
- [ ] Task reminders (time-based, stored per task)

### 1.3 Dailies-Specific
- [ ] Repeat frequency: daily / weekly (pick days) / monthly / yearly
- [ ] Streak counter (increment on completion, reset on miss)
- [ ] Daily cron job (runs at user's custom day start time, resets dailies, triggers damage)

### 1.4 To-Do-Specific
- [ ] Due dates with overdue visual indicator
- [ ] "Clear completed To-Dos" bulk action

### 1.5 Habit-Specific
- [ ] Counter value display (+N / -N)
- [ ] Positive-only / negative-only / both toggle per habit

---

## Phase 2 — Game Economy
**~2 weeks | Depends on: Phase 1**

Core currency and survival systems. Without this, shops/items/classes are meaningless.

### 2.1 Currency
- [ ] Gold (primary currency — earned from tasks, spent in shops)
- [ ] Silver (fractional gold, auto-convert at 100 silver = 1 gold)
- [ ] Display in header/HUD

### 2.2 Health System
- [ ] HP bar (max HP scales with CON stat, default 50)
- [ ] HP damage from missed Dailies (scales with difficulty + pending damage formula)
- [ ] HP loss on negative Habits
- [ ] Death / revive (drop 1 level, restore HP to 1, lose some gold)
- [ ] Health potions (buy for gold, restore 15 HP, max HP cap)

### 2.3 Mana System
- [ ] MP bar (max mana scales with INT stat + level)
- [ ] Mana gain from task completion
- [ ] Mana used by class spells (Phase 3)
- [ ] Mana regeneration (daily cron tops up some mana)

### 2.4 Passive Mechanics
- [ ] Critical hit system (random bonus XP/gold on task completion, scales with PER stat)
- [ ] Item drop system (random egg / potion / food drops on task completion)
- [ ] Sleep / Inn mode (pause HP damage from missed Dailies — user toggle)

---

## Phase 3 — Character System
**~3 weeks | Depends on: Phase 2**

### 3.1 Stats
- [ ] STR (Strength) — boosts task XP + melee quest damage
- [ ] CON (Constitution) — boosts max HP + reduces daily damage
- [ ] INT (Intelligence) — boosts mana + magic quest damage
- [ ] PER (Perception) — boosts gold earned + crit chance + item drop rate
- [ ] Stat display on profile/character sheet

### 3.2 Level & Stat Points
- [ ] Manual stat point allocation (1 point per level up, allocate to any stat)
- [ ] Auto-allocate option per stat (settings toggle)
- [ ] Level up animation / notification

### 3.3 Class System
- [ ] 4 classes: **Warrior** (STR/CON), **Mage** (INT/PER), **Rogue** (PER/STR), **Healer** (CON/INT)
- [ ] Unlock at level 10
- [ ] Class selection screen
- [ ] Class bonuses (passive stat multipliers per class)
- [ ] Class change (costs gems, resets stat points)
- [ ] Class spells (2-4 spells per class, spend mana, cast on self/party/boss)

> Habitica ref: `website/common/script/ops/castSpell.js`, class definitions in `website/common/script/content/spells.js`

### 3.4 Equipment
- [ ] Gear items (weapon, armor, helmet, shield, accessory)
- [ ] Each gear has stat bonuses
- [ ] Equip / unequip
- [ ] Battle Gear vs Costume mode (toggle — costume shows cosmetic gear, stats come from battle gear)
- [ ] Auto-equip best gear toggle (equips highest-stat gear automatically)
- [ ] Ultimate Gear achievements (collect full set per class)

### 3.5 Buffs
- [ ] Temporary stat bonuses (from class spells, Perfect Day bonus)
- [ ] Display active buffs with duration
- [ ] Perfect Day buff (all Dailies completed = next-day stat bonus)

### 3.6 Rebirth
- [ ] Orb of Rebirth item (costs gems/gold)
- [ ] Resets: level, XP, stats, equipment progression
- [ ] Keeps: achievements, pets/mounts, cosmetics

### 3.7 Avatar Customization
- [ ] Skin color selection
- [ ] Hair style + color
- [ ] Body type
- [ ] Facial hair options
- [ ] Flower / wheelchair / animal ears cosmetics
- [ ] Customization preview before saving

---

## Phase 4 — Inventory & Shop
**~2 weeks | Depends on: Phase 3**

### 4.1 Market
- [ ] Buy gear with Gold
- [ ] Items organized by class + slot
- [ ] Item stats preview before purchase

### 4.2 Quest Shop
- [ ] Buy quest scrolls with Gold
- [ ] Quest unlocks tied to user level

### 4.3 Gem Shop (modified from Habitica)
- [ ] Buy Gems with Gold (not real money — per checklist design decision)
- [ ] Exchange rate: e.g., 25 Gold = 1 Gem
- [ ] Gems used for: cosmetics, rebirth, class change, special items

### 4.4 Seasonal Shop
- [ ] Time-limited items (holiday events)
- [ ] Purchases with Gold or Gems

### 4.5 Enchanted Armoire
- [ ] Single purchase button (costs Gold)
- [ ] Random reward: gear / food / experience / gold
- [ ] Equal probability pool per season

### 4.6 General Inventory
- [ ] View owned items (eggs, potions, food, gear, quest scrolls)
- [ ] Sell items back for partial gold
- [ ] Pinned items (quick-access bar for favorites)
- [ ] Unlock cosmetics (display which are owned vs locked)

---

## Phase 5 — Pets & Mounts
**~2 weeks | Depends on: Phase 4 (item drops)**

### 5.1 Collection
- [ ] Eggs (Wolf, TigerCub, Fox, FlyingPig, Dragon, Cactus, etc.)
- [ ] Hatching Potions (Base, White, Desert, Red, Shade, Skeleton, Zombie, etc.)
- [ ] Food items (Meat, Milk, Potatoe, etc.)

### 5.2 Hatching & Feeding
- [ ] Hatch pet: select egg + compatible potion → new pet created
- [ ] Feed pet: give food (preferred food gives 5 pts, others 2 pts)
- [ ] Pet evolves to mount at 50 feeding points

### 5.3 Equipping
- [ ] Set active pet (shown on character)
- [ ] Set active mount (character rides it)
- [ ] Visual display on avatar

### 5.4 Collections UI
- [ ] Pet collection grid (owned / unowned / evolved)
- [ ] Mount collection grid
- [ ] Release pet (remove from collection, get 1 food back)
- [ ] Release mount

### 5.5 Achievements
- [ ] Beast Master: own all pets
- [ ] Mount Master: own all mounts
- [ ] Triad Bingo: collect 3 types of same species

---

## Phase 6 — Social Features
**~2 weeks | Depends on: Phase 1-3 (user profiles need full data)**

### 6.1 Private Messages
- [ ] Inbox view (sent + received)
- [ ] Send message to user (by username or from profile)
- [ ] Delete message (sender or receiver can delete their copy)
- [ ] Message notification badge

### 6.2 Safety Features
- [ ] Block user (no messages, hides their chat)
- [ ] Unblock user
- [ ] Flag / report user (sends report to admin queue)
- [ ] Flag / report chat message

### 6.3 Profile Enhancements
- [ ] Full profile view: stats, class, gear, active pet/mount, achievements, badges
- [ ] Profile completeness indicator
- [ ] @mention in chat (links to profile, sends notification)

### 6.4 Chat Interactions
- [ ] Like chat message (heart/+1)
- [ ] Chat message flagging (mark for admin review)

---

## Phase 7 — Guilds & Parties
**~3 weeks | Depends on: Phase 6**

### 7.1 Party (1 per user)
- [ ] Create party (name, description)
- [ ] Invite members by username / email / UUID
- [ ] Accept / reject invitation
- [ ] Leave party
- [ ] Remove member (leader only)
- [ ] Assign / remove co-leader
- [ ] Party chat (group message thread)
- [ ] View party member stats + task progress

### 7.2 Guilds (multiple per user)
- [ ] Create guild (name, description, summary, category)
- [ ] Privacy: public / private
- [ ] Join public guild
- [ ] Invite to private guild
- [ ] Leave guild
- [ ] Remove member (leader / manager)
- [ ] Assign / remove managers
- [ ] Guild chat
- [ ] Guild discovery / search (browse public guilds by category)
- [ ] Looking for Party feature (flag your party as open)
- [ ] Group plans: skip (marked unchecked in checklist)

---

## Phase 8 — Boss Quests
**~2 weeks | Depends on: Phase 7 (needs party)**

This is different from "tasks" — these are party RPG-style events.

### 8.1 Quest Flow
- [ ] Quest scrolls (from shop or item drops) owned by one party member
- [ ] Invite party to quest (leader starts it)
- [ ] Members accept / reject invitation
- [ ] Force-start (leader starts even if not all accepted)
- [ ] Cancel / abort quest mid-progress

### 8.2 Boss Quests
- [ ] Boss HP bar (party chips away with task completions)
- [ ] Boss rage system (missed Dailies fills rage bar → boss attacks party)
- [ ] Damage formula: task completion → party damage based on STR stat
- [ ] Boss defeat → reward distribution

### 8.3 Collection Quests
- [ ] Each task completion has chance to drop quest-specific items
- [ ] Progress bar shows collected / needed
- [ ] Completion → reward distribution

### 8.4 Quest Types
- [ ] Boss quests
- [ ] Collection quests
- [ ] Pet unlock quests (completing gives specific egg/potion)
- [ ] Mount unlock quests

---

## Phase 9 — Challenges
**~1-2 weeks | Depends on: Phase 7**

### 9.1 Core
- [ ] Create challenge with name, description, prize (gems), and task templates
- [ ] Challenges belong to a guild or are public
- [ ] Join challenge → task templates cloned to your task list
- [ ] Leave challenge → clone tasks optionally kept or deleted

### 9.2 Management
- [ ] View challenge member list + their progress on challenge tasks
- [ ] Select winner → award gems to winner
- [ ] Clone challenge (copy to new challenge)
- [ ] Export challenge data (CSV of member completion rates)
- [ ] Flag / report challenge

---

## Phase 10 — Achievements Expansion
**~1-2 weeks | Depends on: Phases 1-8**

### 10.1 Achievement Types
- [ ] Streak achievements (7-day, 21-day, 90-day, 180-day, 365-day streak)
- [ ] Perfect Day (all Dailies done — gives buff next day)
- [ ] Ultimate Gear achievements (full class gear set per class × 4 classes)
- [ ] Party achievements (quest completions, challenge wins)
- [ ] Pet / mount collection milestones
- [ ] Login streak achievements
- [ ] Task completion milestones (10, 50, 100, 500 tasks done)

### 10.2 Trigger System
- [ ] Hook achievement checks into: task completion, cron, level up, gear equip, quest complete
- [ ] Push notification to notification table on unlock
- [ ] Unlock animation / toast display

---

## Phase 11 — Notification UI
**~1 week | Depends on: Phase 10 (needs real triggers)**

API already exists. Need frontend.

- [ ] Notification bell icon in nav with unread count badge
- [ ] Dropdown panel showing recent notifications
- [ ] Mark single as read / mark all as read
- [ ] Notification types with icons: Badge, Achievement, Streak, Quest, Social, System
- [ ] News / announcements feed (admin creates, all users see)

---

## Phase 12 — Settings & Auth Polish
**~1 week**

### 12.1 Auth Gaps
- [ ] Username change (settings page)
- [ ] Email change (requires password confirm)
- [ ] Delete account (requires password confirm + "DELETE" type confirmation)
- [ ] Social login: Google (half-done, needs client ID + secret in config)
- [ ] Link social account to existing local account

### 12.2 Settings
- [ ] Custom day start time (cron offset: 0-23h, default midnight)
- [ ] Site preferences: sticky header, reverse chat order, suppress all notifications, etc.
- [ ] Privacy settings: who can PM you, who can see your profile
- [ ] API token display (show user their API key, allow regeneration)
- [ ] Data export (download account data as JSON/CSV)
- [ ] Reset account (keep user, wipe tasks/progress — debug tool)

---

## Phase 13 — Admin Expansion
**~1 week**

- [ ] User search by username / email / UUID
- [ ] View user history (task completions, level history, purchases)
- [ ] IP / email blocklist management (ban emails, flag IPs)
- [ ] Chat privilege revocation (mute user from chat globally)
- [ ] Group management (admin view of all guilds/parties)

---

## Deferred / Out of Scope

These were unchecked in FEATURES_CHECKLIST.md — skip for now:

- Group plans (shared subscriptions)
- Push notifications (mobile)
- PayPal / Stripe / Apple IAP / Google Play IAP (free sub by design)
- Cancel / gift subscription (free sub by design)
- Social login: Facebook, Apple
- Webhook integrations (3rd party triggers)
- Public REST API
- i18n / localization
- World state / global events system
- API status endpoint
- Language selection
- Webhook management

---

## Build Order Summary

```
Phase 1  → Task System Overhaul       (Habits/Dailies/To-Dos/Rewards)
Phase 2  → Game Economy               (Gold, HP, Mana, Cron)
Phase 3  → Character System           (Stats, Classes, Equipment, Avatar)
Phase 4  → Inventory & Shop           (Market, Gem Shop, Armoire)
Phase 5  → Pets & Mounts              (Eggs, Potions, Hatch, Feed, Evolve)
Phase 6  → Social Features            (PM, Block, Flag, Mentions, Likes)
Phase 7  → Guilds & Parties           (Create, Invite, Chat, Discovery)
Phase 8  → Boss Quests                (Boss HP, Rage, Collection, Rewards)
Phase 9  → Challenges                 (Create, Join, Progress, Winner)
Phase 10 → Achievements Expansion     (Streak, Perfect Day, Gear, Pet/Mount)
Phase 11 → Notification UI            (Bell, Panel, News Feed)
Phase 12 → Settings & Auth Polish     (Day start, Privacy, Delete account)
Phase 13 → Admin Expansion            (Search, History, Blocklist)
```

**Estimated total: ~25-30 weeks at 1 phase/week pace.**  
Phases 1-3 are the highest leverage — they unlock nearly everything else.
