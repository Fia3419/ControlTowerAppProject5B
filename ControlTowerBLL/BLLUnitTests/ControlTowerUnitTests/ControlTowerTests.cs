using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ControlTowerBLL;
using ControlTowerBLL.Interfaces;
using System;

namespace ControlTowerBLL.BLLUnitTests
{
    [TestClass]
    public class ControlTowerTests
    {
        private ControlTower CreateControlTower()
        {
            return new ControlTower();
        }

        [DataTestMethod]
        [DataRow("Flight001", false)]
        [DataRow("Flight002", true)]
        public void AddFlight_ValidFlight_FlightIsAddedToList(string flightId, bool inFlight)
        {
            // Arrange
            Mock<IFlight> flightMock = new Mock<IFlight>();
            flightMock.SetupGet(f => f.Id).Returns(flightId);
            flightMock.SetupGet(f => f.InFlight).Returns(inFlight);

            ControlTower controlTower = CreateControlTower();

            // Act
            controlTower.AddFlight(flightMock.Object);

            // Assert
            IFlight addedFlight = controlTower.FindFlightById(flightId);
            Assert.IsNotNull(addedFlight);
            Assert.AreEqual(flightId, addedFlight.Id);
        }

        [DataTestMethod]
        [DataRow(10000, 5000, 15000)]
        [DataRow(20000, -5000, 15000)]
        public void ChangeFlightHeight_FlightInAir_ChangesAltitudeCorrectly(int initialHeight, int changeValue, int expectedHeight)
        {
            // Arrange
            Mock<IFlight> flightMock = new Mock<IFlight>();
            flightMock.SetupGet(f => f.InFlight).Returns(true);
            flightMock.SetupProperty(f => f.FlightHeight, initialHeight);
            flightMock.Setup(f => f.ChangeFlightHeight(It.IsAny<int>()))
                      .Callback<int>(newHeight => flightMock.Object.FlightHeight = newHeight);

            ControlTower controlTower = CreateControlTower();

            // Act
            controlTower.ChangeFlightHeight(flightMock.Object, changeValue);

            // Assert
            Assert.AreEqual(expectedHeight, flightMock.Object.FlightHeight, "The flight height was not changed as expected.");
        }

        [TestMethod]
        public void TakeOffFlight_FlightIsNotInAir_TakeOffEventTriggered()
        {
            // Arrange
            Mock<IFlight> flightMock = new Mock<IFlight>();
            bool eventTriggered = false;

            ControlTower controlTower = CreateControlTower();
            controlTower.FlightTakeOff += (sender, args) => eventTriggered = true;

            flightMock.Setup(f => f.TakeOffFlight()).Raises(f => f.FlightTakeOff += null, new TakeOffEventArgs(flightMock.Object));
            flightMock.SetupGet(f => f.InFlight).Returns(false);

            controlTower.AddFlight(flightMock.Object);

            // Act
            controlTower.TakeOffFlight(flightMock.Object);

            // Assert
            Assert.IsTrue(eventTriggered, "The TakeOffFlight event was not triggered.");
        }
    }
}
