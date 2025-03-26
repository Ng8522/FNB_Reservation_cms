using System.Net.Http.Json;
using Models;

namespace Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5092";

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var loginData = new LoginRequest { Email = email, Password = password };
        Console.WriteLine($"Sending login request with email: {email}");
        
        var response = await _httpClient.PostAsJsonAsync("/api/users/login", loginData);
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response status: {response.StatusCode}");
        Console.WriteLine($"Response content: {responseContent}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Login failed: {responseContent}");
        }

        return await response.Content.ReadFromJsonAsync<LoginResponse>() 
            ?? throw new Exception("Failed to deserialize login response");
    }
} 