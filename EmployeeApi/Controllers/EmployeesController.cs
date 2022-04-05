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

    [HttpPost]
    public async Task<ActionResult> AddEmployee([FromBody] PostEmployeeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // 1. Validate it. If it's bad, return a 400. (Bad Request)

        // 2. Save the thing to the database ...

        // 3. Return
        //    - 201 Created Status Code
        //    - Include a header in the response with the Location of the new employee
        //      Location: http://localhost:1337/employees/39323463264287
        //      - Just send them a copy of whatever they would get if they want to that location
        return Ok(request);
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
