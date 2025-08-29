using Microsoft.Data.SqlClient;

namespace Tools.SQLServer;


// Object
public readonly record struct SetupToken
{
    // core
    public required string DBName { get; init; }
    public required Source DBSource { get; init; }
    internal bool Encrpyt { get; init; } = false;
    internal bool MultipleActiveResultSets { get; init; } = true;
    internal bool TrustServerCertificate { get; init; } = true;
    internal int ConnectTimeout { get; init; } = 15;
    internal bool IntegratedSecurity { get; init; } = true;

    public SetupToken() {  }


    // value
    public readonly record struct Source
    {
        // core
        public required string RawValue { get; init; }

        public static Source LocalHost = new Source { RawValue = "(localdb)\\MSSQLLocalDB" };
    }


    // operator
    internal string GetConnectionString()
    {
        var builder = new SqlConnectionStringBuilder();

        builder.DataSource = this.DBSource.RawValue;
        builder.InitialCatalog = this.DBName;
        builder.Encrypt = this.Encrpyt;
        builder.MultipleActiveResultSets = this.MultipleActiveResultSets;
        builder.ConnectTimeout = this.ConnectTimeout;
        builder.IntegratedSecurity = this.IntegratedSecurity;

        return builder.ConnectionString;
    }
}