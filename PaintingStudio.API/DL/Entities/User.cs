using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace DL.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "Client"; // Admin, Manager, Client

        [MaxLength(10)]
        public string AccessCode { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    }
}