using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class EmployeeService : ICatalogService<Employee>
{
    private readonly ICatalogRepository<Employee> _repository;
    private readonly IValidator<Employee> _validator;
    public EmployeeService(ICatalogRepository<Employee> repository, IValidator<Employee> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<int>> Create(Employee employee)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(employee);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            var employeeId = await _repository.Create(employee);

            return Result.Ok(employeeId);
        }
        catch (Exception ex)
        {
            return Result.Fail<int>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<Employee>>> GetAll()
    {
        try
        {
            var employees = await _repository.GetAll();

            return Result.Ok(employees);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<Employee>>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<Employee>> GetById(int id)
    {
        try
        {
            var employee = await _repository.GetById(id);

            if (employee is null)
                return Result.Fail<Employee>(new NotFoundError(nameof(Employee), id));

            return Result.Ok(employee);
        }
        catch (Exception ex)
        {
            return Result.Fail<Employee>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result> Remove(int id)
    {
        try
        {
            await _repository.Remove(id);

            return Result.Ok();
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result> Update(Employee employee)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(employee);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            await _repository.Update(employee);

            return Result.Ok();
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }
}