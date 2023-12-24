using backendresume.Core.Enum;

namespace backendresume.Core.Dtos.Company
{
    public class ComapanyCreateDto
    {
        public string? Name { get; set; }
        public CompanySize Size { get; set; }
    }
}
