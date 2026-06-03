using HabitTracker.Models;
using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services;

public interface IGuildService
{
    Task<(bool Success, string? Error, Guild? Guild)> CreateAsync(int leaderId, string name, string? description, string? summary, string privacy);
    Task<List<Guild>> GetPublicGuildsAsync(string? search = null);
    Task<List<GuildCardModel>> GetMyGuildsAsync(int userId);
    Task<GuildViewModel?> GetGuildViewAsync(int guildId, int userId, int page = 0);
    Task<(bool Success, string? Error)> JoinPublicAsync(int userId, int guildId);
    Task<(bool Success, string? Error)> LeaveAsync(int userId, int guildId);
    Task<(bool Success, string? Error)> InviteAsync(int inviterId, int guildId, int targetUserId);
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
    Task<string> RenderBodyAsync(string body, int guildId);
}
