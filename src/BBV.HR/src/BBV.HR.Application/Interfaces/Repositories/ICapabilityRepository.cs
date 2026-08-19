using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Interfaces.Repositories;

public interface ICapabilityRepository
{
    Task<IEnumerable<Capability>> GetAllAsync(string? search, string? category);
    Task<Capability?> GetByIdAsync(Guid id);
    Task<bool> ExistsNameAsync(string name, Guid? excludeId = null);
    Task<bool> IsInUseAsync(Guid id);
    Task<Capability> AddAsync(Capability capability);
    Task UpdateAsync(Capability capability);
    Task DeleteAsync(Capability capability);
}