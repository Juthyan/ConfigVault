using ConfigVault.Domain;

namespace ConfigVault.Application;

public class ApproveConfigVersionHandler
{
    private readonly IConfigVersionRepository _repository;

    public ApproveConfigVersionHandler(IConfigVersionRepository repository)
    {
        _repository = repository;
    }

    public void Handle(ApproveConfigVersionCommand command)
    {
        var version = _repository.Get(command.ConfigVersionId);

        if (version is null)
        {
            throw new DomainException($"Config version {command.ConfigVersionId} not found.");
        }

        version.Approve();
        _repository.Save(version);
    }
}