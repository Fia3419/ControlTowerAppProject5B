using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlTowerBLL;
using System;

namespace ControlTowerBLL.BLLUnitTests
{
    [TestClass]
    public class FlightTests
    {
        [DataTestMethod]
        [DataRow("AirlineA", "Flight001", "DestinationA", 2.5)]
        [DataRow("AirlineB", "Flight002", "DestinationB", 3.0)]
        public void TakeOffFlight_ValidFlight_SetsInFlightToTrueAndDepartureTime(string airliner, string id, string destination, double duration)
        {
            // Arrange
            Flight flight = new Flight(airliner, id, destination, duration);

            // Act
            flight.TakeOffFlight();

            // Assert
            Assert.IsTrue(flight.FlightData.InFlight);
            Assert.AreEqual(DateTime.Now.Hour, flight.FlightData.DepartureTime.Hour);  // Checking hour to avoid exact second mismatch
        }

        [DataTestMethod]
        [DataRow(10000)]
        [DataRow(35000)]
        public void ChangeFlightHeight_ValidHeight_UpdatesFlightHeight(int newHeight)
        {
            // Arrange
            Flight flight = new Flight("AirlineA", "Flight001", "DestinationA", 2.5);
            flight.TakeOffFlight();

            // Act
            flight.ChangeFlightHeight(newHeight);

            // Assert
            Assert.AreEqual(newHeight, flight.FlightHeight);
        }

        [TestMethod]
        public void LandFlight_FlightIsInFlight_SetsInFlightToFalse()
        {
            // Arrange
            Flight flight = new Flight("AirlineA", "Flight001", "DestinationA", 2.5);
            flight.TakeOffFlight();

            // Act
            flight.LandFlight();

            // Assert
            Assert.IsFalse(flight.FlightData.InFlight);
        }
    }
}