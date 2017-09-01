namespace Ucsmy.Elink.RabbitMq.SDK.Config
{
    /// <summary>
    /// 交换机Map
    /// </summary>
    public static class MqExchangeMap
    {
        /// <summary>
        /// 默认交换机 一对一
        /// </summary>
        public static string Default_Exchange = "";

        /// <summary>
        /// 默认模糊匹配交换机 
        /// </summary>
        public static string Default_Topic_Exchange = "amq.topic";
    }
}
