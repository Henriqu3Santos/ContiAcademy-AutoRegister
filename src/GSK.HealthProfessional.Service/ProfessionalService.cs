using GSK.HealthProfessional.Data;
using GSK.HealthProfessional.Model;
using GSK.HealthProfessional.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using ServiceStack.Logging;
using System;
using System.Linq;

namespace GSK.HealthProfessional.Service
{

    public class ProfessionalService : BaseService, IProfessionalService
    {

        private readonly IConfiguration _configuration;
        private readonly string _urlApiNeolude;
        private readonly AuthenticationApiService _authenticationAPIService;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly ILogger<DebugLogger> _logger;
        private readonly IOccupationAreaService _occupationAreaService;


        public ProfessionalService(IConfiguration configuration,
                                    AuthenticationApiService authenticationAPIService,
                                    IProfessionalRepository professionalRepository,
                                    ILogger<DebugLogger> logger,
                                    INotifier notificador,
                                    IOccupationAreaService occupationAreaService) : base(notificador)
        {
            _configuration = configuration;
            _urlApiNeolude = _configuration.GetSection("AppSettings").GetSection("UrlApiNeolude").Value;
            _authenticationAPIService = authenticationAPIService;
            _professionalRepository = professionalRepository;
            _logger = logger;
            _occupationAreaService = occupationAreaService;
        }


        public bool Add(HealthProfessionalModel professional, out string message)
        {
            var error = "";

            var integrationClientUniqueIdentifier = _configuration.GetSection("AppSettings:IntegrationClientUI").Value;
            var IntegrationClientSecret = _configuration.GetSection("AppSettings:IntegrationClientSecret").Value;
            var businessUnitUI = _configuration.GetSection("AppSettings:BusinessUnitClientUIRegister").Value;

            var ticks = DateTime.Now.Ticks.ToString();
            var integrationToken = AuthenticationApiService.GenerateToken(integrationClientUniqueIdentifier, IntegrationClientSecret, ticks);
            var action = string.Format("/api/integration/UserIntegration/UserImport/?ticks={0}&clientIdentifier={1}", ticks, integrationClientUniqueIdentifier);
            var client = new RestClient(_urlApiNeolude);
            var request = new RestRequest(action, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            string[] name = professional.Name.Split(' ');
            string firtName = professional.Name;
            Array.Reverse(name);
            string lastName = professional.Sobrenome;

            if (string.IsNullOrWhiteSpace(lastName))
                lastName = ".";
            request.AddJsonBody(
            new
            {
                Name = firtName,
                LastName = lastName,
                ClientUniqueIdentifier = professional.Email,
                Token = integrationToken,
                Password = professional.Password,
                Login = professional.Email,
                Email = professional.Email,
                ForcePasswordChange = false,
                TermsOfUseAcceptance = -1,
                Suspense = professional.CodigoSAP == null ? true : false,
                SendEmail = true,
                Destinatario = _configuration.GetSection("AppSettings:Destinatario").Value
        });
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var objReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
                if (objReturn.ResultCode < 0)
                {
                    
                    error = $"Não foi possível cadastrar o usuário: Erro:{objReturn.ResultCode.ToString()} - {objReturn.Message} ";
                    Notify(error);
                    _logger.LogError(error);
                }
                else
                {
                    if (professional.CodigoSAP != null)
                    {
                        var model = _occupationAreaService.GetProfile().Where( x => x.CodigoSAP.ToString() == professional.CodigoSAP).FirstOrDefault();

                        LinkUserToCompany(professional.Email, string.Empty, model);
                    }
                    else
                    {
                        InativeUser(professional.Email);
                    }
                }

                message = objReturn.Message;
            }
            else
            {
                error = $"Não foi possível cadastrar o usuário: httpError{response.StatusCode}  Erro: {response.Content} ";
                Notify(error);
                _logger.LogError(error);
                message = response.Content;
            }

            return false;

        }

