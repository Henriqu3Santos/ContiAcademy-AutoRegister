using System.ComponentModel.DataAnnotations;

namespace GSK.HealthProfessional.Model
{
    public class StateModel
    {
        public string Id { get; set; }

        [Key]
        public string StateId { get; set; }

        public string Description { get; set; }
    }
}
