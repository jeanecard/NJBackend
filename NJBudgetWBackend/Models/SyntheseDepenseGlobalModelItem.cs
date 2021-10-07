using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetBackEnd.Models
{
    public class SyntheseDepenseGlobalModelItem
    {
        public Guid AppartenanceId { get; set; }
        public String AppartenanceCaption { get; set; }

        public float BudgetValuePrevu { get; set; }

        public float BudgetValueDepense { get; set; }
        public float BudgetPourcentageDepense { get; set; }
        public CompteStatusEnum Status { get; set; }
    }
}
