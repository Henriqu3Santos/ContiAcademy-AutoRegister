using GSK.HealthProfessional.Model;
using GSK.HealthProfessional.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GSK.HealthProfessional.Service
{
    public class StateService : IStateService
    {
        #region " properties "

        private List<StateModel> _states = null;

        #endregion

        #region " ctor "

        public StateService()
        {
            _states = new List<StateModel>
            {
                new StateModel() { Id = "12", StateId = "AC", Description = "Acre" },
                new StateModel() { Id = "27", StateId = "AL", Description = "Alagoas" },
                new StateModel() { Id = "13", StateId = "AM", Description = "Amazonas" },
                new StateModel() { Id = "16", StateId = "AP", Description = "Amapá" },
                new StateModel() { Id = "29", StateId = "BA", Description = "Bahia" },
                new StateModel() { Id = "23", StateId = "CE", Description = "Ceará" },
                new StateModel() { Id = "53", StateId = "DF", Description = "Distrito Federal" },
                new StateModel() { Id = "32", StateId = "ES", Description = "Espírito Santo" },
                new StateModel() { Id = "52", StateId = "GO", Description = "Goiás" },
                new StateModel() { Id = "21", StateId = "MA", Description = "Maranhão" },
                new StateModel() { Id = "31", StateId = "MG", Description = "Minas Gerais" },
                new StateModel() { Id = "50", StateId = "MS", Description = "Mato Grosso do Sul" },
                new StateModel() { Id = "51", StateId = "MT", Description = "Mato Grosso" },
                new StateModel() { Id = "15", StateId = "PA", Description = "Pará" },
                new StateModel() { Id = "25", StateId = "PB", Description = "Paraíba" },
                new StateModel() { Id = "26", StateId = "PE", Description = "Pernambuco" },
                new StateModel() { Id = "22", StateId = "PI", Description = "Piauí" },
                new StateModel() { Id = "41", StateId = "PR", Description = "Paraná" },
                new StateModel() { Id = "33", StateId = "RJ", Description = "Rio de Janeiro" },
                new StateModel() { Id = "24", StateId = "RN", Description = "Rio Grande do Norte" },
                new StateModel() { Id = "11", StateId = "RO", Description = "Rondônia" },
                new StateModel() { Id = "14", StateId = "RR", Description = "Roraima" },
                new StateModel() { Id = "43", StateId = "RS", Description = "Rio Grande do Sul" },
                new StateModel() { Id = "42", StateId = "SC", Description = "Santa Catarina" },
                new StateModel() { Id = "28", StateId = "SE", Description = "Sergipe" },
                new StateModel() { Id = "35", StateId = "SP", Description = "São Paulo" },
                new StateModel() { Id = "17", StateId = "TO", Description = "Tocantins" }
            };
        }

        #endregion

        #region " methods "

        public IEnumerable<StateModel> GetAll()
        {
            return _states;
        }

        public StateModel getIBGEId(string stateId)
        {
            return _states.Where(p => p.StateId == stateId).FirstOrDefault();
        }

        #endregion
    }

   

}
