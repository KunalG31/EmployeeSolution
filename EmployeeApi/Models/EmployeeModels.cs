namespace EmployeeApi.Models;

public record GetEmployeeDetailsResponse(string id, string firstName, string lastName, string phone, string email, string department);
