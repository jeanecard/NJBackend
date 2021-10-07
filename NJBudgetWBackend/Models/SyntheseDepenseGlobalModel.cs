using NJBudgetBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Models
{
    public class SyntheseDepenseGlobalModel
    {
        public IEnumerable<SyntheseDepenseGlobalModelItem> Data { get; set; }
        public CompteStatusEnum Status { get; set; }
    }
}
