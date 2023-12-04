using System;
using System.Collections.Generic;
using System.Linq;

namespace TripAdministrations
{
    public class TripAdministrator : ITripAdministrator
    {
        Dictionary<string, Company> companies = new Dictionary<string, Company>();
        Dictionary<string, Trip> trips = new Dictionary<string, Trip>();
        Dictionary<string, HashSet<Trip>> companyTrips = new Dictionary<string, HashSet<Trip>>();
       // Dictionary<string, string> tripCompanies = new Dictionary<string, string>();

        public void AddCompany(Company c)
        {
            if(this.Exist(c))
                throw new ArgumentException();

            companies.Add(c.Name, c);
            companyTrips.Add(c.Name, new HashSet<Trip>());
        }

        public void AddTrip(Company c, Trip t)
        {
            if(!this.Exist(c))
                throw new ArgumentException();

            trips.Add(t.Id, t);
            companyTrips[c.Name].Add(t);
            c.CurrentTrips++;
        }

        public bool Exist(Company c)
            => companies.Values.Contains(c);

        public bool Exist(Trip t)
            => trips.Values.Contains(t);

        public void RemoveCompany(Company c)
        {
            if(!this.Exist(c))
                throw new ArgumentException();

            companies.Remove(c.Name);
            companyTrips[c.Name].Clear();
        }

        public IEnumerable<Company> GetCompanies()
            => companies.Values;

        public IEnumerable<Trip> GetTrips()
            => trips.Values;

        public void ExecuteTrip(Company c, Trip t)
        {
            if(!this.Exist(c) || !this.Exist(t))
                throw new ArgumentException();
            if (!companyTrips[c.Name].Contains(t))
                throw new ArgumentException();

            companyTrips[c.Name].Remove(t);
            c.CurrentTrips--;
        }

        public IEnumerable<Company> GetCompaniesWithMoreThatNTrips(int n)
            => this.GetCompanies().Where(c => c.CurrentTrips > n);

        public IEnumerable<Trip> GetTripsWithTransportationType(Transportation t)
            => this.GetTrips().Where(t => t.Transportation.Equals(t));

        public IEnumerable<Trip> GetAllTripsInPriceRange(int lo, int hi)
            => this.GetTrips().Where(t => t.Price >= lo && t.Price <= hi);
    }
}
