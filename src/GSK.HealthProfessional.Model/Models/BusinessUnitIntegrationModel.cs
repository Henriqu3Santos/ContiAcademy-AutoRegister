using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSK.HealthProfessional.Model
{
    public class BusinessUnitIntegrationModel
    {
        public long ResultCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<BusinessUnit> AditionalInformation { get; set;}

        public class BusinessUnit
        {
            public string ClientUniqueIdentifier { get; set; }
            public long NeoludeID { get; set; }
            public string Name { get; set; }
            public string ParentClientUniqueIdentifier { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
