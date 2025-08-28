namespace Database.ADONET;


public readonly record struct DBToken()
{
    // core
    public required string DBName { get; init; }
    public required Source DBSource { get; init; }
    internal bool MultipleActiveResultSets { get; init; } = true;
    internal bool TrustServerCertificate { get; init; } = true;
    internal int ConnectTimeout { get; init; } = 15;
    internal bool IntegratedSecurity { get; init; } = true;

    // value
    public enum Source
    {
        Localhost
    }
}