namespace EmployeeApi.Domain;

public class SalaryResponse
{
    public decimal amount { get; set; }
}

public class SalaryRequest
{
    public string department { get; set; } = "";
}
