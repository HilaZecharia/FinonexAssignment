using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

const string EventFile = "EventFile.json";
const string ConnectionString = "Host=localhost;Database=postgres;Username=postgres;Password=admin";// Replace with your SQL Server connection string
const string SecretHeader = "secret";

// Define a POST request endpoint
app.MapPost("/liveEvent", async (HttpContext context) =>
{
    try
    {
        context.Response.ContentType = "application/json";
        // Check authorization
        if (context.Request.Headers.Authorization != SecretHeader)
        {
            return Results.Problem(detail: "Unauthorized user", statusCode: 401);
        }
        else
        {
            using var reader = new StreamReader(context.Request.Body);
            var liveEvent = await reader.ReadToEndAsync();

            if (!File.Exists(EventFile))
            {
                using FileStream fs = File.Create(EventFile);
            }
            await File.AppendAllTextAsync(EventFile, Environment.NewLine + liveEvent);

            return Results.Ok();
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500, title: "Internal Server Error");
    }
});

// Define a GET request endpoint
app.MapGet("/userEvents/{userId}", async (string userId) =>
{
    try
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User ID cannot be null or empty");
        }
        using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        var query = "Select revenue From users_revenue Where user_id = @userId";

        using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("userId", userId);

        var result = await cmd.ExecuteScalarAsync();

        if (result == null)
        {
            return Results.NotFound("User not found or no revenue data available");
        }

        return Results.Ok(new
        {
            userId = userId,
            revenue = Convert.ToInt32(result)
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500, title: "Internal Server Error");
    }
});

app.Run("http://localhost:8000");
