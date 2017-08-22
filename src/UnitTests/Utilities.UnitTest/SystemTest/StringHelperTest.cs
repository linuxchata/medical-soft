using System;
using NUnit.Framework;

namespace Utilities.UnitTest.SystemTest
{
    [TestFixture]
    public class StringHelperTest
    {
        [TestCase(null)]
        [TestCase("")]
        public void IsNullOrEmpty_WhenNullString_Test(string @string)
        {
            // Arrange
            // Act
            var result = @string.IsNullOrEmpty();

            // Assert
            Assert.IsTrue(result, "Unexpected result.");
        }

        [Test]
        public void IsNullOrEmpty_WhenValidString_Test()
        {
            // Arrange
            var @string = "String";

            // Act
            var result = @string.IsNullOrEmpty();

            // Assert
            Assert.IsFalse(result, "Unexpected result.");
        }

        [TestCase("String")]
        [TestCase("*")]
        [TestCase("/")]
        [TestCase(@"\")]
        [TestCase("-")]
        [TestCase("+38-067s")]
        [TestCase("0")]
        public void IsPhoneNumber_WhenStringIsNotValid_Test(string @string)
        {
            // Arrange
            // Act
            var result = @string.IsPhoneNumber();

            // Assert
            Assert.IsFalse(result, "Unexpected result.");
        }

        [TestCase("06")]
        [TestCase("+38")]
        [TestCase("+38-067")]
        [TestCase("+38(067)1")]
        [TestCase("+38(067)4301246")]
        [TestCase("+38(067)430-12-46")]
        public void IsPhoneNumber_WhenStringIsValid_Test(string @string)
        {
            // Arrange
            // Act
            var result = @string.IsPhoneNumber();

            // Assert
            Assert.IsTrue(result, "Unexpected result.");
        }
    }
}
