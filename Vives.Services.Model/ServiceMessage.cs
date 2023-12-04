namespace Vives.Services.Model
{
    public class ServiceMessage
    {
        public required string Code { get; set; }
        public ServiceMessageType Type { get; set; }
        public string? PropertyName { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
    }
}
