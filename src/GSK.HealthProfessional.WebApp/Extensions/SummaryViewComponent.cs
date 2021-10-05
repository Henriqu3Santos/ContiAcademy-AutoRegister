using GSK.HealthProfessional.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GSK.HealthProfessional.WebApp.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotifier _notificador;

        public SummaryViewComponent(INotifier notificador)
        {
            _notificador = notificador;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notificador.GetNotifications());
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Message));

            return View();
        }
    }
}
