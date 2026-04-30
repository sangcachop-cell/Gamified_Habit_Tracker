using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequesterId { get; set; }   // người gửi

        [Required]
        public int ReceiverId { get; set; }    // người nhận

        // "Pending" | "Accepted" | "Rejected"
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public User? Requester { get; set; }
        public User? Receiver { get; set; }
    }
}