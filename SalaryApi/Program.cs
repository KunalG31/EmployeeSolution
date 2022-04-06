using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISystemTime, TestingSystemTime>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/greeting", ([FromServices] ISystemTime clock) =>
{
    var response = new Greeting("Hello from a minimal API!", clock.GetCurrent());
    return response;
});

app.MapPost("/salary-requests", ([FromBody] SalaryRequest request) =>
{
    var department = request.department.Trim().ToLower();
    decimal salary = department switch
    {
        "dev" => 180000,
        "qa" => 170000,
        "ceo" => 250000,
        _ => 80000
    };
    return new SalaryResponse(salary);
});


app.Run();



public record Greeting(string message, DateTime generated);

public record SalaryRequest(string department);

public record SalaryResponse(decimal amount);

public interface ISystemTime
{
    DateTime GetCurrent();
}

public class SystemTime : ISystemTime
{
    public DateTime GetCurrent()
    {
        return DateTime.Now;
    }
}

public class TestingSystemTime : ISystemTime
{
    public DateTime GetCurrent()
    {
        return new DateTime(1969, 4, 20, 23, 59, 00);
    }
}
