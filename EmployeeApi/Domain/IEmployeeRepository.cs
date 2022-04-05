using EmployeeApi.Models;

namespace EmployeeApi.Domain;

public interface IEmployeeRepository
{
    Task<GetEmployeeDetailsResponse?> GetEmployeeByIdAsync(ObjectId id);
    Task<GetCollectionResponse<GetEmployeeSummaryResponse>> GetEmployeesAsync();
}
