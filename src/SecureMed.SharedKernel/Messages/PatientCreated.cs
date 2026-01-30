using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.SharedKernel.Messages;

public sealed record PatientCreated(PatientId Id, string FirstName, string LastName);
