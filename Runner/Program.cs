using Database.ADONET;

var setUpToken = new SetupToken
{
    DBName = "tempdb",
    DBSource = SetupToken.Source.LocalHost,
};

using var connection = Connection.Create(setUpToken);

await connection.Connect();