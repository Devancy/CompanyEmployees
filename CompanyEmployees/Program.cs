using CompanyEmployees;
using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers(config => config.RespectBrowserAcceptHeader = true)
    // enable xml formatters
    .AddXmlDataContractSerializerFormatters()
    // dictates the app to find controllers inside of the 'CompanyEmployees.Presentation' project
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(opt => { });
if(app.Environment.IsProduction())
{
    // The HTTP Strict-Transport-Security response header informs browsers that the site should only be accessed using HTTPS,
    // and that any future attempts to access it using HTTP should automatically be converted to HTTPS.
    // Note: This is more secure than simply configuring a HTTP to HTTPS (301)
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
