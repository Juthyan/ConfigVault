namespace ConfigVault.Application.Tests;

using ConfigVault.Domain;
using ConfigVault.Application;
using Microsoft.Extensions.Logging.Abstractions;

using ConfigVault.Infrastructure.Repositories;

public class ActivateConfigVersionHandlerTests
{
    [Fact]
    public void Handle_Activates_And_Saves_ConfigVersion()
    {
        var repo = new InMemoryConfigVersionRepository();
        var logger = NullLogger<ActivateConfigVersionHandler>.Instance;

        var createdAt = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var version = ConfigVersion.CreateDraft("genetec.health", "{\"enabled\": true}", createdAt);
        version.Approve(createdAt.AddHours(1));

        repo.Save(version);
        var id = version.Id;

        var handler = new ActivateConfigVersionHandler(repo, logger);
        var command = new ActivateConfigVersionCommand(id);

        handler.Handle(command);

        var updated = repo.Get(id);

        Assert.NotNull(updated);
        Assert.Equal(ConfigStatus.Active, updated!.Status);
    }

    [Fact]
    public void Handle_Throws_When_ConfigVersion_Not_Found()
    {
        var repo = new InMemoryConfigVersionRepository();
        var logger = NullLogger<ActivateConfigVersionHandler>.Instance;
        var handler = new ActivateConfigVersionHandler(repo, logger);
        var command = new ActivateConfigVersionCommand(Guid.NewGuid());

        Assert.Throws<DomainException>(() => handler.Handle(command));
    }
}
