namespace ConfigVault.Api.Models;

public record CreateConfigVersionRequest(string Key, int VersionNumber, string Content);
