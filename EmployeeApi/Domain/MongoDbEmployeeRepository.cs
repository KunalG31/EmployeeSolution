using EmployeeApi.Adapters;
using EmployeeApi.Models;
using MongoDB.Driver;

namespace EmployeeApi.Domain;

public class MongoDbEmployeeRepository : IEmployeeRepository
{
    private readonly MongoDbContext _context;

    public MongoDbEmployeeRepository(MongoDbContext context)
    {
        _context = context;
    }
    public async Task<GetEmployeeDetailsResponse?> GetEmployeeByIdAsync(ObjectId id)
    {
        // In the context we have employees, but we need a GetEmployeeDetailResponse
        var projection = Builders<Employee>.Projection.Expression(emp =>
       new GetEmployeeDetailsResponse(emp.Id.ToString(), emp.FirstName, emp.LastName, emp.Phone, emp.Email, emp.Department)
        );
        //var response = new GetEmployeeDetailsResponse(id, "Joe", "Schmidt", "888-1212", "joe@aol.com", "Sales");
        var response = await _context.GetEmpoyeeCollection().Find(options => options.Id == id)
            .Project(projection)
            .SingleOrDefaultAsync(); // Todo
        return response;
    }

    public async Task<GetCollectionResponse<GetEmployeeSummaryResponse>> GetEmployeesAsync()
    {
        var projection = Builders<Employee>.Projection.Expression(emp => new GetEmployeeSummaryResponse(emp.Id.ToString(), emp.FirstName,
            emp.LastName, emp.Department));

        var employees = await _context.GetEmpoyeeCollection().Find(_ => true) // Give them all to me
            .Project(projection)
            .ToListAsync();

        return new GetCollectionResponse<GetEmployeeSummaryResponse>() {  Data = employees };
    }

    public async Task<GetEmployeeDetailsResponse> HireEmployee(PostEmployeeRequest request)
    {
        var employeeToAdd = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            Department = request.Department,
            Salary = 100000
        };
        await _context.GetEmpoyeeCollection().InsertOneAsync(employeeToAdd);

        return new GetEmployeeDetailsResponse(employeeToAdd.Id.ToString(), employeeToAdd.FirstName, employeeToAdd.LastName, employeeToAdd.Phone, employeeToAdd.Email, employeeToAdd.Department);
    }
}
