using Application.Cqrs;
using Common;
using IdGen.DependencyInjection;
using Serilog;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
using WebFramework.Filters;
using WebFramework.Middlewares;
using WebFramework.Swagger;

var configuration = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfiguration(configuration);
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
builder.Host.UseSerilog(SerilogConfig.ConfigureLogger);


builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 102400000;
    e.EnableDetailedErrors = true;
});

builder.Services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));
var _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

builder.Services.InitializeAutoMapper();

builder.Services.AddDbContext(configuration);
builder.Services.AddCustomIdentity(_siteSetting.IdentitySettings);
builder.Services.AddMinimalMvc();
builder.Services.AddJwtAuthentication(_siteSetting.JwtSettings);
builder.Services.AddServices();
builder.Services.AddCustomApiVersioning();
builder.Services.AddIdGen(0);
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = configuration.GetConnectionString("Redis"));
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSwagger();
builder.Services.AddCqrs();
builder.Services.AddCors();

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.IntializeDatabase();

app.UseCustomExceptionHandler();
app.UseHsts(app.Environment);

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = r =>
    {
        string path = r?.File?.PhysicalPath;
        if ((path is not null) && (path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".png") || path.EndsWith(".svg") || path.EndsWith(".webp")))
        {
            TimeSpan maxAge = new(365, 0, 0, 0);
            r.Context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
        }
    }
});

app.UseSwaggerAndUI();
app.UseRouting();

//Use this config just in Develoment (not in Production)
if (app.Environment.IsDevelopment())
{
    // تنظیمات CORS برای Development
    app.UseCors(o =>
    {
        o.AllowAnyMethod();
        o.AllowAnyHeader();
        o.SetIsOriginAllowed(origin => true);
        o.AllowCredentials();
    });
}
else
{
    // تنظیمات CORS برای Production
    app.UseCors(o =>
    {
        o.WithOrigins(
            "https://kolbeh.liara.run",
            "https://localhost:5001",
            "http://localhost:5000"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
}

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(config =>
{

});
app.Map("/", () =>
{
    return Results.Redirect("/swagger/index.html");
});
app.MapControllers();
app.Run();


public partial class Program
{
    static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var config = builder.Build();
        return builder.Build();
    }
}


