namespace P2.Platform.ContactManagement.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using P2.Platform.ContactManagement.Database;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ContactService : IContactService
    {
        private readonly ContactContext context;

        public ContactService(ContactContext context)
        {
            this.context = context;
        }

        #region Contacts
        public int AddContact(Contact contact)
        {
            context.Save<Contact>(contact);

            return contact.Id;
        }

        public void UpdateContact(Contact contact)
        {
            context.Save<Contact>(contact);
        }

        public Contact GetContactById(int id)
        {
            // only return active record 
            return context.Find<Contact>(id);
        }

        public IList<Contact> GetContacts(string[] tags, string keywords, out int count, out int toal, string orderBy, bool ascending = true, int skip = 0, int top = 50)
        {
            string[] words = new string[] { };

            if (tags == null)
                tags = new string[] { };

            if (!String.IsNullOrEmpty(keywords))
                words = keywords.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();



            return context.Filter<Contact>(FilterContact(tags, words), orderBy, out count, out toal, ascending, skip, top)
                .Include("Addresses")
                .Where(a => a.IsActive == true)
                .ToList();
        }

        #endregion

        #region Addresses

        public IList<Address> GetAddressesByContact(int contactId)
        {
            return context.Query<Address>().Where(a => a.ContactId == contactId).ToList();
        }

        #endregion

        private Expression<Func<Contact, bool>> FilterContact(string[] tags, string[] keywords)
        {
            // keywords & types
            if (keywords.Count() > 0 && tags.Count() > 0)
            {
                return contact =>
                    (
                        keywords.Where(word => contact.FirstName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                        || keywords.Where(word => contact.LastName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                        || keywords.Where(word => !String.IsNullOrEmpty(contact.PhoneNumber) && contact.PhoneNumber.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                        || keywords.Where(word => !String.IsNullOrEmpty(contact.EmailAddress) && contact.EmailAddress.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                        || keywords.Where(word => !String.IsNullOrEmpty(contact.Organization) && contact.Organization.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                    )
                    && tags.Where(tag => contact.Tags.Where(t => String.Compare(t, tag, StringComparison.OrdinalIgnoreCase) == 0).Any()).Any();
            }

            // keywords only
            if (keywords.Count() > 0)
            {
                return contact =>
                    (
                        keywords.Where(word => contact.FirstName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                        || keywords.Where(word => contact.LastName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                        || keywords.Where(word => !String.IsNullOrEmpty(contact.PhoneNumber) && contact.PhoneNumber.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                        || keywords.Where(word => !String.IsNullOrEmpty(contact.EmailAddress) && contact.EmailAddress.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                        || keywords.Where(word => !String.IsNullOrEmpty(contact.Organization) && contact.Organization.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Any()
                    );
            }

            // types only
            if (tags.Count() > 0)
            {
                return contact => tags.Where(tag => contact.Tags.Where(t => String.Compare(t, tag, StringComparison.OrdinalIgnoreCase) == 0).Any()).Any();
            }


            // no filter
            return contact => true;
        }
    }
}
