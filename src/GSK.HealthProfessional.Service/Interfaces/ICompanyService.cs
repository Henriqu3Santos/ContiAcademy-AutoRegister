using GSK.HealthProfessional.Model;
using System.Collections.Generic;

namespace GSK.HealthProfessional.Service.Interfaces
{
    public interface ICompanyService
    {
        IEnumerable<CompanyModel> GetAll();
    }
}
