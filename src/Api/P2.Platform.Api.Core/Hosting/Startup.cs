namespace P2.Platform.Api.Core.Hosting
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Collections.Generic;
    using System.Reflection;

    public abstract class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environement { get; }

        public Startup(IHostingEnvironment env)
        {
            this.Environement = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Defines the implementation of the MVC runtime features.
            // This is the only required reference to produce something that can serve requests using AddMvcCore(...).
            var builder = services.AddMvcCore();

            // Defines the implementation of the IApiDescriptionProvider and other infrastructure for describing API endpoints in an application.
            // Required if using Swashbuckle or a similar package.
            builder.AddApiExplorer();

            // Adds support for discoving [Authorize(...)] attributes on controllers and actions and applying authorization policy.
            // This must be used with an authorization middleware, and only adds support for MVC to recognize the attributes.
            builder.AddAuthorization();

            // Register assembly contains api controllers
            OnRegisterResources(builder);

            // Defines the ModelMetadata and validation support for the attributes defined in System.ComponentModel.DataAnnotations.
            // Almost no request validation will take place without including these validators.
            builder.AddDataAnnotations();

            builder.AddControllersAsServices();

            // Includes the JSON input and output formatters and also the JSON-patch input formatter - using Json.NET. Includes JsonResult.
            builder.AddJsonFormatters();
            builder.AddJsonOptions(options =>
            {
                ConfigureJsonFormatter(options.SerializerSettings);
            });

            // Defines the discovery and implementation of CORS policies to be applied at the action or controller level in MVC.
            // Adding these services will allow MVC to recognize [EnableCors(...)] and [DisableCors] attributes.
            builder.AddCors();

            // Resolve for IConfiguration
            services.AddSingleton<IConfiguration>(this.Configuration);

            // Resolve dependencies for the container
            ConfigureDependency(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ConfigureLoggerFactory(loggerFactory);

            app.UseCors(builder =>
            {
                ConfigureCors(builder);
            });

            app.UseMvcWithDefaultRoute();

            ConfigureApplication(app);
        }

        protected abstract void RegisterResourceParts(IList<Assembly> parts);

        protected virtual void ConfigureJsonFormatter(JsonSerializerSettings settings)
        {
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        protected virtual void ConfigureCors(CorsPolicyBuilder builder)
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }

        protected virtual void ConfigureDependency(IServiceCollection services) { }

        protected virtual void ConfigureLoggerFactory(ILoggerFactory loggerFactory) { }

        protected virtual void ConfigureApplication(IApplicationBuilder app) { }

        private void OnRegisterResources(IMvcCoreBuilder builder)
        {
            var parts = new List<Assembly>();

            RegisterResourceParts(parts);

            // register api controllers with framework
            foreach (var part in parts)
            {
                builder.AddApplicationPart(part);
            }
        }
    }
}
