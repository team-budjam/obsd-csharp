using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Database.ADONET;

// Object
public class DBManager
{
    // core
    public static DBManager Create(DBToken token)
    {
        return new DBManager() { Token = token };
    }
    

    // state
    public required DBToken Token { get; init; }

    private SqlConnection? Connection = null;
    public bool IsConnected
    {
        get => this.Connection is not null;
    }


    // action
    public async Task Connect()
    {
        if (this.Connection is not null)
        {
            // 이미 연결된 상태라면 재연결 방지
            return;
        }

        var builder = new SqlConnectionStringBuilder
        {
            // DBToken의 Source를 해석
            DataSource = this.Token.DBSource switch
            {
                DBToken.Source.Localhost => "localhost",
                _ => throw new NotSupportedException($"Unsupported source: {this.Token.DBSource}")
            },
            InitialCatalog = this.Token.DBName,
            IntegratedSecurity = this.Token.IntegratedSecurity,
            MultipleActiveResultSets = this.Token.MultipleActiveResultSets,
            TrustServerCertificate = this.Token.TrustServerCertificate,
            ConnectTimeout = this.Token.ConnectTimeout
        };

        this.Connection = new SqlConnection(builder.ConnectionString);
        
        await this.Connection.OpenAsync();
    }
    public async Task EndConnection()
    {
        if (this.Connection is not null)
        {
            try
            {
                if (this.Connection.State != System.Data.ConnectionState.Closed)
                {
                    await this.Connection.CloseAsync();   // 연결 종료
                }

                this.Connection.Dispose();      // 리소스 해제
            }
            finally
            {
                this.Connection = null;         // 참조 제거
            }
        }
    }
}