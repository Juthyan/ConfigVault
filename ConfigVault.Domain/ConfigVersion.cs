namespace ConfigVault.Domain;

public class ConfigVersion
{
    public ConfigStatus Status { get; private set; }

    public ConfigVersion()
    {
        Status = ConfigStatus.Draft;
    }

    public void Approve()
    {
        if (Status != ConfigStatus.Draft)
            throw new DomainException("Only draft configurations can be approved.");

        Status = ConfigStatus.Approved;
    }

    public void Activate()
    {
        if (Status != ConfigStatus.Approved)
            throw new DomainException("A configuration must be approved before activation.");

        Status = ConfigStatus.Active;
    }

    public void Archive()
    {
        if (Status != ConfigStatus.Active)
            throw new DomainException("Only active configurations can be archived.");

        Status = ConfigStatus.Archived;
    }
}