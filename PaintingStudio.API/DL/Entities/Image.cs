using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [MaxLength(2000)]
        public string Instructions { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}