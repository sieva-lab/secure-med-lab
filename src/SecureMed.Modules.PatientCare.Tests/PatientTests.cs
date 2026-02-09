using Microsoft.Extensions.Time.Testing;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.SharedKernel.Domain;
using SecureMed.SharedKernel.StronglyTypedIds;
using TUnit.Assertions; // Belangrijk voor de Assert.That syntax

namespace SecureMed.Modules.PatientCare.Tests.Domain;

public sealed class PatientTests
{
    [Test]
    public async Task Patient_is_created_with_correct_properties()
    {
        // Arrange
        var firstName = "Marc";
        var lastName = "Van Ranst";
        var insz = "84010112366";

        // Act
        var patient = Patient.Create(firstName, lastName, insz);

        // Assert
        await Assert.That(patient.Name.FirstName).IsEqualTo(firstName);
        await Assert.That(patient.Name.LastName).IsEqualTo(lastName);
        await Assert.That(patient.Insz).IsEqualTo(insz);
        await Assert.That(patient.Id).IsNotDefault();
        await Assert.That(patient.Id.Value).IsNotEqualTo(Guid.Empty);
    }

    [Test]
    public async Task Patient_FullName_ToString_is_formatted_correctly()
    {
        var patient = Patient.Create("Marc", "Van Ranst", "84010112366");

        await Assert.That(patient.Name.ToString()).IsEqualTo("Marc Van Ranst");
    }

    [Test]
    public async Task Patient_is_soft_deleted_sets_deleted_at_timestamp()
    {
        // Arrange
        var patient = Patient.Create("Marc", "Van Ranst", "84010112366");
        var fakeTime = new FakeTimeProvider();
        var now = new DateTimeOffset(2026, 2, 5, 10, 0, 0, TimeSpan.Zero);
        fakeTime.SetUtcNow(now);

        // Act
        // We gebruiken de interface methode die je in de SharedKernel hebt gedefinieerd
        ((ISoftDelete)patient).Delete(fakeTime);

        // Assert
        await Assert.That((patient as ISoftDelete).IsDeleted).IsTrue();
        await Assert.That(patient.DeletedAt).IsEqualTo(now);
    }
}
