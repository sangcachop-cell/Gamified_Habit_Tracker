# Phase 6 — Social Features Implementation Plan

## Context

Building Phase 6 (Social Features) on top of the existing friend system. The friend system (add/accept/reject/unfriend, profile view) is already complete. The notification infrastructure (service + API) exists but has no UI. Phase 6 adds private messaging, safety tools (block/report), profile enhancements, and notification bell UI.

**What already exists (do not duplicate):**
- `Friendship` model + `FriendController` — full friend request flow
- `Notification` model + `NotificationService` — API-only, no bell/dropdown UI
- `SearchService.SearchUsersAsync()` — user lookup by username/email
- `Views/Friend/ViewProfile.cshtml` — public profile (basic: avatar, bio, XP, streak, badges)
- `_Layout.cshtml` — HUD (HP/mana/gold/gems), toast system, `refreshHud()` fetches `/Economy/GetStats`

**Habitica source reference:**
- `website/server/models/message.js` — inbox schema
- `website/common/script/ops/blockUser.js` — toggle-based block
- `website/server/libs/chatReporting/` — flag + report flow
- `website/server/controllers/api-v3/members.js` — public profile fields
- `website/server/libs/highlightMentions.js` — @mention regex

---

## Design Decisions

| Decision | Choice | Reason |
|----------|--------|--------|
| Message storage | Single row with `DeletedBySender`/`DeletedByReceiver` | Simpler than Habitica two-copy |
| PM access control | Any user can PM any user (unless blocked) | ROADMAP says "by username or from profile" |
| Block storage | `UserBlocks` table (BlockerId, BlockedId) toggle | Habitica pattern |
| Likes | `MessageLike` join table (MessageId, LikerUserId) | Supports toggle unlike |
| Reports | Single `Reports` table, `ReportedMessageId?` nullable | Handles both user + message reports |
| @mention | Regex replace in Razor (`@username` → profile link) | No notification needed yet (Phase 7) |
| Notification bell | JS polls `/api/notification/unread-count` on load | Existing API, no SignalR needed |
| Unread badge | Separate `/Message/UnreadCount` JSON endpoint | Keep separate from notification count |

---

## New DB Entities

### `Message`
```csharp
Id (PK), SenderId (FK→User Restrict), ReceiverId (FK→User Restrict),
Body (string 2000), SentAt (DateTime), IsRead (bool), ReadAt (DateTime?),
DeletedBySender (bool), DeletedByReceiver (bool)
```
Index: `(SenderId, ReceiverId)`, `(ReceiverId, IsRead)`

### `UserBlock`
```csharp
Id (PK), BlockerId (FK→User Restrict), BlockedId (FK→User Restrict), CreatedAt (DateTime)
```
Unique index: `(BlockerId, BlockedId)`

### `Report`
```csharp
Id (PK), ReporterId (FK→User Restrict), ReportedUserId (FK→User Restrict),
ReportedMessageId (int? FK→Message SetNull), Reason (string 500),
CreatedAt (DateTime), IsResolved (bool), ResolvedAt (DateTime?), ResolvedByAdminId (int?)
```

### `MessageLike`
```csharp
MessageId (FK→Message Cascade), LikerUserId (FK→User Restrict)
Composite PK: (MessageId, LikerUserId)
```

**Migration name:** `AddPhase6Social`

---

## New Files

| File | Purpose |
|------|---------|
| `Models/Message.cs` | Entity |
| `Models/UserBlock.cs` | Entity |
| `Models/Report.cs` | Entity |
| `Models/MessageLike.cs` | Entity |
| `Models/ViewModels/InboxViewModel.cs` | Conversation list + unread counts |
| `Models/ViewModels/ConversationViewModel.cs` | Full thread between two users |
| `Services/IMessageService.cs` | Interface |
| `Services/Implementations/MessageService.cs` | All message + block + report logic |
| `Controllers/MessageController.cs` | GET inbox, GET conversation, POST send/delete/like/block/report |
| `Views/Message/Index.cshtml` | Inbox — left sidebar + right thread panel |
| `Views/Admin/Reports.cshtml` | Admin report queue |

---

## Files Modified

| File | What changes |
|------|-------------|
| `Data/AppDbContext.cs` | Add DbSets + relationships + indexes |
| `Program.cs` | Register `IMessageService` as Scoped |
| `Views/Shared/_Layout.cshtml` | Message badge + notification bell + dropdown |
| `Views/Friend/ViewProfile.cshtml` | Class icon, gear, pet/mount, completeness bar, Block/Report buttons |
| `Controllers/AdminController.cs` | `Reports()` GET + `ResolveReport(id)` POST |

---

## IMessageService Interface

```csharp
Task<(bool Success, string? Error, Message? Msg)> SendAsync(int senderId, int receiverId, string body);
Task<List<ConversationSummary>> GetConversationsAsync(int userId);
Task<List<Message>> GetConversationAsync(int userId, int otherId, int page = 0);
Task MarkReadAsync(int userId, int otherId);
Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId);
Task<int> GetUnreadCountAsync(int userId);
Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId);
Task<bool> IsBlockedAsync(int userA, int userB);
Task<bool> ToggleBlockAsync(int blockerId, int blockedId);
Task<(bool Success, string? Error)> ReportUserAsync(int reporterId, int reportedUserId, string reason);
Task<(bool Success, string? Error)> ReportMessageAsync(int reporterId, int messageId, string reason);
```

---

## MessageController Endpoints

| Method | Route | Returns |
|--------|-------|---------|
| GET | `/Message` | View(InboxViewModel) |
| GET | `/Message/Conversation/{userId}` | View(ConversationViewModel) |
| GET | `/Message/UnreadCount` | JSON `{count}` |
| POST | `/Message/Send` | JSON `{success, error, message}` |
| POST | `/Message/Delete/{id}` | JSON `{success, error}` |
| POST | `/Message/Like/{id}` | JSON `{success, liked, count}` |
| POST | `/Message/Block/{userId}` | JSON `{success, blocked}` |
| POST | `/Message/Report` | JSON `{success, error}` |

---

## Verification Checklist

1. `/Message` loads inbox with conversation list
2. Send PM → appears in both users' inboxes
3. Unread dot clears on open; badge in nav updates
4. Delete message → disappears from own view only
5. Like → ❤️ count increments; unlike → decrements
6. Block → PM returns error in both directions
7. Report user → record in Admin/Reports
8. Report message → record with ReportedMessageId set
9. Admin resolves report → removed from active queue
10. ViewProfile: class icon, gear thumbnails, active pet/mount, completeness bar
11. Notification bell: unread count badge; dropdown lists recent; click marks read
12. PM triggers notification for receiver linking to conversation
