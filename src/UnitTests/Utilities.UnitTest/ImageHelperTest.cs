using NUnit.Framework;

namespace Utilities.UnitTest
{
    [TestFixture]
    public class ImageHelperTest
    {
        [Test]
        public void IsImage_WhenNullFilePath_ShouldReturnFalse_Test()
        {
            // Arrange
            // Act
            var image = ImageHelper.IsImage(null);

            // Asset
            Assert.That(image, Is.False, "Unexpected result.");
        }

        [Test]
        public void IsImage_WhenWrongFilePath_ShouldReturnFalse_Test()
        {
            // Arrange
            // Act
            var image = ImageHelper.IsImage("WrongFilePath");

            // Asset
            Assert.That(image, Is.False, "Unexpected result.");
        }
    }
}