        private bool BusinessUnitImportExec(string name, string parentClientUniqueIdentifier, string clientUniqueIdentifier)
        {

            var error = "";
            var integrationClientUniqueIdentifier = _configuration.GetSection("AppSettings:IntegrationClientUI").Value;
            var IntegrationClientSecret = _configuration.GetSection("AppSettings:IntegrationClientSecret").Value;
            var ticks = DateTime.Now.Ticks.ToString();

            var integrationToken = AuthenticationApiService.GenerateToken(integrationClientUniqueIdentifier, IntegrationClientSecret, ticks);
            var action = string.Format("/api/integration/BusinessUnitIntegration/BusinessUnitImport/?ticks={0}&clientIdentifier={1}", ticks, integrationClientUniqueIdentifier);
            var client = new RestClient(_urlApiNeolude);
            var request = new RestRequest(action, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var requestCity = new RestRequest(action, Method.POST);

            requestCity.AddJsonBody(
                new
                {
                    Name = name,
                    ParentClientUniqueIdentifier = parentClientUniqueIdentifier,
                    ClientUniqueIdentifier = clientUniqueIdentifier,
                    IsDeleted = false,
                    StateIdentifier = "ACTIVE",
                    Token = integrationToken
                });

            IRestResponse response = client.Execute(requestCity);
            var objReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
            if (objReturn.ResultCode < 0)
            {
                error = $"Não conseguiu fazer o vinculo com a empresa:{name} Erro: { objReturn.ResultCode.ToString()}- { objReturn.Message}";
                Notify(error);
                _logger.LogError(error);
                return false;
            }
            else
            {
                return true;
            }

        }


        public bool BusinessUnitImport(string companyId, string companyDescription, string city, string cityId, string state, string stateId, string businessUnitCUI)
        {

            if (BusinessUnitImportExec(companyId == Convert.ToString((int)CompanyModel.ComapanysEnum.Outro) ? CompanyModel.ComapanysEnum.Outro.ToString() : companyDescription, businessUnitCUI, companyId))//Nivel 1
                if (BusinessUnitImportExec(state, companyId, stateId))//Nivel2
                    if (BusinessUnitImportExec(city, stateId, cityId))//Nivel3                    
                        if (companyId == Convert.ToString((int)CompanyModel.ComapanysEnum.Outro))
                        {//Possivel Nivel4
                            if (BusinessUnitImportExec(companyDescription, cityId, companyId + "_" + companyDescription))
                                return true;
                        }
                        else
                            return true;
            return false;
        }




        public bool LinkUserToCompany(string email, string registrationNumber, OccupationAreaModel occupationAreaModel)
        {

            var error = "";
            var integrationClientUniqueIdentifier = _configuration.GetSection("AppSettings:IntegrationClientUI").Value;
            var IntegrationClientSecret = _configuration.GetSection("AppSettings:IntegrationClientSecret").Value;
            var ticks = DateTime.Now.Ticks.ToString();

            var integrationToken = AuthenticationApiService.GenerateToken(integrationClientUniqueIdentifier, IntegrationClientSecret, ticks);
            var action = string.Format("/api/integration/BusinessUnitIntegration/LinkUserToCompany/?ticks={0}&clientIdentifier={1}", ticks, integrationClientUniqueIdentifier);
            var client = new RestClient(_urlApiNeolude);
            var request = new RestRequest(action, Method.POST);
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(
            new
            {
                RegistrationNumber = registrationNumber,
                UserClientUniqueIdentifier = email,
                BusinessUnitClientUniqueIdentifier = occupationAreaModel.CodigoSAP,
                OccupationAreaClientUniqueIdentifier = occupationAreaModel.Perfil,
                PositionName = occupationAreaModel.Cargo,
                DirectSuperiorClientUniqueIdentifier = occupationAreaModel.Gestor,
                AdmissionDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                StateIdentifier = "ACTIVE",
                Token = integrationToken,
            });

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var objReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
                if (objReturn.ResultCode < 0)
                {
                    error = $"Não conseguiu fazer o vinculo com a empresa: Erro: { objReturn.ResultCode.ToString()}- { objReturn.Message}";
                    Notify(error);
                    _logger.LogError(error);
                }
                else
                    return true;
            }
            else
            {
                error = $"Não conseguiu fazer o vinculo com a empresa: Erro:{response.StatusCode} - {response.Content}";
                Notify(error);
                _logger.LogError(error);
            }

            return false;

        }

        public bool HasCodigoSAP(string CodigoSAP, out string name)
        {
            var error = "";
            var integrationClientUniqueIdentifier = _configuration.GetSection("AppSettings:IntegrationClientUI").Value;
            var IntegrationClientSecret = _configuration.GetSection("AppSettings:IntegrationClientSecret").Value;
            var ticks = DateTime.Now.Ticks.ToString();

            var integrationToken = AuthenticationApiService.GenerateToken(integrationClientUniqueIdentifier, IntegrationClientSecret, ticks);
            var encodedToken = System.Web.HttpUtility.UrlEncode(integrationToken);
            var action = string.Format("/api/integration/BusinessUnitIntegration/GetAll?ticks={0}&clientIdentifier={1}&token={2}", ticks, integrationClientUniqueIdentifier, encodedToken);
            var client = new RestClient(_urlApiNeolude);
            var request = new RestRequest(action, Method.GET);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var getCompany = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessUnitIntegrationModel>(response.Content)
                    .AditionalInformation.Where(w => w.ClientUniqueIdentifier == CodigoSAP).FirstOrDefault();

                if (getCompany != null)
                {
                    name = getCompany.Name;
                    return true;
                }

            }

            name = string.Empty;
            return false;

        }

        public bool InativeUser(string email)
        {

            var error = "";
            var integrationClientUniqueIdentifier = _configuration.GetSection("AppSettings:IntegrationClientUI").Value;
            var IntegrationClientSecret = _configuration.GetSection("AppSettings:IntegrationClientSecret").Value;
            var ticks = DateTime.Now.Ticks.ToString();

            var integrationToken = AuthenticationApiService.GenerateToken(integrationClientUniqueIdentifier, IntegrationClientSecret, ticks);
            var action = string.Format("/api/integration/UserIntegration/InativeUser/?ticks={0}&clientIdentifier={1}", ticks, integrationClientUniqueIdentifier);
            var client = new RestClient(_urlApiNeolude);
            var request = new RestRequest(action, Method.POST);
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(
            new
            {
                ClientUniqueIdentifier = email,
                Motive = "Custom Cliente Externo",
                Feedback = "Seu acesso encontra pendente para desbloqueio, por favor entre em contato com o administrador da plataforma",
                Token = integrationToken
            });

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var objReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
                if (objReturn.ResultCode < 0)
                {
                    error = $"Não conseguiu inativar o usuário: Erro: { objReturn.ResultCode.ToString()}- { objReturn.Message}";
                    Notify(error);
                    _logger.LogError(error);
                }
                else
                    return true;
            }
            else
            {
                error = $"Não conseguiu inativar o usuário: Erro:{response.StatusCode} - {response.Content}";
                Notify(error);
                _logger.LogError(error);
            }

            return true;
        }

    }
}
