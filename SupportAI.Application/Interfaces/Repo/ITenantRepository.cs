using SupportAI.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Application.Interfaces.Repo
{
    public interface ITenantRepository
    {
        Task<List<TenantDto>> GetAllAsync();
        Task<TenantDto?> GetByIdAsync(Guid id);
        Task AddAsync(TenantDto tenant);
    }
}
