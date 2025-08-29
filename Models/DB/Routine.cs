using static System.Console;

namespace Models.DB;


// Routine
public static class Routine
{
    public static async Task First()
    {
        // define value
        var setUpToken = new SetupToken
        {
            DBName = "tempdb",
            DBSource = SetupToken.Source.LocalHost,
        };

        // create object
        var connection = Connector.CreateBy(setUpToken);

        // execute action(Connector.Connect)
        await connection.Connect();

        // execute action(Connector.Disconnect)
        await connection.Disconnect();
    }
}
