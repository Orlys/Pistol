namespace Pistol.Sms
{
    using System;
    using System.Threading.Tasks;

    public interface ISmsService<TSmsMessage, TSmsBalance>
        where TSmsMessage : SmsMessage, new()
        where TSmsBalance : SmsBalance, new()
    {
        Task<bool> FireMessage(TSmsMessage smsMessage);

        Task<TSmsBalance> GetBalance();
    }
}
