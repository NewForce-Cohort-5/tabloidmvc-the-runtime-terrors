namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public int Date { get; set; }
    }
}
