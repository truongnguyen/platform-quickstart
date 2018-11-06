using System.Collections.Generic;

namespace P2.Platform.ContactManagement
{
    public interface IContactService
    {
        #region Contacts
        int AddContact(Contact contact);

        Contact GetContactById(int id);

        void UpdateContact(Contact contact);

        IList<Contact> GetContacts(string[] types, string keywords, out int count, out int total, string orderBy, bool ascending = true, int skip = 0, int top = 50);

        #endregion

        #region Addresses

        IList<Address> GetAddressesByContact(int contactid);

        #endregion
    }
}
