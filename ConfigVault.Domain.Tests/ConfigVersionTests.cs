namespace ConfigVault.Domain.Tests;

public class ConfigVersionTests
{
    private static readonly DateTimeOffset CreatedAt = new(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ApprovedAt = CreatedAt.AddHours(1);

    private static ConfigVersion CreateDraft() =>
        ConfigVersion.CreateDraft("genetec.health", "{\"enabled\": true}", CreatedAt);

    [Fact]
    public void New_configuration_starts_in_draft()
    {
        var setting = CreateDraft();
        Assert.Equal(ConfigStatus.Draft, setting.Status);
    }

    [Fact]
    public void Approved_configuration_can_be_activated()
    {
        var setting = CreateDraft();
        setting.Approve(ApprovedAt);
        setting.Activate();
        Assert.Equal(ConfigStatus.Active, setting.Status);
    }

    [Fact]
    public void Active_configuration_can_be_archived()
    {
        var setting = CreateDraft();
        setting.Approve(ApprovedAt);
        setting.Activate();
        setting.Archive();
        Assert.Equal(ConfigStatus.Archived, setting.Status);
    }

    [Fact]
    public void Cannot_activate_configuration_without_approval()
    {
        var setting = CreateDraft();

        Assert.Throws<DomainException>(() => setting.Activate());
    }

    [Fact]
    public void Cannot_archive_configuration_that_is_not_active()
    {
        var setting = CreateDraft();

        Assert.Throws<DomainException>(() => setting.Archive());
    }

    [Fact]
    public void Cannot_approve_configuration_that_is_not_draft()
    {
        var setting = CreateDraft();

        setting.Approve(ApprovedAt);

        Assert.Throws<DomainException>(() => setting.Approve(ApprovedAt));
    }
}
