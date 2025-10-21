// See https://aka.ms/new-console-template for more information


using TitleConsole.Object;

Console.WriteLine("Start Program");

var titleClient = new TitleClient();

await titleClient.Subscribe();

while (true)
{
    // user input
    Console.WriteLine("Title> ");
    var line = Console.ReadLine();

    if (line is null)            // 입력 스트림 종료
        break;

    if (line.Equals("/exit", StringComparison.OrdinalIgnoreCase))
        break;

    if (string.IsNullOrWhiteSpace(line))
        continue;

    // pushtitle
    titleClient.Title = line;

    await titleClient.PushTitle();
}


Console.WriteLine("End Program");