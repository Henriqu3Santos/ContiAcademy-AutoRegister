using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace GSK.HealthProfessional.Model
{
    public class CityModel
    {
        [Key]
        [JsonProperty("name")]
        public string Nome { get; set; }       

    }
}
