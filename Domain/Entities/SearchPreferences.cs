namespace Domain.Entities
{
    public class SearchPreferences : Entity
    {
        public int Days { get; set; }
        public string OrderBy { get; set; }
        public int PageSize { get; set; }
    }
}