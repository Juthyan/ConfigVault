using ConfigVault.Domain;
using Microsoft.Extensions.Logging;

namespace ConfigVault.Application;

public class ApproveConfigVersionHandler
{
    private readonly IConfigVersionRepository _repository;
    private readonly ILogger<ApproveConfigVersionHandler> _logger;

    public ApproveConfigVersionHandler(IConfigVersionRepository repository, ILogger<ApproveConfigVersionHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public void Handle(ApproveConfigVersionCommand command)
    {
        _logger.LogInformation(
            "Handling ApproveConfigVersion for {ConfigVersionId}",
            command.ConfigVersionId
        );

        var version = _repository.Get(command.ConfigVersionId);

        if (version is null)
        {
            _logger.LogWarning(
               "Config version {ConfigVersionId} not found when trying to approve",
               command.ConfigVersionId
           );

            throw new DomainException($"Config version {command.ConfigVersionId} not found.");
        }

        version.Approve(DateTimeOffset.UtcNow);
        _repository.Save(version);

        _logger.LogInformation(
            "Config version {ConfigVersionId} approved successfully",
            command.ConfigVersionId
        );
    }
}