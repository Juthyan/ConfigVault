using ConfigVault.Application;
using ConfigVault.Domain;
using ConfigVault.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConfigVault.Infrastructure.Repositories;

public class EfCoreConfigVersionRepository : IConfigVersionRepository
{

    private readonly ConfigVaultDbContext _context;

    public EfCoreConfigVersionRepository(ConfigVaultDbContext context)
    {
        _context = context;
    }

    public ConfigVersion? Get(Guid id)
    {
        return _context.ConfigVersions
            .SingleOrDefault(v => v.Id == id);
    }

      public void Save(ConfigVersion version)
    {
        var exists = _context.ConfigVersions
            .Any(v => v.Id == version.Id);

        if (!exists)
        {
            _context.ConfigVersions.Add(version);
        }
        else
        {
            _context.ConfigVersions.Update(version);
        }

        _context.SaveChanges();
    }

}