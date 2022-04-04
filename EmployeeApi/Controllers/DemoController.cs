namespace EmployeeApi.Controllers;

public class DemoController : ControllerBase
{
    // GET /status

    [HttpGet("status")]
    public async Task<ActionResult<GetStatusResponse>> GetTheStatus()
    {
        // fake classroom code (FOR NOW)
        var response = new GetStatusResponse(DateTime.Now, 0, "Looks Good!");
        return Ok(response); // 200 Ok Status
    }

    // GET /products/39389839  - Route Data
    [HttpGet("products/{productId:int}")]
    public async Task<ActionResult<GetStatusResponse>> GetProduct(int productId)
    {

        return Ok($"I see you want product {productId}");
    }

    [HttpGet("blogs/{year:int}/{month:int:range(1,12)}/{day:int:range(1,31)}")]
    public async Task<ActionResult> GetBlogPosts(int year, int month, int day)
    {
        return Ok($"Getting blog posts for {year}-{month}-{day}");
    }

    // GET /products?category=candy - Query String Argument
    [HttpGet("products")]
    public async Task<ActionResult> GetProducts([FromQuery] string category = "all")
    {
        return Ok($"Returning {category} products!");
    }

    // GET /products (and data in the header)

    // What about stuff that won't fit in the URL, the query string, or the headers.

    // POST /products - having data in the request body.
    [HttpPost("products")]
    public async Task<ActionResult> AddProduct([FromBody] PostProductRequest request)
    {
       
        return Ok(request);

        // GET /products/3893893
    }

}

public record GetStatusResponse(DateTime lastChecked, ServerStatus Status, string Message);
public enum ServerStatus { Optimal, UnderHeavyLoad, AboutDead, Dying } // Enumerated Constant

public record PostProductRequest(string id, string description, decimal price);
