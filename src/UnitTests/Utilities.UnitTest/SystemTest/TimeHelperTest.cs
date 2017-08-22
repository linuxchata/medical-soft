using NUnit.Framework;

namespace Utilities.UnitTest.SystemTest
{
    [TestFixture]
    public class TimeHelperTest
    {
        [Test]
        public void Hours_ShouldReturnHours_Test()
        {
            // Arrange
            // Act
            var hours = System.TimeHelper.Hours;

            // Assert
            Assert.AreEqual(24, hours.Count, "Unexpected hours count.");

            Assert.AreEqual("00", hours[0], "Unexpected hour.");
            Assert.AreEqual("01", hours[1], "Unexpected hour.");
            Assert.AreEqual("02", hours[2], "Unexpected hour.");
            Assert.AreEqual("03", hours[3], "Unexpected hour.");
            Assert.AreEqual("04", hours[4], "Unexpected hour.");
            Assert.AreEqual("05", hours[5], "Unexpected hour.");
            Assert.AreEqual("06", hours[6], "Unexpected hour.");
            Assert.AreEqual("07", hours[7], "Unexpected hour.");
            Assert.AreEqual("08", hours[8], "Unexpected hour.");
            Assert.AreEqual("09", hours[9], "Unexpected hour.");
            Assert.AreEqual("10", hours[10], "Unexpected hour.");
            Assert.AreEqual("11", hours[11], "Unexpected hour.");

            Assert.AreEqual("12", hours[12], "Unexpected hour.");
            Assert.AreEqual("13", hours[13], "Unexpected hour.");
            Assert.AreEqual("14", hours[14], "Unexpected hour.");
            Assert.AreEqual("15", hours[15], "Unexpected hour.");
            Assert.AreEqual("16", hours[16], "Unexpected hour.");
            Assert.AreEqual("17", hours[17], "Unexpected hour.");
            Assert.AreEqual("18", hours[18], "Unexpected hour.");
            Assert.AreEqual("19", hours[19], "Unexpected hour.");
            Assert.AreEqual("20", hours[20], "Unexpected hour.");
            Assert.AreEqual("21", hours[21], "Unexpected hour.");
            Assert.AreEqual("22", hours[22], "Unexpected hour.");
            Assert.AreEqual("23", hours[23], "Unexpected hour.");
        }

        [Test]
        public void Minutes_ShouldReturnMinutes_Test()
        {
            // Arrange
            // Act
            var minutes = System.TimeHelper.Minutes;

            // Assert
            Assert.AreEqual(4, minutes.Count, "Unexpected minutes count.");

            Assert.AreEqual("00", minutes[0], "Unexpected minute.");
            Assert.AreEqual("15", minutes[1], "Unexpected minute.");
            Assert.AreEqual("30", minutes[2], "Unexpected minute.");
            Assert.AreEqual("45", minutes[3], "Unexpected minute.");
        }
    }
}
