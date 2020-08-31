namespace Application.Infrastructure.Redering
{
    public class Template
    {
        public InfrastructureCredential Credential { get; set; }
        public string Content { get; set; }
        public string DataBound { get; set; }
        public TemplateType Type { get; set; }
        public RenderAs RenderAs{ get; set; }
    }
}