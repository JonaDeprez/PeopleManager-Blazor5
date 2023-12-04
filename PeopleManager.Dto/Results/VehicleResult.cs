using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Dto.Results
{
    public class VehicleResult
    {
        public int Id { get; set; }

        public required string LicensePlate { get; set; }
        public string? Brand { get; set; }
        public string? Type { get; set; }

        public int? ResponsiblePersonId { get; set; }

        public string? ResponsiblePersonFirstName { get; set; }
        public string? ResponsiblePersonLastName { get; set; }
    }
}
