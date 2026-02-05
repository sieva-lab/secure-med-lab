using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.SharedKernel.Messages;

public sealed record PatientRegistered(PatientId Id, string FirstName, string LastName);
