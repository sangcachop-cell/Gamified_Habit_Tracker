# Plan: Skills in Task System + Party Mention Fix

**Date:** 2026-05-30  
**Status:** Pending implementation

---

## Context

Phase 8 (Boss Quests + Party) is complete. Two outstanding items:

1. **Skills UX overhaul** — Skills are currently on a separate `/Character/Spells` page with a dropdown. Goal: surface 4 skill buttons directly in the task board. Click skill → activates it → click task → spell fires. Party spells (valorousPresence, intimidate, mpheal, earth, toolsOfTrade, protectAura, healAll) were stubbed "Phase 7 pending" — now unblock them. Sync all formulas to match Habitica source exactly.

2. **Party @mention fix** — Same bug as guild chat (fixed in Phase 7 session 2 Bug Fix 6): `PartyController.SendMessage` returns `msg.Body` (raw text) instead of rendered HTML. Mentions appear as plain `@username` until page reload.

---

## Task 1 — Fix Party @mention (small)

**Root cause:** `Controllers/PartyController.cs` line ~162 returns `body = msg.Body` (raw). Guild was fixed to call `_guilds.RenderBodyAsync()`.

### Files
- `Services/IPartyService.cs` — add `Task<string> RenderBodyAsync(string body);`
- `Services/Implementations/PartyService.cs` — implement (same as GuildService lines 440-453: regex extract → DB lookup → GroupBy OrdinalIgnoreCase → `RenderMentions`)
- `Controllers/PartyController.cs` `SendMessage` — call `await _party.RenderBodyAsync(msg!.Body)` → return `body = renderedBody`

No JS changes — `appendMessage` already uses `${d.body}` as innerHTML.

---

## Task 2 — Skills in Task System

### 2A — ViewModel + Controller

**`Models/ViewModels/TaskBoardViewModel.cs`**
- Add `User User`, `EffectiveStats EffectiveStats`, `IReadOnlyList<SpellDefinition> Skills`
- Add `bool CanUseSkills => !string.IsNullOrEmpty(User?.Class) && User.Level >= 11;`

**`Controllers/TaskController.cs`**
- `Index`: load user with gear → `GetEffectiveStatsAsync` → `GetSpellsForClass` → populate new fields
- Inject `ISpellService`, `ICharacterService`
- Add `POST /Task/CastSpell` — delegates to `SpellService.CastAsync(userId, spellKey, taskId?)` — same JSON response shape as `CharacterController.CastSpell`

### 2B — Sync Spell Formulas (SpellService.cs)

**Add helpers:**
```csharp
static double DimRet(double bonus, double max, double halfway = -1)
    => max * bonus / (bonus + (halfway < 0 ? max / 2 : halfway));

static double CalcBonus(double taskValue, double stat, double critMult = 1, double statScale = 0.5)
    => (taskValue < 0 ? 1 : taskValue + 1) + stat * statScale * critMult;

double CritMult(double statVal, double chance = 0.03)
    // Random.Shared.NextDouble() <= chance * (1 + statVal/100)
    // hit → 1.5 + 4*statVal/(statVal+200)  else → 1.0
```

**Fix formulas:**

| Spell | Current | Correct |
|-------|---------|---------|
| `smash` task delta | flat | `DimRet(STR * CritMult(CON), 2.5, 35)` |
| `smash` boss damage | exists | `DimRet(STR * CritMult(CON), 55, 70)` |
| `fireball` XP | flat | `DimRet(INT * CritMult(PER), 75)` |
| `fireball` boss | missing | `INT * 0.1` added to quest progress |
| `defensiveStance` | flat | `ceil(DimRet(CON - BuffCON, 40, 200))` |
| `pickPocket` | flat | `DimRet(CalcBonus(task.Value, PER), 25, 75)` |
| `backStab` gold | flat | `DimRet(CalcBonus(task.Value, STR, CritMult(STR, 0.3)), 18, 75)` |
| `backStab` XP | flat | `DimRet(CalcBonus(task.Value, STR, CritMult(STR, 0.3)), 75, 50)` |
| `heal` | close | `(CON + INT + 5) * 0.075`, max 50 HP restored |
| `stealth` | exists | `ceil(DimRet(PER, incompleteDailies * 0.64, 55))` — load incomplete dailies |
| `brightness` | close | `4 * INT / (INT + 40)` per task ✓ keep |
| `frost` | works | keep, already-cast guard ✓ |

**Implement party spells** (load all party members, apply to each, `SaveChanges`, post `[SYS]` chat message):

| Key | Effect |
|-----|--------|
| `valorousPresence` | STR buff: `ceil(DimRet(STR - BuffSTR, 20, 200))` each member |
| `intimidate` | CON buff: `ceil(DimRet(CON - BuffCON, 24, 200))` each member |
| `mpheal` | MP restore: `ceil(DimRet(INT, 25, 125))` — skip mages |
| `earth` | INT buff: `ceil(DimRet(INT - BuffINT, 30, 200))` each member |
| `toolsOfTrade` | PER buff: `ceil(DimRet(PER - BuffPER, 100, 50))` each member |
| `protectAura` | CON buff: `ceil(DimRet(CON - BuffCON, 200, 200))` each member |
| `healAll` | HP: `(CON + INT + 5) * 0.04` per member, max 50 HP each |

