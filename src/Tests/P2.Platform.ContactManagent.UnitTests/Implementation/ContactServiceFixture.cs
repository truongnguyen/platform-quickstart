namespace P2.Platform.ContactManagent.UnitTests.Implementation.ContactServiceFixture
{
    using ContactManagement;
    using ContactManagement.Database;
    using ContactManagement.Implementation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class given_contact_context 
    {
        // system under test
        private static IContactService sut;

        static given_contact_context()
        {
            var options = new DbContextOptionsBuilder<ContactContext>().UseInMemoryDatabase();
            var ctx = new ContactContext(options.Options);
            // seed data for testing
            ctx.Seed();

            sut = new ContactService(ctx);
        }

        [TestMethod]
        public void when_get_contact_with_id_1_then_has_contact()
        {
            var contact = sut.GetContactById(1);

            Assert.IsNotNull(contact);
            Assert.AreEqual(1, contact.Id);
        }

        [TestMethod]
        public void when_get_addresses_for_contact_1_then_should_have_2()
        {
            var result = sut.GetAddressesByContact(1);

            Assert.AreEqual(2, result.Count);
        }
    }
}
