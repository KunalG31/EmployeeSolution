namespace EmployeeApi.Adapters;

public class SalaryApiContext
{
    private readonly HttpClient _httpClient;

    public SalaryApiContext(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "EmployeeApi");
    }

    public async Task<SalaryResponse> GetSalary(SalaryRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/salary-requests", request);

        response.EnsureSuccessStatusCode(); // Please punch me in the face if this is anything but a 200-299 status code.

        var data = await response.Content.ReadFromJsonAsync<SalaryResponse>();
        return data!;
    }
}
