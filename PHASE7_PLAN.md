# Phase 7 — Guilds & Parties Implementation Plan

## Context

Phase 6 (Social/PM/Block/Report) is complete. Phase 7 adds group social features: Guilds (public/private communities) and Parties (small invite-only groups). This is a direct dependency for Phase 8 (Boss Quests) and Phase 9 (Challenges).

Habitica source reference: `D:\Download\habitica-develop` — uses single `Group` model with `type` discriminator in MongoDB. We use **separate SQL tables** (Guild, Party) for cleaner relational design.

---

## Scope (from ROADMAP.md Phase 7)

| Sub-phase | Feature |
|-----------|---------|
| 7.1 | Guilds — create, join public, invite private, leave, chat, admin roles |
| 7.2 | Parties — create, invite-only, join, leave, chat, member stats |
| 7.3 | @mention — detect `@username` in chat, link to profile, notify user |
| 7.4 | Guild/Party Challenges | → **DEFERRED to Phase 9** |

---

## Architecture Decisions

**Separate Guild/Party tables** (not single Group table with discriminator):
- Cleaner FK constraints, no nullable columns per entity
- Party has unique constraints (1 party per user) that differ from guilds
- Easier EF Core navigation properties

**Chat messages** → separate `GuildMessage` / `PartyMessage` tables:
- Different display (broadcast, not sender/receiver)
- Soft-delete by author or admin

**Invites** → separate `GuildInvite` / `PartyInvite` tables with `Status` string:
- Mirrors Friendship pattern from Phase 6

---

## New Files

### Models (10 files)
| File | Purpose |
|------|---------|
| `Models/Guild.cs` | Guild entity |
| `Models/GuildMember.cs` | Member junction (role) |
| `Models/GuildMessage.cs` | Chat message in guild |
| `Models/GuildMessageLike.cs` | Like on guild chat msg |
| `Models/GuildInvite.cs` | Invite tracking |
| `Models/Party.cs` | Party entity |
| `Models/PartyMember.cs` | Member junction |
| `Models/PartyMessage.cs` | Chat message in party |
| `Models/PartyMessageLike.cs` | Like on party chat msg |
| `Models/PartyInvite.cs` | Party invite tracking |

### ViewModels (3 files)
| File | Purpose |
|------|---------|
| `Models/ViewModels/GuildViewModel.cs` | GuildView page VM |
| `Models/ViewModels/GuildIndexViewModel.cs` | Discovery + my guilds |
| `Models/ViewModels/PartyViewModel.cs` | Party page VM |

### Services (4 files)
| File | Purpose |
|------|---------|
| `Services/IGuildService.cs` | Interface |
| `Services/Implementations/GuildService.cs` | Implementation |
| `Services/IPartyService.cs` | Interface |
| `Services/Implementations/PartyService.cs` | Implementation |

### Controllers (2 files)
| File | Purpose |
|------|---------|
| `Controllers/GuildController.cs` | Guild endpoints |
| `Controllers/PartyController.cs` | Party endpoints |

### Views (4 files)
| File | Purpose |
|------|---------|
| `Views/Guild/Index.cshtml` | Discovery + "My Guilds" tab |
| `Views/Guild/View.cshtml` | Single guild — chat + members |
| `Views/Guild/Create.cshtml` | Create guild form |
| `Views/Party/Index.cshtml` | Party dashboard — chat + members |

---

## Data Models (Exact Schema)

### Guild.cs
```
Id, Name (max 100, unique index), Description (max 500),
Summary (max 200), Logo?, Privacy ("public"/"private"),
LeaderId (FK→User Restrict), CreatedAt, UpdatedAt?
Nav: Leader?, Members, Invites, Messages
```

### GuildMember.cs
```
Id, GuildId (FK→Guild Cascade), UserId (FK→User Restrict),
Role ("Leader"/"Manager"/"Member"), JoinedAt
Unique index: (GuildId, UserId)
```

### GuildMessage.cs
```
Id, GuildId (FK→Guild Cascade), AuthorId (FK→User Restrict),
Body (max 2000), SentAt, IsDeleted (bool default false)
Index: (GuildId, SentAt)
Nav: Author?, Likes
```

