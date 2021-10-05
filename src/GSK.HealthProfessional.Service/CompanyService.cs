using GSK.HealthProfessional.Model;
using GSK.HealthProfessional.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace GSK.HealthProfessional.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IConfiguration _configuration;

        public CompanyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<CompanyModel> GetAll() => _configuration.GetSection("AppSettings").GetSection("Company").Get<List<CompanyModel>>();
    }
}
