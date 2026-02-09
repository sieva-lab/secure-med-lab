using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
        [Required, Length(11,11), BelgianInsz] string Insz);

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

internal sealed class BelgianInszAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is not string insz)
        {
            return new ValidationResult("INSZ must be a string.");
        }

        if (string.IsNullOrWhiteSpace(insz))
        {
            return ValidationResult.Success;
        }

        return IsValidInsz(insz)
            ? ValidationResult.Success
            : new ValidationResult("INSZ is not a valid Belgian national register number.");
    }

    private static bool IsValidInsz(string insz)
    {
        if (insz.Length != 11)
        {
            return false;
        }

        for (var i = 0; i < insz.Length; i++)
        {
            if (!char.IsDigit(insz[i]))
            {
                return false;
            }
        }

        if (!int.TryParse(insz.AsSpan(0, 9), NumberStyles.None, CultureInfo.InvariantCulture, out var baseNumber))
        {
            return false;
        }

        if (!int.TryParse(insz.AsSpan(9, 2), NumberStyles.None, CultureInfo.InvariantCulture, out var checksum))
        {
            return false;
        }

        if (checksum <= 0 || checksum > 97)
        {
            return false;
        }

        var computed = 97 - (baseNumber % 97);
        if (checksum == computed)
        {
            return true;
        }

        var baseNumber2000 = 2000000000L + baseNumber;
        var computed2000 = 97 - (int)(baseNumber2000 % 97);
        return checksum == computed2000;
    }
}
