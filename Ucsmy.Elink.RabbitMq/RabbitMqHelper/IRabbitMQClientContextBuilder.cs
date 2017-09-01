namespace Ucsmy.Elink.RabbitMq.SDK
{
    /// <summary>
    /// IRabbitMQClientContextBuilder
    /// </summary>
    public interface IRabbitMQClientContextBuilder
    {
        /// <summary>
        /// Build
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        IRabbitMQClientContext Build(IMqConnectionConfig config);
    }
}
