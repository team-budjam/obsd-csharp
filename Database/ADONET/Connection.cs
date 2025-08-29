using System;
using System.Collections.Generic;
using System.Text;
using Database.ADONET;
using Microsoft.Data.SqlClient;

namespace Database.ADONET;

// Object
public class Connection : IDisposable
{
    // core
    public static Connection Create(SetupToken token)
    {
        return new Connection() { Token = token };
    }
    private Connection() { }

    // state
    public required SetupToken Token { get; init; }

    private SqlConnection? _Connection = null;
    public bool IsConnected
    {
        get => this._Connection is not null;
    }


    // action
    public async Task Connect()
    {
        if (this._Connection is not null)
        {
            // 이미 연결된 상태라면 재연결 방지
            return;
        }


        this._Connection = new SqlConnection(this.Token.GetConnectionString());

        try
        {
            await this._Connection.OpenAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    public async Task EndConnection()
    {
        if (this._Connection is not null)
        {
            try
            {
                if (this._Connection.State != System.Data.ConnectionState.Closed)
                {
                    await this._Connection.CloseAsync();   // 연결 종료
                }

                this._Connection.Dispose();      // 리소스 해제
            }
            finally
            {
                this._Connection = null;         // 참조 제거
            }
        }
    }
    public void Dispose()
    {
        this._Connection?.Dispose();
        this._Connection = null;

        Console.WriteLine("Connection이 제거됩니다.");
        return;
    }
}