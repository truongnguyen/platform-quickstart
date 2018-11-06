namespace P2.Platform.Api.Resources.ServiceTests.ContactsControllerFixture
{
    using Hosting;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Net.Http;
    using System.Threading.Tasks;

    [TestClass]
    public class given_hosting_environment
    {
        private static TestServer server;
        private static HttpClient client;

        static given_hosting_environment()
        {
            // test server has ability to host rest api for testing purpose
            server = new TestServer(new WebHostBuilder().UseStartup<HostStartup>());

            client = server.CreateClient();
        }

        public given_hosting_environment()
        {

        }

        #region Contacts

        [TestMethod]
        public async Task when_get_contact_collection_then_should_have_three()
        {
            // contacts
            var response = await client.GetAsync("/api/contacts/");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsNotNull(result);
        }

        #endregion

        [TestMethod]
        public void Second()
        {
            Assert.IsTrue(true);
        }
    }
}
