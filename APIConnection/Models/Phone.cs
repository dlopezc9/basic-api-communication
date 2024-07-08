namespace APIConnection.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> ?Data { get; set; }
    }
}
