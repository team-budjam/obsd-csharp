using System.Collections.Concurrent;
using System.Data;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.SqlClient;

namespace Tools.SQLServer;

// Object
public class Manager
{
    // core
    public Manager()
    {
        ConnectorManager.Register(this);
    }
    internal void Delete()
    {
        ConnectorManager.Unregister(Id);
    }

    // state
    public ID Id { get; } = ID.Random();
    public SetupToken? Token { get; set; } = null;

    public SQLQuery? Query { get; set; } = null;

    // action
    public void SetUpForTempDB()
    {
        // compute
        var token = new SetupToken
        {
            DBName = "master",
            DBSource = SetupToken.Source.LocalHost,
        };

        var query = new SQLQuery()
        {
            Value = "SELECT TOP 5 CustomerID, CompanyName FROM master.dbo.Customers"
        };

        // mutate
        this.Token = token;
        this.Query = query;
    }

    public async Task PringtueryResult()
    {
        // capture
        if (Token is not SetupToken token) return;
        if (Query is not SQLQuery query) return;

        // compute
        using var connection = new SqlConnection(token.GetConnectionString());
        await connection.OpenAsync();

        using var command = new SqlCommand(query.Value, connection);
        using var reader = await command.ExecuteReaderAsync();

        Console.WriteLine("-------------------------");

        while (await reader.ReadAsync())
        {
            string name = reader["CustomerID"].ToString()!;
            Console.WriteLine($"{name}");
        }
    }


    // value
    public readonly record struct ID
    {
        // core
        public required Guid RawValue { get; init; }
        
        public static ID Random() => new ID { RawValue = Guid.NewGuid() };
    }
}


// ObjectManager
internal static class ConnectorManager
{
    // core
    internal static ConcurrentDictionary<Manager.ID, Manager> container = new();
    internal static void Register(Manager obj)
    {
        container.TryAdd(obj.Id, obj);
    }
    internal static void Unregister(Manager.ID id)
    {
        container.TryRemove(id, out _);
    }
}