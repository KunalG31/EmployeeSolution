using MongoDB.Driver;

namespace EmployeeApi.Adapters;

public class MongoDbContext
{
    private readonly IMongoCollection<Employee> _employeesCollection;
    public MongoDbContext()
    {
        var clinetSettings = MongoClientSettings.FromConnectionString("mongodb://admin:TokyoJoe138!@localhost:27017/");
        var conn = new MongoClient(clinetSettings);
        var db = conn.GetDatabase("employees_db");
        _employeesCollection = db.GetCollection<Employee>("employees");
    }

    public IMongoCollection<Employee> GetEmpoyeeCollection() => _employeesCollection;
}
