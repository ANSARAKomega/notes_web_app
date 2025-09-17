using System.ComponentModel.DataAnnotations;

namespace NotesWebApp.Models
{
    public class Users
    {
        [Key]

        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Password { get; set; } = string.Empty;

    }
}
