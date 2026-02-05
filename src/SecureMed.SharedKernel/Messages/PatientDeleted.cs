using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.SharedKernel.Messages;

public sealed record PatientDeleted(PatientId Id, string FirstName, string LastName);
