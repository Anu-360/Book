using System.ComponentModel.DataAnnotations;
namespace Book_Coding_Challenge.Models
{
    public class Book
    {
        [Required]  
        [Key]
        public string ISBN {  get; set; }
        [Required]
        public string Title {  get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
      
        public int Publication_Year { get; set; }

        

      
    }
}
