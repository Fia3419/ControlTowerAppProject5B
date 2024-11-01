using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlTowerBLL;

namespace ControlTowerBLL.BLLUnitTests
{
    /// <summary>
    /// Unit tests for the ControlTower class, testing various functionalities including adding flights,
    /// finding flights by ID, and ensuring that events are triggered appropriately.
    /// </summary>
    [TestClass]
    public class ControlTowerTests
    {
        private ControlTower controlTower;
        private Flight testFlight1;
        private Flight testFlight2;

        /// <summary>
        /// Initializes test data before each test method.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            controlTower = new ControlTower();
            testFlight1 = new Flight("AirlineA", "FL001", "DestinationA", 3.0);
            testFlight2 = new Flight("AirlineB", "FL002", "DestinationB", 2.0);

            controlTower.AddFlight(testFlight1);
            controlTower.AddFlight(testFlight2);
        }

        /// <summary>
        /// Tests that a valid flight ID returns the correct flight.
        /// </summary>
        /// <param name="validId">The valid flight ID to search for.</param>
        /// <param name="expectedAirliner">The expected airliner name of the flight.</param>
        [DataTestMethod]
        [DataRow("FL001", "AirlineA")]
        [DataRow("FL002", "AirlineB")]
        public void FindFlightById_ValidId_ShouldReturnCorrectFlight(string validId, string expectedAirliner)
        {
            // Act
            Flight result = controlTower.FindFlightById(validId);

            // Assert
            Assert.IsNotNull(result, $"The flight with ID '{validId}' should be found.");
            Assert.AreEqual(expectedAirliner, result.FlightData.Airliner, $"Expected airliner '{expectedAirliner}' for flight ID '{validId}', but found '{result.FlightData.Airliner}'.");
        }

        /// <summary>
        /// Tests that an invalid flight ID returns null.
        /// </summary>
        /// <param name="invalidId">The invalid flight ID to search for.</param>
        [DataTestMethod]
        [DataRow("INVALID001")]
        [DataRow("UNKNOWN")]
        [DataRow("")]
        [DataRow(null)]
        public void FindFlightById_InvalidId_ShouldReturnNull(string invalidId)
        {
            // Act
            Flight result = controlTower.FindFlightById(invalidId);

            // Assert
            Assert.IsNull(result, $"Expected null for invalid flight ID '{invalidId}', but a flight was returned.");
        }

        /// <summary>
        /// Creates a test flight with the provided data.
        /// </summary>
        /// <param name="airliner">The airliner name.</param>
        /// <param name="id">The flight ID.</param>
        /// <param name="destination">The destination of the flight.</param>
        /// <param name="duration">The duration of the flight.</param>
        /// <returns>A new instance of the Flight class.</returns>
        private static Flight CreateTestFlight(string airliner, string id, string destination, double duration)
        {
            return new Flight(airliner, id, destination, duration);
        }

        /// <summary>
        /// Tests that adding a flight with different data correctly adds it to the control tower.
        /// </summary>
        [DataTestMethod]
        [DataRow("AirlinerA", "FL100", "DestinationA", 5.0)]
        [DataRow("AirlinerB", "FL200", "DestinationB", 3.5)]
        public void AddFlight_DifferentFlightData_ShouldAddFlight(string airliner, string id, string destination, double duration)
        {
            // Arrange
            Flight flight = CreateTestFlight(airliner, id, destination, duration);

            // Act
            controlTower.AddFlight(flight);

            // Assert
            Assert.AreEqual(flight, controlTower.FindFlightById(id), $"The flight with ID '{id}' should be correctly added to the control tower.");
        }

        /// <summary>
        /// Tests that the TakeOffFlight method triggers the takeoff event and marks the flight as in-flight.
        /// </summary>
        [DataTestMethod]
        [DataRow("AirlinerC", "FL300", "DestinationC", 2.0)]
        [DataRow("AirlinerD", "FL400", "DestinationD", 4.5)]
        public void TakeOffFlight_NotInFlight_ShouldTriggerTakeOffEvent(string airliner, string id, string destination, double duration)
        {
            // Arrange
            bool eventTriggered = false;
            controlTower.FlightTakeOff += (sender, e) => eventTriggered = true;

            Flight flight = CreateTestFlight(airliner, id, destination, duration);
            controlTower.AddFlight(flight);

            // Act
            controlTower.TakeOffFlight(flight);

            // Assert
            Assert.IsTrue(eventTriggered, "The takeoff event should be triggered.");
            Assert.IsTrue(flight.FlightData.InFlight, "The flight should be marked as in-flight.");
        }

        /// <summary>
        /// Tests that changing the flight's altitude updates it accordingly when the flight is in the air.
        /// </summary>
        [DataTestMethod]
        [DataRow("AirlinerE", "FL500", "DestinationE", 1.5, 10000, 3000)]
        [DataRow("AirlinerF", "FL600", "DestinationF", 3.0, 15000, -5000)]
        public void ChangeFlightHeight_ValidInFlight_ShouldUpdateHeight(
            string airliner, string id, string destination, double duration, int initialHeight, int changeValue)
        {
            // Arrange
            Flight flight = CreateTestFlight(airliner, id, destination, duration);
            controlTower.AddFlight(flight);
            flight.TakeOffFlight();
            flight.ChangeFlightHeight(initialHeight);
            int expectedHeight = initialHeight + changeValue;

            // Act
            controlTower.ChangeFlightHeight(flight, changeValue);

            // Assert
            Assert.AreEqual(expectedHeight, flight.FlightHeight, $"The flight's altitude should be updated to {expectedHeight}.");
        }

        /// <summary>
        /// Tests that the LandFlight method triggers the landing event and marks the flight as not in-flight.
        /// </summary>
        [DataTestMethod]
        [DataRow("AirlinerG", "FL700", "DestinationG", 2.5)]
        [DataRow("AirlinerH", "FL800", "DestinationH", 4.0)]
        public void LandFlight_FlightInFlight_ShouldTriggerLandingEvent(string airliner, string id, string destination, double duration)
        {
            // Arrange
            bool eventTriggered = false;
            controlTower.FlightLanded += (sender, e) => eventTriggered = true;

            Flight flight = CreateTestFlight(airliner, id, destination, duration);
            controlTower.AddFlight(flight);
            flight.TakeOffFlight();

            // Act
            controlTower.LandFlight(flight);

            // Assert
            Assert.IsTrue(eventTriggered, "The landing event should be triggered.");
            Assert.IsFalse(flight.FlightData.InFlight, "The flight should be marked as not in-flight.");
        }
    }
}
