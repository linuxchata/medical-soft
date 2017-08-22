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
            var isImage = ImageHelper.IsImage(null);

            // Asset
            Assert.That(isImage, Is.False, "Unexpected result.");
        }

        [Test]
        public void IsImage_WhenWrongFilePath_ShouldReturnFalse_Test()
        {
            // Arrange
            // Act
            var isImage = ImageHelper.IsImage("WrongFilePath");

            // Asset
            Assert.That(isImage, Is.False, "Unexpected result.");
        }
    }
}
