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
| `simple_rig` | Simple Rig | 🦺 | 2×1 | Uncommon | RigSlot | Unlocks 4×2 quick-access grid |

### Food ⚠️ defined, consume logic NOT implemented
| ID | Name | Asset | Size | Rarity | Intended Effect |
|----|------|-------|------|--------|-----------------|
| `bread` | Bread | 🍞 | 1×1 | Common | +30 HP during battle |

### Utility ⚠️ defined, effect NOT implemented
| ID | Name | Asset | Size | Rarity | Intended Effect |
|----|------|-------|------|--------|-----------------|
| `water_bottle` | Water Bottle | 🧴 | 2×1 | Common | Flee from battle (flee system disabled) |

### Material ⚠️ drops + stores, craft system pending
| ID | Name | Asset | Size | Rarity | Notes |
|----|------|-------|------|--------|-------|
| `wood` | Wood | 🪵 | 2×1 | Common | Forest loot pool (Common tier); process at Hideout (TODO) |
| `stone` | Stone | 🪨 | 1×1 | Common | Forest loot pool (Common tier); process at Hideout (TODO) |

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

### Hideout Facilities
| ID | Name | Asset | Buff |
|----|------|-------|------|
| 1 | Training Grounds | 🏋️ | +5 ATK per level |
| 2 | Meditation Hall | 🧘 | +20 HP per level |
| 3 | Archive | 📚 | +2% XP Gain per level |
| 4 | Agility Course | 🏃 | +10 Stamina per level |
| 5 | Barracks | 🛡️ | +5 Armor per level |
| 6 | Storage Room | 📦 | +3 storage rows per level |

Stat formula: category base + difficulty bonus to primary stat + Hard→+1 END + Daily→+1 END + always +1 AGL

### Sức khỏe — base: STR+2, AGL+1
| ID | Name | Asset | Frequency | XP (E/M/H) | Hideout | Minigame | STR | WILL | INT | AGL | END |
|----|------|-------|-----------|------------|---------|----------|-----|------|-----|-----|-----|
| 1 | Tập thể dục | 🏋️ | Daily | 10/25/50 | 🏋️ Training Grounds | QTE | +2/3/4 | — | — | +2 | +1/1/2 |
| 2 | Chạy bộ | 🏃 | Daily | 10/25/100 | 🏃 Agility Course | Dino (survive 15s/25s/40s) | +2/3/4 | — | — | +2/2/4 | +1/1/4 |

---

## Summary

| Type | Total | Working | Placeholder |
|------|-------|---------|-------------|
| Items | 7 | 3 | 4 |
| Monsters | 2 | 2 | 0 |
| Bosses | 0 | — | — |
| Currencies | 0 | — | — |
| Map Zones | 3 | 3 | 0 |
