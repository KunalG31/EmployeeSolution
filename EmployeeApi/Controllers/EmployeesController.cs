using EmployeeApi.Models;

namespace EmployeeApi.Controllers;

public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet("employees/{id:bsonid}")]
    public async Task<ActionResult> GetById(string id)
    {
        var objectId = ObjectId.Parse(id);
        GetEmployeeDetailsResponse response = await _employeeRepository.GetEmployeeByIdAsync(objectId);
        return Ok(response);
    }
}
