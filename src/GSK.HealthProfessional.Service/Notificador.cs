using GSK.HealthProfessional.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GSK.HealthProfessional.Service
{
    public class Notificador : INotifier
    {
        private readonly List<Notification> _notificacoes;

        public Notificador()
        {
            _notificacoes = new List<Notification>();
        }

        public void Handle(Notification notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public List<Notification> GetNotifications()
        {
            return _notificacoes;
        }

        public bool HasNotification()
        {
            return _notificacoes.Any();
        }
    }
}
