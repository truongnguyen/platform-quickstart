namespace P2.Platform.ContactManagement.Database
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class DbContextExtensions
    {
        public static void Seed(this ContactContext context)
        {
            context.Database.EnsureDeleted();

            context.AddRange(
                new Contact
                {
                    TagsValue = "Work,Mobile",
                    CreatedDate = new DateTime(2016, 10, 10),
                    EmailAddress = "t.nguyen@p2es.com",
                    FirstName = "Truong",
                    LastName = "Nguyen",
                    Organization = "P2ES",
                    IsActive = true,
                    Addresses = InitializeAddresses()
                },
                new Contact
                {
                    TagsValue = "Home",
                    CreatedDate = new DateTime(2016, 10, 11),
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "713.481.2000",
                    Organization = "P2ES",
                    IsActive = true,
                    Addresses = InitializeAddresses()
                },
                new Contact
                {
                    TagsValue = "Work",
                    CreatedDate = new DateTime(2016, 10, 12),
                    EmailAddress = "jane.doe@p2es.com",
                    FirstName = "Jane",
                    LastName = "Doe",
                    PhoneNumber = "713.481.2000",
                    IsActive = true,
                    Addresses = InitializeAddresses()
                }
            );

            context.SaveChanges();
        }

        private static ICollection<Address> InitializeAddresses()
        {
            var addresses = new Collection<Address>();

            addresses.Add(new Address
            {
                AddressLine1 = "1331 Lamar St",
                AddressLine2 = "Suite 1400",
                AddressType = "Office",
                City = "Houston",
                County = "Harris",
                IsPrimary = true,
                PostalCode = "77010",
                StateProvince = "TX"
            });

            addresses.Add(new Address
            {
                AddressLine1 = "1670 Broadway",
                AddressLine2 = "Suite 2800",
                AddressType = "Office",
                City = "Denver",
                County = "Denver",
                IsPrimary = false,
                PostalCode = "80202",
                StateProvince = "CO"
            });

            return addresses;
        }
    }
}
