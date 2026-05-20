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
| ID | Name | Size | Rarity | Intended Effect |
|----|------|------|--------|-----------------|
| `bread` | Bread | 1×1 | Common | +30 HP during battle |

### Consumable ⚠️ defined, effect NOT implemented
| ID | Name | Size | Rarity | Intended Effect |
|----|------|------|--------|-----------------|
| `health_vial` | Health Vial | 1×2 | Uncommon | +80 HP in battle |
| `stamina_draught` | Stamina Draught | 1×2 | Uncommon | +50 Stamina on forest map |
| `mana_flask` | Mana Flask | 1×3 | Rare | +15% XP gain for next quest |

### Utility ⚠️ defined, effect NOT implemented
| ID | Name | Size | Rarity | Intended Effect |
|----|------|------|--------|-----------------|
| `water_bottle` | Water Bottle | 2×1 | Common | Flee from battle (disabled) |
| `rope_coil` | Rope Coil | 2×2 | Uncommon | TBD — utility/trap system |
| `enchanted_lantern` | Enchanted Lantern | 2×2 | Rare | Reveal hidden paths on map |
| `ancient_tome` | Ancient Tome | 2×3 | Rare | TBD — knowledge system |
| `abyssal_tome` | Abyssal Tome | 2×4 | Mythic | TBD |

### Weapon ⚠️ defined, equip system NOT implemented
| ID | Name | Size | Rarity | Intended Effect |
|----|------|------|--------|-----------------|
| `iron_dagger` | Iron Dagger | 1×3 | Uncommon | +5 ATK |
| `hunter_quiver` | Hunter's Quiver | 1×3 | Rare | +10 ATK ranged |
| `silver_sword` | Silver Sword | 1×4 | Rare | TBD — bonus vs cursed |
| `enchanted_blade` | Enchanted Blade | 1×4 | Epic | 20% stun chance |
| `elder_staff` | Elder Staff | 1×5 | Legendary | TBD — spellcasting amplifier |

### Armor ⚠️ defined, equip system NOT implemented
| ID | Name | Size | Rarity | Intended Effect |
|----|------|------|--------|-----------------|
| `leather_cap` | Leather Cap | 2×2 | Uncommon | TBD — minor damage reduction |
| `shadow_cloak` | Shadow Cloak | 3×2 | Rare | TBD — 20% dodge |
| `chain_mail` | Chain Mail | 2×3 | Rare | 15% damage reduction |
| `plate_cuirass` | Plate Cuirass | 3×3 | Epic | 30% damage reduction |
| `wardens_shield` | Warden's Shield | 3×3 | Legendary | 45% damage reduction |

### Accessory ⚠️ defined, equip system NOT implemented
| ID | Name | Size | Rarity | Intended Effect |
|----|------|------|--------|-----------------|
| `beast_fang` | Beast Fang | 1×1 | Uncommon | TBD — alchemical reagent |
| `runic_pendant` | Runic Pendant | 1×1 | Epic | 2× XP next forest run |
| `phoenix_feather` | Phoenix Feather | 1×3 | Legendary | Revive once at 50% HP |
| `heart_of_the_forest` | Heart of the Forest | 2×2 | Mythic | TBD — forest control |
| `crown_of_echoes` | Crown of Echoes | 3×2 | Mythic | TBD — all stats +10 |

### Equipment (grid containers) ⚠️ scout_bag/tactical_webbing/infinity_satchel NOT implemented
| ID | Name | Size | Rarity | Grid Unlocked |
|----|------|------|--------|---------------|
| `simple_backpack` | Simple Backpack | 2×2 | Uncommon | 4×4 ✅ |
| `simple_rig` | Simple Rig | 2×1 | Uncommon | 4×2 (1×2 slots only) ✅ |
| `scout_bag` | Scout Bag | 3×2 | Uncommon | 5×5 (placeholder) |
| `tactical_webbing` | Tactical Webbing | 3×2 | Epic | 6×3 (placeholder) |
| `infinity_satchel` | Infinity Satchel | 3×3 | Legendary | 8×8 (placeholder) |

