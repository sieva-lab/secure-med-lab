using Microsoft.AspNetCore.Builder;

namespace SecureMed.SharedKernel.Modules;

public interface IModule
{
    WebApplicationBuilder AddModule(WebApplicationBuilder builder);
    WebApplication UseModule(WebApplication app);
}
