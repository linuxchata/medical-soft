using NUnit.Framework;

namespace Utilities.UnitTest
{
    [TestFixture]
    public class HardwareTest
    {
        [Test]
        public void GetEnvironmentInfomation_ShouldReturnEnvironmentInformation_Test()
        {
            // Arrange
            var hardware = new Hardware();

            // Act
            var environmentInfomation = hardware.GetEnvironmentInfomation();

            // Asset
            Assert.That(environmentInfomation, Is.Not.Null, "Environment Information is null");
            Assert.That(environmentInfomation, Is.Not.Empty, "Environment Information is empty.");
        }
    }
}
