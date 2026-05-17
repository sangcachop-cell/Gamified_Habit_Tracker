# Game Content Registry

## Rarity Tiers

| Tier | Color | Hex |
|------|-------|-----|
| Common | White | `#ffffff` |
| Uncommon | Green | `#4caf50` |
| Rare | Cyan | `#00bcd4` |
| Epic | Purple | `#9c27b0` |
| Legendary | Orange | `#ff9800` |
| Mythic | Red | `#f44336` |

---

## Items

### Equipment ✅ fully implemented
| ID | Name | Asset | Size | Rarity | Slot | Effect |
|----|------|-------|------|--------|------|--------|
| `simple_backpack` | Simple Backpack | 🎒 | 2×2 | Uncommon | BackpackSlot | Unlocks 4×4 storage grid |
| `simple_armor` | Simple Armor | 🛡️ | 1×2 | Uncommon | ArmorSlot | 5% incoming damage reduction |
| `simple_rig` | Simple Rig | 🦺 | 2×1 | Uncommon | RigSlot | Unlocks 4 tall slots (W=1,H=2 each); only 1×2 items allowed; rotation blocked if violates constraint |

### Food ⚠️ defined, consume logic NOT implemented
| ID | Name | Asset | Size | Rarity | Intended Effect |
|----|------|-------|------|--------|-----------------|
| `bread` | Bread | 🍞 | 1×1 | Common | +30 HP during battle |

### Utility ⚠️ defined, effect NOT implemented
| ID | Name | Asset | Size | Rarity | Intended Effect |
|----|------|-------|------|--------|-----------------|
| `water_bottle` | Water Bottle | 🧴 | 2×1 | Common | Flee from battle (flee system disabled) |

### Material ✅ drops + stores; instant craft implemented; timer pending
| ID | Name | Asset | Size | Rarity | Notes |
|----|------|-------|------|--------|-------|
| `wood` | Wood | 🪵 | 2×1 | Common | Forest loot (Common tier); craft 1 raw → +10 User.Wood (instant) |
| `stone` | Stone | 🪨 | 1×1 | Common | Forest loot (Common tier); craft 1 raw → +10 User.Stone (instant) |

---

## Monsters

### Forest ✅ fully implemented (stats scale with player level)
| ID | Name | Asset | Tier | Trigger Chance | Base Stats (player lvl 1) |
|----|------|-------|------|----------------|--------------------------|
| `forest_scout` | Forest Scout | 🐺 | common | 5% open field / 25% inside zone | HP 85, ATK 6, ARM 0, SPD ~8 |
| `forest_brute` | Forest Brute | 👹 | rare | inside named zones only | HP 136, ATK 9, ARM 0, SPD ~12 |

Formula: `HP = max(20, (80 + lvl×5) × mult)`, `ATK = max(3, (5+lvl) × mult)`, `ARM = max(0, lvl×0.5 × mult)`
Mult: common = 1.0, rare = 1.6

#### Loot Chances — Forest Scout (common)
| Rarity | Drop Chance | Pool | Items |
|--------|-------------|------|-------|
| No drop | 15% | — | — |
| Common | 50% | wood, stone | 25% each |
| Uncommon | 20% | *(empty)* | — |
| Rare | 10% | *(empty)* | — |
| Epic | 3.5% | *(empty)* | — |
| Legendary | 1.3% | *(empty)* | — |
| Mythic | 0.2% | *(empty)* | — |

#### Loot Chances — Forest Brute (rare)
| Rarity | Drop Chance | Pool | Items |
|--------|-------------|------|-------|
| No drop | 17.7% | — | — |
| Common | 35% | wood, stone | 17.5% each |
| Uncommon | 25% | *(empty)* | — |
| Rare | 15% | *(empty)* | — |
| Epic | 5% | *(empty)* | — |
| Legendary | 2% | *(empty)* | — |
| Mythic | 0.3% | *(empty)* | — |

---

## Map Locations

| ID | Name | Asset | Position | Size | Event Chance |
|----|------|-------|----------|------|--------------|
| `cave` | Cave | 🦇 | (18,15) | 12×8 | 25% rare encounter |
| `warehouse` | Warehouse | 🏚️ | (54,58) | 14×10 | 25% rare encounter |
| `lake` | Lake | 🌊 | (88,22) | 16×10 | 25% rare encounter |

---

## Quests

### Hideout Facilities (seeded in DB — IDs 1–7)
| ID | Name | Asset | Stat | Buff | Max Level |
|----|------|-------|------|------|-----------|
| 1 | Training Grounds | 🏋️ | ATK | +5 ATK/level | 5 |
| 2 | Meditation Hall | 🧘 | HP | +20 HP/level | 5 |
| 3 | Archive | 📚 | XPGain | +2% XP/level | 5 |
| 4 | Agility Course | 🏃 | Stamina | +10 Stamina/level | 5 |
| 5 | Barracks | 🛡️ | Armor | +5 Armor/level | 5 |
| 6 | Storage Room | 📦 | Storage | +30 slots/level (10×3 rows) | 5 |
| 7 | Workbench | 🔨 | Crafting | +1 craft slot/level | 5 |

Upgrade cost: Wood + Stone (amounts TBD — upgrade endpoint is placeholder).

### Workbench Recipes ✅ instant craft (timer defined in catalogue but NOT yet wired)
| Recipe ID | Name | Input | Output | Craft Time (future) | Min Level |
|-----------|------|-------|--------|---------------------|-----------|
| `wood_to_material` | Process Wood | 1× raw Wood | +10 User.Wood | 2 hrs | 1 |
| `stone_to_material` | Process Stone | 1× raw Stone | +10 User.Stone | 2 hrs | 1 |

Slots available: `1 + workbench_level` (Lv1=2, Lv2=3, …)

### Quest Cards (seeded — Minigame cooldown currently DISABLED for testing)
| ID | Name | Asset | Category | Frequency | XP (E/M/H) | Facility | Minigame | Stat Gains |
|----|------|-------|----------|-----------|------------|----------|----------|------------|
| 1 | Tập thể dục | 🏋️ | Sức khỏe | Daily | 10/25/50 | Training Grounds | QTE circle | STR+2/3/4, AGL+2, END+1/1/2 |
| 2 | Chạy bộ | 🏃 | Sức khỏe | Daily | 10/25/100 | Agility Course | Dino runner (15s/25s/40s) | STR+2/3/4, AGL+2/2/4, END+1/1/4 |

Stat formula: category base + difficulty bonus to primary stat + Hard→+1 END + Daily→+1 END + always +1 AGL

---

## Summary

| Type | Total | Working | Placeholder |
|------|-------|---------|-------------|
| Items | 7 | 3 equip + 2 material (craft) | 2 (bread, water_bottle) |
| Monsters | 2 | 2 | 0 |
| Bosses | 0 | — | — |
| Map Zones | 3 | 3 | 0 |
| Facilities | 7 | 6 (stat buff) + 1 (workbench) | 0 |
| Quest Cards | 2 | 2 | 0 |
| Craft Recipes | 2 | 2 (instant) | 0 (timer pending) |
| Loot Tiers | 2 | Common only | Uncommon–Mythic pools empty |
