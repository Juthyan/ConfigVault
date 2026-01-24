using ConfigVault.Domain;

namespace ConfigVault.Application;

public interface IConfigVersionRepository
{
    ConfigVersion? Get(Guid id);
    void Save(ConfigVersion version);
}