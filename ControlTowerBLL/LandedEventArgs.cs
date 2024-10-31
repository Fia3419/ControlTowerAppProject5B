
using ControlTowerBLL;
using ControlTowerBLL.Interfaces;

/// <summary>
/// Provides data for the FlightLanded event, containing information about the flight that has landed.
/// </summary>
public class LandedEventArgs : EventArgs
{
    /// <summary>
    /// The flight that has landed.
    /// </summary>
    public IFlight Flight { get; }

    /// <summary>
    /// Initializes a new instance of the LandedEventArgs class with the flight data.
    /// </summary>
    /// <param name="flight">The flight that has landed.</param>
    public LandedEventArgs(IFlight flight)
    {
        Flight = flight;
    }
}
