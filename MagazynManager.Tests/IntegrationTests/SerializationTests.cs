using MagazynManager.Application.DataProviders;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NUnit.Framework;
using System;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void TestAuthResult()
        {
            var settings = new JsonSerializerSettings().ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            var authResult = new AuthResult
            {
                Token = Guid.NewGuid().ToString(),
                RefreshToken = Guid.NewGuid().ToString(),
                ExpireAt = SystemClock.Instance.GetCurrentInstant()
            };
            var json = JsonConvert.SerializeObject(authResult, settings);
            var obj = JsonConvert.DeserializeObject<AuthResult>(json, settings);
            Assert.That(authResult.Token, Is.EqualTo(obj.Token));
            Assert.That(authResult.RefreshToken, Is.EqualTo(obj.RefreshToken));
            Assert.That(authResult.ExpireAt, Is.EqualTo(obj.ExpireAt));
        }
    }
}