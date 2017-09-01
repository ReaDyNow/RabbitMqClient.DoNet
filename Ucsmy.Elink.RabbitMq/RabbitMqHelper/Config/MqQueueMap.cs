namespace Ucsmy.Elink.RabbitMq.SDK.Config
{
    /// <summary>
    /// 队列Map
    /// </summary>
    public static class MqQueueMap
    {
        /// <summary>
        /// 短信队列
        /// </summary>
        public static string MesCenter = "Elink_Q_MesCenter_SMS";

        /// <summary>
        /// 邮件队列
        /// </summary>
        public static string Mail = "Elink_Q_MesCenter_Mail";

        /// <summary>
        /// 系统消息队列
        /// </summary>
        public static string SysMes = "Elink_Q_MesCenter_SysMes";

        /// <summary>
        /// 日志队列
        /// </summary>
        public static string Log = "Elink_Q_Log";

        /// <summary>
        /// 交易订单入队队列
        /// </summary>
        public static string TradeInQueue = "Elink_Q_TradeIn";

        /// <summary>
        /// 交易处理队列
        /// </summary>
        public static string TradeProcessQueue = "Elink_Q_TradeProcess";

        /// <summary>
        /// 交易结果入队队列
        /// </summary>
        public static string TradeOutQueue = "Elink_Q_TradeOut";

        /// <summary>
        /// 线下核身消息队列
        /// </summary>
        public static string OfflinePayAuditQueue = "Elink_Q_OfflinePayAudit";
    }
}
