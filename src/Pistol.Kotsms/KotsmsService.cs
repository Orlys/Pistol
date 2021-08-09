namespace Pistol.Sms
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    public class KotsmsService : IKotsmsService
    {
        static KotsmsService()
        {
#if NETCOREAPP1_0_OR_GREATER
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            s_big5 = Encoding.GetEncoding("big5");
        }


        private static readonly Encoding s_big5;
        private static string UrlEncodeWithBig5(string content)
        {
            return HttpUtility.UrlEncode(content, s_big5);
        }

        private KotsmsConfigurations _configurations;
        private HttpClient _httpClient;
        private static HttpClient s_sharedClient = new HttpClient();

        public KotsmsService(KotsmsConfigurations configutations) : this(configutations, s_sharedClient)
        {

        }

        public KotsmsService(KotsmsConfigurations configutations, HttpClient client)
        {
            this._configurations = configutations ?? throw new ArgumentNullException(nameof(configutations));
            this._httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        private const string FireMessageQueryString = "https://api2.kotsms.com.tw/kotsmsapi-{0}.php?username={1}&password={2}&dstaddr={3}&smbody={4}&dlvtime={5}&vldtime={6}";
        private const string GetBalanceQueryString = "https://api2.kotsms.com.tw/memberpoint.php?username={0}&password={1}";

        private string MakeDlvTimeString(DateTime? dlvTime)
        {
            var time = dlvTime?.ToString("yyyy/MM/dd HH:mm:ss") ?? "0";
            return UrlEncodeWithBig5(time);
        }

        private static TimeSpan EightHours = TimeSpan.FromHours(8);
        private static TimeSpan ThirtyMinutes = TimeSpan.FromMinutes(30);
        private string MakeVldTimeString(TimeSpan? vldTime)
        {
            if (vldTime.HasValue)
            {
                if (vldTime > EightHours)
                {
                    vldTime = EightHours;
                }
                else if (ThirtyMinutes > vldTime)
                {
                    vldTime = ThirtyMinutes;
                }
            }
            else
            {
                vldTime = EightHours;
            }

            var vldTimeString = vldTime.Value.TotalSeconds.ToString();
            return UrlEncodeWithBig5(vldTimeString);
        }


        public virtual async Task<bool> FireMessage(KotsmsMessage smsMessage)
        {
            try
            {
                var encodedBody = UrlEncodeWithBig5(smsMessage.Content);
                var encodedDlvTime = MakeDlvTimeString(smsMessage.DeliveryTime);
                var encodedVldTime = MakeVldTimeString(smsMessage.ExpiryTime);

                var queryString = string.Format(FireMessageQueryString,
                        (int)smsMessage.Type,
                        _configurations.Account,
                        _configurations.Password,
                        smsMessage.PhoneNumber,
                        encodedBody,
                        encodedDlvTime,
                        encodedVldTime);

                var response = await _httpClient.GetAsync(queryString);
                var responseBody = await GetResponseBody(response);

                return true;
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
            return false;
        }


        private async Task<string> GetResponseBody(HttpResponseMessage response)
        {
            var responseBytes = await response.EnsureSuccessStatusCode().Content.ReadAsByteArrayAsync();
            var responseBody = s_big5.GetString(responseBytes, 0, responseBytes.Length - 1);
            return responseBody;
        }

        public virtual async Task<KotsmsBalance> GetBalance()
        {
            try
            {
                var queryString = string.Format(GetBalanceQueryString,
                    _configurations.Account,
                    _configurations.Password);

                var response = await _httpClient.GetAsync(queryString);
                var responseBody = await GetResponseBody(response);

                return new KotsmsBalance
                {
                    Point = int.Parse(responseBody)
                };
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
            return null;
        }
    }
}
