namespace EmployeeApi.Domain;

public interface ILookupSalary
{
    Task<decimal> GetSalaryForNewHireAsync(string department);
}
