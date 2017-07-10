using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reborn.Domain.Infrastructure;
using Reborn.Domain.Repository;
using Reborn.Service;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Attributes;

namespace Reborn.Web.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }




        // This method gets called by the runtime. Use this method to add services to theer.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddMvc().AddFluentValidation(fv =>
            {
                fv.ValidatorFactoryType = typeof(AttributedValidatorFactory);
                // fv.RegisterValidatorsFromAssemblyContaining<ValidatorAttribute>();

                //foreach (var item in AssemblyScanner.FindValidatorsInAssembly(Assembly.GetEntryAssembly()))
                //{
                //    //  builder.RegisterType(item.ValidatorType).Keyed<IValidator>(item.InterfaceType).As<IValidator>();


                //    //var type = item.ValidatorType;
                //    fv.RegisterValidatorsFromAssemblyContaining<IValidator>();
                //}

                //fv.RegisterValidatorsFromAssembly(Startup);

                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
            services.AddAutoMapper();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);

            });

            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    options.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "API v1",
                        Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
                        Contact = new Contact() { Name = "Özgür KARA", Email = "ozgurkara85@gmail.com" },
                        TermsOfService = "Shareware"
                    });

                    options.SwaggerDoc("v2", new Info
                    {
                        Version = "v2",
                        Title = "API v2",
                        Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
                        Contact = new Contact() { Name = "Özgür KARA", Email = "ozgurkara85@gmail.com" },
                        TermsOfService = "Shareware"
                    });
                });

            services.AddTransient<IMapper, Mapper>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IDatabaseFactory, DatabaseFactory>();
        }

        /// <summary>
        /// Configures the application using the provided builder, hosting environment, and logging factory.
        /// </summary>
        /// <param name="app">The current application builder.</param>
        /// <param name="env">The current hosting environment.</param>
        /// <param name="loggerFactory">The logging factory used for instrumentation.</param>
        /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
                });
        }
    }

    /// <summary>
    /// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
    /// </summary>
    /// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
    /// Once they are fixed and published, this class can be removed.</remarks>
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata.Description;
                }

                if (parameter.Default == null)
                {
                    parameter.Default = description.RouteInfo.DefaultValue;
                }

                parameter.Required |= !description.RouteInfo.IsOptional;
            }
        }
    }
}
