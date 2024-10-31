using ControlTowerDTO;

namespace ControlTowerBLL.Interfaces
{
    /// <summary>
    /// Interface for a flight object.
    /// </summary>

    public interface IFlight
    {
        event EventHandler<TakeOffEventArgs> FlightTakeOff;
        event EventHandler<LandedEventArgs> FlightLanded;

        string Airliner { get; }
        string Id { get; }
        string Destination { get; set; }
        double Duration { get; }
        bool InFlight { get; set; }
        DateTime DepartureTime { get; set; }
        int FlightHeight { get; set;}

        void TakeOffFlight();
        void ChangeFlightHeight(int newHeight);
        void LandFlight();
    }
}