### GuildMessageLike.cs
```
Composite PK: (GuildMessageId, LikerUserId)
GuildMessageId (FK→GuildMessage Cascade)
LikerUserId (FK→User Restrict), LikedAt
```

### GuildInvite.cs
```
Id, GuildId (FK→Guild Cascade), InviterId (FK→User Restrict),
InviteeId (FK→User Restrict), Status ("Pending"/"Accepted"/"Declined"),
CreatedAt
Index: (GuildId, InviteeId)
```

### Party.cs
```
Id, Name (max 100), LeaderId (FK→User Restrict), CreatedAt
Nav: Leader?, Members, Messages, Invites
```

### PartyMember.cs
```
Id, PartyId (FK→Party Cascade), UserId (FK→User Restrict),
Role ("Leader"/"Member"), JoinedAt
Unique index: (PartyId, UserId)
```

### PartyMessage.cs
```
Id, PartyId (FK→Party Cascade), AuthorId (FK→User Restrict),
Body (max 2000), SentAt, IsDeleted (bool default false)
Index: (PartyId, SentAt)
Nav: Author?, Likes
```

### PartyMessageLike.cs
```
Composite PK: (PartyMessageId, LikerUserId)
PartyMessageId (FK→PartyMessage Cascade)
LikerUserId (FK→User Restrict), LikedAt
```

### PartyInvite.cs
```
Id, PartyId (FK→Party Cascade), InviterId (FK→User Restrict),
InviteeId (FK→User Restrict), Status ("Pending"/"Accepted"/"Declined"),
CreatedAt
Index: (PartyId, InviteeId)
```

---

## Service Interfaces

### IGuildService.cs
```csharp
Task<(bool Success, string? Error, Guild? Guild)> CreateAsync(int leaderId, string name, string? description, string? summary, string privacy);
Task<List<Guild>> GetPublicGuildsAsync(string? search = null);
Task<List<Guild>> GetMyGuildsAsync(int userId);
Task<Guild?> GetGuildAsync(int guildId, int? viewerId = null);
Task<(bool Success, string? Error)> JoinPublicAsync(int userId, int guildId);
Task<(bool Success, string? Error)> LeaveAsync(int userId, int guildId);
Task<(bool Success, string? Error)> InviteAsync(int inviterId, int guildId, string username);
Task<(bool Success, string? Error)> AcceptInviteAsync(int userId, int inviteId);
Task<(bool Success, string? Error)> DeclineInviteAsync(int userId, int inviteId);
Task<(bool Success, string? Error)> KickMemberAsync(int actorId, int guildId, int memberId);
Task<(bool Success, string? Error)> PromoteManagerAsync(int leaderId, int guildId, int memberId);
Task<(bool Success, string? Error)> DemoteManagerAsync(int leaderId, int guildId, int memberId);
Task<(bool Success, string? Error, GuildMessage? Msg)> SendMessageAsync(int userId, int guildId, string body);
Task<List<GuildMessageEntry>> GetMessagesAsync(int guildId, int userId, int page = 0);
Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId);
Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId);
Task<List<GuildInvite>> GetPendingInvitesAsync(int userId);
Task<bool> IsMemberAsync(int userId, int guildId);
Task<string?> GetRoleAsync(int userId, int guildId);
```

### IPartyService.cs
```csharp
Task<(bool Success, string? Error, Party? Party)> CreateAsync(int leaderId, string name);
Task<Party?> GetPartyAsync(int partyId, int? viewerId = null);
Task<Party?> GetMyPartyAsync(int userId);
Task<(bool Success, string? Error)> InviteAsync(int inviterId, string username);
Task<(bool Success, string? Error)> AcceptInviteAsync(int userId, int inviteId);
Task<(bool Success, string? Error)> DeclineInviteAsync(int userId, int inviteId);
Task<(bool Success, string? Error)> LeaveAsync(int userId);
Task<(bool Success, string? Error)> KickMemberAsync(int leaderId, int memberId);
Task<(bool Success, string? Error, PartyMessage? Msg)> SendMessageAsync(int userId, string body);
Task<List<PartyMessageEntry>> GetMessagesAsync(int partyId, int userId, int page = 0);
Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId);
Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId);
Task<List<PartyInvite>> GetPendingInvitesAsync(int userId);
```

---

## Controller Endpoints

