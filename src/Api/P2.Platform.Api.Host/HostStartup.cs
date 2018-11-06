namespace P2.Platform.Api.Host
{
    using ContactManagement;
    using ContactManagement.Database;
    using ContactManagement.Implementation;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using P2.Platform.Api.Core.Hosting;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using LandManagement.Database;
    using LandManagement;
    using LandManagement.Implementation;

    public class HostStartup : Startup
    {
        public HostStartup(IHostingEnvironment env) : base(env) { }

        protected override void RegisterResourceParts(IList<Assembly> parts)
        {
            parts.Add(typeof(Api.Resources.ContactsController).Assembly);
        }
        protected override void ConfigureLoggerFactory(ILoggerFactory loggerFactory)
        {
            base.ConfigureLoggerFactory(loggerFactory);

            loggerFactory.AddConsole();
        }

        protected override void ConfigureDependency(IServiceCollection services)
        {
            base.ConfigureDependency(services);

            // shared throughout the request's lifetime
            services.AddDbContext<ContactContext>(options => options.UseInMemoryDatabase(), ServiceLifetime.Scoped);

            // shared throughout application's lifetime
            services.AddSingleton<IContactService, ContactService>();

            services.AddScoped<OracleContext>(options => new OracleContext(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<ILandService, LandService>();
        }

        protected override void ConfigureApplication(IApplicationBuilder app)
        {
            base.ConfigureApplication(app);

            app.ApplicationServices.GetRequiredService<ContactContext>().Seed();
        }
    }
}
