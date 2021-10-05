using GSK.HealthProfessional.Model;
using System.Collections.Generic;

namespace GSK.HealthProfessional.Service.Interfaces
{
    public interface ICityService
    {
        IEnumerable<CityModel> GetByStateId(string stateId);
    }
}
