using ConfigVault.Api.Models;
using ConfigVault.Application;
using ConfigVault.Domain;
using ConfigVault.Infrastructure.Repositories;
using ConfigVault.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConfigVault")
                       ?? "Data Source=configvault.db";

builder.Services.AddDbContext<ConfigVaultDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IConfigVersionRepository, EfCoreConfigVersionRepository>();
builder.Services.AddScoped<ApproveConfigVersionHandler>();
builder.Services.AddScoped<ActivateConfigVersionHandler>();

var app = builder.Build();

app.MapPost("/config-versions", (IConfigVersionRepository repo) =>
{
    var id = Guid.NewGuid();
    var version = new ConfigVersion(id);

    repo.Save(version);

    var response = new ConfigVersionResponse(version.Id, version.Status.ToString());
    return Results.Created($"/config-versions/{version.Id}", response);
});

app.MapPost("/config-versions/{id:guid}/approve", (Guid id, ApproveConfigVersionHandler handler) =>
{
    try
    {
        var command = new ApproveConfigVersionCommand(id);
        handler.Handle(command);
        return Results.NoContent();
    }
    catch (DomainException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPost("/config-versions/{id:guid}/activate", (Guid id, ActivateConfigVersionHandler handler) =>
{
    try
    {
        var command = new ActivateConfigVersionCommand(id);
        handler.Handle(command);
        return Results.NoContent();
    }
    catch (DomainException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/config-versions/{id:guid}", (Guid id, IConfigVersionRepository repo) =>
{
    var version = repo.Get(id);

    if (version is null)
    {
        return Results.NotFound();
    }

    var response = new ConfigVersionResponse(version.Id, version.Status.ToString());
    return Results.Ok(response);
});

app.Run();
