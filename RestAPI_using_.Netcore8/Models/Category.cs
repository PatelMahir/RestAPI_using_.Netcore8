using System.ComponentModel.DataAnnotations;

namespace RestAPI_using_.Netcore8.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
