using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SecureMed.SharedKernel.Messages;
using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.Modules.PatientCare.Handlers;


// Partial class to enable source-generated logging
public partial class PatientRegisteredHandler
{

    public async Task Handle(
        PatientRegistered message,
        ILogger<PatientRegisteredHandler> logger,
        [FromKeyedServices("PatientCare")] HybridCache cache,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(cache);

        // 1. High-performance logging
        LogPatientRegistered(logger, message.Id, message.FirstName, message.LastName);

        // 2. Cache Invalidation
        // We verwijderen de tag 'patients-list' zodat alle overzichten
        // ververst worden bij de volgende opvraag.
        await cache.RemoveByTagAsync("patients-list", ct);
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Patient {PatientId} registered: {FirstName} {LastName}. Invaliding cache.")]
    public static partial void LogPatientRegistered(
        ILogger logger,
        PatientId patientId,
        string firstName,
        string lastName);
}
