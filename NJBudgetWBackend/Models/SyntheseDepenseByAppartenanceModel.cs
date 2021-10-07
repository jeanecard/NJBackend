using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Models
{
    public class SyntheseDepenseByAppartenanceModel
    {
        public Guid AppartenanceId { get; set; }
        public String AppartenanceCaption { get; set; }
        
        public IEnumerable<SyntheseDepenseByAppartenanceModelItem> Data { get; set; }
    }
}
