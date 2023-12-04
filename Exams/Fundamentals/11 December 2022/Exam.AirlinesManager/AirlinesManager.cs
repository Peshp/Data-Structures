using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.DeliveriesManager
{
    public class AirlinesManager : IAirlinesManager
    {
        private readonly List<Airline> airlines = new List<Airline>();
        private readonly List<Flight> flights = new List<Flight>();

        public void AddAirline(Airline airline)
            => airlines.Add(airline);

        public void AddFlight(Airline airline, Flight flight)
        {
            if (!this.Contains(airline))
                throw new ArgumentException();

            this.flights.Add(flight);
            airline.flightes.Add(flight);
        }

        public bool Contains(Airline airline)
            => airlines.Contains(airline);

        public bool Contains(Flight flight)
            => flights.Contains(flight);

        public void DeleteAirline(Airline airline)
        {
            if(!this.Contains(airline))
                throw new ArgumentException();

            airlines.Remove(airline);
            airline.flightes.Clear();
        }

        public IEnumerable<Airline> GetAirlinesOrderedByRatingThenByCountOfFlightsThenByName()
            => airlines.OrderByDescending(a => a.Rating)
                .ThenByDescending(a => a.flightes.Count)
                .ThenBy(a => a.Name);

        public IEnumerable<Airline> GetAirlinesWithFlightsFromOriginToDestination(string origin, string destination)
            => airlines.Where(a => a.flightes.Where(f => f.IsCompleted == false && 
                f.Origin == origin && f.Destination == destination).Count() >= 1);

        public IEnumerable<Flight> GetAllFlights()
            => flights;

        public IEnumerable<Flight> GetCompletedFlights()
            => flights.Where(f => f.IsCompleted == true);

        public IEnumerable<Flight> GetFlightsOrderedByCompletionThenByNumber()
            => flights.OrderBy(f => f.Number).OrderBy(f => f.IsCompleted);

        public Flight PerformFlight(Airline airline, Flight flight)
        {
            if (flight == null || airline == null)
                throw new ArgumentException();

            flight.IsCompleted = true;
            return flight;
        }
    }
}
