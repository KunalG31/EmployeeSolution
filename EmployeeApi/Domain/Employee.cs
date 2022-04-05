using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EmployeeApi.Domain;

public class Employee
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("firstName")]
    public string FirstName { get; set; }
    [BsonElement("lastName")]
    public string LastName { get; set; }
    [BsonElement("email")]
    public string Email { get; set; }
    [BsonElement("phone")]
    public string Phone { get; set; }
    [BsonElement("department")]
    public string Departmant { get; set; }
    [BsonElement("salary")]
    public decimal Salary { get; set; }


}