### GuildController
| Method | Route | Action |
|--------|-------|--------|
| GET | `/Guild` | Index — discover + my guilds |
| GET | `/Guild/View/{id}` | Guild page with chat |
| GET | `/Guild/Create` | Create form |
| POST | `/Guild/Create` | Create submit |
| POST | `/Guild/Join/{id}` | Join public guild |
| POST | `/Guild/Leave/{id}` | Leave guild |
| POST | `/Guild/Invite/{id}` | Send invite by username |
| POST | `/Guild/AcceptInvite/{inviteId}` | Accept invite |
| POST | `/Guild/DeclineInvite/{inviteId}` | Decline invite |
| POST | `/Guild/Kick/{guildId}/{memberId}` | Kick member |
| POST | `/Guild/Promote/{guildId}/{memberId}` | Promote to manager |
| POST | `/Guild/Demote/{guildId}/{memberId}` | Demote from manager |
| POST | `/Guild/SendMessage/{id}` | AJAX — send chat msg |
| GET | `/Guild/Messages/{id}` | AJAX — get messages (pagination) |
| POST | `/Guild/LikeMessage/{id}` | AJAX — toggle like |
| POST | `/Guild/DeleteMessage/{id}` | AJAX — delete message |

### PartyController
| Method | Route | Action |
|--------|-------|--------|
| GET | `/Party` | My party page |
| POST | `/Party/Create` | Create party |
| POST | `/Party/Invite` | Send invite by username |
| POST | `/Party/AcceptInvite/{inviteId}` | Accept invite |
| POST | `/Party/DeclineInvite/{inviteId}` | Decline invite |
| POST | `/Party/Leave` | Leave party |
| POST | `/Party/Kick/{memberId}` | Kick member |
| POST | `/Party/SendMessage` | AJAX — send chat msg |
| GET | `/Party/Messages` | AJAX — get messages (pagination) |
| POST | `/Party/LikeMessage/{id}` | AJAX — toggle like |
| POST | `/Party/DeleteMessage/{id}` | AJAX — delete message |

---

## @mention (Phase 7.3)

After chat message saved, scan `body` with regex `@(\w+)`. For each matched username:
1. Look up user in DB
2. `_notifications.CreateNotificationAsync(mentionedUserId, "@mention in [GuildName]", preview, "Social", link, "@")`
3. Server-side render: replace `@username` → `<a href="/Friend/ViewProfile/{id}">@username</a>`

Private helper `ProcessMentionsAsync(string body, string groupName, string groupLink)` in both services.

---

## Constraints

- Party: 1 user can only be in 1 party at a time (enforced in `CreateAsync` + `AcceptInviteAsync`)
- Guild membership: no cap
- Party invites: only party leader can invite
- Guild invites: Leader or Manager can invite
- Kick: Leader or Manager can kick Members; Leader only can kick Managers
- Promote/Demote: Leader only
- Delete own message: author can soft-delete; Leader/Manager can delete any message
- Message pagination: PAGE_SIZE = 20, oldest-first

---

## Implementation Order

1. Models (10 files)
2. AppDbContext (DbSets + OnModelCreating Phase 7 block)
3. Migration: `AddPhase7GuildsParties`
4. ViewModels (3 files)
5. IGuildService + GuildService
6. IPartyService + PartyService
7. Program.cs registrations
8. GuildController
9. PartyController
10. Views (4 files)
11. _Layout.cshtml nav links
12. dotnet build verify

---

## Test Checklist

1. Create guild (public) → appears in Discover tab
2. Create guild (private) → NOT in Discover, invite required
3. Join public guild → member list updates
4. Invite to private guild → invite appears in invitee's pending list
5. Accept invite → joins guild, invite removed
6. Decline invite → invite removed, not a member
7. Send guild chat message → appears in thread
8. Like guild message → count increments; unlike → decrements
9. @mention in guild chat → mentioned user gets notification
10. Kick member (as leader) → removed from guild
11. Promote to manager → role badge changes
12. Leave guild → no longer in "My Guilds"
13. Create party → party page shows
14. Invite to party by username → invite appears
15. Accept party invite → member appears in party member list
16. 1-user-1-party constraint: user already in party cannot join another
17. Send party chat → appears in thread
18. @mention in party chat → notification sent
19. Party leader kicks member → removed
20. Leave party → party page shows "no party" state
