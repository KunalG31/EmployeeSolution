namespace EmployeesApi;

public class BsonIdConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var routeValue))
        {
            var parameterValue = Convert.ToString(routeValue);
            if (ObjectId.TryParse(parameterValue, out var _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}



// {id:bsonid}



// status/{id:int}
