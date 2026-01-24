using ConfigVault.Domain;

namespace ConfigVault.Application;

public class ActivateConfigVersionHandler
{
    private readonly IConfigVersionRepository _repository;

    public ActivateConfigVersionHandler(IConfigVersionRepository repository)
    {
        _repository = repository;
    }

    public void Handle(ActivateConfigVersionCommand command)
    {
        var version = _repository.Get(command.ConfigVersionId);

        if (version is null)
        {
            throw new DomainException($"Config version {command.ConfigVersionId} not found.");
        }

        version.Activate();
        _repository.Save(version);
    }
}
