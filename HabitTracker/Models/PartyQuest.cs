namespace HabitTracker.Models;

public class PartyQuest
{
    public int Id { get; set; }
    public int PartyId { get; set; }
    public int BossQuestId { get; set; }
    public int LeaderUserId { get; set; }               // scroll owner who started the quest

    public string Status { get; set; } = "Pending";    // "Pending"|"Active"|"Complete"|"Aborted"

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ActiveAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public double BossHpRemaining { get; set; }         // ticks down as party deals damage
    public double RageMeter { get; set; } = 0;          // ticks up from missed dailies; resets on trigger

    // Running tally for collection quests: {"itemKey": currentCount, ...}
    public string? CollectProgressJson { get; set; }

    public virtual Party? Party { get; set; }
    public virtual BossQuest? BossQuest { get; set; }
    public virtual User? Leader { get; set; }
    public virtual ICollection<PartyQuestMember> Members { get; set; } = new List<PartyQuestMember>();
}
