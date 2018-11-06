namespace P2.Platform.ContactManagement
{
    using System.ComponentModel.DataAnnotations;

    public class Address
    {
        [Key]
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string StateProvince { get; set; }

        public string PostalCode { get; set; }

        public string County { get; set; }

        public string AddressType { get; set; }

        public bool IsPrimary { get; set; }

        // reference
        public int ContactId { get; protected set; }

        public virtual Contact Contact { get; protected set; }
    }

    public enum AddressType
    {
        Office,
        Home
    }

    public enum AddressType2
    {
        Remote,
        Local
    }
}
