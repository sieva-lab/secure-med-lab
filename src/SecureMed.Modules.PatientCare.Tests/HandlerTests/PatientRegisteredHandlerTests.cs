using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.Modules.PatientCare.Handlers;
using SecureMed.SharedKernel.Messages;
using SecureMed.SharedKernel.StronglyTypedIds;
using TUnit.Assertions;

namespace SecureMed.Modules.PatientCare.Tests.Handlers;

public sealed class PatientRegisteredHandlerTests
{
       [Test]
    public async Task Handler_invalidates_cache_when_patient_is_registered()
    {
        var cache = Substitute.For<HybridCache>();
        var logger = Substitute.For<ILogger<PatientRegisteredHandler>>();
        var handler = new PatientRegisteredHandler();
        var message = new PatientRegistered(PatientId.New(), "John", "Doe");

        await handler.Handle(message, logger, cache, CancellationToken.None);

        await cache.Received(1).RemoveByTagAsync(
                    "patients-list",
                    Arg.Any<CancellationToken>());
    }
}