### Material ✅ wood/stone drop + craft; rest ⚠️ defined only
| ID | Name | Size | Rarity | Notes |
|----|------|------|--------|-------|
| `wood` | Wood | 2×1 | Common | Forest drop; craft → +10 User.Wood ✅ |
| `stone` | Stone | 1×1 | Common | Forest drop; craft → +10 User.Stone ✅ |
| `herb_bundle` | Herb Bundle | 2×1 | Common | TBD — brewing |
| `flint` | Flint | 1×1 | Common | TBD — crafting |
| `wolf_pelt` | Wolf Pelt | 2×2 | Common | TBD — armor crafting |
| `leather_strip` | Leather Strip | 2×1 | Common | TBD — crafting |
| `iron_ore` | Iron Ore | 1×1 | Common | TBD — smelt at workbench |
| `cave_mushroom` | Cave Mushroom | 1×1 | Common | TBD — alchemy |
| `tattered_cloth` | Tattered Cloth | 2×1 | Common | TBD — crafting |
| `crystal_shard` | Crystal Shard | 1×2 | Uncommon | TBD — enchanting |
| `dragon_scale` | Dragon Scale | 2×2 | Epic | TBD — high-tier armor craft |
| `void_crystal` | Void Crystal | 1×2 | Epic | TBD — Mythic crafting reagent |

---

## Monsters

Stats formula: base constants + `lvl × growth`. Player level is passed in at combat init.
`Speed = min(cap, base + √lvl × factor)`. Boss tiers hit hard enough to one-shot at low level.

### Spawn table (which monster appears where)
| Location | Monster pool |
|----------|-------------|
| Open world | forest_scout 45%, skeleton_archer 25%, swamp_toad 9%, forest_brute 20%, ancient_warden 0.2% |
| Cave (interior) | iron_golem 55%, shadow_stalker 38%, bone_colossus 7% |
| Warehouse (interior) | iron_golem 55%, shadow_stalker 39%, corrupted_treant 6% |
| Lake (interior) | lake_serpent 52%, shadow_stalker 40%, void_walker 8% |

### Monster roster

| ID | Name | Icon | Tier | Base HP (lv1→lv20) | Base ATK | Base ARM | Speed cap | Unique Mythic drop |
|----|------|------|------|--------------------|----------|----------|-----------|-------------------|
| `forest_scout` | Forest Scout | 🐺 | common | 78→230 | 7→30 | 0→6 | 100 | `wolf_spirit_gem` |
| `skeleton_archer` | Skeleton Archer | 💀 | common | 62→165 | 11→45 | 0→2 | 100 | `ancient_bowstring` |
| `swamp_toad` | Swamp Toad | 🐸 | uncommon | 136→440 | 6→23 | 0→24 | 40 | `eye_of_the_bog` |
| `forest_brute` | Forest Brute | 👹 | rare | 148→690 | 19→74 | 3→22 | 70 | `brute_war_mask` |
| `corrupted_treant` | Corrupted Treant | 🌳 | rare | 198→620 | 21→78 | 10→48 | 35 | `crown_of_echoes` |
| `iron_golem` | Iron Golem | ⚙️ | elite | 228→760 | 14→56 | 21→88 | 25 | `soul_of_iron` |
| `shadow_stalker` | Shadow Stalker | 👁️ | elite | 90→280 | 27→90 | 0→10 | 100 | `shadow_veil` |
| `lake_serpent` | Lake Serpent | 🐍 | rare | 160→540 | 19→76 | 6→33 | 75 | `serpent_sovereign_scale` |
| `bone_colossus` | Bone Colossus | 🦴 | boss | 365→1220 | 41→155 | 15→72 | 55 | `colossus_skull` |
| `void_walker` | Void Walker | 🌀 | boss | 309→1080 | 35→135 | 10→58 | 100 | `void_heart` |
| `ancient_warden` | Ancient Warden | 🌲 | raid_boss | 970→3300 | 82→310 | 31→145 | 75 | `heart_of_the_forest` (ONLY source) |

### Per-monster loot tables

| ID | No Drop | Common | Uncommon | Rare | Epic | Legendary | Mythic |
|----|---------|--------|----------|------|------|-----------|--------|
| forest_scout | 15% | 50% | 20% | 10% | 3.5% | 1.45% | 0.05% |
| skeleton_archer | 12% | 42% | 25% | 13% | 5% | 2.92% | 0.08% |
| swamp_toad | 10% | 33% | 28% | 16% | 8% | 4.85% | 0.15% |
| forest_brute | 10% | 32% | 25% | 18% | 10% | 4.75% | 0.25% |
| corrupted_treant | 8% | 25% | 22% | 22% | 14% | 8.8% | 0.2% |
| iron_golem | 7% | 20% | 18% | 22% | 20% | 12.5% | 0.5% |
| shadow_stalker | 8% | 25% | 22% | 20% | 15% | 9.7% | 0.3% |
| lake_serpent | 9% | 30% | 25% | 19% | 11% | 5.85% | 0.15% |
| bone_colossus | 10% | 14% | 16% | 22% | 22% | 15% | 1% |
| void_walker | 14% | 8% | 13% | 18% | 24% | 21% | 2% |
| ancient_warden | 2% | 4% | 7% | 14% | 24% | 35% | 14% |

