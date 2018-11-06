namespace P2.Platform.ContactManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string Organization { get; set; }

        public string TagsValue { get; set; }

        public string[] Tags
        {
            get
            {
                var result = new string[] { };

                if (!String.IsNullOrEmpty(this.TagsValue))
                {
                    result = this.TagsValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                }

                return result;
            }
        }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
