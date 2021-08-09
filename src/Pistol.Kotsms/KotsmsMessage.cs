namespace Pistol.Sms
{
    using System;

    public class KotsmsMessage : SmsMessage
    {
        public KotsmsApiTypes Type { get; set; }

        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 簡訊有效時間。
        /// </summary>
        public TimeSpan? ExpiryTime { get; set; }
    }
}
