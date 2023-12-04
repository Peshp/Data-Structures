using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.DeliveriesManager
{
    public class DeliveriesManager : IDeliveriesManager
    {
        private readonly List<Deliverer> deliverers = new List<Deliverer>();
        private readonly List<Package> packages = new List<Package>();

        public void AddDeliverer(Deliverer deliverer)
            => deliverers.Add(deliverer);

        public void AddPackage(Package package)
            => packages.Add(package);

        public void AssignPackage(Deliverer deliverer, Package package)
        {
            if (!this.Contains(package) || !this.Contains(deliverer))
                throw new ArgumentException();

            package.Deliverer = deliverer;
        }

        public bool Contains(Deliverer deliverer)
            => deliverers.Contains(deliverer);

        public bool Contains(Package package)
            => packages.Contains(package);

        public IEnumerable<Deliverer> GetDeliverers()
            => deliverers;

        public IEnumerable<Deliverer> GetDeliverersOrderedByCountOfPackagesThenByName()
            => deliverers.OrderByDescending(d => packages.Where(p => p.Deliverer == d).Count())
                .ThenBy(d => d.Name);
                
        public IEnumerable<Package> GetPackages()
            => packages;

        public IEnumerable<Package> GetPackagesOrderedByWeightThenByReceiver()
            => packages.OrderByDescending(p => p.Weight).ThenBy(p => p.Receiver);

        public IEnumerable<Package> GetUnassignedPackages()
            => packages.Where(p => p.Deliverer == null);
    }
}
