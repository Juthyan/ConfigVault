namespace ConfigVault.Domain.Tests;

public class ConfigVersionTests
{
    [Fact]
    public void New_configuration_starts_in_draft()
    {
        var setting = new ConfigVersion(Guid.NewGuid());
        Assert.Equal(ConfigStatus.Draft, setting.Status); 
    }
    
    [Fact]
    public void Approved_configuration_can_be_activated()
    {
        var setting = new ConfigVersion(Guid.NewGuid());
        setting.Approve();
        setting.Activate();
        Assert.Equal(ConfigStatus.Active, setting.Status);
    }

    [Fact]
    public void Active_configuration_can_be_archived()
    {
        var setting = new ConfigVersion(Guid.NewGuid());
        setting.Approve();
        setting.Activate();
        setting.Archive();
        Assert.Equal(ConfigStatus.Archived, setting.Status);
    }
    
    [Fact]
    public void Cannot_activate_configuration_without_approval()
    {
        var setting = new ConfigVersion(Guid.NewGuid());

        Assert.Throws<DomainException>(() => setting.Activate());
    }

    [Fact]
    public void Cannot_archive_configuration_that_is_not_active()
    {
        var setting = new ConfigVersion(Guid.NewGuid());

        Assert.Throws<DomainException>(() => setting.Archive());
    }
    
    [Fact]
    public void Cannot_approve_configuration_that_is_not_draft()
    {
        var setting = new ConfigVersion(Guid.NewGuid());

        setting.Approve();

        Assert.Throws<DomainException>(() => setting.Approve());
    }
}
