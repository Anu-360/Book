﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Book_Coding_Challenge.Constant;

namespace Book_Coding_Challenge.Models
{
    public class User
    {
        [Required]
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(55)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserMail { get; set; }

        [Required]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain Uppercase, alphanumeric and special characters")]

        public string Password { get; set; }

        [Required]
        public Types.UserType User_Type { get; set; }

       

    }
}
