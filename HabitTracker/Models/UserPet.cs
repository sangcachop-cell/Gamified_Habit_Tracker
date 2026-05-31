using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    public class UserPet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        /// <summary>"{AnimalKey}-{PotionColor}" e.g. "Wolf-Base", "BearCub-Golden"</summary>
        [Required]
        [StringLength(100)]
        public string PetKey { get; set; } = string.Empty;

        /// <summary>
        /// Feeding progress. Starts at 5 when hatched.
        /// Capped at 50 when evolved. Preferred food = +5 pts, other = +2 pts.
        /// </summary>
        public int FeedingPoints { get; set; } = 5;

        /// <summary>True once FeedingPoints reached 50 — pet became a mount.</summary>
        public bool IsMount { get; set; } = false;

        public DateTime HatchedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; } = null!;

        // ── Computed helpers (no DB columns) ──

        [NotMapped]
        public string AnimalName => PetKey.Contains('-') ? PetKey.Split('-')[0] : PetKey;

        [NotMapped]
        public string ColorName => PetKey.Contains('-') ? PetKey.Split('-')[1] : string.Empty;

        [NotMapped]
        public string PetImagePath => $"/fe/stable/pets/Pet-{PetKey}.png";

        [NotMapped]
        public string MountBodyPath => $"/fe/stable/mounts/body/Mount_Body_{PetKey}.png";

        [NotMapped]
        public string MountHeadPath => $"/fe/stable/mounts/head/Mount_Head_{PetKey}.png";

        [NotMapped]
        public string MountIconPath => $"/fe/stable/mounts/icon/Mount_Icon_{PetKey}.png";
    }
}
