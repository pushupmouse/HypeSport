using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HypeSport.Models
{
    [Table("Cart")]
    public class Cart
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}