using Tools.SQLServer;

var managerRef = new Manager();

managerRef.SetUpForTempDB();
await managerRef.PringtueryResult();

Console.WriteLine("End Runner");