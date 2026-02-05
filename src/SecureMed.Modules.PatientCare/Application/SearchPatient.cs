using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.SharedKernel.Infrastructure.Security;
using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.Modules.PatientCare.Application;

public static class SearchPatient
{
    // Door 'Parameters' te gebruiken houden we het endpoint clean
    public sealed record Parameters([FromQuery] string Insz);

    public sealed record Response(
        PatientId Id,
        string FirstName,
        string LastName,
        string Insz);

    /// <summary>
    /// Zoek patiënten op basis van criteria. Momenteel ondersteunt dit
    /// de blind index lookup via INSZ.
    /// </summary>
    public static async Task<Results<Ok<Response>, NotFound>> Query(
        [AsParameters] Parameters parameters,
        [FromServices] IQueryable<Patient> patients,
        [FromServices] IEncryptionService encryptionService,
        CancellationToken ct)
    {

        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(encryptionService);

        if (string.IsNullOrWhiteSpace(parameters.Insz))
        {
            return TypedResults.NotFound();
        }

        // We gebruiken de service om de zoekterm om te zetten naar de hash
        var searchHash = encryptionService.CreateHash(parameters.Insz);

        var result = await patients
            .AsNoTracking()
            .Where(p => p.InszHash == searchHash)
            .Select(p => new Response(
                p.Id,
                p.Name.FirstName,
                p.Name.LastName,
                p.Insz
            ))
            .SingleOrDefaultAsync(ct);

        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
}
