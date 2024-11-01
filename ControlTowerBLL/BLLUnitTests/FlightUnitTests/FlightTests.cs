using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlTowerBLL;

namespace ControlTowerBLL.BLLUnitTests
{
    [TestClass]
    public class FlightTests
    {
        [DataTestMethod]
        [DataRow("AirlinerA", "FL123", "DestinationA", 3.0)]
        [DataRow("AirlinerB", "FL456", "DestinationB", 5.5)]
        public void TakeOffFlight_ValidFlight_ShouldSetInFlightAndTriggerEvent(
            string airliner, string id, string destination, double duration)
        {
            // Arrange
            Flight flight = new Flight(airliner, id, destination, duration);
            bool eventTriggered = false;
            flight.FlightTakeOff += (sender, e) => eventTriggered = true;

            // Act
            flight.TakeOffFlight();

            // Assert
            Assert.IsTrue(flight.FlightData.InFlight, "The flight should be in-flight after takeoff.");
            Assert.IsTrue(eventTriggered, "The takeoff event should be triggered.");
        }

        /// <summary>
        /// Tests that the <see cref="Flight.ChangeFlightHeight(int)"/> method updates the flight height as expected.
        /// </summary>
        /// <param name="initialHeight">The initial altitude of the flight.</param>
        /// <param name="changeValue">The value to change the altitude by.</param>
        /// <param name="expectedHeight">The expected altitude after the change.</param>
        [TestMethod]
        [DataRow(10000, 5000, 15000)] // Test case with a positive change
        [DataRow(15000, -5000, 10000)] // Test case with a negative change
        [DataRow(15000, 0, 15000)] // Test case with no change
        public void ChangeFlightHeight_ShouldUpdateFlightHeight_WhenCalled(int initialHeight, int changeValue, int expectedHeight)
        {
            // Arrange
            const string testAirline = "TestAirline";
            const string testFlightId = "FL123";
            const string testDestination = "TestDestination";
            const double testDuration = 3.5;

            Flight flight = new Flight(testAirline, testFlightId, testDestination, testDuration);
            flight.ChangeFlightHeight(initialHeight); // Set initial height

            // Act
            flight.ChangeFlightHeight(changeValue);

            // Assert
            Assert.AreEqual(expectedHeight, flight.FlightHeight, $"Expected flight height to be {expectedHeight} after changing by {changeValue}.");
        }

        [DataTestMethod]
        [DataRow("AirlinerD", "FL012", "DestinationD", 2.0)]
        [DataRow("AirlinerE", "FL345", "DestinationE", 1.0)]
        public void LandFlight_InFlight_ShouldSetInFlightToFalseAndTriggerEvent(
            string airliner, string id, string destination, double duration)
        {
            // Arrange
            Flight flight = new Flight(airliner, id, destination, duration);
            flight.TakeOffFlight();
            bool eventTriggered = false;
            flight.FlightLanded += (sender, e) => eventTriggered = true;

            // Act
            flight.LandFlight();

            // Assert
            Assert.IsFalse(flight.FlightData.InFlight, "The flight should be marked as not in-flight after landing.");
            Assert.IsTrue(eventTriggered, "The landing event should be triggered.");
        }
    }
}