namespace ConfigVault.Application;

public record ActivateConfigVersionCommand(Guid ConfigVersionId);

public record ApproveConfigVersionCommand(Guid ConfigVersionId);
