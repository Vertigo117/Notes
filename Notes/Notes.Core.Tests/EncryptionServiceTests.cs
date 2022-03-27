using Notes.Core.Services;
using Notes.Core.Tests.Attributes;
using Xunit;

namespace Notes.Core.Tests
{
    public class EncryptionServiceTests
    {
        [Theory]
        [AutoMoqData]
        public void EncryptionService_WorksAsExpected(string password, EncryptionService encryptionService)
        {
            //Act
            string hashedPassword = encryptionService.HashPassword(password);
            bool actual = encryptionService.ValidatePassword(password, hashedPassword);

            //Assert
            Assert.True(actual);
        }
    }
}
