using GSK.HealthProfessional.Model;
using GSK.HealthProfessional.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Collections.Generic;

namespace GSK.HealthProfessional.Service
{
    public class CityService : ICityService
    {
        #region "properties "

        private readonly IConfiguration _configuration;

        private readonly IStateService _stateService;

        #endregion

        #region " ctor "

        public CityService(IConfiguration configuration, IStateService stateService)
        {
            _configuration = configuration;
            _stateService = stateService;
        }

        #endregion

        #region " methdos "

        public IEnumerable<CityModel> GetByStateId(string stateId)
        {
            //var urlApi = _configuration.GetSection("AppSettings").GetSection("CEPAPIURL").Value;
            //var CepAbertoToken = _configuration.GetSection("AppSettings").GetSection("CepAbertoToken").Value;
            //var action = $"/api/v3/cities?estado={stateId}";

            var state = _stateService.getIBGEId(stateId);
            var urlApi = _configuration.GetSection("AppSettings").GetSection("IBGEAPIURL").Value.Replace("{stateId}", state != null ? state.Id : "");
            var client = new RestClient(urlApi);
            var request = new RestRequest(urlApi, Method.GET);
            //request.AddHeader("Authorization", $"Token token={CepAbertoToken}");

            var response = client.Execute<List<CityModel>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return response.Data;

            return new List<CityModel>();
        }

        #endregion
    }
}
