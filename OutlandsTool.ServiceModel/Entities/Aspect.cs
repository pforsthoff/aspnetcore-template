using System.ComponentModel.DataAnnotations;
using OutlandsTool.ServiceModel.Entities.Base;

namespace OutlandsTool.ServiceModel.Entities
{
    public class Aspect : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoreImage { get; set; }
        public string ExtractImage { get; set; }
        public string DistillationImage { get; set; }
        public string Color { get; set; }

    }
}
