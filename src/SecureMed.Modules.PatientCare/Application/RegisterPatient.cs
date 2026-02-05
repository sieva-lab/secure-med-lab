using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SecureMed.Modules.PatientCare.Data;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.SharedKernel.Messages;
using SecureMed.SharedKernel.StronglyTypedIds;
using Wolverine.EntityFrameworkCore;

namespace SecureMed.Modules.PatientCare.Application;

public static class RegisterPatient
{
    // 1. Input: Command with validation attributes
    public sealed record Command(
        [Required, MinLength(2)] string FirstName,
        [Required, MinLength(2)] string LastName,
        [Required, Length(11,11)] string Insz);

    // 2. The "Handler"
    // Wolverine finds this method via convention (public, static, named "Handle", first parameter is Command)
    public static async Task<Created<PatientId>> Handle(
        [FromBody] Command command,
        [NotNull][FromServices] IDbContextOutbox<PatientCareDbContext> outbox,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Maak de entiteit aan via je domein logica
        var patient = Patient.Create(command.FirstName, command.LastName, command.Insz);

        // Add patient to the database through the outbox, publish event and save changes in one transaction
        await outbox.DbContext.AddAsync(patient, cancellationToken);
        await outbox.PublishAsync(new PatientRegistered(patient.Id, patient.Name.FirstName, patient.Name.LastName));
        await outbox.SaveChangesAndFlushMessagesAsync(cancellationToken);

        // Give back the ID of the created patient
        return TypedResults.Created($"/patients/{patient.Id.Value}", patient.Id);
    }
}
