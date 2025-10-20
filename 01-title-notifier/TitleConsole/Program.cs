// Create flow

using TitleConsole;

var titleClientRef = new TitleClient();


// Subscribe flow
await titleClientRef.Subscribe();


// PushTitle flow
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
    titleClientRef.TitleInput = line;

    await titleClientRef.PushTitle();
}
