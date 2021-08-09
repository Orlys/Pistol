
namespace Pistol.Sms
{
    using System.Net.Http;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class Every8dService : IEvery8dService
    {
        private Every8dConfigurations _configurations;
        private HttpClient _httpClient;
        private static HttpClient s_sharedClient = new HttpClient();

        public Every8dService(Every8dConfigurations configutations) : this(configutations, s_sharedClient)
        {

        }

        public Every8dService(Every8dConfigurations configutations, HttpClient client)
        {
            this._configurations = configutations ?? throw new ArgumentNullException(nameof(configutations));
            this._httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }



        private const string FireMessageEndpoint = "http://api.every8d.com/API21/HTTP/sendSMS.ashx";
        private const string GetBalanceEndpoint = "http://api.every8d.com/API21/HTTP/getCredit.ashx";

        public async Task<bool> FireMessage(Every8dMessage smsMessage)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "UID", _configurations.Account },
                { "PWD", _configurations.Password },
                { "SB", smsMessage.Subject },
                { "MSG", smsMessage.Content },
                { "DEST", smsMessage.PhoneNumber },
                { "ST", (smsMessage.DeliveryTime??DateTime.Now).ToString("YYYYMMDDhhmnss") },
            });
            var response = await _httpClient.PostAsync(FireMessageEndpoint, content);
            var pointString = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();


            return true;
            //return new Every8dBalance
            //{
            //    Point = double.Parse(pointString)
            //};
        }

        public async Task<Every8dBalance> GetBalance()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "UID", _configurations.Account },
                { "PWD", _configurations.Password },
            });
            var response = await _httpClient.PostAsync(GetBalanceEndpoint, content);
            var pointString = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

            return new Every8dBalance
            {
                Point = double.Parse(pointString)
            };
        }
    }
}
