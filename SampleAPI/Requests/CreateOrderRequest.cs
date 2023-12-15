using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Requests
{
    public class CreateOrderRequest
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }

        public bool IsInvoiced { get; set; } = true;
    }
}
