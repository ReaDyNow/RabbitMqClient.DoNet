namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// IMqConnectionConfig
    /// </summary>
    public interface IMqConnectionConfig
    {
        bool AutomaticRecoveryEnabled { get; }
        string HostName { get; }
        string Password { get; }
        int Port { get; }
        ushort RequestedHeartbeat { get; }
        string UserName { get; }
        string VirtualHost { get; }
    }
}