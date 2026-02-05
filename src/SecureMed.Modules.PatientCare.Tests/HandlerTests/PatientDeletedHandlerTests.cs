using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SecureMed.Modules.PatientCare.Handlers;
using SecureMed.SharedKernel.Messages;
using SecureMed.SharedKernel.StronglyTypedIds;
using TUnit.Assertions;

namespace SecureMed.Modules.PatientCare.Tests.Handlers;

public sealed class PatientDeletedHandlerTests
{
    [Test]
    public async Task Handler_invalidates_cache_when_patient_is_deleted()
    {
        // Arrange
        var cache = Substitute.For<HybridCache>();
        var logger = Substitute.For<ILogger<PatientDeletedHandler>>();
        var handler = new PatientDeletedHandler();
        var patientId = PatientId.New();
        var message = new PatientDeleted(patientId, "Marc", "Van Ranst");

        // Act
        await handler.Handle(message, logger, cache, CancellationToken.None);

        // Assert
        await cache.Received(1).RemoveByTagAsync(
            "patients-list",
            Arg.Any<CancellationToken>());

        await cache.Received(1).RemoveAsync(
            $"patient-{patientId}",
            Arg.Any<CancellationToken>());
            }
}
