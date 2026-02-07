using System.Net;
using Microsoft.AspNetCore.Mvc;
using SecureMed.Modules.PatientCare.IntegrationTests.Setup;
using SecureMed.SharedKernel.StronglyTypedIds;

namespace SecureMed.Modules.PatientCare.IntegrationTests;

[ClassDataSource<PatientCareWebApplicationFactory>(Shared = SharedType.None)]
public class PatientCareApiTests
{
    private readonly PatientCareWebApplicationFactory _webAppFactory;
    public PatientCareApiTests(PatientCareWebApplicationFactory webAppFactory)
    {
        _webAppFactory = webAppFactory;
        VerifierSettings.ScrubMember("Id");
    }

    [Test]
    public async Task RegiserPatient_WithValidData_Returns_CreatedResponse()
    {
        var apiClient = _webAppFactory.CreateApiClient();

        var response = await apiClient.Patients.PostAsync(new ApiServiceSDK.Models.Command()
        {
            FirstName = "John",
            LastName = "Doe",
            Insz = "12345678901",
        });

        await Assert.That(response).IsNotNull();
    }

    [Test]
    public async Task RegiserPatient_WithMinimalData_Returns_CreatedResponse()
    {
        var apiClient = _webAppFactory.CreateApiClient();

        var response = await apiClient.Patients.PostAsync(new ApiServiceSDK.Models.Command()
        {
            FirstName = "Jane",
            LastName = "Smith",
            Insz = "12345678901"
        });

        await Assert.That(response).IsNotNull();
    }

    [Test]
    public async Task RegiserPatient_WithInvalidData_Returns_BadRequestProblemDetails()
    {
        var apiClient = _webAppFactory.CreateApiClient();

        try
        {
            await apiClient.Patients.PostAsync(new ApiServiceSDK.Models.Command()
            {
                FirstName = "A",
                LastName = "B",
                Insz = "123"
            });


            Assert.Fail("Expected ApiException was not thrown");
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            await Assert.That(ex.ResponseStatusCode).IsEqualTo(400);
        }
    }

    [Test]
    public async Task RegiserPatient_WithUnknownProperty_Returns_BadRequestProblemDetails()
    {
        using var client = _webAppFactory.CreateClient();

        // For this test, we'll send a raw HTTP request with unknown property since Kiota's typed client
        // won't allow unknown properties by design. This test verifies server-side validation.
        var request = new
        {
            FirstName = "Firstname",
            LastName = "Lastname",
            UnknownProperty = "This should not be here"
        };

        var response = await client.PostAsJsonAsync("/patients", request);
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        await Assert.That(problemDetails).IsNotNull();
    }

    // [Test]
    // public async Task GetPatients_Returns_OkWithPatientsList()
    // {
    //     var apiClient = _webAppFactory.CreateApiClient();

    //     await apiClient.Patients.PostAsync(new ApiServiceSDK.Models.Command()
    //     {
    //         FirstName = "Test",
    //         LastName = "User",
    //         Insz = "12345678901"
    //     });
    //     await apiClient.Patients.PostAsync(new ApiServiceSDK.Models.Command()
    //     {
    //         FirstName = "Test 2",
    //         LastName = "User 2",
    //         Insz = "12345678902"
    //     });

    //     var patients = await apiClient.Patients.GetAsync();
    //     await Assert.That(patients).IsNotNull();
    //     await Assert.That(patients.Count).IsGreaterThanOrEqualTo(2);

    //     await Verify(patients.OrderBy(c => c.FirstName).ThenBy(c => c.LastName));
    // }

    [Test]
    public async Task GetPatient_WithValidId_Returns_OkWithPatient()
    {
        var apiClient = _webAppFactory.CreateApiClient();

        var createResponse = await apiClient.Patients.PostAsync(new ApiServiceSDK.Models.Command()
        {
            FirstName = "Individual",
            LastName = "Customer",
            Insz = "12345678901"
        });
        var patientId = (string)createResponse!.GetType().GetField("_value", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(createResponse)!;

        var patient = await apiClient.Patients[patientId!.ToString()].GetAsync();
        await Assert.That(patient).IsNotNull();
        await Assert.That(patient!.FirstName).IsEqualTo("Individual");
        await Assert.That(patient.LastName).IsEqualTo("Customer");

        await Verify(patient);
    }

    [Test]
    public async Task GetPatient_WithNonExistentId_Returns_NotFound()
    {
        var apiClient = _webAppFactory.CreateApiClient();

        var nonExistentId = PatientId.New();

        try
        {
            await apiClient.Patients[nonExistentId.ToString()].GetAsync();
            Assert.Fail("Expected ApiException was not thrown");
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            await Assert.That(ex.ResponseStatusCode).IsEqualTo(404);
        }
    }

    [Test]
    public async Task RemovedPatient_IsNotIncluded()
    {
        var apiClient = _webAppFactory.CreateApiClient();

        var createResponse = await apiClient.Patients.PostAsync(new ApiServiceSDK.Models.Command()
        {
            FirstName = "Jane",
            LastName = "Smith",
            Insz = null
        });
        var patientId = (string)createResponse!.GetType().GetField("_value", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(createResponse)!;

        var patient = await apiClient.Patients[patientId!.ToString()].GetAsync();
        await Assert.That(patient).IsNotNull();

        await apiClient.Patients[patientId.ToString()].DeleteAsync();

        try
        {
            await apiClient.Patients[patientId.ToString()].GetAsync();
            Assert.Fail("Expected ApiException was not thrown");
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            await Assert.That(ex.ResponseStatusCode).IsEqualTo(404);
        }
    }
}
