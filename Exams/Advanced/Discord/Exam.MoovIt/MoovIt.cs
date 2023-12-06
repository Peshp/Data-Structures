using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Exam.MoovIt
{
    public class MoovIt : IMoovIt
    {
        private HashSet<Route> routes = new HashSet<Route>();

        public int Count => routes.Count;

        public void AddRoute(Route route)
        {
            if (Contains(route))
                throw new ArgumentException();

            routes.Add(route);
        }

        public void ChooseRoute(string routeId)
        {
            var curr = GetRoute(routeId);
            curr.Popularity += 1;
        }

        public bool Contains(Route route)
        {
            return routes.Contains(route);
        }

        public IEnumerable<Route> GetFavoriteRoutes(string destinationPoint)
        {
            return routes
                .Where(r => r.IsFavorite && r.LocationPoints.Contains(destinationPoint, StringComparer.OrdinalIgnoreCase) && r.LocationPoints.IndexOf(destinationPoint, StringComparison.OrdinalIgnoreCase) > 0)
                .OrderBy(r => r.Distance)
                .ThenByDescending(r => r.Popularity);
        }

        public Route GetRoute(string routeId)
        {
            var curr = routes.FirstOrDefault(r => r.Id == routeId);
            if (curr == null)
                throw new ArgumentException();

            return curr;
        }

        public IEnumerable<Route> GetTop5RoutesByPopularityThenByDistanceThenByCountOfLocationPoints()
        {
            return routes
                .OrderByDescending(r => r.Popularity)
                .ThenBy(r => r.Distance)
                .ThenBy(r => r.LocationPoints.Count)
                .Take(5);
        }

        public void RemoveRoute(string routeId)
        {
            var curr = GetRoute(routeId);
            routes.Remove(curr);
        }

        public IEnumerable<Route> SearchRoutes(string startPoint, string endPoint)
            => routes
                .Where(route => route.LocationPoints.Contains(startPoint) && route.LocationPoints.Contains(endPoint))
                .OrderByDescending(route => route.IsFavorite)
                .ThenBy(route => GetDistance(route.LocationPoints, startPoint, endPoint))
                .ThenByDescending(route => route.Popularity);

        private int GetDistance(List<string> locationPoints, string startPoint, string endPoint)
        {
            var startIndex = locationPoints.IndexOf(startPoint);
            var endIndex = locationPoints.IndexOf(endPoint);

            if (startIndex == -1 || endIndex == -1)
            {
                return int.MaxValue;
            }

            return Math.Abs(endIndex - startIndex);
        }
    }
}
