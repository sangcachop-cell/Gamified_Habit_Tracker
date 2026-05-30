namespace HabitTracker.Models;

public class PartyQuestMember
{
    public int Id { get; set; }
    public int PartyQuestId { get; set; }
    public int UserId { get; set; }

    // null=pending, "accepted", "rejected"
    public string? Response { get; set; }

    public double TotalDamageDealt { get; set; } = 0;   // cumulative boss damage dealt by this member
    public double TotalRageGiven   { get; set; } = 0;   // cumulative rage contributed via missed dailies

    public virtual PartyQuest? PartyQuest { get; set; }
    public virtual User? User { get; set; }
}
