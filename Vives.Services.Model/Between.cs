namespace Vives.Services.Model
{
    public class Between<T> 
        where T: struct
    {
        public T From { get; set; }
        public bool IncludeFrom { get; set; }
        public T To { get; set; }
        public bool IncludeTo { get; set; }
    }
}
