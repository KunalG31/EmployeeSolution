using System.Linq.Expressions;
using EmployeeApi.Adapters;
using EmployeeApi.Models;
using MongoDB.Driver;

namespace EmployeeApi.Domain;

public class MongoDbEmployeeRepository : IEmployeeRepository
{
    private readonly MongoDbContext _context;
    private readonly FilterDefinition<Employee>  _onlyActiveEmployees;

    public MongoDbEmployeeRepository(MongoDbContext context)
    {
        _context = context;
        _onlyActiveEmployees = Builders<Employee>.Filter.Where(emp => emp.InActive != true);
    }

    public async Task<bool> ChangePropertyAsync<TField>(ObjectId id, Expression<Func<Employee, TField>> field, TField value)
    {
        var employeeFilter = Builders<Employee>.Filter.Where(emp => emp.Id == id);
        var filter = Builders<Employee>.Filter.And(_onlyActiveEmployees, employeeFilter);

        var update = Builders<Employee>.Update.Set(field, value);
        var result = await _context.GetEmployeeCollection().UpdateOneAsync(filter, update);
        return result.ModifiedCount == 1;
    }

    //public async Task<bool> ChangeEmailAsync(ObjectId objectId, string email)
    //{
    //    var employeefilter = Builders<Employee>.Filter.Where(emp => emp.Id == objectId);
    //    var filter = Builders<Employee>.Filter.And(_onlyActiveEmployees, employeefilter);

    //    var update = Builders<Employee>.Update.Set(emp => emp.Email, email);
    //    var result = await _context.GetEmpoyeeCollection().UpdateOneAsync(filter, update);
    //    return result.MatchedCount == 1;
    //}

    public async Task FireAsync(ObjectId objectId)
    {
        var filter = Builders<Employee>.Filter.Where(emp => emp.Id == objectId);
        var update = Builders<Employee>.Update.Set(emp => emp.InActive, true);
        await _context.GetEmployeeCollection().UpdateOneAsync(filter, update);
    }

    public async Task<GetEmployeeDetailsResponse?> GetEmployeeByIdAsync(ObjectId id)
    {
        // In the context we have employees, but we need a GetEmployeeDetailResponse
        var projection = Builders<Employee>.Projection.Expression(emp =>
       new GetEmployeeDetailsResponse(emp.Id.ToString(), emp.FirstName, emp.LastName, emp.Phone, emp.Email, emp.Department)
        );
        //var response = new GetEmployeeDetailsResponse(id, "Joe", "Schmidt", "888-1212", "joe@aol.com", "Sales");
        var filterByThisEmployee = Builders<Employee>.Filter.Where(emp => emp.Id == id);
        var filter = Builders<Employee>.Filter.And(_onlyActiveEmployees, filterByThisEmployee);
        var response = await _context.GetEmployeeCollection().Find(filter)
            .Project(projection)
            .SingleOrDefaultAsync(); // Todo
        return response;
    }

    public async Task<GetCollectionResponse<GetEmployeeSummaryResponse>> GetEmployeesAsync()
    {
        var projection = Builders<Employee>.Projection.Expression(emp => new GetEmployeeSummaryResponse(emp.Id.ToString(), emp.FirstName,
            emp.LastName, emp.Department));

        //var employees = await _context.GetEmpoyeeCollection().Find(_ => true) // Give them all to me
        var employees = await _context.GetEmployeeCollection().Find(_onlyActiveEmployees) // Give only active to me
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
        await _context.GetEmployeeCollection().InsertOneAsync(employeeToAdd);

        return new GetEmployeeDetailsResponse(employeeToAdd.Id.ToString(), employeeToAdd.FirstName, employeeToAdd.LastName, employeeToAdd.Phone, employeeToAdd.Email, employeeToAdd.Department);
    }
}
