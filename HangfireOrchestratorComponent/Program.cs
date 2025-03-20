using Hangfire;
using Hangfire.PostgreSql;
using HangfireOrchestratorComponent.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHangfire(hangfire =>
    hangfire.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(psql =>
        psql.UseNpgsqlConnection(
            connectionString: builder.Configuration.GetConnectionString("Database")!)));

builder.Services.AddHangfireServer();

builder.Services.AddDbContext<PocDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("Database")!, npgsql =>
    {
        npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
