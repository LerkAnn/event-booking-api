using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

var client = new HttpClient { BaseAddress = new Uri("https://localhost:7032") };

// 1. Log in
var loginResponse = await client.PostAsJsonAsync("/api/auth/login",
    new { email = "meow@gmail.com", password = "meow" });
loginResponse.EnsureSuccessStatusCode();

var loginJson = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
var token = loginJson.GetProperty("token").GetString();

// 2. Put the token on every later request
client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);

// 3. Create 10 requests WITHOUT awaiting each one
var seatId = 5;
var tasks = Enumerable.Range(0, 10)
    .Select(_ => client.PostAsJsonAsync("/api/bookings", new { seatId }));

// 4. Wait for all of them together
var responses = await Task.WhenAll(tasks);

// 5. Count the status codes
foreach (var group in responses.GroupBy(r => (int)r.StatusCode))
{
    Console.WriteLine($"{group.Key}: {group.Count()}");
}