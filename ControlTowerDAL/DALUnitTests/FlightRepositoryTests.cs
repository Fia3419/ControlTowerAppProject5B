using NUnit.Framework;
using ControlTowerDAL;
using System.Collections.Generic;
using ControlTowerDTO;

namespace ControlTowerDAL.DALUnitTests
    /// <summary>
    /// Unit tests for the <see cref="FlightRepository"/> class, ensuring that flights can be added, retrieved, and validated correctly.
    /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="FlightRepository.AddFlight(FlightDTO)"/> method correctly adds a flight to the repository.
        /// </summary>
        /// <param name="airliner">The airliner operating the flight.</param>
        /// <param name="id">The unique identifier for the flight.</param>
        /// <param name="destination">The destination of the flight.</param>
        /// <param name="duration">The duration of the flight in hours.</param>
        /// <param name="flightHeight">The initial flight height.</param>
        /// <param name="inFlight">Indicates whether the flight is in-flight.</param>
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


        /// <summary>
        /// Tests that the <see cref="FlightRepository.GetFlights"/> method returns an empty list when no flights have been added.
        /// </summary>
        [Test]
        public void GetFlights_NoFlightsAdded_ReturnsEmptyList()
        {
            // Act
            List<FlightDTO> flights = flightRepository.GetFlights();

            // Assert
            Assert.That(flights, Is.Empty, "The flight repository should be empty when no flights are added.");
        }

        /// <summary>
        /// Tests that the <see cref="FlightRepository.GetFlights"/> method returns the correct number of flights after multiple additions.
        /// </summary>
        /// <param name="airliner1">The airliner of the first flight.</param>
        /// <param name="id1">The ID of the first flight.</param>
        /// <param name="destination1">The destination of the first flight.</param>
        /// <param name="duration1">The duration of the first flight.</param>
        /// <param name="airliner2">The airliner of the second flight.</param>
        /// <param name="id2">The ID of the second flight.</param>
        /// <param name="destination2">The destination of the second flight.</param>
        /// <param name="duration2">The duration of the second flight.</param>
        [TestCase("AirlinerA", "FL001", "DestinationX", 2.0, "AirlinerB", "FL002", "DestinationY", 3.0)]
        [TestCase("AirlinerC", "FL003", "DestinationZ", 1.5, "AirlinerD", "FL004", "DestinationW", 4.0)]
        public void GetFlights_AfterAddingMultipleFlights_ReturnsCorrectNumber(
            string airliner1, string id1, string destination1, double duration1,
            string airliner2, string id2, string destination2, double duration2)
        {
            // Arrange
            flightRepository.AddFlight(CreateFlight(airliner1, id1, destination1, duration1, DefaultFlightHeight, false));
            flightRepository.AddFlight(CreateFlight(airliner2, id2, destination2, duration2, DefaultFlightHeight, true));

            // Act
            List<FlightDTO> flights = flightRepository.GetFlights();

            // Assert
            Assert.That(flights.Count, Is.EqualTo(2), "The number of flights returned should match the number of flights added.");
        }

        /// <summary>
        /// Helper method to create a <see cref="FlightDTO"/> object with the given parameters.
        /// </summary>
        /// <param name="airliner">The airliner operating the flight.</param>
        /// <param name="id">The unique identifier for the flight.</param>
        /// <param name="destination">The destination of the flight.</param>
        /// <param name="duration">The duration of the flight in hours.</param>
        /// <param name="flightHeight">The initial flight height.</param>
        /// <param name="inFlight">Indicates whether the flight is currently in-flight.</param>
        /// <returns>A <see cref="FlightDTO"/> object populated with the specified data.</returns>
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
