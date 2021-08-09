using NUnit.Framework;

using Pistol.Sms;

using System;
using System.Threading.Tasks;

namespace Pistol.Sms.Tests
{
    [TestFixture]
    public class KotsmsServiceTests
    {
        private KotsmsConfigurations _config;

        [SetUp]
        public void Setup()
        {
            _config = new KotsmsConfigurations
            {
                Account = Environment.GetEnvironmentVariable("Pistol_Kotsms_Account", EnvironmentVariableTarget.Machine),
                Password = Environment.GetEnvironmentVariable("Pistol_Kotsms_Password", EnvironmentVariableTarget.Machine)
            };
        }

        [Test]
        public async Task GetBalanceTest()
        {
            var service = new KotsmsService(_config);

            var balance = await service.GetBalance();

            Console.WriteLine(balance.Point);
            Assert.Greater(balance.Point, 0);
        }
    }
}