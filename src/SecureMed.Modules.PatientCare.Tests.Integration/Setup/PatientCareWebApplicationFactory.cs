extern alias migrations;

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

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await _redisContainer.StartAsync();

        _ = Server;

        using var scope = Server.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PatientCareDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public override async ValueTask DisposeAsync()
    {
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
        var authProvider = new AnonymousAuthenticationProvider();
        using var adapter = new HttpClientRequestAdapter(authProvider, httpClient: client);
        return new ApiClient(adapter);
    }
}