### Notable exclusive drops
- `wolf_spirit_gem` — Forest Scout only
- `bone_shard` — Skeleton Archer (common pool)
- `toxic_gland` — Swamp Toad (uncommon pool)
- `corrupted_bark` — Corrupted Treant (common/uncommon pool)
- `iron_core` — Iron Golem (uncommon/epic pool)
- `shadow_essence` — Shadow Stalker (uncommon pool)
- `serpent_scale` — Lake Serpent (common/uncommon pool)
- `ancient_bone` — Bone Colossus (uncommon pool)
- `void_tendril` — Void Walker (uncommon/epic pool)
- `warden_heartwood` — Ancient Warden only (all rarities)
- `heart_of_the_forest` — **Ancient Warden only** — no other source

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
| ID | Name (VN) | Asset | Stat | Buff | Max Level |
|----|-----------|-------|------|------|-----------|
| 1 | Sân Tập Luyện | 🏋️ | ATK | +5 ATK/level | 5 |
| 2 | Thiền Đường | 🧘 | HP | +20 HP/level | 5 |
| 3 | Thư Viện | 📚 | XPGain | +2% XP/level | 5 |
| 4 | Đường Chướng Ngại | 🏃 | Stamina | +10 Stamina/level | 5 |
| 5 | Doanh Trại | 🛡️ | Armor | +5 Armor/level | 5 |
| 6 | Phòng Kho | 📦 | Storage | +30 slots/level (10×3 rows) | 5 |
| 7 | Bàn Thợ | 🔨 | Crafting | +1 craft slot/level | 5 |

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
| 1 | Tập thể dục | 🏋️ | Sức khỏe | Daily | 10/25/100 | Sân Tập Luyện | QTE circle | STR+2/3/4, AGL+2, END+1/1/2 |
| 2 | Chạy bộ | 🏃 | Sức khỏe | Daily | 10/25/100 | Đường Chướng Ngại | Dino runner (15s/25s/40s) | STR+2/3/4, AGL+2/2/4, END+1/1/4 |
| 3 | Đọc sách | 📖 | Học tập | Daily | 10/25/100 | Thư Viện | Tetris (3/8/15 lines) | INT+2/3/4, AGL+1/1/2, END+1/1/2 |

Stat formula: category base + difficulty bonus to primary stat + Hard→+1 END + Daily→+1 END + always +1 AGL

---

## Summary

| Type | Total | Working | Placeholder |
|------|-------|---------|-------------|
| Items | 42 | 3 equip + 2 material (craft) | 37 (defined, no logic yet) |
| Monsters | 2 | 2 | 0 |
| Bosses | 0 | — | — |
| Map Zones | 3 | 3 | 0 |
| Facilities | 7 | 6 (stat buff) + 1 (workbench) | 0 |
| Quest Cards | 3 | 3 | 0 |
| Craft Recipes | 2 | 2 (instant) | 0 (timer pending) |
| Loot Tiers | 2 | Common only | Uncommon–Mythic pools empty |

### Item Count by Rarity
| Rarity | Count |
|--------|-------|
| Common | 9 (bread, water_bottle, wood, stone, herb_bundle, flint, wolf_pelt, leather_strip, iron_ore, cave_mushroom, tattered_cloth) |
| Uncommon | 8 (simple_backpack, simple_armor, simple_rig, health_vial, stamina_draught, iron_dagger, leather_cap, scout_bag, beast_fang, rope_coil, crystal_shard) |
| Rare | 7 (mana_flask, silver_sword, chain_mail, enchanted_lantern, hunter_quiver, shadow_cloak, ancient_tome) |
| Epic | 6 (dragon_scale, runic_pendant, enchanted_blade, void_crystal, plate_cuirass, tactical_webbing) |
| Legendary | 4 (phoenix_feather, elder_staff, wardens_shield, infinity_satchel) |
| Mythic | 3 (heart_of_the_forest, abyssal_tome, crown_of_echoes) |
