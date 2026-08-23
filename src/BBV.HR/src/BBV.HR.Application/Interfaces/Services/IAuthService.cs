using System.Security.Claims;
using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Interfaces.Services;

public interface IAuthService
{
    Task<Employee?> ValidateCredentialsAsync(string email, string password);
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task<Employee?> GetEmployeeByEmailAsync(string email);
    IEnumerable<Claim> GetClaimsForEmployee(Employee employee);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
