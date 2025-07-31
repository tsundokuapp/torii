namespace TsundokuTraducoes.Auth.Api;

public static class Configuration
{
    public static ConnectionStrings ConnectionString = new();

    public class ConnectionStrings
    {
        public string Default { get; set; }
    }
}
