namespace InsecureWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        
        // SECURITY VULNERABILITY: No authorization checks for order access
        // SECURITY VULNERABILITY: No audit trail
        // SECURITY VULNERABILITY: No input validation
    }
}