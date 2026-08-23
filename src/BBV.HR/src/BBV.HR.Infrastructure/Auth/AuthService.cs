using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Interfaces.Services;

namespace BBV.HR.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IEmployeeRepository _employeeRepository;

    public AuthService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee?> ValidateCredentialsAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var employee = await _employeeRepository.GetByEmailAsync(email.Trim());
        if (employee == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(employee.PasswordHash))
        {
            return null;
        }

        bool isValid = VerifyPassword(password, employee.PasswordHash);
        return isValid ? employee : null;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
    {
        return await _employeeRepository.GetByIdAsync(id);
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        return await _employeeRepository.GetByEmailAsync(email.Trim());
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch
        {
            return false;
        }
    }
}
