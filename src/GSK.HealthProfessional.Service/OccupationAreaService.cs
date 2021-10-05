using GSK.HealthProfessional.Model;
using GSK.HealthProfessional.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace GSK.HealthProfessional.Service
{
    public class OccupationAreaService : IOccupationAreaService
    {
        private readonly IConfiguration _configuration;

        public OccupationAreaService( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  IEnumerable<OccupationAreaModel> GetProfile() =>_configuration.GetSection("AppSettings").GetSection("OccupationArea").Get<List<OccupationAreaModel>>();
    }
}
