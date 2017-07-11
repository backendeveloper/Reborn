using System;
using System.Collections.Generic;
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
using FluentValidation.Validators;
using Reborn.Web.Api.Utils.Exception;
using Reborn.Web.Api.V1.Models;
using Reborn.Web.Api.V1.Models.Validators;

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
                // fv.ValidatorFactoryType = typeof(FluentValidatorFactory); //typeof(AttributedValidatorFactory);
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

                    options.SchemaFilter<FluentValidationRules>();
                    options.OperationFilter<SwaggerDefaultValues>();
                });

            services.AddTransient<IMapper, Mapper>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IDatabaseFactory, DatabaseFactory>();
            // services.AddTransient<IValidatorFactory, FluentValidatorFactory>();

            //services.AddTransient<IServiceProvider,Service.>()

            return services.BuildServiceProvider();
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

    //public class FluentValidatorFactory : IValidatorFactory
    //{
    //    private IServiceScopeFactory scopeFactory;
    //    public FluentValidatorFactory(IServiceScopeFactory scopeFactory)
    //    {
    //        this.scopeFactory = scopeFactory;
    //    }

    //    public IValidator<T> GetValidator<T>()
    //    {
    //        return (IValidator<T>)this.GetValidator(typeof(T));
    //    }

    //    public IValidator GetValidator(Type type)
    //    {
    //        IValidator validator;

    //        try
    //        {
    //            // Obtain instance of validator. If not registered, SimpleIoc will throw exception (although documentation said it will return null)
    //            validator = this.CreateInstance(typeof(IValidator<>).MakeGenericType(type));
    //        }
    //        catch (Exception exception)
    //        {
    //            // Get base type and try to find validator for base type (used for polymorphic classes)
    //            var baseType = type.GetTypeInfo().BaseType;
    //            if (baseType == null)
    //            {
    //                throw;
    //            }

    //            validator = this.CreateInstance(typeof(IValidator<>).MakeGenericType(baseType));
    //        }

    //        return validator;
    //    }

    //    public IValidator CreateInstance(Type validatorType)
    //    {
    //        using (var scope = scopeFactory.CreateScope())
    //        {
    //            return scope.ServiceProvider.GetService(validatorType) as IValidator;
    //        }

    //        // return SimpleIoc.Default.GetInstance(validatorType) as IValidator;
    //    }
    //}


    public class FluentValidationRules : ISchemaFilter
    {
        private readonly IValidatorFactory _factory;

        /// <summary>
        ///     Default constructor with DI
        /// </summary>
        /// <param name="factory"></param>
        public FluentValidationRules(IValidatorFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void Apply(Schema model, SchemaFilterContext context)
        {
            // use IoC or FluentValidatorFactory to get AbstractValidator<T> instance
            var validator = _factory.GetValidator(context.SystemType);
            if (validator == null) return;
            if (model.Required == null)
                model.Required = new List<string>();

            var validatorDescriptor = validator.CreateDescriptor();
            foreach (var key in model.Properties.Keys)
            {
                foreach (var propertyValidator in validatorDescriptor
                    .GetValidatorsForMember(ToPascalCase(key)))
                {
                    if (propertyValidator is NotNullValidator
                        || propertyValidator is NotEmptyValidator)
                        model.Required.Add(key);

                    if (propertyValidator is LengthValidator lengthValidator)
                    {
                        if (lengthValidator.Max > 0)
                            model.Properties[key].MaxLength = lengthValidator.Max;

                        model.Properties[key].MinLength = lengthValidator.Min;
                    }

                    if (propertyValidator is RegularExpressionValidator expressionValidator)
                        model.Properties[key].Pattern = expressionValidator.Expression;

                    // Add more validation properties here;
                }
            }
        }

        /// <summary>
        ///     To convert case as swagger may be using lower camel case
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string ToPascalCase(string inputString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (inputString == null) return null;
            if (inputString.Length < 2) return inputString.ToUpper();
            return inputString.Substring(0, 1).ToUpper() + inputString.Substring(1);
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

                if (description.RouteInfo == null)
                {
                    var definititon = context.SchemaRegistry.Definitions.FirstOrDefault();
                    if (definititon.Value.Required!=null  && definititon.Value.Required
                        .Any(x => x.ToLower() == parameter.Name.ToLower()))
                    {
                        parameter.Required = true;
                    }
                    continue;
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
