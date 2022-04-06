using System.Linq.Expressions;
using EmployeeApi.Models;

namespace EmployeeApi.Domain;

public interface IEmployeeRepository
{
    Task<GetEmployeeDetailsResponse?> GetEmployeeByIdAsync(ObjectId id);
    Task<GetCollectionResponse<GetEmployeeSummaryResponse>> GetEmployeesAsync();
    Task<GetEmployeeDetailsResponse> HireEmployee(PostEmployeeRequest request);
    Task FireAsync(ObjectId objectId);

    //Task<bool> ChangeEmailAsync(ObjectId objectId, string email);

    Task<bool> ChangePropertyAsync<TField>(ObjectId id, Expression<Func<Employee, TField>> field, TField value);
}
