# Phase 8 — Boss Quests (Implementation Log)

## Status: COMPLETE ✅

All core components implemented and migration applied.

---

## What was built

### Models (new)
- `Models/BossQuest.cs` — quest catalog entity (115 rows seeded)
- `Models/PartyQuest.cs` — active quest state per party
- `Models/PartyQuestMember.cs` — per-member RSVP + damage/rage stats
- `Models/ViewModels/QuestShopViewModel.cs` — shop + `PartyQuestStatusDto` + `QuestMemberRsvp`

### Constants
- `ItemType.QuestScroll = 3` added to enum in `AppConstants.cs`
- `GameItem.ImagePath` updated to handle QuestScroll type
- 115 `GameItem` seed rows (IDs 182–296) for quest scrolls

### Database
- Migration: `AddPhase8BossQuests`
- Tables: `BossQuests`, `PartyQuests`, `PartyQuestMembers`
- `BossQuest.Key` unique index; all FK constraints set

### Seed data (`Data/Seeds/BossQuestSeed.cs`)
115 quests across 7 categories:
- 60 pet boss quests
- 6 potion boss quests
- 6 potion collection quests
- 7 seasonal quests
- 15 series quests (atom, goldenknight, moon, moonstone, vice)
- 16 masterclasser quests (4 series × 4 parts)
- 3 time travel quests (canBuy=false, goldCost=0)
- 2 generic quests (basilist, dustbunnies)

**Excluded:** 5 world boss quests (bewilder, burnout, dilatory, dysheartener, stressbeast)

### Service layer
- `Services/IBossQuestService.cs` — full interface
- `Services/Implementations/BossQuestService.cs` — shop, lifecycle, combat, view data

### Integration
- `TaskService.ScoreTaskAsync` → calls `ApplyTaskDamageAsync` on task completion
- `TaskService.RunCronAsync` → calls `ApplyMissedDailyRageAsync` for missed dailies
- `Program.cs` → `AddScoped<IBossQuestService, BossQuestService>()`

### Controllers
- `Controllers/QuestShopController.cs` — GET /QuestShop + POST /QuestShop/Buy
- `Controllers/PartyQuestController.cs` — Invite/Accept/Reject/ForceStart/Cancel/Abort/Status

### Views
- `Views/QuestShop/Index.cshtml` — tabbed grid with boss art, scroll thumbnails, level/prereq locks, buy buttons
- `Views/Party/Index.cshtml` — quest panel added to left sidebar (3 states: none/pending/active)
- `Models/ViewModels/PartyViewModel.cs` — added `QuestStatus` + `OwnedScrolls`

---

## Damage formulas

```
// Task completion → boss damage
damage = rawDelta * critMult * (1.0 + user.STR / 200)   // todos + dailies
damage = rawDelta * critMult * (0.5 + user.STR / 400)   // habits
damage /= BossDef  (default 1.0)

// Missed daily → rage
rage = |cronDelta| * PriorityMultiplier

// rawDelta = Math.Pow(0.9747, task.Value)
```

## Rage effects
- `RageHealing`: boss heals `BossHp * fraction`
- `RageMpDrain`: all accepted members lose `Mana * fraction`
- `RageProgressDrain`: boss "regains" `alreadyDealt * fraction` HP
- Rage meter resets to 0 after trigger

## Quest lifecycle
`Pending` → all accept/force-start → `Active` → boss dies/collection complete → `Complete`
Leader can `Cancel` (Pending, returns scroll) or `Abort` (Active, no rewards)
