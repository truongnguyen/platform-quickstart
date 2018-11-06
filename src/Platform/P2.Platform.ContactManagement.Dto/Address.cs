using System.Collections.Generic;

namespace P2.Platform.ContactManagement.Dto
{
    public class Address
    {
        public int Id { get; set; }

        /// <summary>
        /// Street name
        /// </summary>
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string StateProvince { get; set; }

        public string PostalCode { get; set; }

        public string County { get; set; }

        public string AddressType { get; set; }

        public bool IsPrimary { get; set; }

    }
}
