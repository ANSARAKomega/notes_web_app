using System.ComponentModel.DataAnnotations;

namespace NotesWebApp.Models
{
    public class Notes
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsFavorite { get; set; } = false;

        public int UserId { get; set; }
        public Users? User { get; set; }

    }
}
