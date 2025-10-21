namespace TitleServer.Value;


// value
public readonly record struct ClientId(Guid RawValue)
{
    public static ClientId New()
    {
        return new ClientId(Guid.NewGuid());
    }
}
