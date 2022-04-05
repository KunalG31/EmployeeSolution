using EmployeeApi.Models;

namespace EmployeeApi.Controllers;

[Route("employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetAllEmployees()
    {
        GetCollectionResponse<GetEmployeeSummaryResponse> response = await _employeeRepository.GetEmployeesAsync();
        return Ok(response);
    }

    [HttpGet("{id:bsonid}")]
    public async Task<ActionResult> GetById(string id)
    {
        var objectId = ObjectId.Parse(id);
        GetEmployeeDetailsResponse response = await _employeeRepository.GetEmployeeByIdAsync(objectId);
        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
        
    }
}
