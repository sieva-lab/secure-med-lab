using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.Modules.PatientCare.Application;

// Vertical slice: everything to get a patient by ID in one place
// Parameters, Response and Query handler

public static class GetPatient
{
    // Input: parameters for the query
    public sealed record Parameters([FromRoute] PatientId PatientId);

    // Output: response record
    public sealed record Response(
        PatientId Id,
        string FirstName,
        string LastName,
        string Insz,
        DateTime CreatedAt);

    /// <summary>
    /// Query Handler: Get one patient by ID
    /// </summary>
    public static async Task<Results<Ok<Response>, NotFound>> Query(
        [AsParameters] Parameters parameters,
        [FromServices] IQueryable<Patient> patients, // Directe injectie via EF Core
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        // Filter by id in the database, then project to the Response record
        var patient = await patients
            .AsNoTracking()
            .Where(p => p.Id == parameters.PatientId)
            .Select(p => new Response(
                p.Id,
                p.Name.FirstName,
                p.Name.LastName,
                p.Insz,
                p.CreatedAt
            ))
            .SingleOrDefaultAsync(cancellationToken);

        return patient switch
        {
            null => TypedResults.NotFound(),
            _ => TypedResults.Ok(patient)
        };
    }
}
