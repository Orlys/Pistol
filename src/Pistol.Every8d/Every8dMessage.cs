using System;

namespace Pistol.Sms
{
    public class Every8dMessage : SmsMessage
    {
        /// <summary>
        /// 簡訊主旨，主旨不會隨著簡訊內容發送出去。用以註記本次發送之用途。
        /// </summary>
        public virtual string Subject { get; set; }
        public virtual DateTime? DeliveryTime { get; set; }
    }
}
