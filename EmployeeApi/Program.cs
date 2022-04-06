// This compiles to the Program class with a static Main method.
// It is the entry point for our API. When we start the API, it starts here.
// When this is done runnning, the applicaiton quits.

// Prior to .NET 5, Web APIs used an open source library called NewtonSoft.Json
// In .NET 5 + they use their own. This System.Test.Json
using System.Text.Json.Serialization;
using EmployeeApi;
using EmployeeApi.Adapters;
using EmployeesApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("bsonid", typeof(BsonIdConstraint));
});

// Configuration Stuff

builder.Services.Configure<MongoConnectionOptions>(builder.Configuration.GetSection(MongoConnectionOptions.SectionName));

//Debuggign example of reading value from AppSettings.Development.json file
//var thingy = builder.Configuration.GetValue<string>("url");
//Console.WriteLine("Here is the url:" + thingy);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(pol =>
    {
        pol.AllowAnyOrigin();
        pol.AllowAnyHeader();
        pol.AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // No brainer. Always do this.
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // This is optimal, but talk to the team first
}
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Domain Services
builder.Services.AddScoped<IEmployeeRepository, MongoDbEmployeeRepository>();
builder.Services.AddScoped<ILookupSalary, RpcSalaryLookup>();

// Adapter Services
builder.Services.AddSingleton<MongoDbContext>(); // Created "lazily"
//var mongoDbContext = new MongoDbContext();
//// configure this thing, etc.
//builder.Services.AddSingleton(mongoDbContext);

builder.Services.AddHttpClient<SalaryApiContext>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("salaryApiUrl"));
});

// CC [Class Comment] -Above here is configuring "behind the scenes stuff
var app = builder.Build();
// CC- Below is the configuring of HTTP pipeline - How requests and responses are made.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();  // CC - When using Cors. Seems like needed in Angular apps
app.UseAuthorization();

app.MapControllers(); // CC - This is where routing table is created
app.Run(); // CC - This is Kestrel Web Server running!
