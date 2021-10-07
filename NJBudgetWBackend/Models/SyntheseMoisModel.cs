using NJBudgetBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Models
{
    public class SyntheseMoisModel
    {
        public float BudgetValuePrevu { get; set; }
        public float BudgetValueDepense { get; set; }
        public CompteStatusEnum Status { get; set; }
    }
}
