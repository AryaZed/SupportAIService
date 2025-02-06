using Microsoft.EntityFrameworkCore;
using SupportAI.Application.Interfaces.Repo;
using SupportAI.Domain.Entities;
using SupportAI.Infrastructure.Persistence;
using SupportAI.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Infrastructure.Repositories
{
    public class TenantRepository(AppDbContext _context) : ITenantRepository
    {
        public async Task<List<TenantDto>> GetAllAsync() =>
            await _context.Tenants.Select(t => new TenantDto(t.Id, t.Name, t.Domain)).ToListAsync();

        public async Task<TenantDto?> GetByIdAsync(Guid id) =>
            await _context.Tenants.Where(t => t.Id == id)
                .Select(t => new TenantDto(t.Id, t.Name, t.Domain))
                .FirstOrDefaultAsync();

        public async Task AddAsync(TenantDto tenant)
        {
            var entity = new Tenant { Id = tenant.Id, Name = tenant.Name, Domain = tenant.Domain};
            await _context.Tenants.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
