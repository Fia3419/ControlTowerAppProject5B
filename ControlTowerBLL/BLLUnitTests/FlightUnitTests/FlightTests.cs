using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ControlTowerBLL;
using ControlTowerDTO;
using System;

namespace ControlTowerBLL.BLLUnitTests
{
    [TestClass]
    public class FlightTests
    {
        private Flight CreateFlight(string airliner, string id, string destination, double duration)
        {
            return new Flight(airliner, id, destination, duration);
        }

        [DataTestMethod]
        [DataRow("AirlineA", "Flight001", "DestinationA", 2.5)]
        [DataRow("AirlineB", "Flight002", "DestinationB", 3.0)]
        public void TakeOffFlight_ValidFlight_FlightShouldBeMarkedAsInAir(string airliner, string id, string destination, double duration)
        {
            // Arrange
            Flight flight = CreateFlight(airliner, id, destination, duration);

            // Act
            flight.TakeOffFlight();

            // Assert
            Assert.IsTrue(flight.InFlight, "The flight did not take off as expected.");
        }

        [DataTestMethod]
        [DataRow("AirlineA", "Flight001", "DestinationA", 2.5)]
        public void LandFlight_FlightInAir_FlightShouldBeMarkedAsLanded(string airliner, string id, string destination, double duration)
        {
            // Arrange
            Flight flight = CreateFlight(airliner, id, destination, duration);
            flight.TakeOffFlight();

            // Act
            flight.LandFlight();

            // Assert
            Assert.IsFalse(flight.InFlight, "The flight did not land as expected.");
        }
    }
}
