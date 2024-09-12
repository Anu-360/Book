using System.ComponentModel.DataAnnotations;

namespace Book_Coding_Challenge.Models
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string UserMail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
