using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class EmployeeRepository : ICatalogRepository<Employee>
{
    private readonly HTContext _context;
    public EmployeeRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Employee employee)
    {
        await _context.Employees.AddAsync(employee);

        await _context.SaveChangesAsync();

        return employee.Id;
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        var employees = await _context.Employees.ToListAsync();
        return employees;
    }

    public async Task<Employee?> GetById(int id)
    {
        var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (existingEmployee == default) 
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        return existingEmployee;
    }

    public async Task Remove(int id)
    {
        var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (existingEmployee == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        _context.Employees.Remove(existingEmployee);

        await _context.SaveChangesAsync();
    }

    public async Task Update(Employee employee)
    {
        var employeeIsExist = _context.Employees.Any(e => e.Id == employee.Id);

        if (!employeeIsExist)
            throw new NullReferenceException($"Объект с ID {employee.Id} не найден!");

        _context.Employees.Update(employee);

        await _context.SaveChangesAsync();
    }
}