
using System.ComponentModel.DataAnnotations;

namespace GSK.HealthProfessional.Model
{
    public class BusinessUnit
    {
        [Key]
        public long BusinessUnitId { get; set; }
        public string Description { get; set; }
    }
}
