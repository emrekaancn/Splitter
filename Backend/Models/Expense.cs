using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Split.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        // Swagger / POST sırasında zorunlu olmasın diye
        [JsonIgnore]
        public User? User { get; set; }
    }
}