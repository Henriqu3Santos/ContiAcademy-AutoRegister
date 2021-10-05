using System;
using System.Linq;
using System.Web.Mvc;
using GSK.HealthProfessional.Service;
using GSK.HealthProfessional.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace GSK.HealthProfessional.WebApp.Controllers
{
    public class ProfessionalRegistrationController : BaseController
    {


        private readonly IOccupationAreaService _occupationAreaService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IConfiguration _configuration;
        private readonly IProfessionalService _professionalService;
        private readonly ICompanyService _companyService;

        public ProfessionalRegistrationController(IConfiguration configuration
            ,IProfessionalService professionalService
            ,IOccupationAreaService occupationAreaService
            ,ICompanyService companyService
            , IStateService stateService
            , INotifier notificador) :base(notificador)
        {
            _configuration = configuration;
            _occupationAreaService = occupationAreaService;
            _cityService = new CityService(_configuration, stateService);
            _professionalService =  professionalService;
            _stateService = stateService;
            _companyService = companyService;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCity(string stateId)
        {
            if(stateId != "")
                return  Ok(_cityService.GetByStateId(stateId));

            return null;
        }

        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(Model.HealthProfessionalModel professional)
        {
            string name = string.Empty;
            if (professional.CodigoSAP != null)
            {
                if (!_professionalService.HasCodigoSAP(professional.CodigoSAP, out name))
                {
                    ModelState.AddModelError("", "Código SAP inválido");
                    return View("Index", professional);
                }
                
            }
             
            if (!ModelState.IsValid)
            {
                return View("Index", professional);
            }
            if (professional.CodigoSAP == null && professional.ClientType == "ClienteContinental")
            {
                ModelState.AddModelError("", "Para opção Cliente Continental é obrigatório informar o campo Código SAP");
                return View("Index", professional);
            }

            professional.CodigoSAP = professional.ClientType == "ClienteExterno" ? null : professional.CodigoSAP;

            string message;

            _professionalService.Add(professional, out message);  

            if (message == "E-mail já existente")
            {
                ViewBag.Error = "O E-mail informado já existe clique aqui para voltar a tela de login e clique na opção (Esqueci minha senha)";
                return View("Index", professional);
            }

            if (professional.CodigoSAP == null)
            {
                ViewBag.ClienteExternoMessage = "Cadastrado efetuado com sucesso, Por favor aguarde o administrador da plataforma liberar o acesso de seu usuário";
            }
            else
            {
                ViewBag.Url = _configuration.GetSection("AppSettings:UrlApiNeolude").Value;
            }

            return View("Index", professional); 
        }

        public Microsoft.AspNetCore.Mvc.JsonResult GETBU(string codigo) 
        {
            string name = string.Empty;

            if (!string.IsNullOrEmpty(codigo))
            {
                _professionalService.HasCodigoSAP(codigo, out name);

                if (string.IsNullOrEmpty(name))
                {
                    name = "Código SAP inválido";
                }
            }

            return Json(name);
        }

    }
}