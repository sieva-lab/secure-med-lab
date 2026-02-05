using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SecureMed.SharedKernel.Messages;
using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.Modules.PatientCare.Handlers;


// Partial class to enable source-generated logging
public partial class PatientDeletedHandler
{

    public async Task Handle(
        PatientDeleted message,
        ILogger<PatientDeletedHandler> logger,
        [FromKeyedServices("PatientCare")] HybridCache cache,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(cache);

        // 1. High-performance logging
        LogPatientDeleted(logger, message.Id, message.FirstName, message.LastName);

        // 2. Cache Invalidation
        // Remove the cache entry for the deleted patient, if it exists
        await cache.RemoveAsync($"patient-{message.Id}", ct);

        // Remove the tag 'patients-list' so that all patient lists are invalidated
        await cache.RemoveByTagAsync("patients-list", ct);
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Patient {PatientId} deleted: {FirstName} {LastName}. Invaliding cache.")]
    public static partial void LogPatientDeleted(
        ILogger logger,
        PatientId patientId,
        string firstName,
        string lastName);
}
