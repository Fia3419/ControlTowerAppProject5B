using System;
using System.Collections.Generic;
using ControlTowerBLL.Interfaces;
using ControlTowerBLL.Managers;
using Moq;

namespace ControlTowerBLL
{
    public class ControlTower : ListManager<IFlight>
    {
        public event EventHandler<TakeOffEventArgs> FlightTakeOff;
        public event EventHandler<LandedEventArgs> FlightLanded;

        public delegate int ChangeAltitudeDelegate(int currentAltitude, int changeValue);
        public ChangeAltitudeDelegate ChangeAltitudeHandler { get; private set; }

        public ControlTower()
        {
            ChangeAltitudeHandler = ChangeAltitude;
        }

        private int ChangeAltitude(int currentAltitude, int changeValue)
        {
            return currentAltitude + changeValue;
        }

        public void AddFlight(IFlight flight)
        {
            flight.FlightTakeOff += OnFlightTakeOff;
            flight.FlightLanded += OnFlightLanded;
            Add(flight);
        }

        public IFlight FindFlightById(string flightId)
        {
            return items.Find(f => f.Id == flightId);
        }

        public void TakeOffFlight(IFlight flight)
        {
            if (flight != null && !flight.InFlight)
            {
                flight.TakeOffFlight();
            }
        }

        public void ChangeFlightHeight(IFlight flight, int changeValue)
        {
            if (flight != null && flight.InFlight)
            {
                int newAltitude = ChangeAltitudeHandler(flight.FlightHeight, changeValue);
                flight.ChangeFlightHeight(newAltitude);
            }
        }

        public void LandFlight(IFlight flight)
        {
            if (flight != null)
            {
                flight.LandFlight();
                FlightLanded?.Invoke(this, new LandedEventArgs(flight));
            }
        }

        private void OnFlightTakeOff(object sender, TakeOffEventArgs e)
            => FlightTakeOff?.Invoke(this, e);

        private void OnFlightLanded(object sender, LandedEventArgs e)
            => FlightLanded?.Invoke(this, e);
    }
}
