namespace InsecureWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        
        // SECURITY VULNERABILITY: Plain text password storage
        public string Password { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        
        // SECURITY VULNERABILITY: Sensitive data stored in plain text
        public string SocialSecurityNumber { get; set; } = string.Empty;
        public string CreditCardNumber { get; set; } = string.Empty;
        
        public bool IsAdmin { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // SECURITY VULNERABILITY: No input validation attributes
        // SECURITY VULNERABILITY: No data classification or protection
    }
}