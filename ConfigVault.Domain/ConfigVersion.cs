using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ConfigVault.Domain;

public class ConfigVersion
{
    public const int MaxKeyLength = 100;
    public const int MaxContentBytes = 64 * 1024;
    public Guid Id { get; private init; }
    public string Key { get; private set; } = null!;
    public int VersionNumber { get; private set; }
    public string Content { get; private set; } = null!;
    public ConfigStatus Status { get; private set; } = ConfigStatus.Draft;

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ApprovedAt { get; private set; }
    public DateTimeOffset? ActivatedAt { get; private set; }
    public DateTimeOffset? ArchivedAt { get; private set; }
    public DateTimeOffset? RejectedAt { get; private set; }

    private ConfigVersion() { }

    public void Approve(DateTimeOffset at)
    {
        if (Status != ConfigStatus.Draft)
            throw new DomainException("Seule une version en Draft peut être approuvée.");

        if (at < CreatedAt)
            throw new DomainException("La date d'approbation ne peut pas précéder la création.");

        Status = ConfigStatus.Approved;
        ApprovedAt = at;
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

    public void Rejected()
    {

    }

    public static ConfigVersion CreateDraft(
        string key, string content, DateTimeOffset createdAt)
    {
        ValidateKey(key);
        ValidateContent(content);

        return new ConfigVersion
        {
            Id = Guid.NewGuid(),
            Key = key,
            Content = content,
            Status = ConfigStatus.Draft,
            CreatedAt = createdAt
        };
    }

    private static readonly Regex KeyFormat =
        new(@"^[a-z0-9]+([.-][a-z0-9]+)*\z", RegexOptions.Compiled);
    private static void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new DomainException("Key is required.");

        if (key.Length > MaxKeyLength)
            throw new DomainException(
                $"Key is {key.Length} characters long; the limit is {MaxKeyLength}.");

        if (!KeyFormat.IsMatch(key))
            throw new DomainException(
                $"Key '{key}' is invalid. Expected format: lowercase letters, digits, " +
                "dots and hyphens, with no leading, trailing or consecutive separators " +
                "(example: genetec.health).");
    }
    private static void ValidateVersionNumber(int versionNumber)
    {

        if (versionNumber < 1)
        {
            throw new DomainException("Version Number incorrect");
        }
    }
    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Le contenu est obligatoire.");

        var byteCount = Encoding.UTF8.GetByteCount(content);
        if (byteCount > MaxContentBytes)
            throw new DomainException(
                $"Le contenu fait {byteCount} octets, la limite est de {MaxContentBytes}.");

        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(content);
        }
        catch (JsonException)
        {
            throw new DomainException("Le contenu n'est pas un JSON valide.");
        }

        using (document)
        {
            if (document.RootElement.ValueKind != JsonValueKind.Object)
                throw new DomainException(
                    "Le contenu doit être un objet JSON (entre accolades).");

            if (!document.RootElement.EnumerateObject().Any())
                throw new DomainException(
                    "Le contenu ne peut pas être un objet vide.");
        }
    }
}