using GSK.HealthProfessional.Model;


namespace GSK.HealthProfessional.Data
{
    public class ProfessionalRepository : BaseRepository<Model.HealthProfessionalModel>, IProfessionalRepository        
    {
        public ProfessionalRepository(IMongoContext context) : base(context) { }
    }
}
