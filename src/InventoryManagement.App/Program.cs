using InventoryManagement.Domain.Services;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using InventoryManagement.Domain;

while (true)
{
    Console.WriteLine("\n1. Run app");
    Console.WriteLine("2. Exit");
    Console.Write("Select option: ");

    string? choice = Console.ReadLine();

    if (choice == "2")
    {
        Console.WriteLine("Application closed.");
        break;
    }

    if (choice != "1")
    {
        Console.WriteLine("Invalid option. Try again.");
        continue;
    }


    try
    {
        string configFile = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(configFile, optional: false)
            .Build();

        string relativepath = config["Storage:UsersFilePath"] ?? throw new InvalidOperationException("Storage:UsersFilePath is not configured.");

        string filePath = Path.Combine(AppContext.BaseDirectory, relativepath);

        string BaseUrl = config["ExternalApi:BaseUrl"] ?? throw new InvalidOperationException("ExternalApi:BaseUrl is not configured.");

        var options = new JsonSerializerOptions
        {
            WriteIndented = config.GetValue<bool?>("Storage:WriteIndented") ?? throw new InvalidOperationException("Storage:WriteIndented is not configured.")
        };

        var client = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        ExternalUserService userService = new ExternalUserService(client);
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        Console.WriteLine("Input first id:");
        int.TryParse(Console.ReadLine(), out int firstId);
        Console.WriteLine("Input second id:");
        int.TryParse(Console.ReadLine(), out int secondId);
        Console.WriteLine("Input third id:");
        int.TryParse(Console.ReadLine(), out int thirdId);

        var user1 = await userService.GetUserAsync(firstId, cts.Token);
        var user2 = await userService.GetUserAsync(secondId, cts.Token);
        var user3 = await userService.GetUserAsync(thirdId, cts.Token);
        Console.WriteLine(user1);
        Console.WriteLine(user2);
        Console.WriteLine(user3);

        List<ExternalUser> listOfUsers = [];

        var users = await userService.GetThreeUsersAsync(firstId, secondId, thirdId, cts.Token);
        Console.WriteLine("\nUsers from GetThreeUsersAsync");
        foreach (var user in users)
        {
            Console.WriteLine(user);
            listOfUsers.Add(user);
        }

        string json = JsonSerializer.Serialize(listOfUsers, options);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await File.WriteAllTextAsync(filePath, json);

        string savedJson = await File.ReadAllTextAsync(filePath);
        var deserealized = JsonSerializer.Deserialize<List<ExternalUser>>(savedJson);
        Console.WriteLine("\nDeserealized users list");
        foreach (var user in deserealized)
        {
            Console.WriteLine(user);
        }
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (JsonException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (OperationCanceledException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}