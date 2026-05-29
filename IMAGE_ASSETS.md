# Habitica Image Asset Reference

## How Habitica Loads Images (Source)

Habitica's frontend uses a `<Sprite :image-name="...">` Vue component that resolves every image to:
```
https://habitica-assets.s3.amazonaws.com/mobileApp/images/{name}.png
```
GIF sprites (animated quest bosses, special gear) use `.gif` instead.

Our local copies live in:
```
wwwroot/images/habitica/
```
Use `<img src="/images/habitica/{subfolder}/{filename}">` instead of S3.

---

## Naming Conventions by Category

### Food
**Pattern:** `Pet_Food_{FoodName}.png`  
**Local path:** `/images/habitica/stable/food/Pet_Food_{FoodName}.png`  
**Examples:** `Pet_Food_Meat.png`, `Pet_Food_Chocolate.png`, `Pet_Food_Honey.png`  
**Special cases:**
- `food_Potato` → `Pet_Food_Potatoe.png` (Habitica typo — extra 'e')

### Eggs
**Pattern:** `Pet_Egg_{AnimalName}.png`  
**Local path:** `/images/habitica/stable/eggs/Pet_Egg_{AnimalName}.png`  
**Examples:** `Pet_Egg_Wolf.png`, `Pet_Egg_Dragon.png`, `Pet_Egg_Axolotl.png`  
**Special cases:**
- `egg_Bear` → `Pet_Egg_BearCub.png`

### Hatching Potions
**Pattern:** `Pet_HatchingPotion_{PotionName}.png`  
**Local path:** `/images/habitica/stable/potions/Pet_HatchingPotion_{PotionName}.png`  
**Examples:** `Pet_HatchingPotion_Base.png`, `Pet_HatchingPotion_Golden.png`, `Pet_HatchingPotion_CottonCandyBlue.png`

### Pets (collection grid icon)
**Pattern:** `Pet-{AnimalName}-{Color}.png` (dash separators)  
**Local path:** `/images/habitica/stable/pets/Pet-{AnimalName}-{Color}.png`  
**Examples:** `Pet-Wolf-Base.png`, `Pet-Dragon-Golden.png`, `Pet-BearCub-Skeleton.png`  
> Note: uses dash (`-`), not underscore (`_`) between parts.

### Mounts (avatar overlay layers)
**Head layer:** `Mount_Head_{AnimalName}-{Color}.png`  
**Body layer:** `Mount_Body_{AnimalName}-{Color}.png`  
**Local paths:**
- `/images/habitica/stable/mounts/head/Mount_Head_{AnimalName}-{Color}.png`
- `/images/habitica/stable/mounts/body/Mount_Body_{AnimalName}-{Color}.png`  
**Examples:** `Mount_Head_Wolf-Base.png`, `Mount_Body_Dragon-Golden.png`

### Gear — Shop Thumbnail (inventory/market card)
**Pattern:** `shop_{key}.png`  
**Local path:** `/images/habitica/gear/{slot}/shop/shop_{key}.png`  
**Examples:**
- `shop_weapon_warrior_1.png` → `/images/habitica/gear/weapon/shop/shop_weapon_warrior_1.png`
- `shop_armor_healer_2.png` → `/images/habitica/gear/armor/shop/shop_armor_healer_2.png`

Habitica source uses: `:image-name="'shop_' + item.key"` (profileStats.vue, equipment/index.vue)

### Gear — Worn on Avatar
**Armor pattern:** `broad_{key}.png` or `slim_{key}.png` (body shape variants)  
**Other slots:** `{key}.png`  
**Local path:** `/images/habitica/gear/{slot}/{filename}`  
**Examples:**
- `broad_armor_healer_1.png` → `/images/habitica/gear/armor/broad_armor_healer_1.png`
- `weapon_warrior_1.png` → `/images/habitica/gear/weapon/weapon_warrior_1.png`

> Mage weapon keys use `wizard` not `mage`: `weapon_wizard_1.png` not `weapon_mage_1.png`

### Skills / Spells
**Pattern:** `shop_{spellKey}.png`  
**Local path:** `/images/habitica/skills/shop_{spellKey}.png`  
**Examples:** `shop_fireball.png`, `shop_heal.png`, `shop_backStab.png`, `shop_valorousPresence.png`

Habitica source uses: `:image-name="'shop_' + spell.key"` (spells.vue)

