using Microsoft.Data.SqlClient;
using mvc_project.EMS.Application.Interfaces;
using mvc_project.EMS.Domain.Entities;

namespace mvc_project.EMS.Infrastructure.Repositories;

public class EmployeeRepositoryADO : IEmployeeRepository
{
    private readonly IConfiguration _config;

    public EmployeeRepositoryADO(
        IConfiguration config)
    {
        _config = config;
    }
    public Task AddAsync(Employee emp)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

   public async Task<List<Employee>> GetAllAsync()
    {
        List<Employee> list = [];

        using SqlConnection con =
            new (
                _config.GetConnectionString(
                    "DefaultConnection"));

        string query = "SELECT * FROM Employees";

        SqlCommand cmd = new(query, con);

        await con.OpenAsync();

        SqlDataReader dr =
            await cmd.ExecuteReaderAsync();

        while(await dr.ReadAsync())
        {
            list.Add(new Employee
            {
                Id = Convert.ToInt32(dr["Id"]),
                Name = dr["Name"].ToString()
            });
        }

        return list;
    }
    public Task<Employee?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Employee emp)
    {
        throw new NotImplementedException();
    }

}
