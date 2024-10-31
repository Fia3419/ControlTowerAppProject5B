using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlTowerBLL;

namespace ControlTowerBLL.BLLUnitTests
{
    [TestClass]
    public class ControlTowerTests
    {
        [TestMethod]
        public void AddFlight_ShouldAddFlightToList()
        {
            // Arrange
            ControlTower controlTower = new ControlTower();
            Flight flight = new Flight("AirlineA", "Flight001", "DestinationA", 2.5);

            // Act
            controlTower.AddFlight(flight);

            // Assert
            Flight addedFlight = controlTower.FindFlightById("Flight001");
            Assert.IsNotNull(addedFlight);
            Assert.AreEqual("Flight001", addedFlight.FlightData.Id);
        }

        [TestMethod]
        public void ChangeFlightHeight_ShouldChangeFlightAltitude()
        {
            // Arrange
            ControlTower controlTower = new ControlTower();
            Flight flight = new Flight("AirlineA", "Flight001", "DestinationA", 2.5);
            controlTower.AddFlight(flight);
            flight.TakeOffFlight();

            // Act
            controlTower.ChangeFlightHeight(flight, 10000);

            // Assert
            Assert.AreEqual(10000, flight.FlightHeight);
        }
    }
}