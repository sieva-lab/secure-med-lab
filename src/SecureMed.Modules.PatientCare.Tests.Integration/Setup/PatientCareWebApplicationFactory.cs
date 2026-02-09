extern alias migrations;

using System.Collections.Concurrent;
using ApiServiceSDK;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using SecureMed.Modules.PatientCare.Data;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using TUnit.Core.Interfaces;

namespace SecureMed.Modules.PatientCare.IntegrationTests.Setup;

public class PatientCareWebApplicationFactory : WebApplicationFactory<Program>, IAsyncInitializer, IAsyncDisposable
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder("postgres:17.6").Build();
    private readonly RedisContainer _redisContainer = new RedisBuilder("redis:7.0").Build();
// Track adapters om ze later netjes op te ruimen
    private readonly ConcurrentBag<HttpClientRequestAdapter> _adapters = new();
    public async Task InitializeAsync()
    {
        // Start containers parallel voor snelheid
        await Task.WhenAll(_postgreSqlContainer.StartAsync(), _redisContainer.StartAsync());

        // Forceer server start
        var client = CreateClient();

        using var scope = Server.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PatientCareDbContext>();

        dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
        await dbContext.Database.MigrateAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        // Ruim eerst de adapters op
        foreach (var adapter in _adapters)
        {
            adapter.Dispose();
        }

        await base.DisposeAsync();
        await _postgreSqlContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.UseSetting("ConnectionStrings:securemed-db", _postgreSqlContainer.GetConnectionString());
        builder.UseSetting("ConnectionStrings:cache", _redisContainer.GetConnectionString());
        builder.UseSetting("Encryption:Key", "test-encryption-key-1234567890123456"); // 32 chars for AES-256

        builder.ConfigureServices(services =>
        {
            services.AddAuthentication(defaultScheme: "Test")
                .AddScheme<AuthenticationSchemeOptions, PatientApiAuthenticationHandler>("Test", options => { });

            services.AddDbContext<PatientCareDbContext>(
                options =>
                    options.UseNpgsql(
                        _postgreSqlContainer.GetConnectionString(),
                        x => x.MigrationsAssembly(typeof(migrations.Program).Assembly.GetName().Name)
                )
            );
        });

        builder.UseEnvironment("IntegrationTest");
    }

    public ApiClient CreateApiClient()
    {
        var client = CreateClient();
        client.Timeout = TimeSpan.FromSeconds(60); // Voorkomt 'The client aborted the request'

        var authProvider = new AnonymousAuthenticationProvider();
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: client);

        // Houd hem bij voor de dispose fase
        _adapters.Add(adapter);
        return new ApiClient(adapter);
    }
}
