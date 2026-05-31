using HabitTracker.Models;
using HabitTracker.Models.ViewModels;

namespace HabitTracker.Services;

public interface IPartyService
{
    Task<(bool Success, string? Error, Party? Party)> CreateAsync(int leaderId, string name);
    Task<PartyViewModel?> GetPartyViewAsync(int partyId, int userId, int page = 0);
    Task<Party?> GetMyPartyAsync(int userId);
    Task<(bool Success, string? Error)> InviteAsync(int inviterId, int targetUserId);
    Task<(bool Success, string? Error)> AcceptInviteAsync(int userId, int inviteId);
    Task<(bool Success, string? Error)> DeclineInviteAsync(int userId, int inviteId);
    Task<(bool Success, string? Error)> LeaveAsync(int userId);
    Task<(bool Success, string? Error)> KickMemberAsync(int leaderId, int memberId);
    Task<(bool Success, string? Error, PartyMessage? Msg)> SendMessageAsync(int userId, string body);
    Task<List<PartyMessageEntry>> GetMessagesAsync(int partyId, int userId, int page = 0);
    Task<(bool Liked, int Count)> ToggleLikeAsync(int userId, int messageId);
    Task<(bool Success, string? Error)> DeleteMessageAsync(int userId, int messageId);
    Task<List<PartyInvite>> GetPendingInvitesAsync(int userId);
    Task<string> RenderBodyAsync(string body);
}
