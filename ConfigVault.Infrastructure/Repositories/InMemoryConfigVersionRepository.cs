using System;
using System.Collections.Generic;
using ConfigVault.Application;
using ConfigVault.Domain;

namespace ConfigVault.Infrastructure.Repositories;

public class InMemoryConfigVersionRepository : IConfigVersionRepository
{
    private readonly Dictionary<Guid, ConfigVersion> _store = new();

    public ConfigVersion? Get(Guid id)
    {
        _store.TryGetValue(id, out var version);
        return version;
    }

    public void Save(ConfigVersion version)
    {
        _store[version.Id] = version;
    }
}
