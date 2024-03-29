﻿using EmployeeApi.Models;

namespace EmployeeApi.Controllers;

[Route("employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }


    //[HttpPut("{id:bsonid}/email")]
    //public async Task<ActionResult> UpdateEmail(string id, [FromBody]string email)
    //{
    //    var objectId = ObjectId.Parse(id);
    //    bool found = await _employeeRepository.ChangeEmailAsync(objectId, email);
    //    return found switch
    //    {
    //        true => Accepted(),
    //        false => NotFound()
    //    };
    //}

    [HttpPut("{id:bsonid}/email")]
    public async Task<ActionResult> UpdateEmail(string id, [FromBody] string email)
    {
        var objectId = ObjectId.Parse(id); // try catch.
        bool found = await _employeeRepository.ChangePropertyAsync(objectId, emp => emp.Email, email);

        return found switch
        {
            true => Accepted(),
            false => NotFound()
        };


    }

    [HttpPut("{id:bsonid}/phone")]
    public async Task<ActionResult> UpdatePhone(string id, [FromBody] string phone)
    {
        var objectId = ObjectId.Parse(id); // try catch.
        bool found = await _employeeRepository.ChangePropertyAsync(objectId, emp => emp.Phone, phone);

        return found switch
        {
            true => Accepted(),
            false => NotFound()
        };

    }


    [HttpDelete("{id:bsonid}")]
    public async Task<ActionResult> RemoveEmployee(string id)
    {
        var objectId = ObjectId.Parse(id);
        await _employeeRepository.FireAsync(objectId);
        return NoContent();
    }

    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Client)] // this is for the thing we are sending down
    [HttpPost]
    public async Task<ActionResult> AddEmployee([FromBody] PostEmployeeRequest request)
    {
        // 1. Validate it. If it's bad, return a 400. (Bad Request)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        // 2. Save the thing to the database ...
        GetEmployeeDetailsResponse response = await _employeeRepository.HireEmployee(request);

        // 3. Return
        //    - 201 Created Status Code
        //    - Include a header in the response with the Location of the new employee
        //      Location: http://localhost:1337/employees/39323463264287
        //      - Just send them a copy of whatever they would get if they want to that location
        return CreatedAtRoute("employees#getbyid", new { id = response.id }, response);
    }

    [HttpGet("")]
    public async Task<ActionResult> GetAllEmployees()
    {
        GetCollectionResponse<GetEmployeeSummaryResponse> response = await _employeeRepository.GetEmployeesAsync();
        return Ok(response);
    }

    // GET /employees/:id
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Client)]
    [HttpGet("{id:bsonid}", Name = "employees#getbyid")] // it won't even create this controller if that id isn't a valid bsonid (return 404)
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
