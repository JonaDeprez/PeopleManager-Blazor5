using Vives.Services.Model;

namespace PeopleManager.Dto.Filters
{
    public class PersonFilter
    {
        public string? Search { get; set; }

        public Between<int>? NumberOfResponsibleVehicles { get; set; }
    }
}
