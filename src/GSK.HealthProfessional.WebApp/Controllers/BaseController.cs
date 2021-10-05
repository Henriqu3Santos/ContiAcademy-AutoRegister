using GSK.HealthProfessional.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GSK.HealthProfessional.WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        public readonly INotifier _notifier;

        protected BaseController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected bool ValidOperation()
        {
            return !_notifier.HasNotification();
        }
    }
}
