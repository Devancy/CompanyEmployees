using Contracts;
using LoggerService;
using Repository;
using Service.Contracts;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using CompanyEmployees.Presentation.Controllers;
using System.Threading.RateLimiting;

namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    // enable the client application to read the new X-Pagination header
                    .WithExposedHeaders("X-Pagination"));
        });

    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {

        });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter = config.OutputFormatters
            .OfType<SystemTextJsonOutputFormatter>()?
            .FirstOrDefault();
            if (systemTextJsonOutputFormatter != null)
            {
                systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.identifier.hateoas+json");
                systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.identifier.apiroot+json");
            }
            var xmlOutputFormatter = config.OutputFormatters
            .OfType<XmlDataContractSerializerOutputFormatter>()?
            .FirstOrDefault();
            if (xmlOutputFormatter != null)
            {
                xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.identifier.hateoas+xml");
                xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.identifier.apiroot+xml");
            }
        });
    }

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
        }).AddMvc(opt =>
        {
            opt.Conventions.Controller<CompaniesController>()
            .HasApiVersion(new ApiVersion(1, 0));
            opt.Conventions.Controller<EmployeesController>()
            .HasApiVersion(new ApiVersion(1, 0));
        });
    }
    public static void ConfigureOutputCaching(this IServiceCollection services) => services.AddOutputCache(opt =>
    {
        // apply this base policy to all endpoints in our controllers
        opt.AddBasePolicy(bp => bp.Expire(TimeSpan.FromSeconds(10)).Tag("tag=all"));
        opt.AddPolicy("120SecondsDuration", p => p.Expire(TimeSpan.FromSeconds(120)).Tag("tag-expensive"));
    });

    public static void ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        services.AddRateLimiter(opt =>
        {
            opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetSlidingWindowLimiter("GlobalLimiter",
            partition => new SlidingWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10),
                SegmentsPerWindow = 2,
                QueueLimit = 5,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));

            opt.AddPolicy("3Per10sPolicy", context =>
             RateLimitPartition.GetFixedWindowLimiter("3Per10sLimiter",
             partition => new FixedWindowRateLimiterOptions
             {
                 AutoReplenishment = true,
                 PermitLimit = 3,
                 Window = TimeSpan.FromSeconds(10)
             }));


            opt.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    await context.HttpContext.Response
                    .WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} second(s).", token);
                else
                    await context.HttpContext.Response
                    .WriteAsync("Too many requests. Please try again later.", token);
            };
        });
    }
}