namespace PeopleManager.Dto.Results
{
    public class PersonResult
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Description { get; set; }

        public int NumberOfResponsibleVehicles { get; set; }
    }
}
