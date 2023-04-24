using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HypeSport.Models
{
    [Table("Category")]
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(30)]
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public List<Product>? Products { get; set; }
    }
}
