namespace P2.Platform.Api.Resources.ServiceTests.Hosting
{
    using Microsoft.AspNetCore.Hosting;
    using P2.Platform.Api.Core.Hosting;
    using System.Collections.Generic;
    using System.Reflection;

    public class HostStartup : Startup
    {
        public HostStartup(IHostingEnvironment env) : base(env) { }

        protected override void RegisterResourceParts(IList<Assembly> parts)
        {
            parts.Add(typeof(Api.Resources.ContactsController).Assembly);
        }
    }
}
