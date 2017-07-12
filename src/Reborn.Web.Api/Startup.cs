using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
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
using Reborn.Web.Api.Utils.Exception;
using Reborn.Web.Api.Utils.Swagger;
using Reborn.Web.Api.Utils.Validator;
using Swashbuckle.AspNetCore.Swagger;
using System;

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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddMvc(m =>
            {
                m.Filters.Add(new ValidationActionFilter());
                m.Filters.Add(new ErrorActionFilter());
            }).AddFluentValidation(fv =>
            {
                fv.ValidatorFactoryType = typeof(FluentValidatorFactory); //typeof(AttributedValidatorFactory);
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                fv.RegisterValidatorsFromAssemblyContaining<Reborn.Service.BaseService>();
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

                    options.OperationFilter<SwaggerDefaultValues>();
                });

            services.AddTransient<IMapper, Mapper>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IDatabaseFactory, DatabaseFactory>();

            services.AddTransient<IValidatorFactory, FluentValidatorFactory>();
            services.AddTransient<IServiceValidator, ServiceValidator>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Configures the application using the provided builder, hosting environment, and logging factory.
        /// </summary>
        /// <param name="app">The current application builder.</param>
        /// <param name="env">The current hosting environment.</param>
        /// <param name="loggerFactory">The logging factory used for instrumentation.</param>
        /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider provider)
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
}
