using SupportAI.Application.Interfaces.Repo;
using SupportAI.Application.Interfaces.Services;
using SupportAI.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SupportAI.Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<List<TenantDto>> GetAllTenantsAsync()
        {
            return await _tenantRepository.GetAllAsync();
        }
    }

}
