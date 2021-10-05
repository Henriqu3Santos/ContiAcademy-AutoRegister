using System.Collections.Generic;

namespace GSK.HealthProfessional.Service.Interfaces
{
    public interface INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notifications);
    }
}