### Backgrounds
**Full size (avatar background, 330×189px):** `background_{key}.png`  
**Thumbnail icon (60×60):** `icon_background_{key}.png`  
**Local path:** `/images/habitica/backgrounds/background_{key}.png`  
**Example:** `background_beach.png`, `background_autumn_forest.png`

Habitica source uses CSS class `background_{key}` for full-size and `:image-name="'icon_background_' + bg.key"` for thumbnails.
We have 396 background PNGs locally.

### Shop Items (generic)
**Local path:** `/images/habitica/shop/shop_{key}.png`  
**Examples:**
- `shop_gem.png` — gem currency icon
- `shop_potion.png` — health potion
- `shop_armoire.png` — armoire chest

### NPC Characters
**Pattern:** `npc_{name}.png` or `npc_{name}_{season}.png`  
**Local path:** `/images/habitica/npc/npc_{name}.png`  
**Examples:** `npc_bailey.png`, `npc_bailey_spring.png`, `npc_bailey_winter.png`

### Achievements
**Pattern:** `achievement-{name}.png` (1x) or `achievement-{name}2x.png` (2x)  
**Local path:** `/images/habitica/achievements/achievement-{name}.png`  
**Examples:** `achievement-armor2x.png`, `achievement-alien2x.png`

### Misc
**Local path:** `/images/habitica/misc/`  
**Key files:**
- `Pet_Currency_Gem.png` — gem icon (HUD)
- `Pet_Currency_Gem1x.png`, `Pet_Currency_Gem2x.png` — size variants
- `PixelPaw-Gold.png` — gold coin icon

### Notifications
**Local path:** `/images/habitica/notifications/notif_{key}.png`

---

## Quick Reference Table

| Category | Local path pattern | Key variable |
|---|---|---|
| Food | `/images/habitica/stable/food/Pet_Food_{Name}.png` | strip `food_` prefix |
| Egg | `/images/habitica/stable/eggs/Pet_Egg_{Name}.png` | strip `egg_` prefix |
| Potion | `/images/habitica/stable/potions/Pet_HatchingPotion_{Name}.png` | strip `potion_` prefix |
| Pet grid | `/images/habitica/stable/pets/Pet-{Animal}-{Color}.png` | dash separators |
| Mount head | `/images/habitica/stable/mounts/head/Mount_Head_{Animal}-{Color}.png` | |
| Mount body | `/images/habitica/stable/mounts/body/Mount_Body_{Animal}-{Color}.png` | |
| Gear shop icon | `/images/habitica/gear/{slot}/shop/shop_{key}.png` | full gear key |
| Gear worn | `/images/habitica/gear/{slot}/{key}.png` | full gear key |
| Skill/spell | `/images/habitica/skills/shop_{spellKey}.png` | |
| Background | `/images/habitica/backgrounds/background_{key}.png` | |
| Bg thumbnail | `/images/habitica/backgrounds/icon_background_{key}.png` | |
| Shop item | `/images/habitica/shop/shop_{key}.png` | |
| NPC | `/images/habitica/npc/npc_{name}.png` | |
| Achievement | `/images/habitica/achievements/achievement-{name}2x.png` | |
| Misc (gem) | `/images/habitica/misc/Pet_Currency_Gem.png` | |

---

## Special Naming Gotchas

| Issue | Wrong | Correct |
|---|---|---|
| Potato food | `Pet_Food_Potato.png` | `Pet_Food_Potatoe.png` |
| Bear egg | `Pet_Egg_Bear.png` | `Pet_Egg_BearCub.png` |
| Mage gear key | `weapon_mage_1` | `weapon_wizard_1` |
| Pet separator | `Pet_Wolf_Base.png` | `Pet-Wolf-Base.png` (dash) |
| Mount separator | `Mount-Head-Wolf-Base.png` | `Mount_Head_Wolf-Base.png` (underscore then dash) |

---

## Habitica Source Reference

- **Sprite component:** `website/client/src/components/ui/sprite.vue` — renders `<img>` from S3
- **Food item:** `components/inventory/stable/foodItem.vue` — `:image-name="'Pet_Food_' + item.key"`
- **Gear equipment:** `components/inventory/equipment/index.vue` — `:image-name="'shop_' + item.key"`
- **Spells:** `components/tasks/spells.vue` — `:image-name="'shop_' + spell.key"`
- **Backgrounds:** `components/avatar.vue` — CSS class `background_{key}`
- **Gear content structure:** `common/script/content/gear/index.js` — flat key = `{type}_{class}_{tier}`
