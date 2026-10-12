using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApplication.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(10, 10000, ErrorMessage = "Value for {0} must be between {1} and {2} ")]
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public string? imagePath { get; set; }

        [NotMapped]
        public IFormFile? clientFile { get; set; }

        public Category? Category { get; set; }

        [Range (0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; } = 0;

        [NotMapped]
        public string StockStatus
        {
            get
            {
                if (Quantity == 0)
                {
                    return "Out of Stock";
                }
                else if (Quantity <= 5)
                {
                    return "Low Stock";
                }
                else
                {
                    return "In Stock";
                }
            }
        }

    }
}
