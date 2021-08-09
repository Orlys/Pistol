using System.Collections.Generic;

namespace Pistol.Sms
{
    public abstract class SmsMessage
    {
        public virtual string PhoneNumber { get; set; }

        public virtual string Content { get; set; }
    }
}
