using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Entities
{
    public class Order
    {
        // Primary key for the order
        [Key]
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
        public bool IsInvoiced { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

    }
}
