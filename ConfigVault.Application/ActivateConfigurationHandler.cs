using ConfigVault.Domain;
using Microsoft.Extensions.Logging;


namespace ConfigVault.Application;

public class ActivateConfigVersionHandler
{
    private readonly IConfigVersionRepository _repository;

    private readonly ILogger<ActivateConfigVersionHandler> _logger;

    public ActivateConfigVersionHandler(IConfigVersionRepository repository, ILogger<ActivateConfigVersionHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public void Handle(ActivateConfigVersionCommand command)
    {
        _logger.LogInformation(
            "Handling ActivateConfigVersion for {ConfigVersionId}",
            command.ConfigVersionId
        );

        var version = _repository.Get(command.ConfigVersionId);

        if (version is null)
        {
            _logger.LogWarning(
               "Config version {ConfigVersionId} not found when trying to activate",
               command.ConfigVersionId
           );

            throw new DomainException($"Config version {command.ConfigVersionId} not found.");
        }

        version.Activate();
        _repository.Save(version);

        _logger.LogInformation(
            "Config version {ConfigVersionId} activated successfully",
            command.ConfigVersionId
        );
    }
}
