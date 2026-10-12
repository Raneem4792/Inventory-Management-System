using System.ComponentModel.DataAnnotations;

namespace TestWebApplication.Models
{
    public class StockTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [Required]
        public string Type { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
