using static System.Console;

namespace Models.DB;


// Routine
public static class Routine
{
    public static async Task ConnectionTest()
    {
        // define value
        var setUpToken = new SetupToken
        {
            DBName = "tempdb",
            DBSource = SetupToken.Source.LocalHost,
        };

        // create object
        var connection = Connector.CreateBy(setUpToken);

        // execute action
        await connection.Connect();

        // execute action
        await connection.Disconnect();

        // execute action
        await connection.Remove();
    }
}
