using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models;

public record GetEmployeeDetailsResponse(string id, string firstName, string lastName, string phone, string email, string department);

public record GetEmployeeSummaryResponse(string id, string firstName, string lastName, string department);

public class GetCollectionResponse<T>
{
    public List<T>? Data { get; set; }
}



public class PostEmployeeRequest : IValidatableObject
{
    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Department { get; set; } = string.Empty;
    [Required]
    public string Phone { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FirstName.ToLower().Trim() == "darth" && LastName.ToLower().Trim() == "vader" && Department.ToLower().Trim() == "dev")
        {
            yield return new ValidationResult("Sith Belong in Management.", new string[] { nameof(FirstName), nameof(LastName), nameof(Department) });
        }
    }
}
