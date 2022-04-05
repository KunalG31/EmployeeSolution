using EmployeeApi.Models;

namespace EmployeeApi.Controllers;

public class EmployeesController : ControllerBase
{
    [HttpGet("employees/{id}")]
    public async Task<ActionResult> GetById(string id)
    {
        var response = new GetEmployeeDetailsResponse(id, "Joe", "Schmidt", "888-1212", "joe@aol.com", "Sales");
        return Ok(response);
    }
}
