using SecureMed.Modules.PatientCare.Domain;
using SecureMed.SharedKernel.Domain; // Of waar jouw FullName staat
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

namespace SecureMed.Modules.PatientCare.Tests.Domain;

public sealed class FullNameTests
{
    [Test]
    [Arguments("Marc", null)]
    [Arguments("Marc", "")]
    [Arguments(null, "Van Ranst")]
    [Arguments("", "Van Ranst")]
    // TUnit vervangt de variabelen in de DisplayName automatisch
    [DisplayName("FullName validatie: Voornaam='$firstName', Achternaam='$lastName'")]
    public async Task FullName_throws_exception_when_name_is_null_or_empty(string? firstName, string? lastName)
    {
        // Act & Assert
        // We verwachten een ArgumentException (of ArgumentNullException)
        await Assert.That(() =>
        {
            _ = FullName.From(firstName!, lastName!);
        }).Throws<ArgumentException>();
    }

    [Test]
    public async Task FullName_creates_valid_instance()
    {
        // Arrange & Act
        var fullName = FullName.From("Marc", "Van Ranst");

        // Assert: Gebruik de vloeiende Member-syntax van TUnit
        await Assert.That(fullName)
            .Member(s => s.FirstName, f => f.IsEqualTo("Marc"))
            .And.Member(s => s.LastName, l => l.IsEqualTo("Van Ranst"));
    }

    [Test]
    public async Task FullName_ToString_returns_combined_name()
    {
        // Act
        var fullName = FullName.From("Marc", "Van Ranst");

        // Assert
        await Assert.That(fullName.ToString()).IsEqualTo("Marc Van Ranst");
    }
}
