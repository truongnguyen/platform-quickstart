namespace P2.Platform.ContactManagement.Dto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Contact
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string Organization { get; set; }

        public string[] Tags { get; set; }

        public DateTime CreatedDate { get; set; }

        public string NewName { get; set; }


        public List<Address> Addresses { get; set; }
    }
}
