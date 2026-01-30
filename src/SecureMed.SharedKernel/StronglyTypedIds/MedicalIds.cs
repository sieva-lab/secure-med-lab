using Vogen;

namespace SecureMed.SharedKernel.StronglyTypedIds;

[ValueObject<Guid>(conversions: Conversions.SystemTextJson | Conversions.EfCoreValueConverter)]
public readonly partial struct PatientId
{
    public static PatientId New() => From(Guid.NewGuid());
}

[ValueObject<Guid>(conversions: Conversions.SystemTextJson | Conversions.EfCoreValueConverter)]
public readonly partial struct AdmissionId
{
    public static AdmissionId New() => From(Guid.NewGuid());
}

[ValueObject<Guid>(conversions: Conversions.SystemTextJson | Conversions.EfCoreValueConverter)]
public readonly partial struct StaffId
{
    public static StaffId New() => From(Guid.NewGuid());
}
