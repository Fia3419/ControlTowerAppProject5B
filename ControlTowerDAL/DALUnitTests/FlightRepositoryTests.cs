using NUnit.Framework;
using ControlTowerDAL;
using System.Collections.Generic;
using ControlTowerDTO;

namespace ControlTowerDAL.DALUnitTests
{
    [TestFixture]
    public class FlightRepositoryTests
    {
        private FlightRepository flightRepository;

        private const string DefaultAirliner = "TestAirliner";
        private const string DefaultFlightId = "TestFlightId";
        private const string DefaultDestination = "TestDestination";
        private const double DefaultDuration = 3.5;
        private const int DefaultFlightHeight = 10000;
        private const bool DefaultInFlightStatus = false;

        [SetUp]
        public void Setup()
        {
            flightRepository = new FlightRepository();
        }

        [TestCase(DefaultAirliner, DefaultFlightId, DefaultDestination, DefaultDuration, DefaultFlightHeight, DefaultInFlightStatus)]
        public void AddFlight_ValidFlight_FlightAddedToRepository(string airliner, string id, string destination, double duration, int flightHeight, bool inFlight)
        {
            // Arrange
            FlightDTO flight = CreateFlight(airliner, id, destination, duration, flightHeight, inFlight);

            // Act
            flightRepository.AddFlight(flight);

            // Assert
            List<FlightDTO> flights = flightRepository.GetFlights();
            Assert.That(flights, Contains.Item(flight), "The flight should be added to the repository.");
        }

        [Test]
        public void GetFlights_NoFlightsAdded_ReturnsEmptyList()
        {
            // Act
            List<FlightDTO> flights = flightRepository.GetFlights();

            // Assert
            Assert.That(flights, Is.Empty, "The flight repository should be empty when no flights are added.");
        }

        private FlightDTO CreateFlight(string airliner, string id, string destination, double duration, int flightHeight, bool inFlight)
        {
            return new FlightDTO
            {
                Airliner = airliner,
                Id = id,
                Destination = destination,
                Duration = duration,
                InFlight = inFlight,
                FlightHeight = flightHeight
            };
        }
    }
}
