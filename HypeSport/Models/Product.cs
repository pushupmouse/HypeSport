using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HypeSport.Models
{
    [Table("Product")]
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [MaxLength(50)]
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public bool ProductStatus { get; set; }
        public string? ProductImage { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
        public List<CartDetail>? CartDetails { get; set; }

        [NotMapped]
        public string? CategoryName { get; set; }
    }
}
