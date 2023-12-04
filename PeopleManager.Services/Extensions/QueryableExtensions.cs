using PeopleManager.Dto.Results;
using PeopleManager.Model;

namespace PeopleManager.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<PersonResult> ProjectToResults(this IQueryable<Person> query)
        {
            return query.Select(p => new PersonResult
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Description = p.Description,
                NumberOfResponsibleVehicles = p.ResponsibleForVehicles.Count
            });
        }

        public static IQueryable<VehicleResult> ProjectToResults(this IQueryable<Vehicle> query)
        {
            return query.Select(v => new VehicleResult
            {
                Id = v.Id,
                LicensePlate = v.LicensePlate,
                Brand = v.Brand,
                Type = v.Type,
                ResponsiblePersonId = v.ResponsiblePersonId,
                ResponsiblePersonFirstName = v.ResponsiblePerson.FirstName,
                ResponsiblePersonLastName = v.ResponsiblePerson.LastName
            });
        }
    }
}
