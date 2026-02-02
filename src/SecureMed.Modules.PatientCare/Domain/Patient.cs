using SecureMed.SharedKernel.Attributes;
using SecureMed.SharedKernel.Domain;
using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.Modules.PatientCare.Domain;

public sealed class Patient: ISoftDelete, IAuditable
{
    public PatientId Id { get; private set; }

    [Encrypted]
    public FullName Name { get; private set; } // PII - moet versleuteld worden

    [Encrypted]
    public string Insz { get; private set; } = default!; // PII - moet versleuteld worden

    [SearchableHash(nameof(Insz))]
    public string InszHash { get; private set; } = default!;

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }


    public static Patient Create(string firstName, string lastName, string insz)
    {
        return new Patient
        {
            Id = PatientId.New(),
            Name = FullName.From(firstName, lastName),
            Insz = insz,
            CreatedAt = DateTime.UtcNow
        };
    }
}
