using Application.Cqrs;
using Common;
using Data.Contracts;
using Data.Repositories;
using IdGen.DependencyInjection;
using Serilog;
using Services.Hubs;
using StackExchange.Redis;
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
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetSection("Redis:ConnectionString").Value));

builder.Services.AddCqrs();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSwagger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    options.AddPolicy("ProdCors", builder =>
    {
        builder.WithOrigins(
            "https://kolbeh.liara.run",
            "https://localhost:5001",
            "http://localhost:5000"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

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

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
}
else
{
    app.UseCors("ProdCors");
}

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationHub>("/hub/notifications")
   .RequireCors(app.Environment.IsDevelopment() ? "DevCors" : "ProdCors");


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


