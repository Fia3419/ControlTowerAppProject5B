using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlTowerBLL;

namespace ControlTowerBLL.BLLUnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ControlTower"/> class, testing various functionalities such as adding flights,
    /// finding flights by ID, ensuring that events are raised or not raised, and verifying event arguments.
    /// </summary>
    [TestClass]
    public class ControlTowerTests
    {
        private ControlTower controlTower;
        private Flight testFlight1;
        private Flight testFlight2;

        /// <summary>
        /// Initializes the test data before each test method is executed.
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
        /// Tests that the <see cref="ControlTower.FindFlightById(string)"/> method returns the correct flight when a valid ID is provided.
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
        /// Tests that the <see cref="ControlTower.FindFlightById(string)"/> method returns null when an invalid ID is provided.
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
        /// Tests that the <see cref="ControlTower.AddFlight(Flight)"/> method correctly adds flights with different data to the control tower.
        /// </summary>
        [DataTestMethod]
        [DataRow("AirlinerA", "FL100", "DestinationA", 5.0)]
        [DataRow("AirlinerB", "FL200", "DestinationB", 3.5)]
        public void AddFlight_DifferentFlightData_ShouldAddFlight(string airliner, string id, string destination, double duration)
        {
            // Arrange
            Flight flight = new Flight(airliner, id, destination, duration);

            // Act
            controlTower.AddFlight(flight);

            // Assert
            Assert.AreEqual(flight, controlTower.FindFlightById(id), $"The flight with ID '{id}' should be correctly added to the control tower.");
        }

        /// <summary>
        /// Tests that the <see cref="ControlTower.TakeOffFlight(Flight)"/> method triggers the takeoff event and marks the flight as in-flight.
        /// </summary>
        [DataTestMethod]
        [DataRow("AirlinerC", "FL300", "DestinationC", 2.0)]
        [DataRow("AirlinerD", "FL400", "DestinationD", 4.5)]
        public void TakeOffFlight_NotInFlight_ShouldTriggerTakeOffEvent(string airliner, string id, string destination, double duration)
        {
            // Arrange
            bool eventTriggered = false;
            controlTower.FlightTakeOff += (sender, e) => eventTriggered = true;

            Flight flight = new Flight(airliner, id, destination, duration);
            controlTower.AddFlight(flight);

            // Act
            controlTower.TakeOffFlight(flight);

            // Assert
            Assert.IsTrue(eventTriggered, "The takeoff event should be triggered.");
            Assert.IsTrue(flight.FlightData.InFlight, "The flight should be marked as in-flight.");
        }

        /// <summary>
        /// Tests that the <see cref="ControlTower.ChangeFlightHeight(Flight, int)"/> method updates the flight height accordingly when the flight is in-flight.
        /// </summary>
        [DataTestMethod]
        [DataRow("AirlinerE", "FL500", "DestinationE", 1.5, 10000, 3000)]
        [DataRow("AirlinerF", "FL600", "DestinationF", 3.0, 15000, -5000)]
        public void ChangeFlightHeight_ValidInFlight_ShouldUpdateHeight(
            string airliner, string id, string destination, double duration, int initialHeight, int changeValue)
        {
            // Arrange
            Flight flight = new Flight(airliner, id, destination, duration);
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
        /// Tests that the <see cref="ControlTower.LandFlight(Flight)"/> method triggers the landing event and marks the flight as not in-flight.
        /// </summary>
        [DataTestMethod]
        [DataRow("AirlinerG", "FL700", "DestinationG", 2.5)]
        [DataRow("AirlinerH", "FL800", "DestinationH", 4.0)]
        public void LandFlight_FlightInFlight_ShouldTriggerLandingEvent(string airliner, string id, string destination, double duration)
        {
            // Arrange
            bool eventTriggered = false;
            controlTower.FlightLanded += (sender, e) => eventTriggered = true;

            Flight flight = new Flight(airliner, id, destination, duration);
            controlTower.AddFlight(flight);
            flight.TakeOffFlight();

            // Act
            controlTower.LandFlight(flight);

            // Assert
            Assert.IsTrue(eventTriggered, "The landing event should be triggered but isn't.");
            Assert.IsFalse(flight.FlightData.InFlight, "The flight should be marked as not in-flight.");
        }

        /// <summary>
        /// Tests that the <see cref="ControlTower.TakeOffFlight(Flight)"/> method does not trigger an event when there are no subscribers.
        /// </summary>
        [TestMethod]
        public void TakeOffFlight_NoSubscribers_ShouldNotTriggerEvent()
        {
            // Arrange
            bool eventTriggered = false;

            // Act
            controlTower.TakeOffFlight(testFlight1); // No subscribers added

            // Assert
            Assert.IsFalse(eventTriggered, "The takeoff event should not be triggered when there are no subscribers.");
        }


        /// <summary>
        /// Tests that the <see cref="ControlTower.LandFlight(Flight)"/> method does not trigger an event when there are no subscribers.
        /// </summary>
        [TestMethod]
        public void LandFlight_NoSubscribers_ShouldNotTriggerEvent()
        {
            // Arrange
            bool eventTriggered = false;

            // Act
            testFlight1.TakeOffFlight(); // The flight must be in the air to land
            controlTower.LandFlight(testFlight1); // No subscribers added

            // Assert
            Assert.IsFalse(eventTriggered, "The landing event should not be triggered when there are no subscribers.");
        }


        /// <summary>
        /// Tests that the <see cref="ControlTower.TakeOffFlight(Flight)"/> method triggers an event with the correct sender and arguments.
        /// </summary>
        [TestMethod]
        public void TakeOffFlight_EventArgs_VerifyCorrectSender()
        {
            // Arrange
            object eventSender = null;
            TakeOffEventArgs eventArgs = null;

            controlTower.FlightTakeOff += (sender, args) =>
            {
                eventSender = sender;
                eventArgs = args;
            };

            // Act
            controlTower.TakeOffFlight(testFlight1);

            // Assert
            Assert.IsNotNull(eventArgs, "The event arguments should not be null.");
            Assert.AreEqual(controlTower, eventSender, "The sender of the event should be the ControlTower instance.");
            Assert.AreEqual(testFlight1, eventArgs.Flight, "The event arguments should contain the correct flight.");
        }


        /// <summary>
        /// Tests that the <see cref="ControlTower.LandFlight(Flight)"/> method triggers an event with the correct sender and arguments.
        /// </summary>
        [TestMethod]
        public void LandFlight_EventArgs_VerifyCorrectSender()
        {
            // Arrange
            object eventSender = null;
            LandedEventArgs eventArgs = null;

            controlTower.FlightLanded += (sender, args) =>
            {
                eventSender = sender;
                eventArgs = args;
            };

            // Act
            testFlight1.TakeOffFlight(); // The flight must be in the air to land
            controlTower.LandFlight(testFlight1);

            // Assert
            Assert.IsNotNull(eventArgs, "The event arguments should not be null.");
            Assert.AreEqual(controlTower, eventSender, "The sender of the event should be the ControlTower instance.");
            Assert.AreEqual(testFlight1, eventArgs.Flight, "The event arguments should contain the correct flight.");
        }
    }
}
