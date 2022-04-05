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
    public async Task<GetEmployeeDetailsResponse> GetEmployeeByIdAsync(ObjectId id)
    {
        // In the context we have employees, but we need a GetEmployeeDetailResponse
        var projection = Builders<Employee>.Projection.Expression(emp =>
       new GetEmployeeDetailsResponse(emp.Id.ToString(), emp.FirstName, emp.LastName, emp.Phone, emp.Email, emp.Departmant)
        );
        //var response = new GetEmployeeDetailsResponse(id, "Joe", "Schmidt", "888-1212", "joe@aol.com", "Sales");
        var response = await _context.GetEmpoyeeCollection().Find(options => options.Id == id)
            .Project(projection)
            .SingleOrDefaultAsync(); // Todo
        return response;
    }
}
