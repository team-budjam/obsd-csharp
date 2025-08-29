using System.Collections.Concurrent;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml;
using Microsoft.Data.SqlClient;

namespace Models.DB;

// Object
public class Connector
{
    // core
    public static Connector CreateBy(SetupToken token)
    {
        return new Connector() { Token = token };
    }
    private Connector()
    {
        ConnectorManager.Register(this);
    }

    internal void Delete()
    {
        ConnectorManager.Unregister(Id);
    }

    // state
    public ID Id { get; } = ID.Random();
    public required SetupToken Token { get; init; }

    private SqlConnection? _Connection = null;
    public bool IsConnected
    {
        get => _Connection is not null;
    }


    // action
    public async Task Connect()
    {
        if (_Connection is not null)
        {
            // 이미 연결된 상태라면 재연결 방지
            return;
        }


        _Connection = new SqlConnection(Token.GetConnectionString());

        try
        {
            await _Connection.OpenAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    public async Task Disconnect()
    {
        if (_Connection is not null)
        {
            try
            {
                if (_Connection.State != System.Data.ConnectionState.Closed)
                {
                    await _Connection.CloseAsync();   // 연결 종료
                }

                _Connection.Dispose();      // 리소스 해제
            }
            finally
            {
                _Connection = null;         // 참조 제거
            }
        }

        Console.WriteLine("Connection이 제거되었습니다.");
        return;
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
    internal static ConcurrentDictionary<Connector.ID, Connector> container = new();
    internal static void Register(Connector obj)
    {
        container.TryAdd(obj.Id, obj);
    }
    internal static void Unregister(Connector.ID id)
    {
        container.TryRemove(id, out _);
    }
}