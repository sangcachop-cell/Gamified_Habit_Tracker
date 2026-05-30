using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class PartyMessage
{
    public int Id { get; set; }
    public int PartyId { get; set; }
    public int AuthorId { get; set; }

    [Required, MaxLength(2000)]
    public string Body { get; set; } = string.Empty;

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public virtual Party? Party { get; set; }
    public virtual User? Author { get; set; }
    public virtual ICollection<PartyMessageLike> Likes { get; set; } = new List<PartyMessageLike>();
}
