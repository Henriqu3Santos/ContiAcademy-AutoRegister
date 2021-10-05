
using System.ComponentModel.DataAnnotations;

namespace GSK.HealthProfessional.Model
{
    public class CompanyModel
    {
        [Key]
        public long CompanyId { get; set; }
        public string Description { get; set; }

        public enum ComapanysEnum
        {
            Outro = 9999999
        }        
    }
}
