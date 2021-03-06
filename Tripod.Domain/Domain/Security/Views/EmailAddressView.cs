namespace Tripod.Domain.Security
{
    public class EmailAddressView
    {
        public int EmailAddressId { get; set; }
        public int? UserId { get; set; }
        public string Value { get; set; }
        public string HashedValue { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsVerified { get; set; }
    }
}