Solo (no party): apply to self only.

### 2C — Skill Bar UI in Task/Index.cshtml

**Position:** Horizontal flex row above task board columns. Hidden if `!Model.CanUseSkills`.

**Skill card HTML:**
```html
<div class="skill-btn [skill-disabled]" data-key="{key}" data-targets-task="{true|false}"
     data-mana="{cost}" data-name="{name}" onclick="activateSkill(this)">
  <img src="/images/habitica/skills/shop_{key}.png" width="40" height="40"
       onerror="this.src='/images/habitica/skills/shop_special_fall2015Healer.png'" />
  <div class="skill-name small">{name}</div>
  <div class="skill-cost text-muted" style="font-size:11px;">💧{manaCost}</div>
</div>
```

**JS (in Index.cshtml scripts section):**
```javascript
let activeSkill = null;

function activateSkill(el) {
  const key = el.dataset.key;
  if (activeSkill === key) { deactivateSkill(); return; }
  document.querySelectorAll('.skill-btn').forEach(b => b.classList.remove('skill-active'));
  el.classList.add('skill-active');
  activeSkill = key;
  if (el.dataset.targetsTask !== 'true') {
    castSkill(key, null);  // self/party — cast immediately
  } else {
    document.body.classList.add('skill-targeting');
  }
}

function deactivateSkill() {
  activeSkill = null;
  document.querySelectorAll('.skill-btn').forEach(b => b.classList.remove('skill-active'));
  document.body.classList.remove('skill-targeting');
}

// Intercept task card clicks when skill is targeting
$(document).on('click', '.task-card', function(e) {
  if (!activeSkill) return;
  const targetsTask = document.querySelector(`.skill-btn[data-key="${activeSkill}"]`)
                              ?.dataset.targetsTask === 'true';
  if (!targetsTask) return;
  e.stopImmediatePropagation();
  const taskId = this.id.replace('task-', '');
  castSkill(activeSkill, taskId);
});

async function castSkill(key, taskId) {
  const fd = new FormData();
  fd.append('__RequestVerificationToken', token);
  fd.append('spellKey', key);
  if (taskId) fd.append('taskId', taskId);
  const res = await fetch('/Task/CastSpell', { method: 'POST', body: fd });
  const d = await res.json();
  deactivateSkill();
  if (d.success) {
    updateManaBar(d.newMana, d.maxMana);
    showToast('✨', d.data?.spellName ?? key, d.data?.toastMessage ?? 'Spell cast!', 'info');
    refreshHud();
    if (taskId) updateTaskCard(taskId, d.data);  // re-color card if task value changed
  } else {
    showToast('❌', 'Spell Failed', d.error ?? 'Not enough mana?', 'danger');
  }
}
```

**Mana bar** (add small mana display to task page header area, using `Model.User.Mana` / `Model.EffectiveStats`).

**CSS (site.css):**
```css
.skill-btn { cursor:pointer; border:2px solid transparent; border-radius:8px; padding:8px 12px;
             text-align:center; transition:border-color .15s,box-shadow .15s; min-width:90px; }
.skill-btn:hover:not(.skill-disabled) { border-color:#6c757d; }
.skill-btn.skill-active { border-color:#0d6efd; box-shadow:0 0 8px rgba(13,110,253,.4); background:#e7f1ff; }
.skill-btn.skill-disabled { opacity:.45; cursor:not-allowed; }
.skill-targeting .task-card { cursor:crosshair !important; outline:2px dashed #0d6efd; outline-offset:2px; }
```

**Remove** `✨ Cast Spells` link from `Views/Character/Index.cshtml` lines ~59-61.

---

## File List

| File | Change |
|------|--------|
| `Services/IPartyService.cs` | Add `RenderBodyAsync` |
| `Services/Implementations/PartyService.cs` | Implement `RenderBodyAsync` |
| `Controllers/PartyController.cs` | `SendMessage` → return `renderedBody` |
| `Models/ViewModels/TaskBoardViewModel.cs` | Add User, EffectiveStats, Skills, CanUseSkills |
| `Controllers/TaskController.cs` | Inject deps; extend Index; add CastSpell endpoint |
| `Services/Implementations/SpellService.cs` | Helpers + formula fixes + party spells |
| `Views/Task/Index.cshtml` | Skills bar HTML + JS + CSRF form |
| `Views/Character/Index.cshtml` | Remove Cast Spells button |
| `wwwroot/css/site.css` | Skill bar + targeting CSS |

**No DB migration needed** — buff fields and party system already exist.

---

## Verification

1. Party chat: `@username` → link appears immediately after send, no reload
2. Task page (class+level≥11): 4 skill cards visible above board
3. Task page (no class / level<11): skill bar hidden
4. Task-targeting skill (e.g. Smash): click card → crosshair appears on task cards → click task → toast + mana update + task color may change
5. Self spell (e.g. Heal): click card → casts immediately → HP updates in HUD
6. Party spell (e.g. Blessing): all party members get HP — verify in DB
7. Mana too low: skill card shows disabled state
8. Boss active + Smash: boss HP drops extra from skill damage (check party quest panel)
