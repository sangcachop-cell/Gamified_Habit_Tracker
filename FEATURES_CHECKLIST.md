# Gamified Habit Tracker — Feature Checklist

Check off features you want to build. Based on Habitica source code audit.

---

## TASKS
- [x] Create / Edit / Delete tasks (Habits, Dailies, To-Dos, Rewards)
- [x] Task scoring (complete / uncomplete)
- [x] Task checklists (sub-items)
- [x] Task tags (create, apply, remove)
- [x] Task difficulty levels (Trivial, Easy, Medium, Hard)
- [x] Task notes
- [x] Task due dates (To-Dos)
- [x] Task scheduling (daily repeat — frequency, weekly, monthly, yearly)
- [x] Task reminders
- [x] Task reordering
- [x] Clear completed To-Dos
- [x] Daily cron (reset dailies at custom time)
- [x] Streak tracking on Dailies
- [x] Counter tracking on Habits (+ / - buttons)
- [x] Task approval (request / approve for group tasks)

---

## CHARACTER / AVATAR
- [x] Avatar appearance customization (skin, hair, body, facial hair)
- [x] Display name, photo, bio/blurb
- [x] Equipment system (equip / unequip gear)
- [x] Battle Gear vs Costume mode
- [x] Character stats (STR, CON, PER, INT)
- [x] Manual stat point allocation
- [x] Class system (Warrior, Rogue, Mage, Healer)
- [x] Class change
- [x] Level up
- [x] Experience points (XP)
- [x] Health / Mana system
- [x] Buffs (temporary stat bonuses)
- [x] Auto-equip best gear setting
- [x] Rebirth (Orb of Rebirth — reset level, keep achievements)

---

## GAME MECHANICS
- [x] Critical hit system (random bonus rewards)
- [x] Item drop system (random items from tasks)
- [x] Gold / Silver currency
- [x] Health potions (buy to restore HP)
- [x] Sleep / Inn (pause damage from missed dailies)
- [x] Revive from death (lose a level, restore HP)
- [x] Mana regeneration
- [x] Class spells / skills (cast on self, party, or boss)

---

## INVENTORY & SHOP
- [x] Market (buy gear with Gold)
- [x] Quest shop (buy quest scrolls with Gold)
- [x] Gem shop (buy gems with real money) -> change to buy gem with gold
- [x] Seasonal shop
- [x] Enchanted Armoire (random gear/items for Gold)
- [x] Buy / sell items
- [x] Unlock cosmetics
- [x] Pinned items (quick-access favorites)

---

## PETS & MOUNTS
- [x] Collect eggs
- [x] Collect hatching potions
- [x] Hatch pets (egg + potion combination)
- [x] Feed pets (with preferred food)
- [x] Grow pets into mounts
- [x] Equip active pet
- [x] Equip active mount
- [x] Pet collection display
- [x] Mount collection display
- [x] Release pets / mounts
- [x] Quest pets / mounts (earned from quests)
- [x] Beast Master / Mount Master achievements

---

## SOCIAL
- [x] Private messages (send, receive, delete)
- [x] Block / unblock users
- [x] View user profiles (stats, achievements, gear)
- [x] Flag / report users
- [x] @ mention system in chat
- [x] Chat message likes
- [x] Chat message flagging

---

## GUILDS & PARTIES
- [x] Create / edit / delete party
- [x] Create / edit / delete guild
- [x] Join / leave guild or party
- [x] Invite members (by username, email, UUID)
- [x] Accept / reject invitations
- [x] Remove members
- [x] Assign / remove managers
- [x] Guild privacy settings (public / private)
- [x] Guild discovery / search
- [x] Looking for Party feature
- [x] Group chat
- [ ] Group plans (shared subscriptions)

---

## QUESTS
- [x] Invite party to quest
- [x] Accept / reject quest invitation
- [x] Force-start quest
- [x] Cancel / abort quest
- [x] Boss HP + damage system
- [x] Boss rage system
- [x] Quest item collection
- [x] Quest rewards
- [x] Multiple quest types (Boss, Collection, Pet, Mount)

---

## CHALLENGES
- [x] Create challenge (with task templates)
- [x] Join / leave challenge
- [x] View member progress
- [x] Select winner (award Gems)
- [x] Clone challenge
- [x] Export challenge data (CSV)
- [x] Flag / report challenge

---

## ACHIEVEMENTS
- [x] Achievement tracking and display
- [x] Achievement unlock notifications
- [x] Streak achievements
- [x] Perfect Day achievements
- [x] Ultimate Gear achievements (per class)
- [x] Party / challenge win achievements
- [x] Pet / mount collection achievements

---

## NOTIFICATIONS
- [x] In-app notifications
- [x] Mark as read / seen
- [x] News / announcements feed
- [ ] Push notifications (mobile)

---

## PAYMENTS & SUBSCRIPTIONS
- [x] Subscription system (monthly) -> give every user free subscription forever
- [ ] Cancel subscription 
- [ ] Gift subscription
- [x] Buy Gems (real money) -> change to using gold to buy gem
- [x] Monthly mystery items (subscribers)
- [x] Mystic Hourglasses (Time Travelers shop currency)
- [x] Send Gems to other users
- [ ] PayPal integration
- [ ] Stripe integration
- [ ] Apple IAP
- [ ] Google Play IAP

---

## AUTH
- [x] Register (username / email / password)
- [x] Login (local)
- [ ] Social login (Google, Facebook, Apple)
- [ ] Link social account to existing account
- [x] Password reset (email link)
- [x] Username change
- [x] Email change
- [x] Password change
- [x] Delete account

---

## SETTINGS
- [ ] Language selection
- [x] Custom day start time (cron offset)
- [x] Site preferences (sticky header, reverse chat, etc.)
- [ ] API token display / management
- [ ] Webhook management (create, edit, delete)
- [ ] Data export (CSV / JSON / XML)
- [ ] Fix character values (debug tool)
- [x] Reset account
- [x] Privacy settings

---

## ADMIN
- [x] Admin panel
- [x] User search (by username / email / ID)
- [x] View user history
- [ ] IP / email / client blocklist management
- [x] Contributor level management
- [x] Chat privilege revocation
- [x] Group management

---

## MISC / INTEGRATIONS
- [ ] Webhook integrations (3rd party triggers)
- [ ] API (public REST API with token auth)
- [ ] i18n / localization (multi-language support)
- [x] Mobile-responsive UI
- [x] Dark / light mode
- [ ] World state / global events system
- [ ] API status endpoint
