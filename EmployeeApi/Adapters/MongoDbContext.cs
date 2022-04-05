using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace EmployeeApi.Adapters;

public class MongoDbContext
{
    private readonly IMongoCollection<Employee> _employeesCollection;
    private readonly ILogger<MongoDbContext> _logger;
    
    public MongoDbContext(ILogger<MongoDbContext> logger, IOptions<MongoConnectionOptions> connectionOptions) 
    {
        _logger = logger;
        var clientSettings = MongoClientSettings.FromConnectionString(connectionOptions.Value.ConnectionString);
        // Maybe only turn this on during development .... more later.
        if (connectionOptions.Value.LogCommands)
        {
            clientSettings.ClusterConfigurator = db =>
            {
                db.Subscribe<CommandStartedEvent>(e =>
                {
                    _logger.LogInformation($"Running {e.CommandName} - the command looks like this {e.Command.ToJson()}");
                });
            };
        }
        
        var conn = new MongoClient(clientSettings);

        var db = conn.GetDatabase("employees_db");

        _employeesCollection = db.GetCollection<Employee>("employees");
    }

    public IMongoCollection<Employee> GetEmpoyeeCollection() => _employeesCollection;
}
