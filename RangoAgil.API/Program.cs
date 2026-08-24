using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DbContexts;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog early
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("RangoDbConStr"))
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(new { title = "An internal error occurred." });
    });
});

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.MapRangosEndpoints();

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
