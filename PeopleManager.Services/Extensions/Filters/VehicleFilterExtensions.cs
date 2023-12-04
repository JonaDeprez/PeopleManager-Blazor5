using PeopleManager.Dto.Filters;
using PeopleManager.Model;

namespace PeopleManager.Services.Extensions.Filters
{
    public static class VehicleFilterExtensions
    {
        public static IQueryable<Vehicle> ApplyFilter(this IQueryable<Vehicle> query, VehicleFilter? filter)
        {
            if (filter is null)
            {
                return query;
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchCriteria = filter.Search.Split(" ");

                foreach (var search in searchCriteria)
                {
                    query = query.Where(p => p.LicensePlate.ToLowerInvariant().Contains(search.ToLowerInvariant()) ||
                                             p.Type != null && p.Type.ToLowerInvariant().Contains(search.ToLowerInvariant()) ||
                                             p.Brand != null && p.Brand.ToLowerInvariant().Contains(search.ToLowerInvariant()));

                }
            }

            return query;
        }
    }
}
