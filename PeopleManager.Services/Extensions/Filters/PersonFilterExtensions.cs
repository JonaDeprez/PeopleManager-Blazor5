using PeopleManager.Dto.Filters;
using PeopleManager.Model;

namespace PeopleManager.Services.Extensions.Filters
{
    public static class PersonFilterExtensions
    {
        public static IQueryable<Person> ApplyFilter(this IQueryable<Person> query, PersonFilter? filter)
        {
            if (filter is null)
            {
                return query;
            }

            if (filter.NumberOfResponsibleVehicles is not null)
            {
                query = query.Where(p =>
                    ((filter.NumberOfResponsibleVehicles.IncludeFrom && 
                      p.ResponsibleForVehicles.Count >= filter.NumberOfResponsibleVehicles.From) ||
                        (!filter.NumberOfResponsibleVehicles.IncludeFrom && 
                         p.ResponsibleForVehicles.Count > filter.NumberOfResponsibleVehicles.From)) 
                    &&
                    ((filter.NumberOfResponsibleVehicles.IncludeTo 
                      && p.ResponsibleForVehicles.Count <= filter.NumberOfResponsibleVehicles.To) ||
                        (!filter.NumberOfResponsibleVehicles.IncludeTo 
                         && p.ResponsibleForVehicles.Count < filter.NumberOfResponsibleVehicles.To)));
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchCriteria = filter.Search.Split(" ");

                foreach (var search in searchCriteria)
                {
                    query = query.Where(p => p.FirstName.ToLowerInvariant().Contains(search.ToLowerInvariant()) ||
                                             p.LastName.ToLowerInvariant().Contains(search.ToLowerInvariant()) ||
                                             p.Email.ToLowerInvariant().Contains(search.ToLowerInvariant()));

                }
            }

            return query;
        }
    }
}
