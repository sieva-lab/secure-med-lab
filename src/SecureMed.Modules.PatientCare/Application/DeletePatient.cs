using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureMed.Modules.PatientCare.Data;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.SharedKernel.Messages;
using SecureMed.SharedKernel.StronglyTypedIds;
using Wolverine.EntityFrameworkCore;

namespace SecureMed.Modules.PatientCare.Application;

public static class DeletePatient
{
    public sealed record Command([FromRoute] PatientId PatientId);

    /// <summary>
    /// Remove a patient (Soft Delete).
    /// The ISoftDelete interceptor ensures that the patient is not physically deleted,
    /// but marked as deleted.
    /// </summary>
    public static async Task<Results<NoContent, NotFound>> Handle(
        [AsParameters] Command command,
        [NotNull][FromServices] IDbContextOutbox<PatientCareDbContext> outbox,
        CancellationToken ct)
    {

        // 1. Find the patient by ID. If not found, return 404.
        var patient = await outbox.DbContext.Set<Patient>()
            .SingleOrDefaultAsync(p => p.Id == command.PatientId, ct);

        if (patient is null)
        {
            return TypedResults.NotFound();
        }

        // 2. Use the DbContext to remove the patient. The ISoftDelete interceptor will handle this as a soft delete.)'
        outbox.DbContext.Remove(patient);

        // 3. Publish event that patient was deleted
        await outbox.PublishAsync(new PatientDeleted(patient.Id, patient.Name.FirstName, patient.Name.LastName));

        // 4. Save changes and publish events in one transaction
        await outbox.SaveChangesAndFlushMessagesAsync(ct);

        return TypedResults.NoContent();
    }
}
