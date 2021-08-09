using NUnit.Framework;

using Pistol.Sms;

using System;
using System.Threading.Tasks;

namespace Pistol.Sms.Tests
{
    [TestFixture()]
    public class Every8dServiceTests
    {
        private Every8dConfigurations _config;

        [SetUp]
        public void Setup()
        {
            _config = new Every8dConfigurations
            {
                Account = Environment.GetEnvironmentVariable("Pistol_Every8d_Account", EnvironmentVariableTarget.Machine),
                Password = Environment.GetEnvironmentVariable("Pistol_Every8d_Password", EnvironmentVariableTarget.Machine)
            };
        }
        [Test()]
        public async Task GetBalanceTest()
        {
            var service = new Every8dService(_config);

            var balance = await service.GetBalance();

            Console.WriteLine(balance.M);
            //Assert.Fail();
        }
    }
}