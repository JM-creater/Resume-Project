using backendresume.Core.Entities;
using backendresume.Core.Enum;

namespace backendresume.Core.Dtos.Job
{
    public class JobGetDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public JobLevel Level { get; set; }
        public long CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
