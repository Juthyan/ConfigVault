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

        var id = Guid.NewGuid();

        var version = new ConfigVersion(id);
        version.Approve();

        repo.Save(version);

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
