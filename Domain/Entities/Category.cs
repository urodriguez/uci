namespace Domain.Entities
{
    public class Category : Entity
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsFromAfip { get; set; }
    }
}