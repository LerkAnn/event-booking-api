//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using System.Text.Json;

//var client = new HttpClient { BaseAddress = new Uri("https://localhost:7032") };
//var email = Environment.GetEnvironmentVariable("TEST_EMAIL");
//var password = Environment.GetEnvironmentVariable("TEST_PASSWORD");

//// 1. Log in
//var loginResponse = await client.PostAsJsonAsync("/api/auth/login",
//    new { email, password });
//loginResponse.EnsureSuccessStatusCode();

//var loginJson = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
//var token = loginJson.GetProperty("token").GetString();

//// 2. Put the token on every later request
//client.DefaultRequestHeaders.Authorization =
//    new AuthenticationHeaderValue("Bearer", token);

//// 3. Create 10 requests WITHOUT awaiting each one
//var seatId = 5;
//var tasks = Enumerable.Range(0, 10)
//    .Select(_ => client.PostAsJsonAsync("/api/bookings", new { seatId }));

//// 4. Wait for all of them together
//var responses = await Task.WhenAll(tasks);

//// 5. Count the status codes
//foreach (var group in responses.GroupBy(r => (int)r.StatusCode))
//{
//    Console.WriteLine($"{group.Key}: {group.Count()}");
//}


using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

var client = new HttpClient { BaseAddress = new Uri("https://localhost:7032") };
var email = Environment.GetEnvironmentVariable("TEST_EMAIL");
var password = Environment.GetEnvironmentVariable("TEST_PASSWORD");
var mode = Environment.GetEnvironmentVariable("TEST_MODE") ?? "book";
var seatId = int.Parse(Environment.GetEnvironmentVariable("TEST_SEAT_ID") ?? "5");

// 1. Log in
var loginResponse = await client.PostAsJsonAsync("/api/auth/login",
    new { email, password });
loginResponse.EnsureSuccessStatusCode();

var loginJson = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
var token = loginJson.GetProperty("token").GetString();

// 2. Put the token on every later request
client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);

IEnumerable<Task<HttpResponseMessage>> tasks;

if (mode == "pay")
{
    // Create ONE booking first, then pay it 10 times at once
    var bookingResponse = await client.PostAsJsonAsync("/api/bookings", new { seatId });
    bookingResponse.EnsureSuccessStatusCode();
    var bookingJson = await bookingResponse.Content.ReadFromJsonAsync<JsonElement>();
    var bookingId = bookingJson.GetProperty("bookingId").GetInt32();
    Console.WriteLine($"Created booking {bookingId}, paying 10 times at once...");

    tasks = Enumerable.Range(0, 10)
        .Select(_ => client.PostAsync($"/api/bookings/{bookingId}/pay", null));
}
else
{
    // Book the same seat 10 times at once
    tasks = Enumerable.Range(0, 10)
        .Select(_ => client.PostAsJsonAsync("/api/bookings", new { seatId }));
}

// 3. Wait for all of them together
var responses = await Task.WhenAll(tasks);

// 4. Count the status codes
foreach (var group in responses.GroupBy(r => (int)r.StatusCode))
{
    Console.WriteLine($"{group.Key}: {group.Count()}");
}