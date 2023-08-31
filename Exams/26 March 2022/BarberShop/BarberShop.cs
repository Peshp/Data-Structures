using System;
using System.Collections.Generic;
using System.Linq;

namespace BarberShop
{
    public class BarberShop : IBarberShop
    {
        Dictionary<string, Barber> barbers = new Dictionary<string, Barber>();
        Dictionary<string, Client> clients = new Dictionary<string, Client>();
        Dictionary<string, HashSet<Client>> barberClients = new Dictionary<string, HashSet<Client>>();

        public void AddBarber(Barber b)
        {
            if (this.Exist(b))
                throw new ArgumentException();

            barbers.Add(b.Name, b);
            barberClients.Add(b.Name, new HashSet<Client>());
        }

        public void AddClient(Client c)
        {
            if(this.Exist(c))
                throw new ArgumentException();

            clients.Add(c.Name, c);
        }

        public bool Exist(Barber b)
            => barbers.Values.Contains(b);

        public bool Exist(Client c)
            => clients.Values.Contains(c);

        public IEnumerable<Barber> GetBarbers()
            => barbers.Values;

        public IEnumerable<Client> GetClients()
            => clients.Values;

        public void AssignClient(Barber b, Client c)
        {
            if(!this.Exist(b) || !this.Exist(c)) 
                throw new ArgumentException();

            c.Barber = b;
            barberClients[b.Name].Add(c);
        }

        public void DeleteAllClientsFrom(Barber b)
        {
            if(!this.Exist(b))
                throw new ArgumentException();

            var clientsToDelete = barberClients[b.Name];
            foreach(Client c in clientsToDelete)
            {
                barberClients[b.Name].Remove(c);
                c.Barber = null;
            }
        }

        public IEnumerable<Client> GetClientsWithNoBarber()
            => this.GetClients().Where(c => c.Barber == null);

        public IEnumerable<Barber> GetAllBarbersSortedWithClientsCountDesc()
            => this.GetBarbers().OrderByDescending(b => barberClients[b.Name].Count);

        public IEnumerable<Barber> GetAllBarbersSortedWithStarsDecsendingAndHaircutPriceAsc()
            => this.GetBarbers().OrderByDescending(b => b.Stars).ThenBy(b => b.HaircutPrice);

        public IEnumerable<Client> GetClientsSortedByAgeDescAndBarbersStarsDesc()
            => this.GetClients().OrderByDescending(c => c.Age).ThenByDescending(c => c.Barber.Stars);
    }
}
