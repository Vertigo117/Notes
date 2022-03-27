using AutoFixture.Xunit2;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Notes.Core.Models;
using Notes.Core.Services;
using Notes.Core.Tests.Attributes;
using Notes.Core.Tests.Helpers;
using Notes.Core.Tests.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Notes.Core.Tests
{
    public class JwtServiceTests
    {
        [Theory]
        [AutoMoqData]
        public void Generate_EmailPassed_TokenHasThreeParts(
            [Frozen] Mock<IOptions<AuthSettings>> authSettingsMock,
            AuthSettings authSettings,
            string email,
            string name,
            JwtService jwtService)
        {
            //Assert
            int expectedCount = 3;
            authSettingsMock.Setup(mock => mock.Value).Returns(authSettings);

            //Act
            string jwt = jwtService.Generate(email, name);
            int actualCount = jwt.Split('.').Length;

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [AutoMoqData]
        public void Generate_EmailPassed_ReturnsValidHeader(
            [Frozen] Mock<IOptions<AuthSettings>> authSettingsMock,
            AuthSettings authSettings,
            string email,
            string name,
            JwtService jwtService)
        {
            //Arrange
            authSettingsMock.Setup(mock => mock.Value).Returns(authSettings);
            var expected = new Header { Alg = "HS256", Typ = "JWT" };

            //Act
            string jwt = jwtService.Generate(email, name);
            Header actual = TokenHelper.GetHeaderObject(jwt);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [AutoMoqData]
        public void Generate_EmailPassed_ReturnsValidPayload(
            [Frozen] Mock<IOptions<AuthSettings>> authSettingsMock,
            AuthSettings authSettings,
            string email,
            string name,
            JwtService jwtService)
        {
            //Arrange
            authSettingsMock.Setup(mock => mock.Value).Returns(authSettings);
            Payload expected = CreatePayload(authSettings, email, name);

            //Act
            string jwt = jwtService.Generate(email, name);
            Payload actual = TokenHelper.GetPayloadObject(jwt);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [AutoMoqData]
        public void Generate_EmailPassed_ReturnsValidSignature(
            [Frozen] Mock<IOptions<AuthSettings>> authSettingsMock,
            AuthSettings authSettings,
            string email,
            string name,
            JwtService jwtService)
        {
            //Arrange
            authSettingsMock.Setup(mock => mock.Value).Returns(authSettings);

            //Act
            string jwt = jwtService.Generate(email, name);
            var dataToSign = $"{TokenHelper.GetHeader(jwt)}.{TokenHelper.GetPayload(jwt)}";
            string expectedSignature = GetSha256Hash(dataToSign, authSettings);
            string actualSignature = TokenHelper.GetSignature(jwt);

            //Assert
            Assert.Equal(expectedSignature, actualSignature);
        }

        private string GetSha256Hash(string text, AuthSettings authSettings)
        {
            byte[] textBytes = Encoding.ASCII.GetBytes(text);

            using var hmacSha256 = new HMACSHA256(Encoding.ASCII.GetBytes(authSettings.Secret));
            return Base64UrlEncoder.Encode(hmacSha256.ComputeHash(textBytes));
        }

        private static Payload CreatePayload(AuthSettings authSettings, string email, string name)
        {
            var timeNow = DateTimeOffset.UtcNow;
            return new Payload
            {
                Email = email,
                Nbf = timeNow.ToUnixTimeSeconds().ToString(),
                Exp = timeNow.AddHours(authSettings.LifeTimeHours).ToUnixTimeSeconds().ToString(),
                Iat = timeNow.ToUnixTimeSeconds().ToString(),
                Unique_name = name
            };
        }
    }
}
