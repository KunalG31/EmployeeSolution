// This compiles to the Program class with a static Main method.
// It is the entry point for our API. When we start the API, it starts here.
// When this is done runnning, the applicaiton quits.

// Prior to .NET 5, Web APIs used an open source library called NewtonSoft.Json
// In .NET 5 + they use their own. This System.Test.Json
using System.Text.Json.Serialization;
using EmployeeApi.Adapters;
using EmployeesApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("bsonid", typeof(BsonIdConstraint));
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

// Adapter Services
builder.Services.AddSingleton<MongoDbContext>(); // Created "lazily"
//var mongoDbContext = new MongoDbContext();
//// configure this thing, etc.
//builder.Services.AddSingleton(mongoDbContext);

// CC [Class Comment] -Above here is configuring "behind the scenes stuff
var app = builder.Build();
// CC- Below is the configuring of HTTP pipeline - How requests and responses are made.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers(); // CC - This is where routing table is created

app.Run(); // CC - This is Kestrel Web Server running!
