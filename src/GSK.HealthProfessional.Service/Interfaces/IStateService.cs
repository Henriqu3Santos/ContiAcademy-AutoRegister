using GSK.HealthProfessional.Model;
using System.Collections.Generic;


namespace GSK.HealthProfessional.Service.Interfaces
{
    public interface IStateService
    {
        IEnumerable<StateModel> GetAll();

        StateModel getIBGEId(string id);
    }
}
