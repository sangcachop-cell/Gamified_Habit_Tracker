using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class GuildMessage
{
    public int Id { get; set; }
    public int GuildId { get; set; }
    public int AuthorId { get; set; }

    [Required, MaxLength(2000)]
    public string Body { get; set; } = string.Empty;

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public virtual Guild? Guild { get; set; }
    public virtual User? Author { get; set; }
    public virtual ICollection<GuildMessageLike> Likes { get; set; } = new List<GuildMessageLike>();
}
