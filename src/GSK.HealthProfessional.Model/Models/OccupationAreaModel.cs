using System.ComponentModel.DataAnnotations;


namespace GSK.HealthProfessional.Model
{
    public class OccupationAreaModel 
    {
        [Key]
        public long OccupationAreaID { get; set; }
        public string Name { get; set; }
        public string ClientUniqueIdentifier { get; set; }
        public long CodigoSAP { get; set; }
        public string  Gestor { get; set; }
        public string Perfil { get; set; }
        public string Cargo { get; set; }

    }
}
