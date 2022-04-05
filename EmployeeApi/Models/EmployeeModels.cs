namespace EmployeeApi.Models;

public record GetEmployeeDetailsResponse(string id, string firstName, string lastName, string phone, string email, string department);

public record GetEmployeeSummaryResponse(string id, string firstName, string lastName, string department);

public class GetCollectionResponse<T>
{
    public List<T>? Data { get; set; }
}
