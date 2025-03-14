using System.Text.Json;
using Npgsql;


class DataProcessor
{
    private const string DefaultEventFile = @"C:\Users\hila_\source\repos\Finonex\EventFile.json";
    private const string ConnectionString = "Host=localhost;Database=postgres;Username=postgres;Password=admin";// Replace with your SQL Server connection string

    public class LiveEvent
    {
        public required string userId { get; set; }
        public required string name { get; set; }
        public int value { get; set; }
    }



    static async Task Main(string[] args)
    {
        string eventsFile;
        //support in many eventFiles passed as argument or default EventFile
        eventsFile = args.Length != 0 ? args[0] : DefaultEventFile;
        await ReadFromFile(eventsFile);
    }

    private static async Task ReadFromFile(string eventsFile)
    {
        try
        {
            if (!File.Exists(eventsFile))
            {
                Console.WriteLine($"File not exist: {eventsFile}");
                return;
            }
            Console.WriteLine($"Start processing events...");
            string[] events = await File.ReadAllLinesAsync(eventsFile);
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var eventLine in events)
                        {
                            if (!string.IsNullOrEmpty(eventLine))
                            {
                                var liveEvent = JsonSerializer.Deserialize<LiveEvent>(eventLine);
                                if (liveEvent != null)
                                    await ProcessingEvent(connection, transaction, liveEvent);
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing events: {ex.Message}");
                        await transaction.RollbackAsync();
                    }
                }
            }
            Console.WriteLine($"Finish processing events...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static async Task ProcessingEvent(NpgsqlConnection conn, NpgsqlTransaction transaction, LiveEvent liveEvent)
    {
        // Create the SQL query for insert and UPDATE query at once
        string query = @"INSERT INTO users_revenue (user_id, revenue)
                                 VALUES (@userId, @value) 
                                 ON CONFLICT (user_id) 
                                 DO UPDATE SET revenue = users_revenue.revenue + @value 
                                 WHERE users_revenue.user_id = @userId";

        using (var cmd = new NpgsqlCommand(query, conn, transaction))
        {
            // Binding params
            cmd.Parameters.AddWithValue("value", liveEvent.name == "subtract_revenue" ? -liveEvent.value : liveEvent.value);
            cmd.Parameters.AddWithValue("userId", liveEvent.userId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}