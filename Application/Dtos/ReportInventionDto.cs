namespace Application.Dtos
{
    public class ReportInventionDto
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool SendByEmail { get; set; }
    }
}