using Models.DB;

var setUpToken = new SetupToken
{
    DBName = "tempdb",
    DBSource = SetupToken.Source.LocalHost,
};

using var connection = Connector.Create(setUpToken);

await connection.Connect();