namespace PeopleManager.Dto.Requests
{
    public class VehicleRequest
    {
        public required string LicensePlate { get; set; }
        public string? Brand { get; set; }
        public string? Type { get; set; }

        public int? ResponsiblePersonId { get; set; }
    }
}
