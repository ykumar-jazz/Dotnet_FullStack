using Microsoft.EntityFrameworkCore;
using mvc_project.EMS.Application.Interfaces;
using mvc_project.EMS.Domain.Entities;
using mvc_project.EMS.Infrastructure.Data;
namespace mvc_project.EMS.Infrastructure.Repositories;

public class EmployeeRepositoryEF :IEmployeeRepository
{
    public readonly AppDbContext _context;
    public EmployeeRepositoryEF(AppDbContext appDbContext)
    {
        _context=appDbContext;
    }
    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task AddAsync(Employee emp)
    {
        await _context.Employees.AddAsync(emp);

        await _context.SaveChangesAsync();
    }

    public Task<Employee?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Employee emp)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

}
