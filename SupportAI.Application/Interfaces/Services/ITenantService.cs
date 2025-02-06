using SupportAI.Shared.DTOs;

namespace SupportAI.Application.Interfaces.Services
{
    public interface ITenantService
    {
        Task<List<TenantDto>> GetAllTenantsAsync();
    }
}
